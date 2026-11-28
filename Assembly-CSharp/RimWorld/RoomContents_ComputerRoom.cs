using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122C RID: 4652
	public class RoomContents_ComputerRoom : RoomContentsWorker
	{
		// Token: 0x06007386 RID: 29574 RVA: 0x00287740 File Offset: 0x00285940
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			IntVec3 loc;
			if (room.TryGetRandomCellInRoom(ThingDefOf.AncientMachine, map, out loc, null, 0, 1, null, false))
			{
				GenSpawn.Spawn(ThingDefOf.AncientMachine, loc, map, WipeMode.Vanish);
			}
			base.FillRoom(map, room, faction, threatPoints);
		}
	}
}
