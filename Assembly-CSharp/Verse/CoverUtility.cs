using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000726 RID: 1830
	public static class CoverUtility
	{
		// Token: 0x060039D1 RID: 14801 RVA: 0x001702F8 File Offset: 0x0016E4F8
		public static List<CoverInfo> CalculateCoverGiverSet(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			List<CoverInfo> list = new List<CoverInfo>();
			if (target.HasThing && !target.Thing.def.CanBenefitFromCover)
			{
				return list;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				CoverInfo item;
				if (intVec.InBounds(map) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out item) && item.BlockChance > 0f)
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x0017037C File Offset: 0x0016E57C
		public static float CalculateOverallBlockChance(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			float num = 0f;
			if (target.HasThing && !target.Thing.def.CanBenefitFromCover)
			{
				return num;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				CoverInfo coverInfo;
				if (intVec.InBounds(map) && !(shooterLoc == intVec) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out coverInfo))
				{
					num += (1f - num) * coverInfo.BlockChance;
				}
			}
			return num;
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x00170408 File Offset: 0x0016E608
		private static bool TryFindAdjustedCoverInCell(IntVec3 shooterLoc, LocalTargetInfo target, IntVec3 adjCell, Map map, out CoverInfo result)
		{
			IntVec3 cell = target.Cell;
			Thing cover = adjCell.GetCover(map);
			if (cover == null || cover == target.Thing || shooterLoc == cell)
			{
				result = CoverInfo.Invalid;
				return false;
			}
			float angleFlat = (shooterLoc - cell).AngleFlat;
			float num = GenGeo.AngleDifferenceBetween((adjCell - cell).AngleFlat, angleFlat);
			if (!cell.AdjacentToCardinal(adjCell))
			{
				num *= 1.75f;
			}
			float num2 = cover.BaseBlockChance();
			if (num < 15f)
			{
				num2 *= 1f;
			}
			else if (num < 27f)
			{
				num2 *= 0.8f;
			}
			else if (num < 40f)
			{
				num2 *= 0.6f;
			}
			else if (num < 52f)
			{
				num2 *= 0.4f;
			}
			else
			{
				if (num >= 65f)
				{
					result = CoverInfo.Invalid;
					return false;
				}
				num2 *= 0.2f;
			}
			float lengthHorizontal = (shooterLoc - adjCell).LengthHorizontal;
			if (lengthHorizontal < 1.9f)
			{
				num2 *= 0.3333f;
			}
			else if (lengthHorizontal < 2.9f)
			{
				num2 *= 0.66666f;
			}
			result = new CoverInfo(cover, num2);
			return true;
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x00170547 File Offset: 0x0016E747
		public static float BaseBlockChance(this ThingDef def)
		{
			if (def.Fillage == FillCategory.Full)
			{
				return 0.75f;
			}
			return def.fillPercent;
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x00170560 File Offset: 0x0016E760
		public static float BaseBlockChance(this Thing thing)
		{
			Building_Door building_Door = thing as Building_Door;
			if (building_Door != null && building_Door.Open)
			{
				return 0f;
			}
			return thing.def.BaseBlockChance();
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x00170590 File Offset: 0x0016E790
		public static float TotalSurroundingCoverScore(IntVec3 c, Map map)
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(map))
				{
					Thing cover = c2.GetCover(map);
					if (cover != null)
					{
						num += cover.BaseBlockChance();
					}
				}
			}
			return num;
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x001705E0 File Offset: 0x0016E7E0
		public static bool ThingCovered(Thing thing, Map map)
		{
			foreach (IntVec3 c in thing.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(map);
				bool flag = false;
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i] != thing && thingList[i].def.Fillage == FillCategory.Full && thingList[i].def.Altitude >= thing.def.Altitude)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400261A RID: 9754
		public const float CoverPercent_Corner = 0.75f;
	}
}
