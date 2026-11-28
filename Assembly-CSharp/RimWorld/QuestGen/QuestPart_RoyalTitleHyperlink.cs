using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022CF RID: 8911
	public class QuestPart_RoyalTitleHyperlink : QuestPart
	{
		// Token: 0x17002636 RID: 9782
		// (get) Token: 0x0600DBC5 RID: 56261 RVA: 0x0051E319 File Offset: 0x0051C519
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				QuestPart_RoyalTitleHyperlink.<get_Hyperlinks>d__3 <get_Hyperlinks>d__ = new QuestPart_RoyalTitleHyperlink.<get_Hyperlinks>d__3(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x0600DBC6 RID: 56262 RVA: 0x0051E329 File Offset: 0x0051C529
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.titleDef, "titleDef");
			Scribe_Defs.Look<FactionDef>(ref this.factionDef, "factionDef");
		}

		// Token: 0x04008D0E RID: 36110
		public RoyalTitleDef titleDef;

		// Token: 0x04008D0F RID: 36111
		public FactionDef factionDef;
	}
}
