using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126F RID: 4719
	public class RoomPart_Crate : RoomPartWorker
	{
		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x060074EB RID: 29931 RVA: 0x00290E58 File Offset: 0x0028F058
		public new RoomPart_CrateDef def
		{
			get
			{
				return (RoomPart_CrateDef)this.def;
			}
		}

		// Token: 0x060074EC RID: 29932 RVA: 0x00290E65 File Offset: 0x0028F065
		public RoomPart_Crate(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074ED RID: 29933 RVA: 0x00290E70 File Offset: 0x0028F070
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			string signal = null;
			if (this.def.triggerThreatSignal)
			{
				signal = room.ThreatSignal;
			}
			RoomGenUtility.SpawnCratesInRoom(this.def.crateDef, room, map, this.def.thingSetMaker, IntRange.One, true, signal, new Rot4?(this.def.rotations.Random()));
		}
	}
}
