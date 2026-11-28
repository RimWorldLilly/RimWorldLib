using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021E4 RID: 8676
	public class QuestNode_SplitRandomly : QuestNode
	{
		// Token: 0x0600D7C4 RID: 55236 RVA: 0x005014C5 File Offset: 0x004FF6C5
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600D7C5 RID: 55237 RVA: 0x005014CF File Offset: 0x004FF6CF
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D7C6 RID: 55238 RVA: 0x005014DC File Offset: 0x004FF6DC
		private void DoWork(Slate slate)
		{
			float valueOrDefault = this.valueToSplit.GetValue(slate).GetValueOrDefault(1f);
			QuestNode_SplitRandomly.tmpMinFractions.Clear();
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction1.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction2.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction3.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction4.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction5.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Clear();
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow1.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow2.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow3.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow4.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow5.GetValue(slate));
			Rand.SplitRandomly(valueOrDefault, this.countToSplit.GetValue(slate), QuestNode_SplitRandomly.tmpValues, QuestNode_SplitRandomly.tmpZeroIfFractionBelow, QuestNode_SplitRandomly.tmpMinFractions);
			for (int i = 0; i < QuestNode_SplitRandomly.tmpValues.Count; i++)
			{
				if (this.storeAsFormat.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAsFormat.GetValue(slate).Formatted(i.Named("INDEX")), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				if (i == 0 && this.storeAs1.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs1.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 1 && this.storeAs2.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs2.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 2 && this.storeAs3.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs3.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 3 && this.storeAs4.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs4.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 4 && this.storeAs5.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs5.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
			}
		}

		// Token: 0x04008927 RID: 35111
		[NoTranslate]
		public SlateRef<string> storeAsFormat;

		// Token: 0x04008928 RID: 35112
		[NoTranslate]
		public SlateRef<string> storeAs1;

		// Token: 0x04008929 RID: 35113
		[NoTranslate]
		public SlateRef<string> storeAs2;

		// Token: 0x0400892A RID: 35114
		[NoTranslate]
		public SlateRef<string> storeAs3;

		// Token: 0x0400892B RID: 35115
		[NoTranslate]
		public SlateRef<string> storeAs4;

		// Token: 0x0400892C RID: 35116
		[NoTranslate]
		public SlateRef<string> storeAs5;

		// Token: 0x0400892D RID: 35117
		public SlateRef<float?> valueToSplit;

		// Token: 0x0400892E RID: 35118
		public SlateRef<int> countToSplit;

		// Token: 0x0400892F RID: 35119
		public SlateRef<float> zeroIfFractionBelow1;

		// Token: 0x04008930 RID: 35120
		public SlateRef<float> zeroIfFractionBelow2;

		// Token: 0x04008931 RID: 35121
		public SlateRef<float> zeroIfFractionBelow3;

		// Token: 0x04008932 RID: 35122
		public SlateRef<float> zeroIfFractionBelow4;

		// Token: 0x04008933 RID: 35123
		public SlateRef<float> zeroIfFractionBelow5;

		// Token: 0x04008934 RID: 35124
		public SlateRef<float> minFraction1;

		// Token: 0x04008935 RID: 35125
		public SlateRef<float> minFraction2;

		// Token: 0x04008936 RID: 35126
		public SlateRef<float> minFraction3;

		// Token: 0x04008937 RID: 35127
		public SlateRef<float> minFraction4;

		// Token: 0x04008938 RID: 35128
		public SlateRef<float> minFraction5;

		// Token: 0x04008939 RID: 35129
		private static List<float> tmpValues = new List<float>();

		// Token: 0x0400893A RID: 35130
		private static List<float> tmpZeroIfFractionBelow = new List<float>();

		// Token: 0x0400893B RID: 35131
		private static List<float> tmpMinFractions = new List<float>();
	}
}
