using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022C2 RID: 8898
	public class QuestNode_Infestation : QuestNode
	{
		// Token: 0x0600DB85 RID: 56197 RVA: 0x0051C8A8 File Offset: 0x0051AAA8
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			if (!slate.Exists("map", false))
			{
				return false;
			}
			if (Faction.OfInsects == null)
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			IntVec3 intVec;
			return InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x0600DB86 RID: 56198 RVA: 0x0051C8FC File Offset: 0x0051AAFC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return;
			}
			QuestPart_Infestation questPart = new QuestPart_Infestation();
			questPart.mapParent = map.Parent;
			questPart.hivesCount = this.hivesCount.GetValue(slate);
			questPart.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					questPart.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					questPart.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			questPart.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart);
		}

		// Token: 0x04008C95 RID: 35989
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008C96 RID: 35990
		public SlateRef<int> hivesCount;

		// Token: 0x04008C97 RID: 35991
		public SlateRef<string> tag;

		// Token: 0x04008C98 RID: 35992
		public SlateRef<string> customLetterLabel;

		// Token: 0x04008C99 RID: 35993
		public SlateRef<string> customLetterText;

		// Token: 0x04008C9A RID: 35994
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04008C9B RID: 35995
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04008C9C RID: 35996
		private const string RootSymbol = "root";
	}
}
