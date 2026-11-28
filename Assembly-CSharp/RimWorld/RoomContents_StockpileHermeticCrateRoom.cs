using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200123F RID: 4671
	public class RoomContents_StockpileHermeticCrateRoom : RoomContentsWorker
	{
		// Token: 0x060073E5 RID: 29669 RVA: 0x002895DD File Offset: 0x002877DD
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContents_StockpileHermeticCrateRoom.SpawnCrate(map, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x060073E6 RID: 29670 RVA: 0x002895F4 File Offset: 0x002877F4
		private static void SpawnCrate(Map map, LayoutRoom room)
		{
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(map, out centerCell, 3, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			RoomGenUtility.SpawnHermeticCrate(centerCell, map, Rot4.Random, ThingSetMakerDefOf.MapGen_HighValueCrate, true, null);
		}
	}
}
