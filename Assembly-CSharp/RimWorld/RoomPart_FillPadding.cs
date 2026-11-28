using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001273 RID: 4723
	public class RoomPart_FillPadding : RoomPartWorker
	{
		// Token: 0x170013D7 RID: 5079
		// (get) Token: 0x060074F4 RID: 29940 RVA: 0x00290F28 File Offset: 0x0028F128
		public new RoomPart_ThingDef def
		{
			get
			{
				return (RoomPart_ThingDef)this.def;
			}
		}

		// Token: 0x060074F5 RID: 29941 RVA: 0x00290F35 File Offset: 0x0028F135
		public RoomPart_FillPadding(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074F6 RID: 29942 RVA: 0x00290F40 File Offset: 0x0028F140
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomPart_FillPadding.<>c__DisplayClass3_0 CS$<>8__locals1 = new RoomPart_FillPadding.<>c__DisplayClass3_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = map;
			ThingDef thingDef = this.def.thingDef;
			int count = 1;
			Map map2 = CS$<>8__locals1.map;
			Func<IntVec3, Rot4, CellRect, bool> validator = new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<FillRoom>g__Validator|0);
			ThingDef stuffDef = this.def.stuffDef;
			RoomGenUtility.FillWithPadding(thingDef, count, room, map2, null, validator, null, 1, stuffDef, false, false, null, null);
		}
	}
}
