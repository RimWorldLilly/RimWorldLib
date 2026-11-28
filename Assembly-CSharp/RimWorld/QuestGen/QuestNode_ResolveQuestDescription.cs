using System;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022B7 RID: 8887
	public class QuestNode_ResolveQuestDescription : QuestNode
	{
		// Token: 0x0600DB61 RID: 56161 RVA: 0x0051BBFC File Offset: 0x00519DFC
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600DB62 RID: 56162 RVA: 0x0051BBFF File Offset: 0x00519DFF
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestDescription.Resolve();
		}

		// Token: 0x0600DB63 RID: 56163 RVA: 0x0051BC30 File Offset: 0x00519E30
		public static void Resolve()
		{
			string text;
			if (!QuestGen.slate.TryGet<string>("resolvedQuestDescription", out text, false) && !QuestGen.quest.hidden)
			{
				text = QuestGenUtility.ResolveAbsoluteText(QuestGen.QuestDescriptionRulesReadOnly, QuestGen.QuestDescriptionConstantsReadOnly, "questDescription", true);
				QuestGen.slate.Set<string>("resolvedQuestDescription", text, false);
			}
			QuestGen.quest.description = text;
		}

		// Token: 0x04008C68 RID: 35944
		public SlateRef<RulePack> rules;

		// Token: 0x04008C69 RID: 35945
		public const string TextRoot = "questDescription";
	}
}
