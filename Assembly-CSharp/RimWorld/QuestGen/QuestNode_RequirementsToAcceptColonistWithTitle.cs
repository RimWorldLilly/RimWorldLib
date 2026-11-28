using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002230 RID: 8752
	public class QuestNode_RequirementsToAcceptColonistWithTitle : QuestNode
	{
		// Token: 0x0600D8C2 RID: 55490 RVA: 0x005054B0 File Offset: 0x005036B0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.AddPart(new QuestPart_RequirementsToAcceptColonistWithTitle
			{
				minimumTitle = this.minimumTitle.GetValue(slate),
				faction = this.faction.GetValue(slate)
			});
		}

		// Token: 0x0600D8C3 RID: 55491 RVA: 0x005054F6 File Offset: 0x005036F6
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04008A6E RID: 35438
		public SlateRef<RoyalTitleDef> minimumTitle;

		// Token: 0x04008A6F RID: 35439
		public SlateRef<Faction> faction;
	}
}
