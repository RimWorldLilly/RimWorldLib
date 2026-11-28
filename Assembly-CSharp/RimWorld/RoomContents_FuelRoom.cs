using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001232 RID: 4658
	public class RoomContents_FuelRoom : RoomContentsWorker
	{
		// Token: 0x0600739F RID: 29599 RVA: 0x00287D00 File Offset: 0x00285F00
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			float num = (float)room.Area / 100f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_FuelRoom.AncientGeneratorsPer100Cells.RandomInRange * num), 1);
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientGenerator, count, room, map, new Rot4?(Rot4.North), null, null, 1, null, false, false, null, null);
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientFuelNode, RoomContents_FuelRoom.FuelNodeRange.RandomInRange, room, map, new Rot4?(Rot4.North), null, null, 1, null, false, false, null, null);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x040048F2 RID: 18674
		private static readonly IntRange FuelNodeRange = new IntRange(1, 2);

		// Token: 0x040048F3 RID: 18675
		private static readonly FloatRange AncientGeneratorsPer100Cells = new FloatRange(3f, 5f);
	}
}
