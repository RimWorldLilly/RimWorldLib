using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A9E RID: 6814
	public static class BedUtility
	{
		// Token: 0x0600AC04 RID: 44036 RVA: 0x003CBD28 File Offset: 0x003C9F28
		public static int GetSleepingSlotsCount(IntVec2 bedSize)
		{
			return bedSize.x;
		}

		// Token: 0x0600AC05 RID: 44037 RVA: 0x003CBD30 File Offset: 0x003C9F30
		private static IntVec3 GetSlotPos(int index, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize, bool head)
		{
			int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(bedSize);
			if (index < 0 || index >= sleepingSlotsCount)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to get sleeping slot pos with index ",
					index.ToString(),
					", but there are only ",
					sleepingSlotsCount.ToString(),
					" sleeping slots available."
				}));
				return bedCenter;
			}
			CellRect cellRect = GenAdj.OccupiedRect(bedCenter, bedRot, bedSize);
			switch (bedRot.AsInt)
			{
			case 0:
				return new IntVec3(cellRect.maxX - index, bedCenter.y, head ? cellRect.minZ : cellRect.maxZ);
			case 1:
				return new IntVec3(head ? cellRect.minX : cellRect.maxX, bedCenter.y, cellRect.minZ + index);
			case 2:
				return new IntVec3(cellRect.minX + index, bedCenter.y, head ? cellRect.maxZ : cellRect.minZ);
			case 3:
				return new IntVec3(head ? cellRect.maxX : cellRect.minX, bedCenter.y, cellRect.maxZ - index);
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600AC06 RID: 44038 RVA: 0x003CBE53 File Offset: 0x003CA053
		public static IntVec3 GetFeetSlotPos(int index, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize)
		{
			return BedUtility.GetSlotPos(index, bedCenter, bedRot, bedSize, false);
		}

		// Token: 0x0600AC07 RID: 44039 RVA: 0x003CBE5F File Offset: 0x003CA05F
		public static IntVec3 GetSleepingSlotPos(int index, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize)
		{
			return BedUtility.GetSlotPos(index, bedCenter, bedRot, bedSize, true);
		}

		// Token: 0x0600AC08 RID: 44040 RVA: 0x003CBE6C File Offset: 0x003CA06C
		public static int GetSlotFromPosition(IntVec3 pos, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize)
		{
			CellRect cellRect = GenAdj.OccupiedRect(bedCenter, bedRot, bedSize);
			if (!cellRect.Contains(pos))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 4);
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(pos);
				defaultInterpolatedStringHandler.AppendLiteral(" was not inside bed at ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(bedCenter);
				defaultInterpolatedStringHandler.AppendLiteral(" with size ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec2>(bedSize);
				defaultInterpolatedStringHandler.AppendLiteral(" and rotation ");
				defaultInterpolatedStringHandler.AppendFormatted<Rot4>(bedRot);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return 0;
			}
			int num;
			switch (bedRot.AsInt)
			{
			case 0:
				num = cellRect.maxX - pos.x;
				break;
			case 1:
				num = pos.z - cellRect.minZ;
				break;
			case 2:
				num = pos.x - cellRect.minX;
				break;
			case 3:
				num = cellRect.maxZ - pos.z;
				break;
			default:
				throw new NotImplementedException();
			}
			if (num < 0 || num >= BedUtility.GetSleepingSlotsCount(bedSize))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(54, 2);
				defaultInterpolatedStringHandler2.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler2.AppendLiteral(" was outside the valid slot range for this bed: [0, ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(BedUtility.GetSleepingSlotsCount(bedSize));
				defaultInterpolatedStringHandler2.AppendLiteral(").");
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				return 0;
			}
			return num;
		}

		// Token: 0x0600AC09 RID: 44041 RVA: 0x003CBFA4 File Offset: 0x003CA1A4
		public static bool WillingToShareBed(Pawn pawn1, Pawn pawn2)
		{
			if (pawn1 == pawn2)
			{
				return true;
			}
			if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed, pawn1) || !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed, pawn2))
			{
				return false;
			}
			if (pawn1.relations.GetDirectRelation(PawnRelationDefOf.Spouse, pawn2) != null)
			{
				if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_Spouse, pawn1) || !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_Spouse, pawn2))
				{
					return false;
				}
			}
			else if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_NonSpouse, pawn1) || !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_NonSpouse, pawn2))
			{
				return false;
			}
			return true;
		}
	}
}
