using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122F RID: 4655
	public class RoomContents_CryptosleepCaskets : RoomContentsWorker
	{
		// Token: 0x0600738F RID: 29583 RVA: 0x002879A1 File Offset: 0x00285BA1
		public virtual PodContentsType GetContentsType()
		{
			if (!Rand.Bool)
			{
				return PodContentsType.AncientFriendly;
			}
			return PodContentsType.AncientHostile;
		}

		// Token: 0x06007390 RID: 29584 RVA: 0x002879B0 File Offset: 0x00285BB0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContents_CryptosleepCaskets.<>c__DisplayClass4_0 CS$<>8__locals1 = new RoomContents_CryptosleepCaskets.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.groupID = Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID();
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientCryptosleepCasket, RoomContents_CryptosleepCaskets.CasketRange.RandomInRange, room, CS$<>8__locals1.map, new Rot4?(Rot4.South), null, null, 2, null, false, false, new Func<IntVec3, Rot4, Map, Thing>(CS$<>8__locals1.<FillRoom>g__SpawnAction|0), null);
			RoomContents_CryptosleepCaskets.SpawnLockers(CS$<>8__locals1.map, room);
			base.FillRoom(CS$<>8__locals1.map, room, faction, threatPoints);
		}

		// Token: 0x06007391 RID: 29585 RVA: 0x00287A38 File Offset: 0x00285C38
		private static void SpawnLockers(Map map, LayoutRoom room)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_CryptosleepCaskets.LockersPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientLockerBank, count, RoomContents_CryptosleepCaskets.LockerGroupSizeRange, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x040048EE RID: 18670
		private static readonly FloatRange LockersPer10EdgeCells = new FloatRange(1f, 3f);

		// Token: 0x040048EF RID: 18671
		private static readonly IntRange LockerGroupSizeRange = new IntRange(2, 3);

		// Token: 0x040048F0 RID: 18672
		private static readonly IntRange CasketRange = new IntRange(2, 4);
	}
}
