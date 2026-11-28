using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001260 RID: 4704
	public class RoomContents_StockpileEntrance : RoomContentsWorker
	{
		// Token: 0x0600747D RID: 29821 RVA: 0x0028C279 File Offset: 0x0028A479
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.SpawnExit(map, room);
			this.SpawnTurrets(map, room, faction);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600747E RID: 29822 RVA: 0x0028C298 File Offset: 0x0028A498
		private void SpawnExit(Map map, LayoutRoom room)
		{
			List<Thing> list = new List<Thing>();
			ThingDef ancientHatchExit = ThingDefOf.AncientHatchExit;
			int count = 1;
			List<Thing> spawned = list;
			RoomGenUtility.FillWithPadding(ancientHatchExit, count, room, map, null, null, spawned, 3, null, false, false, null, null);
			MapGenerator.PlayerStartSpot = list.First<Thing>().Position;
		}

		// Token: 0x0600747F RID: 29823 RVA: 0x0028C2DC File Offset: 0x0028A4DC
		private void SpawnTurrets(Map map, LayoutRoom room, Faction faction)
		{
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientSecurityTurret, RoomContents_StockpileEntrance.TurretsRange.RandomInRange, IntRange.One, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, faction);
		}

		// Token: 0x04004933 RID: 18739
		private static readonly IntRange TurretsRange = new IntRange(1, 2);
	}
}
