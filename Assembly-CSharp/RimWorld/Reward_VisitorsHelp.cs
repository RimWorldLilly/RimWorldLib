using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F44 RID: 8004
	[StaticConstructorOnStartup]
	public class Reward_VisitorsHelp : Reward
	{
		// Token: 0x1700253C RID: 9532
		// (get) Token: 0x0600CC3D RID: 52285 RVA: 0x004B36BB File Offset: 0x004B18BB
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_VisitorsHelp.<get_StackElements>d__2 <get_StackElements>d__ = new Reward_VisitorsHelp.<get_StackElements>d__2(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CC3E RID: 52286 RVA: 0x004B36CB File Offset: 0x004B18CB
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC3F RID: 52287 RVA: 0x004B36D2 File Offset: 0x004B18D2
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC40 RID: 52288 RVA: 0x004B36DC File Offset: 0x004B18DC
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_VisitorsHelp".Translate().Resolve();
		}

		// Token: 0x04008210 RID: 33296
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/VisitorsHelp", true);
	}
}
