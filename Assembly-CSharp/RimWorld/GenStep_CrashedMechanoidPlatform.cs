using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020012ED RID: 4845
	public class GenStep_CrashedMechanoidPlatform : GenStep_LargeRuins
	{
		// Token: 0x17001488 RID: 5256
		// (get) Token: 0x06007837 RID: 30775 RVA: 0x002A8147 File Offset: 0x002A6347
		protected override IntRange RuinsMinMaxRange
		{
			get
			{
				return new IntRange(1, 3);
			}
		}

		// Token: 0x17001489 RID: 5257
		// (get) Token: 0x06007838 RID: 30776 RVA: 0x002A8150 File Offset: 0x002A6350
		public override int SeedPart
		{
			get
			{
				return 7937381;
			}
		}

		// Token: 0x1700148A RID: 5258
		// (get) Token: 0x06007839 RID: 30777 RVA: 0x002A8157 File Offset: 0x002A6357
		protected override int MoveRangeLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700148B RID: 5259
		// (get) Token: 0x0600783A RID: 30778 RVA: 0x002A815A File Offset: 0x002A635A
		protected override int ContractLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700148C RID: 5260
		// (get) Token: 0x0600783B RID: 30779 RVA: 0x002A815D File Offset: 0x002A635D
		protected override int MinRegionSize
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x1700148D RID: 5261
		// (get) Token: 0x0600783C RID: 30780 RVA: 0x002A8161 File Offset: 0x002A6361
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.CrashedMechanoidPlatform_Standard;
			}
		}

		// Token: 0x1700148E RID: 5262
		// (get) Token: 0x0600783D RID: 30781 RVA: 0x002A8168 File Offset: 0x002A6368
		protected override Faction Faction
		{
			get
			{
				return Faction.OfMechanoids;
			}
		}

		// Token: 0x1700148F RID: 5263
		// (get) Token: 0x0600783E RID: 30782 RVA: 0x002A816F File Offset: 0x002A636F
		protected override bool UseUsedRects
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001490 RID: 5264
		// (get) Token: 0x0600783F RID: 30783 RVA: 0x002A8172 File Offset: 0x002A6372
		protected override TerrainAffordanceDef MinAffordance
		{
			get
			{
				return TerrainAffordanceDefOf.Light;
			}
		}

		// Token: 0x06007840 RID: 30784 RVA: 0x002A8179 File Offset: 0x002A6379
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Crashed Mechanoid Platform"))
			{
				return;
			}
			this.placedReactorLayout = false;
			base.Generate(map, parms);
		}

		// Token: 0x06007841 RID: 30785 RVA: 0x002A8198 File Offset: 0x002A6398
		protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			if (!this.placedReactorLayout)
			{
				this.placedReactorLayout = true;
				layoutDef = LayoutDefOf.CrashedMechanoidPlatform_Engine;
			}
			LayoutStructureSketch layoutStructureSketch = base.GenerateAndSpawn(rect, map, parms, layoutDef);
			MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.MechanoidPlatform, ref rect, GenStep_OrbitalPlatform.LandingPadBorderLumpLengthRange, GenStep_OrbitalPlatform.LandingPadBorderLumpOffsetRange, TerrainDefOf.MechanoidPlatform);
			GenStep_CrashedMechanoidPlatform.SpawnDirtDebrisCraters(rect, map, layoutStructureSketch);
			return layoutStructureSketch;
		}

		// Token: 0x06007842 RID: 30786 RVA: 0x002A81F0 File Offset: 0x002A63F0
		private static void SpawnDirtDebrisCraters(CellRect rect, Map map, LayoutStructureSketch sketch)
		{
			foreach (IntVec3 intVec in rect.Cells)
			{
				if (!intVec.GetTerrain(map).natural)
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.MechanoidPlatform);
					if (Rand.Range(0, 16) == 0)
					{
						ScatterDebrisUtility.ScatterFilthAroundCell(intVec, new IntVec2(6, 6), map, Rand.Bool ? ThingDefOf.Filth_Ash : ThingDefOf.Filth_LooseGround, 0.5f, int.MaxValue, null);
					}
				}
			}
			foreach (IntVec3 c in rect.Cells.InRandomOrder(null).Take((int)((float)rect.Area * 0.01f)))
			{
				if (c.InBounds(map))
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.PackedDirt);
				}
			}
			CellRect container = rect.ExpandedBy(8).ClipInsideMap(map);
			foreach (IntVec3 intVec2 in rect.DifferenceCells(container).InRandomOrder(null).Take((int)((float)container.Area * 0.01f)))
			{
				if (intVec2.InBounds(map))
				{
					ScatterDebrisUtility.ScatterFilthAroundCell(intVec2, new IntVec2(6, 6), map, Rand.Bool ? ThingDefOf.Filth_Ash : ThingDefOf.Filth_LooseGround, 0.5f, int.MaxValue, null);
				}
			}
			foreach (IntVec3 loc in rect.ExpandedBy(4).ClipInsideMap(map).DifferenceCells(container).InRandomOrder(null).Take((int)((float)container.Area * 0.01f)))
			{
				int num = Rand.Range(0, 3);
				ThingDef def;
				if (num == 0)
				{
					def = ThingDefOf.CraterSmall;
				}
				else if (num == 1)
				{
					def = ThingDefOf.CraterMedium;
				}
				else
				{
					def = ThingDefOf.CraterLarge;
				}
				Thing thing;
				GenSpawn.TrySpawn(def, loc, map, out thing, WipeMode.Vanish, false);
			}
		}

		// Token: 0x04004B5D RID: 19293
		private bool placedReactorLayout;

		// Token: 0x04004B5E RID: 19294
		private const float RuinAreaDirtFactor = 0.01f;

		// Token: 0x04004B5F RID: 19295
		private const float RuinAreaFilthFactor = 0.01f;

		// Token: 0x04004B60 RID: 19296
		private const float AreaCraterFactor = 0.01f;
	}
}
