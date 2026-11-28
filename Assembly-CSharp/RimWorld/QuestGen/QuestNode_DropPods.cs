using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200220E RID: 8718
	public class QuestNode_DropPods : QuestNode
	{
		// Token: 0x0600D851 RID: 55377 RVA: 0x005031DD File Offset: 0x005013DD
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0600D852 RID: 55378 RVA: 0x005031EC File Offset: 0x005013EC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate).EnumerableNullOrEmpty<Thing>() && this.contentsDefs.GetValue(slate).EnumerableNullOrEmpty<ThingDefCountClass>())
			{
				return;
			}
			QuestPart_DropPods dropPods = new QuestPart_DropPods();
			dropPods.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					dropPods.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					dropPods.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			dropPods.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? dropPods.sendStandardLetter);
			dropPods.useTradeDropSpot = this.useTradeDropSpot.GetValue(slate);
			dropPods.dropSpot = (this.dropSpot.GetValue(slate) ?? IntVec3.Invalid);
			dropPods.joinPlayer = this.joinPlayer.GetValue(slate);
			dropPods.makePrisoners = this.makePrisoners.GetValue(slate);
			dropPods.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			dropPods.Things = this.contents.GetValue(slate);
			dropPods.canRetargetAnyMap = this.canRetargetAnyMap.GetValue(slate);
			if (this.contentsDefs.GetValue(slate) != null)
			{
				dropPods.thingDefs.AddRange(this.contentsDefs.GetValue(slate));
			}
			if (this.thingsToExcludeFromHyperlinks.GetValue(slate) != null)
			{
				dropPods.thingsToExcludeFromHyperlinks.AddRange(from t in this.thingsToExcludeFromHyperlinks.GetValue(slate)
				select t.GetInnerIfMinified().def);
			}
			QuestGen.quest.AddPart(dropPods);
		}

		// Token: 0x040089B2 RID: 35250
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089B3 RID: 35251
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x040089B4 RID: 35252
		public SlateRef<IEnumerable<ThingDefCountClass>> contentsDefs;

		// Token: 0x040089B5 RID: 35253
		public SlateRef<IntVec3?> dropSpot;

		// Token: 0x040089B6 RID: 35254
		public SlateRef<bool> useTradeDropSpot;

		// Token: 0x040089B7 RID: 35255
		public SlateRef<bool> joinPlayer;

		// Token: 0x040089B8 RID: 35256
		public SlateRef<bool> makePrisoners;

		// Token: 0x040089B9 RID: 35257
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x040089BA RID: 35258
		public SlateRef<string> customLetterLabel;

		// Token: 0x040089BB RID: 35259
		public SlateRef<string> customLetterText;

		// Token: 0x040089BC RID: 35260
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040089BD RID: 35261
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040089BE RID: 35262
		public SlateRef<IEnumerable<Thing>> thingsToExcludeFromHyperlinks;

		// Token: 0x040089BF RID: 35263
		public SlateRef<bool> canRetargetAnyMap;

		// Token: 0x040089C0 RID: 35264
		private const string RootSymbol = "root";
	}
}
