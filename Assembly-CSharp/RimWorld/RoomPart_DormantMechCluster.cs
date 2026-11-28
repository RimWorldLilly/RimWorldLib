using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001271 RID: 4721
	public class RoomPart_DormantMechCluster : RoomPartWorker
	{
		// Token: 0x060074F0 RID: 29936 RVA: 0x00290EED File Offset: 0x0028F0ED
		public RoomPart_DormantMechCluster(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074F1 RID: 29937 RVA: 0x00290EF6 File Offset: 0x0028F0F6
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			if (Faction.OfMechanoids != null)
			{
				RoomGenUtility.SpawnDormantMechCluster(room, map, threatPoints, room.ThreatSignal);
			}
		}
	}
}
