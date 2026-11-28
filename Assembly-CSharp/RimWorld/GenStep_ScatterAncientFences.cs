using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BD RID: 4797
	public class GenStep_ScatterAncientFences : GenStep_Scatterer
	{
		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x06007708 RID: 30472 RVA: 0x0029F8FF File Offset: 0x0029DAFF
		public override int SeedPart
		{
			get
			{
				return 344678634;
			}
		}

		// Token: 0x06007709 RID: 30473 RVA: 0x0029F908 File Offset: 0x0029DB08
		protected override float GetPlacementFactor(Map map)
		{
			float num = 1f;
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				num *= tileMutatorDef.junkDensityFactor;
			}
			return num;
		}

		// Token: 0x0600770A RID: 30474 RVA: 0x0029F964 File Offset: 0x0029DB64
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient fences"))
			{
				return;
			}
			this.count = Rand.RangeInclusive(1, 3);
			GenStep_ScatterAncientFences.tmpUsedRects.Clear();
			base.Generate(map, parms);
			GenStep_ScatterAncientFences.tmpUsedRects.Clear();
			GenStep_ScatterAncientFences.tmpFenceCells.Clear();
		}

		// Token: 0x0600770B RID: 30475 RVA: 0x0029F9B4 File Offset: 0x0029DBB4
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			this.thingToScatter = (Rand.Bool ? ThingDefOf.AncientFence : ThingDefOf.AncientRazorWire);
			int randomInRange = GenStep_ScatterAncientFences.RectSizeRange.RandomInRange;
			CellRect item = CellRect.CenteredOn(loc, randomInRange, randomInRange);
			for (int i = 0; i < GenStep_ScatterAncientFences.tmpUsedRects.Count; i++)
			{
				if (item.Overlaps(GenStep_ScatterAncientFences.tmpUsedRects[i]))
				{
					return false;
				}
			}
			GenStep_ScatterAncientFences.tmpFenceCells.Clear();
			GenStep_ScatterAncientFences.tmpFenceCells.AddRange(item.EdgeCells);
			for (int j = 0; j < GenStep_ScatterAncientFences.tmpFenceCells.Count; j++)
			{
				if (!GenStep_ScatterAncientFences.tmpFenceCells[j].InBounds(map) || GenStep_ScatterAncientFences.tmpFenceCells[j].GetEdifice(map) != null || GenStep_ScatterAncientFences.tmpFenceCells[j].GetRoof(map) != null || GenStep_ScatterAncientFences.tmpFenceCells[j].Impassable(map))
				{
					return false;
				}
				TerrainDef terrain = GenStep_ScatterAncientFences.tmpFenceCells[j].GetTerrain(map);
				if (terrain.IsWater || terrain.IsRoad || !GenConstruct.CanBuildOnTerrain(this.thingToScatter, GenStep_ScatterAncientFences.tmpFenceCells[j], map, Rot4.North, null, null))
				{
					return false;
				}
			}
			GenStep_ScatterAncientFences.tmpUsedRects.Add(item);
			return true;
		}

		// Token: 0x0600770C RID: 30476 RVA: 0x0029FB0C File Offset: 0x0029DD0C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			int num = Rand.Range(0, GenStep_ScatterAncientFences.tmpFenceCells.Count);
			int num2 = Mathf.RoundToInt((float)GenStep_ScatterAncientFences.tmpFenceCells.Count * GenStep_ScatterAncientFences.PerimeterCellsRange.RandomInRange);
			for (int i = 0; i < num2; i++)
			{
				if (!Rand.Chance(0.25f))
				{
					IntVec3 loc2 = GenStep_ScatterAncientFences.tmpFenceCells[(i + num) % GenStep_ScatterAncientFences.tmpFenceCells.Count];
					GenSpawn.Spawn(ThingMaker.MakeThing(this.thingToScatter, null), loc2, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x04004AB4 RID: 19124
		private static readonly FloatRange PerimeterCellsRange = new FloatRange(0.2f, 0.4f);

		// Token: 0x04004AB5 RID: 19125
		private static readonly IntRange RectSizeRange = new IntRange(6, 12);

		// Token: 0x04004AB6 RID: 19126
		private const float SkipChance = 0.25f;

		// Token: 0x04004AB7 RID: 19127
		private CellRect rect;

		// Token: 0x04004AB8 RID: 19128
		private ThingDef thingToScatter;

		// Token: 0x04004AB9 RID: 19129
		private static readonly List<IntVec3> tmpFenceCells = new List<IntVec3>();

		// Token: 0x04004ABA RID: 19130
		private static readonly List<CellRect> tmpUsedRects = new List<CellRect>();
	}
}
