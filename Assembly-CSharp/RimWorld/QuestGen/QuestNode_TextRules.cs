using System;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022BC RID: 8892
	public class QuestNode_TextRules : QuestNode
	{
		// Token: 0x0600DB71 RID: 56177 RVA: 0x0051BFB4 File Offset: 0x0051A1B4
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600DB72 RID: 56178 RVA: 0x0051BFB8 File Offset: 0x0051A1B8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			switch (this.target.GetValue(slate))
			{
			case TextRulesTarget.Description:
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(slate));
				return;
			case TextRulesTarget.Name:
				QuestGen.AddQuestNameRules(this.rules.GetValue(slate));
				return;
			case TextRulesTarget.DecriptionAndName:
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(slate));
				QuestGen.AddQuestNameRules(this.rules.GetValue(slate));
				return;
			default:
				return;
			}
		}

		// Token: 0x04008C75 RID: 35957
		public SlateRef<RulePack> rules;

		// Token: 0x04008C76 RID: 35958
		public SlateRef<TextRulesTarget> target;
	}
}
