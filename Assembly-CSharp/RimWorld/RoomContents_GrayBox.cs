using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001252 RID: 4690
	public class RoomContents_GrayBox : RoomContentsWorker
	{
		// Token: 0x0600744D RID: 29773 RVA: 0x0028B13C File Offset: 0x0028933C
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			Building_Crate building_Crate;
			RoomContents_GrayBox.TrySpawnBoxInRoom(map, room, out building_Crate);
		}

		// Token: 0x0600744E RID: 29774 RVA: 0x0028B160 File Offset: 0x00289360
		public static bool TrySpawnBoxInRoom(Map map, LayoutRoom room, out Building_Crate spawned)
		{
			spawned = null;
			IntVec3 cell;
			if (!room.TryGetRandomCellInRoom(ThingDefOf.GrayBox, map, out cell, null, 2, 1, null, false))
			{
				return false;
			}
			spawned = RoomContents_GrayBox.SpawnBoxInRoom(cell, map, null, true);
			return true;
		}

		// Token: 0x0600744F RID: 29775 RVA: 0x0028B19B File Offset: 0x0028939B
		public static Building_Crate SpawnBoxInRoom(IntVec3 cell, Map map, ThingSetMakerDef rewardMaker = null, bool addRewards = true)
		{
			return RoomGenUtility.SpawnCrate(ThingDefOf.GrayBox, cell, map, rewardMaker ?? ThingSetMakerDefOf.Reward_GrayBox, addRewards, null);
		}
	}
}
