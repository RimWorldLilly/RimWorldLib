using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001270 RID: 4720
	public class RoomPart_DormantInsectCluster : RoomPartWorker
	{
		// Token: 0x060074EE RID: 29934 RVA: 0x00290ECC File Offset: 0x0028F0CC
		public RoomPart_DormantInsectCluster(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074EF RID: 29935 RVA: 0x00290ED5 File Offset: 0x0028F0D5
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			if (Faction.OfInsects != null)
			{
				RoomGenUtility.SpawnDormantInsectCluster(room, map, threatPoints, room.ThreatSignal);
			}
		}
	}
}
