using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using RimWorld;
using Verse.Utility;

namespace Verse
{
	// Token: 0x020002D2 RID: 722
	public class GenStep_ScatterGroupPrefabs : GenStep_Scatterer
	{
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001612 RID: 5650 RVA: 0x00083B88 File Offset: 0x00081D88
		public override int SeedPart
		{
			get
			{
				return 79797732;
			}
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00083B90 File Offset: 0x00081D90
		public override void Generate(Map map, GenStepParams parms)
		{
			try
			{
				base.Generate(map, parms);
			}
			finally
			{
				this.dontChooseIndoor = false;
				GenStep_ScatterGroupPrefabs.tmpSpawns.Clear();
			}
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00083BCC File Offset: 0x00081DCC
		private GenStep_ScatterGroupPrefabs.ScatterGroup GetGroup()
		{
			GenStep_ScatterGroupPrefabs.tmpScatterGroups.Clear();
			using (List<GenStep_ScatterGroupPrefabs.ScatterGroup>.Enumerator enumerator = this.groups.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenStep_ScatterGroupPrefabs.ScatterGroup group = enumerator.Current;
					if ((!this.dontChooseIndoor || !group.indoorRuin) && (group.max <= 0 || GenStep_ScatterGroupPrefabs.tmpSpawns.Count((GenStep_ScatterGroupPrefabs.PrefabSpawn spawn) => spawn.def == group.prefab) < group.max))
					{
						GenStep_ScatterGroupPrefabs.tmpScatterGroups.Add(group);
					}
				}
			}
			if (GenStep_ScatterGroupPrefabs.tmpScatterGroups.Empty<GenStep_ScatterGroupPrefabs.ScatterGroup>())
			{
				return null;
			}
			Rand.PushState(Gen.HashCombineInt(Find.World.info.Seed, this.usedSpots.Count));
			GenStep_ScatterGroupPrefabs.ScatterGroup result;
			try
			{
				result = GenStep_ScatterGroupPrefabs.tmpScatterGroups.RandomElementByWeight((GenStep_ScatterGroupPrefabs.ScatterGroup g) => g.weight);
			}
			finally
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00083CF4 File Offset: 0x00081EF4
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			bool warnOnFail = this.warnOnFail;
			this.warnOnFail = false;
			try
			{
				this.dontChooseIndoor = false;
				GenStep_ScatterGroupPrefabs.ScatterGroup group = this.GetGroup();
				if (group == null)
				{
					result = default(IntVec3);
					return false;
				}
				if (group.indoorRuin)
				{
					if (base.TryFindScatterCell(map, out result))
					{
						return true;
					}
					this.dontChooseIndoor = true;
				}
				if (this.GetGroup() != null && base.TryFindScatterCell(map, out result))
				{
					return true;
				}
			}
			finally
			{
				this.warnOnFail = warnOnFail;
			}
			if (this.warnOnFail)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Scatterer ");
				defaultInterpolatedStringHandler.AppendFormatted(this.ToString());
				defaultInterpolatedStringHandler.AppendLiteral(" from def ");
				GenStepDef def = this.def;
				defaultInterpolatedStringHandler.AppendFormatted((def != null) ? def.defName : null);
				defaultInterpolatedStringHandler.AppendLiteral(" could not find cell to generate at.");
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			result = default(IntVec3);
			return false;
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00083DF0 File Offset: 0x00081FF0
		private bool CalculateScatterInformation(IntVec3 loc, Map map, out GenStep_ScatterGroupPrefabs.PrefabSpawn details)
		{
			GenStep_ScatterGroupPrefabs.<>c__DisplayClass14_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.loc = loc;
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			GenStep_ScatterGroupPrefabs.tmpScatterGroups.Clear();
			GenStep_ScatterGroupPrefabs.ScatterGroup group = this.GetGroup();
			if (group == null)
			{
				details = default(GenStep_ScatterGroupPrefabs.PrefabSpawn);
				return false;
			}
			bool flag = false;
			foreach (GenStep_ScatterGroupPrefabs.ScatterGroup scatterGroup in GenStep_ScatterGroupPrefabs.tmpScatterGroups)
			{
				GenStep_ScatterGroupPrefabs.<CalculateScatterInformation>g__WritePossibleRotations|14_1(scatterGroup.prefab);
				foreach (Rot4 rot in GenStep_ScatterGroupPrefabs.tmpPossibleRotations)
				{
					if (this.<CalculateScatterInformation>g__CanSpawn|14_0(scatterGroup.prefab, CS$<>8__locals1.loc, rot, ref CS$<>8__locals1))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			GenStep_ScatterGroupPrefabs.tmpWeightedPrefabsRandom.Clear();
			foreach (GenStep_ScatterGroupPrefabs.ScatterGroup scatterGroup2 in GenStep_ScatterGroupPrefabs.tmpScatterGroups)
			{
				GenStep_ScatterGroupPrefabs.tmpWeightedPrefabsRandom.Add(scatterGroup2, scatterGroup2.weight * Rand.Value);
			}
			GenStep_ScatterGroupPrefabs.tmpScatterGroups.SortByDescending((GenStep_ScatterGroupPrefabs.ScatterGroup e) => GenStep_ScatterGroupPrefabs.tmpWeightedPrefabsRandom[e]);
			CellRect cellRect = CS$<>8__locals1.loc.RectAbout(new IntVec2(20, 20)).ClipInsideMap(CS$<>8__locals1.map);
			using (List<GenStep_ScatterGroupPrefabs.ScatterGroup>.Enumerator enumerator = GenStep_ScatterGroupPrefabs.tmpScatterGroups.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenStep_ScatterGroupPrefabs.ScatterGroup t = enumerator.Current;
					if (t.max <= 0 || GenStep_ScatterGroupPrefabs.tmpSpawns.Count((GenStep_ScatterGroupPrefabs.PrefabSpawn spawn) => spawn.def == t.prefab) < t.max)
					{
						GenStep_ScatterGroupPrefabs.<CalculateScatterInformation>g__WritePossibleRotations|14_1(t.prefab);
						foreach (IntVec3 intVec in cellRect.Cells.InRandomOrder(GenStep_ScatterGroupPrefabs.tmpCellsRandomOrderWorkingList))
						{
							foreach (Rot4 rot2 in GenStep_ScatterGroupPrefabs.tmpPossibleRotations)
							{
								CellRect occupiedRect = GenAdj.OccupiedRect(intVec, rot2, t.prefab.size);
								if (this.<CalculateScatterInformation>g__CanSpawn|14_0(t.prefab, intVec, rot2, ref CS$<>8__locals1))
								{
									details = new GenStep_ScatterGroupPrefabs.PrefabSpawn
									{
										def = t.prefab,
										occupiedRect = occupiedRect,
										pos = intVec,
										rotation = rot2,
										sourceGroup = group
									};
									return true;
								}
							}
						}
					}
				}
			}
			details = default(GenStep_ScatterGroupPrefabs.PrefabSpawn);
			return false;
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00084194 File Offset: 0x00082394
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			using (Rand.Block(map.NextGenSeed))
			{
				GenStep_ScatterGroupPrefabs.PrefabSpawn prefabSpawn;
				if (!this.CalculateScatterInformation(loc, map, out prefabSpawn) && this.warnOnFail)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Tried scattering group from def ");
					defaultInterpolatedStringHandler.AppendFormatted(this.def.defName);
					defaultInterpolatedStringHandler.AppendLiteral(" at ");
					defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(loc);
					defaultInterpolatedStringHandler.AppendLiteral(" on map ");
					defaultInterpolatedStringHandler.AppendFormatted<Map>(map);
					defaultInterpolatedStringHandler.AppendLiteral(" which is an invalid location!");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					if (PrefabUtility.CanSpawnPrefab(prefabSpawn.def, map, prefabSpawn.pos, prefabSpawn.rotation, true, 0))
					{
						PrefabUtility.SpawnPrefab(prefabSpawn.def, map, prefabSpawn.pos, prefabSpawn.rotation, null, null, null, null, false);
						GenStep_ScatterGroupPrefabs.tmpSpawns.Add(prefabSpawn);
					}
					if (prefabSpawn.sourceGroup.filthDef != null)
					{
						foreach (IntVec3 c in prefabSpawn.occupiedRect.ExpandedBy(prefabSpawn.sourceGroup.filthExpandBy))
						{
							if (Rand.Chance(prefabSpawn.sourceGroup.filthChance) && c.InBounds(map))
							{
								FilthMaker.TryMakeFilth(c, map, prefabSpawn.sourceGroup.filthDef, 1, FilthSourceFlags.None, true);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x00084340 File Offset: 0x00082540
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			bool result;
			using (Rand.Block(map.NextGenSeed))
			{
				GenStep_ScatterGroupPrefabs.PrefabSpawn prefabSpawn;
				result = this.CalculateScatterInformation(loc, map, out prefabSpawn);
			}
			return result;
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x000843D0 File Offset: 0x000825D0
		[CompilerGenerated]
		private bool <CalculateScatterInformation>g__CanSpawn|14_0(PrefabDef prefab, IntVec3 cell, Rot4 rot, ref GenStep_ScatterGroupPrefabs.<>c__DisplayClass14_0 A_4)
		{
			CellRect other = GenAdj.OccupiedRect(cell, rot, prefab.size);
			if (!other.InBounds(A_4.map))
			{
				return false;
			}
			foreach (GenStep_ScatterGroupPrefabs.PrefabSpawn prefabSpawn in GenStep_ScatterGroupPrefabs.tmpSpawns)
			{
				CellRect occupiedRect = prefabSpawn.occupiedRect;
				if (occupiedRect.Overlaps(other))
				{
					return false;
				}
			}
			foreach (CellRect cellRect in A_4.usedRects)
			{
				if (cellRect.Overlaps(other))
				{
					return false;
				}
			}
			foreach (IntVec3 c in other)
			{
				if (!c.GetAffordances(A_4.map).Contains(TerrainAffordanceDefOf.Heavy))
				{
					return false;
				}
				if (this.useFallback)
				{
					if (this.fallbackValidators != null)
					{
						for (int i = 0; i < this.fallbackValidators.Count; i++)
						{
							if (!this.fallbackValidators[i].Allows(A_4.loc, A_4.map))
							{
								return false;
							}
						}
					}
				}
				else if (this.validators != null)
				{
					for (int j = 0; j < this.validators.Count; j++)
					{
						if (!this.validators[j].Allows(A_4.loc, A_4.map))
						{
							return false;
						}
					}
				}
				foreach (Thing thing in c.GetThingList(A_4.map))
				{
					if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Pawn || thing.def.IsEdifice() || !thing.def.destroyable)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x00084664 File Offset: 0x00082864
		[CompilerGenerated]
		internal static void <CalculateScatterInformation>g__WritePossibleRotations|14_1(PrefabDef prefab)
		{
			GenStep_ScatterGroupPrefabs.tmpPossibleRotations.Clear();
			foreach (Enum @enum in prefab.rotations.GetBitFlags())
			{
				RotEnum rot = (RotEnum)@enum;
				GenStep_ScatterGroupPrefabs.tmpPossibleRotations.Add(rot);
			}
			GenStep_ScatterGroupPrefabs.tmpPossibleRotations.Shuffle<Rot4>();
		}

		// Token: 0x04001314 RID: 4884
		public List<GenStep_ScatterGroupPrefabs.ScatterGroup> groups;

		// Token: 0x04001315 RID: 4885
		[Unsaved(false)]
		private bool dontChooseIndoor;

		// Token: 0x04001316 RID: 4886
		private static readonly List<GenStep_ScatterGroupPrefabs.PrefabSpawn> tmpSpawns = new List<GenStep_ScatterGroupPrefabs.PrefabSpawn>();

		// Token: 0x04001317 RID: 4887
		private static readonly List<GenStep_ScatterGroupPrefabs.ScatterGroup> tmpScatterGroups = new List<GenStep_ScatterGroupPrefabs.ScatterGroup>();

		// Token: 0x04001318 RID: 4888
		private static readonly Dictionary<GenStep_ScatterGroupPrefabs.ScatterGroup, float> tmpWeightedPrefabsRandom = new Dictionary<GenStep_ScatterGroupPrefabs.ScatterGroup, float>();

		// Token: 0x04001319 RID: 4889
		private static readonly List<IntVec3> tmpCellsRandomOrderWorkingList = new List<IntVec3>();

		// Token: 0x0400131A RID: 4890
		private static readonly List<Rot4> tmpPossibleRotations = new List<Rot4>();

		// Token: 0x02002609 RID: 9737
		private struct PrefabSpawn
		{
			// Token: 0x04009C05 RID: 39941
			public IntVec3 pos;

			// Token: 0x04009C06 RID: 39942
			public Rot4 rotation;

			// Token: 0x04009C07 RID: 39943
			public CellRect occupiedRect;

			// Token: 0x04009C08 RID: 39944
			public PrefabDef def;

			// Token: 0x04009C09 RID: 39945
			public GenStep_ScatterGroupPrefabs.ScatterGroup sourceGroup;
		}

		// Token: 0x0200260A RID: 9738
		public class ScatterGroup
		{
			// Token: 0x0600F1D9 RID: 61913 RVA: 0x0058C773 File Offset: 0x0058A973
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "prefab", "weight");
			}

			// Token: 0x04009C0A RID: 39946
			public PrefabDef prefab;

			// Token: 0x04009C0B RID: 39947
			public float weight = 1f;

			// Token: 0x04009C0C RID: 39948
			public int max = -1;

			// Token: 0x04009C0D RID: 39949
			public bool indoorRuin;

			// Token: 0x04009C0E RID: 39950
			public ThingDef filthDef;

			// Token: 0x04009C0F RID: 39951
			public int filthExpandBy;

			// Token: 0x04009C10 RID: 39952
			public float filthChance = 0.5f;
		}
	}
}
