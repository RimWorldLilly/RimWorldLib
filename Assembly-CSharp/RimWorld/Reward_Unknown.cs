using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F43 RID: 8003
	[StaticConstructorOnStartup]
	public class Reward_Unknown : Reward
	{
		// Token: 0x1700253B RID: 9531
		// (get) Token: 0x0600CC36 RID: 52278 RVA: 0x004B3633 File Offset: 0x004B1833
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_Unknown.<get_StackElements>d__2 <get_StackElements>d__ = new Reward_Unknown.<get_StackElements>d__2(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CC37 RID: 52279 RVA: 0x004B3643 File Offset: 0x004B1843
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC38 RID: 52280 RVA: 0x004B364A File Offset: 0x004B184A
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC39 RID: 52281 RVA: 0x004B3654 File Offset: 0x004B1854
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_Unknown".Translate().Resolve();
		}

		// Token: 0x0400820F RID: 33295
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);
	}
}
