using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126D RID: 4717
	public class RoomPart_CornerThing : RoomPartWorker
	{
		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x060074E5 RID: 29925 RVA: 0x00290DA7 File Offset: 0x0028EFA7
		public new RoomPart_ThingDef def
		{
			get
			{
				return (RoomPart_ThingDef)this.def;
			}
		}

		// Token: 0x060074E6 RID: 29926 RVA: 0x00290DB4 File Offset: 0x0028EFB4
		public RoomPart_CornerThing(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074E7 RID: 29927 RVA: 0x00290DBD File Offset: 0x0028EFBD
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomGenUtility.TryPlaceInRandomCorner(map, room, this.def.thingDef, faction ?? Faction.OfAncientsHostile, this.def.stuffDef);
		}
	}
}
