using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020022CE RID: 8910
	public class QuestNode_RoyalTitleHyperlink : QuestNode
	{
		// Token: 0x0600DBC2 RID: 56258 RVA: 0x0051E2C4 File Offset: 0x0051C4C4
		protected override void RunInt()
		{
			QuestPart_RoyalTitleHyperlink questPart_RoyalTitleHyperlink = new QuestPart_RoyalTitleHyperlink();
			questPart_RoyalTitleHyperlink.titleDef = this.title.GetValue(QuestGen.slate);
			questPart_RoyalTitleHyperlink.factionDef = this.faction.GetValue(QuestGen.slate);
			QuestGen.quest.AddPart(questPart_RoyalTitleHyperlink);
		}

		// Token: 0x0600DBC3 RID: 56259 RVA: 0x0051E30E File Offset: 0x0051C50E
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04008D0C RID: 36108
		public SlateRef<RoyalTitleDef> title;

		// Token: 0x04008D0D RID: 36109
		public SlateRef<FactionDef> faction;
	}
}
