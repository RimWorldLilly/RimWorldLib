using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F3A RID: 7994
	public class Reward_DevelopmentPoints : Reward
	{
		// Token: 0x0600CBEA RID: 52202 RVA: 0x004B26D9 File Offset: 0x004B08D9
		public Reward_DevelopmentPoints()
		{
		}

		// Token: 0x0600CBEB RID: 52203 RVA: 0x004B26E1 File Offset: 0x004B08E1
		public Reward_DevelopmentPoints(Quest quest)
		{
			this.quest = quest;
		}

		// Token: 0x1700252F RID: 9519
		// (get) Token: 0x0600CBEC RID: 52204 RVA: 0x004B26F0 File Offset: 0x004B08F0
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_DevelopmentPoints.<get_StackElements>d__4 <get_StackElements>d__ = new Reward_DevelopmentPoints.<get_StackElements>d__4(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CBED RID: 52205 RVA: 0x004B2700 File Offset: 0x004B0900
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CBEE RID: 52206 RVA: 0x004B2707 File Offset: 0x004B0907
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CBEF RID: 52207 RVA: 0x004B2710 File Offset: 0x004B0910
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			Ideo fluidIdeo = Faction.OfPlayer.ideos.FluidIdeo;
			int value = IdeoDevelopmentUtility.DevelopmentPointsForQuestSuccess(fluidIdeo, this.quest.root);
			return "Reward_DevelopmentPoints".Translate(fluidIdeo, value, fluidIdeo.development.points, fluidIdeo.development.NextReformationDevelopmentPoints).Resolve();
		}

		// Token: 0x0600CBF0 RID: 52208 RVA: 0x004B277D File Offset: 0x004B097D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x040081FD RID: 33277
		public Quest quest;
	}
}
