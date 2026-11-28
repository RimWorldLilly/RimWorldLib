using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F3F RID: 7999
	[StaticConstructorOnStartup]
	public class Reward_PossibleFutureReward : Reward
	{
		// Token: 0x17002536 RID: 9526
		// (get) Token: 0x0600CC15 RID: 52245 RVA: 0x004B31FD File Offset: 0x004B13FD
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_PossibleFutureReward.<get_StackElements>d__2 <get_StackElements>d__ = new Reward_PossibleFutureReward.<get_StackElements>d__2(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CC16 RID: 52246 RVA: 0x004B320D File Offset: 0x004B140D
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC17 RID: 52247 RVA: 0x004B3214 File Offset: 0x004B1414
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC18 RID: 52248 RVA: 0x004B321C File Offset: 0x004B141C
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_PossibleFutureReward".Translate().Resolve();
		}

		// Token: 0x04008209 RID: 33289
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/QuestionMark", true);
	}
}
