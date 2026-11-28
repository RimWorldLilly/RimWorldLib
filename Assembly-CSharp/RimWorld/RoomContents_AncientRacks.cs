using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122A RID: 4650
	public class RoomContents_AncientRacks : RoomContentsWorker
	{
		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x0600737F RID: 29567 RVA: 0x00287570 File Offset: 0x00285770
		protected virtual FloatRange ShelvesPer100Cells
		{
			get
			{
				return new FloatRange(20f, 40f);
			}
		}

		// Token: 0x06007380 RID: 29568 RVA: 0x00287584 File Offset: 0x00285784
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			float num = (float)room.Area / 100f;
			int count = Mathf.Max(Mathf.RoundToInt(this.ShelvesPer100Cells.RandomInRange * num), 1);
			Rot4 value = Rot4.North;
			CellRect boundary = room.Boundary;
			if (boundary.Height > boundary.Width)
			{
				value = Rot4.East;
			}
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientSystemRack, count, room, map, new Rot4?(value), null, null, 1, null, false, false, null, null);
			base.FillRoom(map, room, faction, threatPoints);
		}
	}
}
