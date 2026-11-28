using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021D9 RID: 8665
	public class QuestNode_GetRandomInRangeForChallengeRating : QuestNode
	{
		// Token: 0x0600D794 RID: 55188 RVA: 0x00500094 File Offset: 0x004FE294
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			float randomInRange = this.GetRangeFromRating().RandomInRange;
			slate.Set<float>(this.storeAs.GetValue(slate), this.roundRandom.GetValue(slate) ? ((float)GenMath.RoundRandom(randomInRange)) : randomInRange, false);
		}

		// Token: 0x0600D795 RID: 55189 RVA: 0x005000E4 File Offset: 0x004FE2E4
		public FloatRange GetRangeFromRating()
		{
			int challengeRating = QuestGen.quest.challengeRating;
			Slate slate = QuestGen.slate;
			if (challengeRating == 3)
			{
				return this.threeStarRange.GetValue(slate);
			}
			if (challengeRating == 2)
			{
				return this.twoStarRange.GetValue(slate);
			}
			return this.oneStarRange.GetValue(slate);
		}

		// Token: 0x0600D796 RID: 55190 RVA: 0x00500130 File Offset: 0x004FE330
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), 0, false);
			return true;
		}

		// Token: 0x040088F5 RID: 35061
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088F6 RID: 35062
		public SlateRef<FloatRange> oneStarRange;

		// Token: 0x040088F7 RID: 35063
		public SlateRef<FloatRange> twoStarRange;

		// Token: 0x040088F8 RID: 35064
		public SlateRef<FloatRange> threeStarRange;

		// Token: 0x040088F9 RID: 35065
		public SlateRef<bool> roundRandom;
	}
}
