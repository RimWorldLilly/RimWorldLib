using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F3D RID: 7997
	[StaticConstructorOnStartup]
	public class Reward_PassageOffworld : Reward
	{
		// Token: 0x17002534 RID: 9524
		// (get) Token: 0x0600CC07 RID: 52231 RVA: 0x004B2F1A File Offset: 0x004B111A
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_PassageOffworld.<get_StackElements>d__2 <get_StackElements>d__ = new Reward_PassageOffworld.<get_StackElements>d__2(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CC08 RID: 52232 RVA: 0x004B2F2A File Offset: 0x004B112A
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC09 RID: 52233 RVA: 0x004B2F31 File Offset: 0x004B1131
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC0A RID: 52234 RVA: 0x004B2F38 File Offset: 0x004B1138
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_PassageOffworld".Translate().Resolve();
		}

		// Token: 0x04008204 RID: 33284
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Stars", true);
	}
}
