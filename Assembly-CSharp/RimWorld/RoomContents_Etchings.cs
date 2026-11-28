using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001251 RID: 4689
	public class RoomContents_Etchings : RoomContentsWorker
	{
		// Token: 0x0600744B RID: 29771 RVA: 0x0028B0F8 File Offset: 0x002892F8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(map, out loc, 1, 0, null, false))
			{
				return;
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.FloorEtching, null), loc, map, WipeMode.Vanish);
		}
	}
}
