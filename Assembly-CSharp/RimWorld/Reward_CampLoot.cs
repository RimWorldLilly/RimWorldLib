using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F38 RID: 7992
	[StaticConstructorOnStartup]
	public class Reward_CampLoot : Reward
	{
		// Token: 0x1700252D RID: 9517
		// (get) Token: 0x0600CBDB RID: 52187 RVA: 0x004B25D3 File Offset: 0x004B07D3
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_CampLoot.<get_StackElements>d__2 <get_StackElements>d__ = new Reward_CampLoot.<get_StackElements>d__2(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CBDC RID: 52188 RVA: 0x004B25E3 File Offset: 0x004B07E3
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CBDD RID: 52189 RVA: 0x004B25EA File Offset: 0x004B07EA
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CBDE RID: 52190 RVA: 0x004B25F4 File Offset: 0x004B07F4
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_CampLoot".Translate().Resolve();
		}

		// Token: 0x040081FB RID: 33275
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);
	}
}
