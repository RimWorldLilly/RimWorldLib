using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001261 RID: 4705
	public class RoomContents_Terraformer : RoomContentsWorker
	{
		// Token: 0x06007482 RID: 29826 RVA: 0x0028C328 File Offset: 0x0028A528
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomGenUtility.TryPlaceInRandomCorner(map, room, ThingDefOf.HunterDroneTrap, faction, null);
			if (Rand.Bool)
			{
				RoomGenUtility.TryPlaceInRandomCorner(map, room, ThingDefOf.HunterDroneTrap, faction, null);
			}
			CellRect largest = (from r in room.rects
			where r.Width >= 6 && r.Height >= 6
			orderby r.Area descending
			select r).FirstOrDefault<CellRect>();
			this.SpawnTerraformer(map, largest);
			this.RemoveRoof(map, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x06007483 RID: 29827 RVA: 0x0028C3C7 File Offset: 0x0028A5C7
		private void SpawnTerraformer(Map map, CellRect largest)
		{
			GenSpawn.Spawn(ThingDefOf.AncientTerraformer, largest.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x06007484 RID: 29828 RVA: 0x0028C3E0 File Offset: 0x0028A5E0
		private void RemoveRoof(Map map, LayoutRoom room)
		{
			foreach (IntVec3 c in room.Cells)
			{
				map.roofGrid.SetRoof(c, null);
			}
		}
	}
}
