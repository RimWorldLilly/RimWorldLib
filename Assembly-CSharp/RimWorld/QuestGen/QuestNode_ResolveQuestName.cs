using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022B8 RID: 8888
	public class QuestNode_ResolveQuestName : QuestNode
	{
		// Token: 0x0600DB65 RID: 56165 RVA: 0x0051BC9C File Offset: 0x00519E9C
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600DB66 RID: 56166 RVA: 0x0051BC9F File Offset: 0x00519E9F
		protected override void RunInt()
		{
			if (this.rules.GetValue(QuestGen.slate) != null)
			{
				QuestGen.AddQuestNameRules(this.rules.GetValue(QuestGen.slate));
			}
			QuestNode_ResolveQuestName.Resolve();
		}

		// Token: 0x0600DB67 RID: 56167 RVA: 0x0051BCD0 File Offset: 0x00519ED0
		public static void Resolve()
		{
			string text;
			if (!QuestGen.slate.TryGet<string>("resolvedQuestName", out text, false))
			{
				text = (QuestGen.quest.hidden ? QuestGen.quest.root.defName : QuestNode_ResolveQuestName.GenerateName().StripTags());
				QuestGen.slate.Set<string>("resolvedQuestName", text, false);
			}
			QuestGen.quest.name = text;
		}

		// Token: 0x0600DB68 RID: 56168 RVA: 0x0051BD38 File Offset: 0x00519F38
		private static string GenerateName()
		{
			GrammarRequest request = default(GrammarRequest);
			request.Rules.AddRange(QuestGen.QuestNameRulesReadOnly);
			foreach (KeyValuePair<string, string> keyValuePair in QuestGen.QuestNameConstantsReadOnly)
			{
				request.Constants.Add(keyValuePair.Key, keyValuePair.Value);
			}
			QuestGenUtility.AddSlateVars(ref request);
			Predicate<string> predicate = (string x) => !Find.QuestManager.QuestsListForReading.Any((Quest y) => y.name == x);
			if (QuestGen.Root.nameMustBeUnique)
			{
				return NameGenerator.GenerateName(request, predicate, false, "questName", null);
			}
			string text = null;
			for (int i = 0; i < 20; i++)
			{
				text = NameGenerator.GenerateName(request, null, false, "questName", null);
				if (predicate(text) || QuestGen.Root.defaultHidden)
				{
					break;
				}
			}
			return text;
		}

		// Token: 0x04008C6A RID: 35946
		public SlateRef<RulePack> rules;

		// Token: 0x04008C6B RID: 35947
		public const string TextRoot = "questName";

		// Token: 0x04008C6C RID: 35948
		private const int MaxTriesTryAvoidDuplicateName = 20;
	}
}
