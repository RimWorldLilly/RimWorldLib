using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02002214 RID: 8724
	public class QuestNode_GiveNearPawn : QuestNode
	{
		// Token: 0x0600D864 RID: 55396 RVA: 0x005038C2 File Offset: 0x00501AC2
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D865 RID: 55397 RVA: 0x005038C8 File Offset: 0x00501AC8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate).EnumerableNullOrEmpty<Thing>() && this.contentsDefs.GetValue(slate).EnumerableNullOrEmpty<ThingDefCountClass>())
			{
				return;
			}
			QuestPart_GiveNearPawn give = new QuestPart_GiveNearPawn();
			give.nearPawn = this.nearPawn.GetValue(slate);
			give.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			if (!this.customDropPodsLetterLabel.GetValue(slate).NullOrEmpty() || this.customDropPodsLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customDropPodsLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customDropPodsLetterLabelRules.GetValue(slate), this.customDropPodsLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customDropPodsLetterText.GetValue(slate).NullOrEmpty() || this.customDropPodsLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customDropPodsLetterText = x;
				}, QuestGenUtility.MergeRules(this.customDropPodsLetterTextRules.GetValue(slate), this.customDropPodsLetterText.GetValue(slate), "root"));
			}
			if (!this.customCaravanInventoryLetterLabel.GetValue(slate).NullOrEmpty() || this.customCaravanInventoryLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customCaravanInventoryLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customCaravanInventoryLetterLabelRules.GetValue(slate), this.customCaravanInventoryLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customCaravanInventoryLetterText.GetValue(slate).NullOrEmpty() || this.customCaravanInventoryLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					give.customCaravanInventoryLetterText = x;
				}, QuestGenUtility.MergeRules(this.customCaravanInventoryLetterTextRules.GetValue(slate), this.customCaravanInventoryLetterText.GetValue(slate), "root"));
			}
			give.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? give.sendStandardLetter);
			give.joinPlayer = this.joinPlayer.GetValue(slate);
			give.makePrisoners = this.makePrisoners.GetValue(slate);
			give.Things = this.contents.GetValue(slate);
			if (this.contentsDefs.GetValue(slate) != null)
			{
				give.thingDefs.AddRange(this.contentsDefs.GetValue(slate));
			}
			QuestGen.quest.AddPart(give);
		}

		// Token: 0x040089DA RID: 35290
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089DB RID: 35291
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x040089DC RID: 35292
		public SlateRef<IEnumerable<ThingDefCountClass>> contentsDefs;

		// Token: 0x040089DD RID: 35293
		public SlateRef<Pawn> nearPawn;

		// Token: 0x040089DE RID: 35294
		public SlateRef<bool> joinPlayer;

		// Token: 0x040089DF RID: 35295
		public SlateRef<bool> makePrisoners;

		// Token: 0x040089E0 RID: 35296
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x040089E1 RID: 35297
		public SlateRef<string> customDropPodsLetterLabel;

		// Token: 0x040089E2 RID: 35298
		public SlateRef<string> customDropPodsLetterText;

		// Token: 0x040089E3 RID: 35299
		public SlateRef<string> customCaravanInventoryLetterLabel;

		// Token: 0x040089E4 RID: 35300
		public SlateRef<string> customCaravanInventoryLetterText;

		// Token: 0x040089E5 RID: 35301
		public SlateRef<RulePack> customDropPodsLetterLabelRules;

		// Token: 0x040089E6 RID: 35302
		public SlateRef<RulePack> customDropPodsLetterTextRules;

		// Token: 0x040089E7 RID: 35303
		public SlateRef<RulePack> customCaravanInventoryLetterLabelRules;

		// Token: 0x040089E8 RID: 35304
		public SlateRef<RulePack> customCaravanInventoryLetterTextRules;

		// Token: 0x040089E9 RID: 35305
		private const string RootSymbol = "root";
	}
}
