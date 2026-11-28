using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001272 RID: 4722
	public class RoomPart_DormantThreatCluster : RoomPartWorker
	{
		// Token: 0x060074F2 RID: 29938 RVA: 0x00290F0E File Offset: 0x0028F10E
		public RoomPart_DormantThreatCluster(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074F3 RID: 29939 RVA: 0x00290F17 File Offset: 0x0028F117
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomGenUtility.SpawnDormantThreatCluster(room, map, threatPoints, room.ThreatSignal);
		}
	}
}
