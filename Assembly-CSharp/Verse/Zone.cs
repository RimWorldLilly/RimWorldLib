using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200032C RID: 812
	public abstract class Zone : IExposable, ISelectable, ILoadReferenceable, IRenameable, IHideable
	{
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060018D7 RID: 6359 RVA: 0x00094E4F File Offset: 0x0009304F
		// (set) Token: 0x060018D8 RID: 6360 RVA: 0x00094E61 File Offset: 0x00093061
		public string RenamableLabel
		{
			get
			{
				return this.label ?? this.baseLabel;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060018D9 RID: 6361 RVA: 0x00094E6A File Offset: 0x0009306A
		public string BaseLabel
		{
			get
			{
				return this.baseLabel;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060018DA RID: 6362 RVA: 0x00094E72 File Offset: 0x00093072
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x00094E7A File Offset: 0x0009307A
		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060018DC RID: 6364 RVA: 0x00094E87 File Offset: 0x00093087
		public IntVec3 Position
		{
			get
			{
				if (this.cells.Count == 0)
				{
					return IntVec3.Invalid;
				}
				return this.cells[0];
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060018DD RID: 6365 RVA: 0x00094EA8 File Offset: 0x000930A8
		// (set) Token: 0x060018DE RID: 6366 RVA: 0x00094EB0 File Offset: 0x000930B0
		public bool Hidden
		{
			get
			{
				return this.hidden;
			}
			set
			{
				this.hidden = value;
				foreach (IntVec3 loc in this.Cells)
				{
					this.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.Zone);
				}
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x00094F20 File Offset: 0x00093120
		public Material Material
		{
			get
			{
				if (this.materialInt == null)
				{
					this.materialInt = this.CreateMaterial();
				}
				return this.materialInt;
			}
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x00094F3C File Offset: 0x0009313C
		protected virtual Material CreateMaterial()
		{
			Material material = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
			material.renderQueue = 3600;
			return material;
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x00094F55 File Offset: 0x00093155
		public IEnumerator<IntVec3> GetEnumerator()
		{
			Zone.<GetEnumerator>d__29 <GetEnumerator>d__ = new Zone.<GetEnumerator>d__29(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060018E2 RID: 6370 RVA: 0x00094F64 File Offset: 0x00093164
		public List<IntVec3> Cells
		{
			get
			{
				if (!this.cellsShuffled)
				{
					this.cells.Shuffle<IntVec3>();
					this.cellsShuffled = true;
				}
				return this.cells;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060018E3 RID: 6371 RVA: 0x00094F86 File Offset: 0x00093186
		public int CellCount
		{
			get
			{
				return this.cells.Count;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060018E4 RID: 6372 RVA: 0x00094F94 File Offset: 0x00093194
		public int HeldThingsCount
		{
			get
			{
				int num = 0;
				ThingGrid thingGrid = this.Map.thingGrid;
				for (int i = 0; i < this.cells.Count; i++)
				{
					List<Thing> list = thingGrid.ThingsListAt(this.cells[i]);
					num += list.Count;
				}
				return num;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x00094FE2 File Offset: 0x000931E2
		public IEnumerable<Thing> AllContainedThings
		{
			get
			{
				Zone.<get_AllContainedThings>d__37 <get_AllContainedThings>d__ = new Zone.<get_AllContainedThings>d__37(-2);
				<get_AllContainedThings>d__.<>4__this = this;
				return <get_AllContainedThings>d__;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x00094FF4 File Offset: 0x000931F4
		public bool ContainsStaticFire
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastStaticFireCheckTick + 1000)
				{
					this.lastStaticFireCheckResult = false;
					for (int i = 0; i < this.cells.Count; i++)
					{
						if (this.cells[i].ContainsStaticFire(this.Map))
						{
							this.lastStaticFireCheckResult = true;
							break;
						}
					}
				}
				return this.lastStaticFireCheckResult;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x060018E7 RID: 6375 RVA: 0x0009505E File Offset: 0x0009325E
		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060018E8 RID: 6376
		protected abstract Color NextZoneColor { get; }

		// Token: 0x060018E9 RID: 6377 RVA: 0x00095061 File Offset: 0x00093261
		public Zone()
		{
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x00095094 File Offset: 0x00093294
		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.baseLabel = baseName;
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.ID = Find.UniqueIDsManager.GetNextZoneID();
			this.color = this.NextZoneColor;
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x00095108 File Offset: 0x00093308
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.baseLabel, "baseLabel", null, false);
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Undefined, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CheckAddHaulDestination();
			}
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x000951A4 File Offset: 0x000933A4
		public virtual void AddCell(IntVec3 c)
		{
			if (this.cells.Contains(c))
			{
				string str = "Adding cell to zone which already has it. c=";
				IntVec3 intVec = c;
				Log.Error(str + intVec.ToString() + ", zone=" + ((this != null) ? this.ToString() : null));
				return;
			}
			List<Thing> list = this.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (!thing.def.CanOverlapZones)
				{
					string str2 = "Added zone over zone-incompatible thing ";
					Thing thing2 = thing;
					Log.Error(str2 + ((thing2 != null) ? thing2.ToString() : null));
					return;
				}
			}
			this.cells.Add(c);
			this.zoneManager.AddZoneGridCell(this, c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Zone);
			AutoHomeAreaMaker.Notify_ZoneCellAdded(c, this);
			this.cellsShuffled = false;
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00095288 File Offset: 0x00093488
		public virtual void RemoveCell(IntVec3 c)
		{
			if (!this.cells.Contains(c))
			{
				string str = "Cannot remove cell from zone which doesn't have it. c=";
				IntVec3 intVec = c;
				Log.Error(str + intVec.ToString() + ", zone=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.cells.Remove(c);
			this.zoneManager.ClearZoneGridCell(c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Zone);
			this.cellsShuffled = false;
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00095322 File Offset: 0x00093522
		public void Delete()
		{
			this.Delete(true);
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x0009532C File Offset: 0x0009352C
		public virtual void Delete(bool playSound)
		{
			if (playSound)
			{
				SoundDefOf.Designate_ZoneDelete.PlayOneShotOnCamera(this.Map);
			}
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
			else
			{
				while (this.cells.Count > 0)
				{
					this.RemoveCell(this.cells[this.cells.Count - 1]);
				}
			}
			Find.Selector.Deselect(this);
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00095398 File Offset: 0x00093598
		public void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x000953A6 File Offset: 0x000935A6
		public virtual void PostRegister()
		{
			this.CheckAddHaulDestination();
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x000953B0 File Offset: 0x000935B0
		public virtual void PostDeregister()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x000953D8 File Offset: 0x000935D8
		public bool ContainsCell(IntVec3 c)
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				if (this.cells[i] == c)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x00095414 File Offset: 0x00093614
		public virtual string GetInspectString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Size".Translate().CapitalizeFirst());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.CellCount);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00095463 File Offset: 0x00093663
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return null;
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x00095466 File Offset: 0x00093666
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			Zone.<GetGizmos>d__57 <GetGizmos>d__ = new Zone.<GetGizmos>d__57(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x00095476 File Offset: 0x00093676
		public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x00095480 File Offset: 0x00093680
		public void CheckContiguous()
		{
			if (this.cells.Count == 0)
			{
				return;
			}
			if (Zone.extantGrid == null)
			{
				Zone.extantGrid = new BoolGrid(this.Map);
			}
			else
			{
				Zone.extantGrid.ClearAndResizeTo(this.Map);
			}
			if (Zone.foundGrid == null)
			{
				Zone.foundGrid = new BoolGrid(this.Map);
			}
			else
			{
				Zone.foundGrid.ClearAndResizeTo(this.Map);
			}
			for (int i = 0; i < this.cells.Count; i++)
			{
				Zone.extantGrid.Set(this.cells[i], true);
			}
			Predicate<IntVec3> passCheck = (IntVec3 c) => Zone.extantGrid[c] && !Zone.foundGrid[c];
			int numFound = 0;
			Action<IntVec3> processor = delegate(IntVec3 c)
			{
				Zone.foundGrid.Set(c, true);
				int numFound = numFound;
				numFound++;
			};
			this.Map.floodFiller.FloodFill(this.cells[0], passCheck, processor, int.MaxValue, false, null);
			if (numFound < this.cells.Count)
			{
				foreach (IntVec3 c2 in this.Map.AllCells)
				{
					if (Zone.extantGrid[c2] && !Zone.foundGrid[c2])
					{
						this.RemoveCell(c2);
					}
				}
			}
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x000955F4 File Offset: 0x000937F4
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			return new Zone.<GetFloatMenuOptions>d__62(-2);
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x00095600 File Offset: 0x00093800
		private void CheckAddHaulDestination()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x00095628 File Offset: 0x00093828
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x00095630 File Offset: 0x00093830
		public string GetUniqueLoadID()
		{
			return "Zone_" + this.ID.ToString();
		}

		// Token: 0x0400149B RID: 5275
		public ZoneManager zoneManager;

		// Token: 0x0400149C RID: 5276
		public int ID = -1;

		// Token: 0x0400149D RID: 5277
		public string label;

		// Token: 0x0400149E RID: 5278
		private string baseLabel;

		// Token: 0x0400149F RID: 5279
		public List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x040014A0 RID: 5280
		private bool cellsShuffled;

		// Token: 0x040014A1 RID: 5281
		public Color color = Color.white;

		// Token: 0x040014A2 RID: 5282
		private Material materialInt;

		// Token: 0x040014A3 RID: 5283
		private bool hidden;

		// Token: 0x040014A4 RID: 5284
		private int lastStaticFireCheckTick = -9999;

		// Token: 0x040014A5 RID: 5285
		private bool lastStaticFireCheckResult;

		// Token: 0x040014A6 RID: 5286
		private const int StaticFireCheckInterval = 1000;

		// Token: 0x040014A7 RID: 5287
		private static BoolGrid extantGrid;

		// Token: 0x040014A8 RID: 5288
		private static BoolGrid foundGrid;
	}
}
