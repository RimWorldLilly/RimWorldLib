using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001236 RID: 4662
	public class RoomContents_IndustrialStorage : RoomContentsWorker
	{
		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x060073AB RID: 29611 RVA: 0x00288120 File Offset: 0x00286320
		protected virtual FloatRange ContainersPer100Cells
		{
			get
			{
				return new FloatRange(3f, 5f);
			}
		}

		// Token: 0x060073AC RID: 29612 RVA: 0x00288134 File Offset: 0x00286334
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			float num = (float)room.Area / 100f;
			int count = Mathf.Max(Mathf.RoundToInt(this.ContainersPer100Cells.RandomInRange * num), 1);
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientLargeContainer, count, room, map, null, null, null, 1, null, false, false, null, null);
			base.FillRoom(map, room, faction, threatPoints);
		}
	}
}
