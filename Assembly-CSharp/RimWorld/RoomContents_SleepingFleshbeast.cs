using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001258 RID: 4696
	public class RoomContents_SleepingFleshbeast : RoomContentsWorker
	{
		// Token: 0x0600745F RID: 29791 RVA: 0x0028B5B4 File Offset: 0x002897B4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(map, out loc, 1, 0, null, false))
			{
				return;
			}
			CompCanBeDormant compCanBeDormant;
			if (GenSpawn.Spawn(PawnGenerator.GeneratePawn(PawnKindDefOf.Fingerspike, Faction.OfEntities, null), loc, map, WipeMode.Vanish).TryGetComp(out compCanBeDormant))
			{
				compCanBeDormant.ToSleep();
			}
			for (int i = 0; i < 5; i++)
			{
				FilthMaker.TryMakeFilth(room.Cells.RandomElement<IntVec3>(), map, ThingDefOf.Filth_Blood, 1, FilthSourceFlags.None, true);
			}
		}
	}
}
