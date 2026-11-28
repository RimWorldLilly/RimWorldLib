using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F3B RID: 7995
	public class Reward_Goodwill : Reward
	{
		// Token: 0x17002530 RID: 9520
		// (get) Token: 0x0600CBF1 RID: 52209 RVA: 0x004B2796 File Offset: 0x004B0996
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_Goodwill.<get_StackElements>d__3 <get_StackElements>d__ = new Reward_Goodwill.<get_StackElements>d__3(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CBF2 RID: 52210 RVA: 0x004B27A8 File Offset: 0x004B09A8
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToGoodwillCurve.Evaluate(rewardValue));
			this.amount = Mathf.Min(this.amount, 100 - parms.giverFaction.PlayerGoodwill);
			this.amount = Mathf.Max(this.amount, 1);
			valueActuallyUsed = RewardsGenerator.RewardValueToGoodwillCurve.EvaluateInverted((float)this.amount);
			if (parms.giverFaction.HostileTo(Faction.OfPlayer))
			{
				this.amount += Mathf.Clamp(-parms.giverFaction.PlayerGoodwill / 2, 0, this.amount);
				this.amount = Mathf.Min(this.amount, 100 - parms.giverFaction.PlayerGoodwill);
				if (this.amount < 1)
				{
					Log.Warning("Tried to use " + this.amount.ToString() + " goodwill in Reward_Goodwill. A different reward type should have been chosen in this case.");
					this.amount = 1;
				}
			}
			this.faction = parms.giverFaction;
		}

		// Token: 0x0600CBF3 RID: 52211 RVA: 0x004B28A2 File Offset: 0x004B0AA2
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Reward_Goodwill.<GenerateQuestParts>d__5 <GenerateQuestParts>d__ = new Reward_Goodwill.<GenerateQuestParts>d__5(-2);
			<GenerateQuestParts>d__.<>4__this = this;
			return <GenerateQuestParts>d__;
		}

		// Token: 0x0600CBF4 RID: 52212 RVA: 0x004B28B4 File Offset: 0x004B0AB4
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_Goodwill".Translate(this.faction, this.amount).Resolve();
		}

		// Token: 0x0600CBF5 RID: 52213 RVA: 0x004B28EC File Offset: 0x004B0AEC
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

		// Token: 0x0600CBF6 RID: 52214 RVA: 0x004B2946 File Offset: 0x004B0B46
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x040081FE RID: 33278
		public int amount;

		// Token: 0x040081FF RID: 33279
		public Faction faction;
	}
}
