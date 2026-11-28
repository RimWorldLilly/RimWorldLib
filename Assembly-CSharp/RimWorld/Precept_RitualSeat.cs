using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001644 RID: 5700
	public class Precept_RitualSeat : Precept_ThingDef
	{
		// Token: 0x170018A4 RID: 6308
		// (get) Token: 0x06008ECE RID: 36558 RVA: 0x00347C7E File Offset: 0x00345E7E
		public override string UIInfoSecondLine
		{
			get
			{
				return base.ThingDef.LabelCap;
			}
		}

		// Token: 0x06008ECF RID: 36559 RVA: 0x00347C90 File Offset: 0x00345E90
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			Precept_RitualSeat.<EditFloatMenuOptions>d__2 <EditFloatMenuOptions>d__ = new Precept_RitualSeat.<EditFloatMenuOptions>d__2(-2);
			<EditFloatMenuOptions>d__.<>4__this = this;
			return <EditFloatMenuOptions>d__;
		}
	}
}
