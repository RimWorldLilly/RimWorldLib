using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001243 RID: 4675
	public class RoomContents_ChildRoom : RoomContents_DeadBodyLabyrinth
	{
		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x060073F8 RID: 29688 RVA: 0x0028A0B6 File Offset: 0x002882B6
		protected override FloatRange CorpseAgeDaysRange
		{
			get
			{
				return new FloatRange(30f, 60f);
			}
		}

		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x060073F9 RID: 29689 RVA: 0x0028A0C7 File Offset: 0x002882C7
		protected override IntRange BloodFilthRange
		{
			get
			{
				return new IntRange(3, 6);
			}
		}

		// Token: 0x060073FA RID: 29690 RVA: 0x0028A0D0 File Offset: 0x002882D0
		protected override void SpawnCorpses(Map map, LayoutRoom room)
		{
			IntVec3 intVec;
			if (!room.TryGetRandomCellInRoom(map, out intVec, 1, 0, null, false))
			{
				return;
			}
			int num = Mathf.RoundToInt(this.CorpseAgeDaysRange.RandomInRange * 60000f);
			base.SpawnCorpse(intVec, PawnKindDefOf.Villager, num, map, new float?(RoomContents_ChildRoom.YearsAgeChild.RandomInRange), true);
			foreach (IntVec3 intVec2 in GenAdjFast.AdjacentCellsCardinal(intVec))
			{
				if (intVec2.Standable(map))
				{
					num += Mathf.RoundToInt((float)(RoomContents_ChildRoom.OtherAgeOffsetDays.RandomInRange * 60000));
					base.SpawnCorpse(intVec2, PawnKindDefOf.Villager, num, map, new float?(RoomContents_ChildRoom.YearsAgeAdult.RandomInRange), false);
					break;
				}
			}
			IntVec3 c2;
			if (ModsConfig.BiotechActive && RCellFinder.TryFindRandomCellNearWith(intVec, (IntVec3 c) => room.Contains(c, 1), map, out c2, 2, 10))
			{
				FilthMaker.TryMakeFilth(c2, map, ThingDefOf.Filth_Floordrawing, 1, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x04004910 RID: 18704
		private static readonly FloatRange YearsAgeChild = new FloatRange(2f, 3f);

		// Token: 0x04004911 RID: 18705
		private static readonly FloatRange YearsAgeAdult = new FloatRange(20f, 40f);

		// Token: 0x04004912 RID: 18706
		private static readonly IntRange OtherAgeOffsetDays = new IntRange(-2, 2);
	}
}
