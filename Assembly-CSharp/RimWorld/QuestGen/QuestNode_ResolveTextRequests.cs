using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022BA RID: 8890
	public class QuestNode_ResolveTextRequests : QuestNode
	{
		// Token: 0x0600DB6D RID: 56173 RVA: 0x0051BE8D File Offset: 0x0051A08D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600DB6E RID: 56174 RVA: 0x0051BE90 File Offset: 0x0051A090
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestDescriptionRules(this.rules.GetValue(QuestGen.slate));
				QuestGen.AddQuestContentRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveTextRequests.Resolve();
		}

		// Token: 0x0600DB6F RID: 56175 RVA: 0x0051BEE0 File Offset: 0x0051A0E0
		public static void Resolve()
		{
			List<QuestTextRequest> textRequestsReadOnly = QuestGen.TextRequestsReadOnly;
			for (int i = 0; i < textRequestsReadOnly.Count; i++)
			{
				try
				{
					List<Rule> list = new List<Rule>();
					list.AddRange(QuestGen.QuestDescriptionRulesReadOnly);
					list.AddRange(QuestGen.QuestContentRulesReadOnly);
					if (textRequestsReadOnly[i].extraRules != null)
					{
						list.AddRange(textRequestsReadOnly[i].extraRules);
					}
					string obj = QuestGenUtility.ResolveAbsoluteText(list, QuestGen.QuestDescriptionConstantsReadOnly, textRequestsReadOnly[i].keyword, true);
					textRequestsReadOnly[i].setter(obj);
				}
				catch (Exception ex)
				{
					string str = "Error while resolving text request: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			textRequestsReadOnly.Clear();
		}

		// Token: 0x04008C70 RID: 35952
		public SlateRef<RulePack> rules;
	}
}
