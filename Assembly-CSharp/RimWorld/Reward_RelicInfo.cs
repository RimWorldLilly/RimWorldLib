using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F40 RID: 8000
	public class Reward_RelicInfo : Reward
	{
		// Token: 0x17002537 RID: 9527
		// (get) Token: 0x0600CC1C RID: 52252 RVA: 0x004B3283 File Offset: 0x004B1483
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_RelicInfo.<get_StackElements>d__3 <get_StackElements>d__ = new Reward_RelicInfo.<get_StackElements>d__3(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CC1D RID: 52253 RVA: 0x004B3293 File Offset: 0x004B1493
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC1E RID: 52254 RVA: 0x004B329A File Offset: 0x004B149A
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC1F RID: 52255 RVA: 0x004B32A1 File Offset: 0x004B14A1
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_RelicInfo".Translate(this.relic.Label);
		}

		// Token: 0x0600CC20 RID: 52256 RVA: 0x004B32C2 File Offset: 0x004B14C2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Precept_Relic>(ref this.relic, "relic", false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x0400820A RID: 33290
		public Precept_Relic relic;

		// Token: 0x0400820B RID: 33291
		public Quest quest;
	}
}
