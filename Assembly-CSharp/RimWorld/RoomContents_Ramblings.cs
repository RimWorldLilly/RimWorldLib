using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001257 RID: 4695
	public class RoomContents_Ramblings : RoomContentsWorker
	{
		// Token: 0x0600745D RID: 29789 RVA: 0x0028B570 File Offset: 0x00289770
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(map, out loc, 1, 0, null, false))
			{
				return;
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.FloorEtchingRambling, null), loc, map, WipeMode.Vanish);
		}
	}
}
