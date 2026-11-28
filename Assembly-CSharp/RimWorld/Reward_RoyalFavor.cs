using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F41 RID: 8001
	public class Reward_RoyalFavor : Reward
	{
		// Token: 0x17002538 RID: 9528
		// (get) Token: 0x0600CC25 RID: 52261 RVA: 0x004B3380 File Offset: 0x004B1580
		public override bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002539 RID: 9529
		// (get) Token: 0x0600CC26 RID: 52262 RVA: 0x004B3383 File Offset: 0x004B1583
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_RoyalFavor.<get_StackElements>d__5 <get_StackElements>d__ = new Reward_RoyalFavor.<get_StackElements>d__5(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CC27 RID: 52263 RVA: 0x004B3394 File Offset: 0x004B1594
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToRoyalFavorCurve.Evaluate(rewardValue));
			this.amount = Mathf.Clamp(this.amount, 1, 12);
			valueActuallyUsed = RewardsGenerator.RewardValueToRoyalFavorCurve.EvaluateInverted((float)this.amount);
			this.faction = parms.giverFaction;
		}

		// Token: 0x0600CC28 RID: 52264 RVA: 0x004B33EA File Offset: 0x004B15EA
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Reward_RoyalFavor.<GenerateQuestParts>d__7 <GenerateQuestParts>d__ = new Reward_RoyalFavor.<GenerateQuestParts>d__7(-2);
			<GenerateQuestParts>d__.<>4__this = this;
			<GenerateQuestParts>d__.<>3__parms = parms;
			return <GenerateQuestParts>d__;
		}

		// Token: 0x0600CC29 RID: 52265 RVA: 0x004B3404 File Offset: 0x004B1604
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (!parms.chosenPawnSignal.NullOrEmpty())
			{
				return "Reward_RoyalFavor_ChoosePawn".Translate(this.faction, this.amount, Faction.OfPlayer.def.pawnsPlural).Resolve();
			}
			return "Reward_RoyalFavor".Translate(this.faction, this.amount).Resolve();
		}

		// Token: 0x0600CC2A RID: 52266 RVA: 0x004B3484 File Offset: 0x004B1684
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.GetType().Name,
				" (faction=",
				this.faction.ToStringSafe<Faction>(),
				", amount=",
				this.amount.ToString(),
				")"
			});
		}

		// Token: 0x0600CC2B RID: 52267 RVA: 0x004B34DE File Offset: 0x004B16DE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x0400820C RID: 33292
		public int amount;

		// Token: 0x0400820D RID: 33293
		public Faction faction;
	}
}
