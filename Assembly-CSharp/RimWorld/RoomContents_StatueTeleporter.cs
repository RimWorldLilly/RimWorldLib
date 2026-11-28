using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125A RID: 4698
	public class RoomContents_StatueTeleporter : RoomContentsWorker
	{
		// Token: 0x06007466 RID: 29798 RVA: 0x0028B734 File Offset: 0x00289934
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(ThingDefOf.GrayStatueTeleporter, map, out centerCell, null, 4, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GrayStatueTeleporter, null), centerCell, map, WipeMode.Vanish);
		}
	}
}
