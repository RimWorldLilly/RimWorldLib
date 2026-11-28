using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020012DF RID: 4831
	public class GenStep_FrozenRuins : GenStep_BaseRuins
	{
		// Token: 0x17001461 RID: 5217
		// (get) Token: 0x060077D9 RID: 30681 RVA: 0x002A68CD File Offset: 0x002A4ACD
		public override int SeedPart
		{
			get
			{
				return 964521;
			}
		}

		// Token: 0x17001462 RID: 5218
		// (get) Token: 0x060077DA RID: 30682 RVA: 0x002A68D4 File Offset: 0x002A4AD4
		protected override int RegionSize
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x17001463 RID: 5219
		// (get) Token: 0x060077DB RID: 30683 RVA: 0x002A68D8 File Offset: 0x002A4AD8
		protected override FloatRange DefaultMapFillPercentRange
		{
			get
			{
				return new FloatRange(0.01f, 0.05f);
			}
		}

		// Token: 0x17001464 RID: 5220
		// (get) Token: 0x060077DC RID: 30684 RVA: 0x002A68E9 File Offset: 0x002A4AE9
		protected override FloatRange MergeRange
		{
			get
			{
				return new FloatRange(0.05f, 0.15f);
			}
		}

		// Token: 0x17001465 RID: 5221
		// (get) Token: 0x060077DD RID: 30685 RVA: 0x002A68FA File Offset: 0x002A4AFA
		protected override int MoveRangeLimit
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17001466 RID: 5222
		// (get) Token: 0x060077DE RID: 30686 RVA: 0x002A68FD File Offset: 0x002A4AFD
		protected override int ContractLimit
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17001467 RID: 5223
		// (get) Token: 0x060077DF RID: 30687 RVA: 0x002A6900 File Offset: 0x002A4B00
		protected override int MinRegionSize
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x17001468 RID: 5224
		// (get) Token: 0x060077E0 RID: 30688 RVA: 0x002A6904 File Offset: 0x002A4B04
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientRuinsGlacier;
			}
		}

		// Token: 0x17001469 RID: 5225
		// (get) Token: 0x060077E1 RID: 30689 RVA: 0x002A690B File Offset: 0x002A4B0B
		protected override Faction Faction
		{
			get
			{
				return Faction.OfAncientsHostile;
			}
		}

		// Token: 0x060077E2 RID: 30690 RVA: 0x002A6912 File Offset: 0x002A4B12
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Frozen Ruins"))
			{
				return;
			}
			base.Generate(map, parms);
		}

		// Token: 0x060077E3 RID: 30691 RVA: 0x002A692C File Offset: 0x002A4B2C
		public override void GenerateRuins(Map map, GenStepParams parms, FloatRange mapFillPercentRange)
		{
			base.GenerateRuins(map, parms, mapFillPercentRange);
			GenStep_FrozenRuins.EncaseStructuresInIce(map, this.structureSketches, null);
		}

		// Token: 0x060077E4 RID: 30692 RVA: 0x002A6958 File Offset: 0x002A4B58
		public static void EncaseStructuresInIce(Map map, List<LayoutStructureSketch> structureSketches, GenMorphology.GenPatchParms? patchParms = null)
		{
			GenStep_FrozenRuins.<>c__DisplayClass21_0 CS$<>8__locals1 = new GenStep_FrozenRuins.<>c__DisplayClass21_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.structureSketches = structureSketches;
			foreach (LayoutStructureSketch layoutStructureSketch in CS$<>8__locals1.structureSketches)
			{
				if (layoutStructureSketch.structureLayout != null)
				{
					List<CellRect> rects = (from r in layoutStructureSketch.structureLayout.Rooms.SelectMany((LayoutRoom room) => room.rects)
					select r.ExpandedBy(24)).ToList<CellRect>();
					GenMorphology.GenPatchParms value = patchParms.GetValueOrDefault();
					if (patchParms == null)
					{
						value = GenMorphology.GenPatchParms.For(ThingDefOf.SolidIce, TerrainDefOf.Ice);
						patchParms = new GenMorphology.GenPatchParms?(value);
					}
					GenMorphology.GenerateNaturalPatch(CS$<>8__locals1.map, rects, patchParms, new Predicate<IntVec3>(CS$<>8__locals1.<EncaseStructuresInIce>g__Validator|2));
					foreach (LayoutRoom layoutRoom in layoutStructureSketch.structureLayout.Rooms)
					{
						foreach (IntVec3 c in layoutRoom.Cells)
						{
							if (CS$<>8__locals1.map.roofGrid.RoofAt(c) == null)
							{
								CS$<>8__locals1.map.roofGrid.SetRoof(c, RoofDefOf.RoofRockThin);
							}
						}
					}
				}
			}
		}

		// Token: 0x060077E5 RID: 30693 RVA: 0x002A6B30 File Offset: 0x002A4D30
		protected override bool IsValidRect(CellRect rect, Map map)
		{
			if (!base.IsValidRect(rect, map))
			{
				return false;
			}
			if (map.TileInfo.Mutators.Contains(TileMutatorDefOf.Crevasse))
			{
				foreach (IntVec3 c in rect.Cells)
				{
					Building edifice = c.GetEdifice(map);
					if (((edifice != null) ? edifice.def : null) != ThingDefOf.SolidIce)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x04004B41 RID: 19265
		private const int CellRange = 24;
	}
}
