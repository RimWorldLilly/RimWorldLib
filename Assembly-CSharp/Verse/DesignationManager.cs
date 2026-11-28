using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using UnityEngine.Rendering;

namespace Verse
{
	// Token: 0x02000252 RID: 594
	public sealed class DesignationManager : IExposable
	{
		// Token: 0x06001167 RID: 4455 RVA: 0x00063924 File Offset: 0x00061B24
		public DesignationManager(Map map)
		{
			this.map = map;
			this.designationsAtCell = new List<Designation>[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001168 RID: 4456 RVA: 0x0006398C File Offset: 0x00061B8C
		public List<Designation> AllDesignations
		{
			get
			{
				DesignationManager.tmpDesignations.Clear();
				foreach (KeyValuePair<DesignationDef, List<Designation>> keyValuePair in this.designationsByDef)
				{
					DesignationManager.tmpDesignations.AddRange(keyValuePair.Value);
				}
				return DesignationManager.tmpDesignations;
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x000639F4 File Offset: 0x00061BF4
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.saveDesignations.Clear();
				this.saveDesignations.AddRange(this.AllDesignations);
			}
			Scribe_Collections.Look<Designation>(ref this.saveDesignations, "allDesignations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.saveDesignations.RemoveAll((Designation x) => x == null) != 0)
				{
					Log.Warning("Some designations were null after loading.");
				}
				if (this.saveDesignations.RemoveAll((Designation x) => x.def == null) != 0)
				{
					Log.Warning("Some designations had null def after loading.");
				}
				foreach (Designation designation in this.saveDesignations)
				{
					designation.designationManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Plan plan = null;
				int i = this.saveDesignations.Count - 1;
				while (i >= 0)
				{
					TargetType targetType = this.saveDesignations[i].def.targetType;
					if (targetType != TargetType.Thing)
					{
						if (targetType != TargetType.Cell)
						{
							goto IL_1F0;
						}
						if (this.saveDesignations[i].target.Cell.IsValid)
						{
							goto IL_1F0;
						}
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Cell-needing designation ");
						defaultInterpolatedStringHandler.AppendFormatted<Designation>(this.saveDesignations[i]);
						defaultInterpolatedStringHandler.AppendLiteral(" had no cell target. Removing...");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					else
					{
						if (this.saveDesignations[i].target.HasThing)
						{
							goto IL_1F0;
						}
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(59, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("Thing-needing designation ");
						defaultInterpolatedStringHandler2.AppendFormatted<Designation>(this.saveDesignations[i]);
						defaultInterpolatedStringHandler2.AppendLiteral(" had no thing target. Removing...");
						Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
					IL_280:
					i--;
					continue;
					IL_1F0:
					if (this.saveDesignations[i].def == DesignationDefOf.Plan)
					{
						Map map = this.map;
						if (map.planManager == null)
						{
							map.planManager = new PlanManager(this.map);
						}
						if (plan == null)
						{
							plan = new Plan(ColorDefOf.PlanGray, this.map.planManager);
						}
						plan.AddCell(this.saveDesignations[i].target.Cell);
						this.saveDesignations.RemoveAt(i);
						goto IL_280;
					}
					this.IndexDesignation(this.saveDesignations[i]);
					goto IL_280;
				}
				if (plan != null)
				{
					plan.CheckContiguous();
				}
				this.saveDesignations.Clear();
			}
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00063CB0 File Offset: 0x00061EB0
		public void DrawDesignations()
		{
			CellRect cellRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(3);
			if (this.tmpPropertyBlock == null)
			{
				this.tmpPropertyBlock = new MaterialPropertyBlock();
			}
			foreach (DesignationDef designationDef in DefDatabase<DesignationDef>.AllDefsListForReading)
			{
				if (designationDef.targetType == TargetType.Cell && designationDef.shouldBatchDraw && SystemInfo.supportsInstancing)
				{
					int count = this.designationsByDef[designationDef].Count;
					if (this.designationsByDef[designationDef].Count != 0)
					{
						this.CalculateCellDesignationDrawMatricies(designationDef);
						List<Matrix4x4[]> list = this.cellDesignationDrawMatricies[designationDef];
						designationDef.iconMat.enableInstancing = true;
						int num = count / 1023;
						for (int i = 0; i < num; i++)
						{
							Graphics.DrawMeshInstanced(MeshPool.plane10, 0, designationDef.iconMat, list[i], 1023, this.tmpPropertyBlock, ShadowCastingMode.Off, true, 0);
						}
						int num2 = count % 1023;
						if (num2 > 0)
						{
							Graphics.DrawMeshInstanced(MeshPool.plane10, 0, designationDef.iconMat, list[num], num2, this.tmpPropertyBlock, ShadowCastingMode.Off, true, 0);
						}
					}
				}
				else
				{
					foreach (Designation designation in this.designationsByDef[designationDef])
					{
						if ((!designation.target.HasThing || designation.target.Thing.Map == this.map) && cellRect.Contains(designation.target.Cell))
						{
							designation.DesignationDraw();
						}
					}
				}
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00063EB0 File Offset: 0x000620B0
		private void DirtyCellDesignationsCache(DesignationDef def)
		{
			if (def.targetType != TargetType.Cell)
			{
				return;
			}
			this.designationMatriciesDirty[def] = true;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00063ECC File Offset: 0x000620CC
		public void AddDesignation(Designation newDes)
		{
			if (newDes.def.targetType == TargetType.Cell && this.DesignationAt(newDes.target.Cell, newDes.def) != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to double-add designation at location ");
				defaultInterpolatedStringHandler.AppendFormatted<LocalTargetInfo>(newDes.target);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (newDes.def.targetType == TargetType.Thing && this.DesignationOn(newDes.target.Thing, newDes.def) != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(41, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Tried to double-add designation on Thing ");
				defaultInterpolatedStringHandler2.AppendFormatted<LocalTargetInfo>(newDes.target);
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				return;
			}
			if (newDes.def.targetType == TargetType.Thing)
			{
				newDes.target.Thing.SetForbidden(false, false);
			}
			this.IndexDesignation(newDes);
			newDes.designationManager = this;
			newDes.Notify_Added();
			Map map = newDes.target.HasThing ? newDes.target.Thing.Map : this.map;
			if (map != null)
			{
				FleckMaker.ThrowMetaPuffs(newDes.target.ToTargetInfo(map));
			}
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00063FF4 File Offset: 0x000621F4
		private void IndexDesignation(Designation designation)
		{
			this.designationsByDef[designation.def].Add(designation);
			this.DirtyCellDesignationsCache(designation.def);
			if (designation.def.targetType == TargetType.Thing)
			{
				Thing thing = designation.target.Thing;
				if (!this.thingDesignations.ContainsKey(thing))
				{
					this.thingDesignations[thing] = new List<Designation>();
				}
				this.thingDesignations[thing].Add(designation);
				return;
			}
			if (designation.def.targetType != TargetType.Cell)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to index unexpected designation type: ");
				defaultInterpolatedStringHandler.AppendFormatted<TargetType>(designation.def.targetType);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			List<Designation> list;
			if (this.TryGetCellDesignations(designation.target.Cell, out list, true))
			{
				list.Add(designation);
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(57, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Tried to create cell target designation at invalid cell: ");
			defaultInterpolatedStringHandler2.AppendFormatted<IntVec3>(designation.target.Cell);
			Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00064104 File Offset: 0x00062304
		private void CalculateCellDesignationDrawMatricies(DesignationDef def)
		{
			if (def.targetType != TargetType.Cell || !this.designationMatriciesDirty[def])
			{
				return;
			}
			List<Designation> list = this.designationsByDef[def];
			int count = list.Count;
			while (this.cellDesignationDrawMatricies[def].Count * 1023 < count)
			{
				this.cellDesignationDrawMatricies[def].Add(new Matrix4x4[1023]);
			}
			for (int i = 0; i < count; i++)
			{
				Designation designation = list[i];
				int index = i / 1023;
				this.cellDesignationDrawMatricies[def][index][i % 1023] = Matrix4x4.TRS(designation.DrawLoc(), Quaternion.identity, Vector3.one);
			}
			this.designationMatriciesDirty[def] = false;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x000641D4 File Offset: 0x000623D4
		public Designation DesignationOn(Thing t)
		{
			List<Designation> list;
			this.thingDesignations.TryGetValue(t, out list);
			if (list.NullOrEmpty<Designation>())
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00064204 File Offset: 0x00062404
		public Designation DesignationOn(Thing t, DesignationDef def)
		{
			if (def.targetType == TargetType.Cell)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by location only and you are trying to get one on a Thing.");
				return null;
			}
			List<Designation> list;
			this.thingDesignations.TryGetValue(t, out list);
			if (list.NullOrEmpty<Designation>())
			{
				return null;
			}
			foreach (Designation designation in list)
			{
				if (designation.def == def)
				{
					return designation;
				}
			}
			return null;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0006429C File Offset: 0x0006249C
		public Designation DesignationAt(IntVec3 c, DesignationDef def)
		{
			if (def.targetType == TargetType.Thing)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by Thing only and you are trying to get one on a location.");
				return null;
			}
			List<Designation> list;
			if (this.TryGetCellDesignations(c, out list, false))
			{
				for (int i = 0; i < list.Count; i++)
				{
					Designation designation = list[i];
					if (designation.def == def)
					{
						return designation;
					}
				}
			}
			return null;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x000642FE File Offset: 0x000624FE
		public List<Designation> AllDesignationsOn(Thing t)
		{
			if (!this.thingDesignations.ContainsKey(t))
			{
				return DesignationManager.EmptyList;
			}
			return this.thingDesignations[t];
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00064320 File Offset: 0x00062520
		public List<Designation> AllDesignationsAt(IntVec3 c)
		{
			DesignationManager.tmpDesignations.Clear();
			List<Designation> collection;
			if (this.TryGetCellDesignations(c, out collection, false))
			{
				DesignationManager.tmpDesignations.AddRange(collection);
			}
			foreach (Thing t in this.map.thingGrid.ThingsListAt(c))
			{
				DesignationManager.tmpDesignations.AddRange(this.AllDesignationsOn(t));
			}
			return DesignationManager.tmpDesignations;
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x000643B0 File Offset: 0x000625B0
		public bool HasMapDesignationAt(IntVec3 c)
		{
			List<Designation> list;
			return this.TryGetCellDesignations(c, out list, false) && !list.NullOrEmpty<Designation>();
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x000643D4 File Offset: 0x000625D4
		public bool HasMapDesignationOn(Thing t)
		{
			return this.DesignationOn(t) != null;
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x000643E0 File Offset: 0x000625E0
		public IEnumerable<Designation> SpawnedDesignationsOfDef(DesignationDef def)
		{
			DesignationManager.<SpawnedDesignationsOfDef>d__26 <SpawnedDesignationsOfDef>d__ = new DesignationManager.<SpawnedDesignationsOfDef>d__26(-2);
			<SpawnedDesignationsOfDef>d__.<>4__this = this;
			<SpawnedDesignationsOfDef>d__.<>3__def = def;
			return <SpawnedDesignationsOfDef>d__;
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x000643F8 File Offset: 0x000625F8
		public bool AnySpawnedDesignationOfDef(DesignationDef def)
		{
			foreach (Designation designation in this.designationsByDef[def])
			{
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00064480 File Offset: 0x00062680
		private bool TryGetCellDesignations(IntVec3 cell, out List<Designation> foundDesignations, bool initializeIfNull = false)
		{
			int num = this.map.cellIndices.CellToIndex(cell);
			if (num < 0 || num >= this.designationsAtCell.Length)
			{
				foundDesignations = null;
				return false;
			}
			foundDesignations = this.designationsAtCell[num];
			if (foundDesignations == null)
			{
				if (initializeIfNull)
				{
					foundDesignations = new List<Designation>();
					this.designationsAtCell[num] = foundDesignations;
				}
				else
				{
					foundDesignations = DesignationManager.EmptyList;
				}
			}
			return true;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x000644E0 File Offset: 0x000626E0
		public void RemoveDesignation(Designation des)
		{
			des.Notify_Removing();
			if (des.def.targetType == TargetType.Cell)
			{
				List<Designation> list;
				if (this.TryGetCellDesignations(des.target.Cell, out list, false))
				{
					list.Remove(des);
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(78, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Tried to remove designation with target cell that couldn't be found in index: ");
					defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(des.target.Cell);
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			else if (des.def.targetType == TargetType.Thing)
			{
				Thing thing = des.target.Thing;
				if (this.thingDesignations.ContainsKey(thing))
				{
					List<Designation> list2 = this.thingDesignations[thing];
					list2.Remove(des);
					if (list2.Count == 0)
					{
						this.thingDesignations.Remove(des.target.Thing);
					}
				}
				else
				{
					Log.Warning("Tried to remove thing designation that wasn't indexed");
				}
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(50, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Tried to remove designation with unexpected type: ");
				defaultInterpolatedStringHandler2.AppendFormatted<TargetType>(des.def.targetType);
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			this.designationsByDef[des.def].Remove(des);
			this.DirtyCellDesignationsCache(des.def);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0006461C File Offset: 0x0006281C
		public void TryRemoveDesignation(IntVec3 c, DesignationDef def)
		{
			Designation designation = this.DesignationAt(c, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0006463C File Offset: 0x0006283C
		public void RemoveAllDesignationsOn(Thing t, bool standardCanceling = false)
		{
			List<Designation> list = this.AllDesignationsOn(t);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Designation designation = list[i];
				if (!standardCanceling || designation.def.designateCancelable)
				{
					this.RemoveDesignation(list[i]);
				}
			}
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0006468C File Offset: 0x0006288C
		public void TryRemoveDesignationOn(Thing t, DesignationDef def)
		{
			Designation designation = this.DesignationOn(t, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x000646AC File Offset: 0x000628AC
		public void RemoveAllDesignationsOfDef(DesignationDef def)
		{
			List<Designation> list = this.designationsByDef[def];
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.RemoveDesignation(list[i]);
			}
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000646E8 File Offset: 0x000628E8
		public void Notify_BuildingDespawned(Thing b)
		{
			CellRect cellRect = b.OccupiedRect();
			foreach (KeyValuePair<DesignationDef, List<Designation>> keyValuePair in this.designationsByDef)
			{
				if (keyValuePair.Key.removeIfBuildingDespawned)
				{
					List<Designation> value = keyValuePair.Value;
					for (int i = value.Count - 1; i >= 0; i--)
					{
						if (cellRect.Contains(value[i].target.Cell))
						{
							this.RemoveDesignation(value[i]);
						}
					}
				}
			}
		}

		// Token: 0x04000FE1 RID: 4065
		public readonly Map map;

		// Token: 0x04000FE2 RID: 4066
		public readonly DefMap<DesignationDef, List<Designation>> designationsByDef = new DefMap<DesignationDef, List<Designation>>();

		// Token: 0x04000FE3 RID: 4067
		private readonly DefMap<DesignationDef, List<Matrix4x4[]>> cellDesignationDrawMatricies = new DefMap<DesignationDef, List<Matrix4x4[]>>();

		// Token: 0x04000FE4 RID: 4068
		private readonly DefMap<DesignationDef, bool> designationMatriciesDirty = new DefMap<DesignationDef, bool>();

		// Token: 0x04000FE5 RID: 4069
		private readonly List<Designation>[] designationsAtCell;

		// Token: 0x04000FE6 RID: 4070
		private readonly Dictionary<Thing, List<Designation>> thingDesignations = new Dictionary<Thing, List<Designation>>();

		// Token: 0x04000FE7 RID: 4071
		private MaterialPropertyBlock tmpPropertyBlock;

		// Token: 0x04000FE8 RID: 4072
		private List<Designation> saveDesignations = new List<Designation>();

		// Token: 0x04000FE9 RID: 4073
		private static readonly List<Designation> EmptyList = new List<Designation>();

		// Token: 0x04000FEA RID: 4074
		private static readonly List<Designation> tmpDesignations = new List<Designation>();
	}
}
