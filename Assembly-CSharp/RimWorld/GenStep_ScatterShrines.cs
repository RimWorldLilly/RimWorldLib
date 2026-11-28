using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C9 RID: 4809
	public class GenStep_ScatterShrines : GenStep_ScatterRuinsSimple
	{
		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x06007759 RID: 30553 RVA: 0x002A19E8 File Offset: 0x0029FBE8
		public override int SeedPart
		{
			get
			{
				return 1801222485;
			}
		}

		// Token: 0x0600775A RID: 30554 RVA: 0x002A19F0 File Offset: 0x0029FBF0
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			this.randomSize.x = GenStep_ScatterShrines.SizeRange.RandomInRange;
			this.randomSize.z = GenStep_ScatterShrines.SizeRange.RandomInRange;
			return base.TryFindScatterCell(map, out result);
		}

		// Token: 0x0600775B RID: 30555 RVA: 0x002A1A35 File Offset: 0x0029FC35
		protected override bool ShouldSkipMap(Map map)
		{
			return !map.IsStartingMap && Rand.Chance(0.75f);
		}

		// Token: 0x0600775C RID: 30556 RVA: 0x002A1A50 File Offset: 0x0029FC50
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(map))
				{
					Building edifice = c2.GetEdifice(map);
					if (edifice != null && edifice.def.building.isNaturalRock)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600775D RID: 30557 RVA: 0x002A1AB1 File Offset: 0x0029FCB1
		protected override CellRect EffectiveRectAt(IntVec3 c)
		{
			return CellRect.CenteredOn(c, this.randomSize.x, this.randomSize.z);
		}

		// Token: 0x0600775E RID: 30558 RVA: 0x002A1AD0 File Offset: 0x0029FCD0
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int stackCount = 1)
		{
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			CellRect cellRect = this.EffectiveRectAt(loc);
			CellRect cellRect2 = cellRect.ClipInsideMap(map);
			if (cellRect2.Width != cellRect.Width || cellRect2.Height != cellRect.Height)
			{
				return;
			}
			foreach (IntVec3 c in cellRect.Cells)
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def == ThingDefOf.AncientCryptosleepCasket)
					{
						return;
					}
				}
			}
			if (!base.CanPlaceAncientBuildingInRange(cellRect, map))
			{
				return;
			}
			if (orGenerateVar.Contains(cellRect))
			{
				return;
			}
			orGenerateVar.Add(cellRect);
			ResolveParams resolveParams = new ResolveParams
			{
				rect = cellRect,
				disableSinglePawn = new bool?(true),
				disableHives = new bool?(true),
				makeWarningLetter = new bool?(true)
			};
			if (Find.Storyteller.difficulty.peacefulTemples)
			{
				resolveParams.podContentsType = new PodContentsType?(PodContentsType.AncientFriendly);
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientTemple", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04004AE1 RID: 19169
		private IntVec2 randomSize;

		// Token: 0x04004AE2 RID: 19170
		private static readonly IntRange SizeRange = new IntRange(15, 20);

		// Token: 0x04004AE3 RID: 19171
		private const float SkipNonStartingMapChance = 0.75f;
	}
}
