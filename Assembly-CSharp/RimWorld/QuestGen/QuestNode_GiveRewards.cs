using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02002215 RID: 8725
	public class QuestNode_GiveRewards : QuestNode
	{
		// Token: 0x0600D867 RID: 55399 RVA: 0x00503B77 File Offset: 0x00501D77
		protected override bool TestRunInt(Slate slate)
		{
			return this.nodeIfChosenPawnSignalUsed == null || this.nodeIfChosenPawnSignalUsed.TestRun(slate);
		}

		// Token: 0x0600D868 RID: 55400 RVA: 0x00503B90 File Offset: 0x00501D90
		protected override void RunInt()
		{
			QuestGen.quest.GiveRewards(this.parms.GetValue(QuestGen.slate), this.inSignal.GetValue(QuestGen.slate), this.customLetterLabel.GetValue(QuestGen.slate), this.customLetterText.GetValue(QuestGen.slate), this.customLetterLabelRules.GetValue(QuestGen.slate), this.customLetterTextRules.GetValue(QuestGen.slate), this.useDifficultyFactor.GetValue(QuestGen.slate), delegate
			{
				QuestNode questNode = this.nodeIfChosenPawnSignalUsed;
				if (questNode == null)
				{
					return;
				}
				questNode.Run();
			}, this.variants.GetValue(QuestGen.slate), this.addCampLootReward.GetValue(QuestGen.slate), QuestGen.slate.Get<Pawn>("asker", null, false), false, false, null);
		}

		// Token: 0x0600D869 RID: 55401 RVA: 0x00503C60 File Offset: 0x00501E60
		[Obsolete("Will be removed in the future")]
		private List<Reward> GenerateRewards(RewardsGeneratorParams parmsResolved, Slate slate, bool addDescriptionRules, ref bool chosenPawnSignalUsed, QuestPart_Choice.Choice choice, int variants)
		{
			return null;
		}

		// Token: 0x0600D86A RID: 55402 RVA: 0x00503C63 File Offset: 0x00501E63
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_SocialOnly(RewardsGeneratorParams parms, bool disallowRoyalFavor)
		{
			return null;
		}

		// Token: 0x0600D86B RID: 55403 RVA: 0x00503C66 File Offset: 0x00501E66
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_RoyalFavorOnly(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x0600D86C RID: 55404 RVA: 0x00503C69 File Offset: 0x00501E69
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_ThingsOnly(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x0600D86D RID: 55405 RVA: 0x00503C6C File Offset: 0x00501E6C
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateNonRepeatingRewards(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x0600D86E RID: 55406 RVA: 0x00503C6F File Offset: 0x00501E6F
		[Obsolete("Will be removed in the future")]
		private int GetDisplayPriority(QuestPart_Choice.Choice choice)
		{
			return 0;
		}

		// Token: 0x040089EA RID: 35306
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089EB RID: 35307
		public SlateRef<RewardsGeneratorParams> parms;

		// Token: 0x040089EC RID: 35308
		public SlateRef<string> customLetterLabel;

		// Token: 0x040089ED RID: 35309
		public SlateRef<string> customLetterText;

		// Token: 0x040089EE RID: 35310
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040089EF RID: 35311
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040089F0 RID: 35312
		public SlateRef<bool?> useDifficultyFactor;

		// Token: 0x040089F1 RID: 35313
		public QuestNode nodeIfChosenPawnSignalUsed;

		// Token: 0x040089F2 RID: 35314
		public SlateRef<int?> variants;

		// Token: 0x040089F3 RID: 35315
		public SlateRef<bool> addCampLootReward;

		// Token: 0x040089F4 RID: 35316
		private List<List<Reward>> generatedRewards = new List<List<Reward>>();

		// Token: 0x040089F5 RID: 35317
		private const float MinRewardValue = 250f;

		// Token: 0x040089F6 RID: 35318
		private const int DefaultVariants = 3;
	}
}
