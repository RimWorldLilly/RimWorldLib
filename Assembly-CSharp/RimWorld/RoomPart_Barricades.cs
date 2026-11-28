using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001269 RID: 4713
	public class RoomPart_Barricades : RoomPartWorker
	{
		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x060074D3 RID: 29907 RVA: 0x002902CE File Offset: 0x0028E4CE
		public new RoomPart_BarricadeDef def
		{
			get
			{
				return (RoomPart_BarricadeDef)this.def;
			}
		}

		// Token: 0x060074D4 RID: 29908 RVA: 0x002902DB File Offset: 0x0028E4DB
		public RoomPart_Barricades(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074D5 RID: 29909 RVA: 0x002902E4 File Offset: 0x0028E4E4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomGenUtility.SpawnDoorBarricades(this.def.wallDef, room, map, this.def.chancePerDoor, this.def.stuffDef, this.def.steps, this.def.offset);
		}
	}
}
