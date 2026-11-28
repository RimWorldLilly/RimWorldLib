using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F37 RID: 7991
	[StaticConstructorOnStartup]
	public class Reward_BestowingCeremony : Reward
	{
		// Token: 0x1700252C RID: 9516
		// (get) Token: 0x0600CBD1 RID: 52177 RVA: 0x004B2453 File Offset: 0x004B0653
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_BestowingCeremony.<get_StackElements>d__7 <get_StackElements>d__ = new Reward_BestowingCeremony.<get_StackElements>d__7(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CBD2 RID: 52178 RVA: 0x004B2463 File Offset: 0x004B0663
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CBD3 RID: 52179 RVA: 0x004B246A File Offset: 0x004B066A
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CBD4 RID: 52180 RVA: 0x004B2474 File Offset: 0x004B0674
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_BestowingCeremony".Translate(this.targetPawnName.Named("PAWNNAME")).Resolve();
		}

		// Token: 0x0600CBD5 RID: 52181 RVA: 0x004B24A4 File Offset: 0x004B06A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.targetPawnName, "targetPawnName", null, false);
			Scribe_Values.Look<string>(ref this.titleName, "titleName", null, false);
			Scribe_Values.Look<bool>(ref this.givePsylink, "givePsylink", false, false);
			Scribe_References.Look<Faction>(ref this.awardingFaction, "awardingFaction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.royalTitle, "royalTitle");
		}

		// Token: 0x040081F5 RID: 33269
		private static readonly Texture2D IconPsylink = ContentFinder<Texture2D>.Get("Things/Item/Special/PsylinkNeuroformer", true);

		// Token: 0x040081F6 RID: 33270
		public string targetPawnName;

		// Token: 0x040081F7 RID: 33271
		public string titleName;

		// Token: 0x040081F8 RID: 33272
		public RoyalTitleDef royalTitle;

		// Token: 0x040081F9 RID: 33273
		public bool givePsylink;

		// Token: 0x040081FA RID: 33274
		public Faction awardingFaction;
	}
}
