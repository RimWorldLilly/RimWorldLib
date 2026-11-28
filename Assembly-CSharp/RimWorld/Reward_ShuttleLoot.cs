using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F42 RID: 8002
	[StaticConstructorOnStartup]
	public class Reward_ShuttleLoot : Reward
	{
		// Token: 0x1700253A RID: 9530
		// (get) Token: 0x0600CC2F RID: 52271 RVA: 0x004B35AC File Offset: 0x004B17AC
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_ShuttleLoot.<get_StackElements>d__2 <get_StackElements>d__ = new Reward_ShuttleLoot.<get_StackElements>d__2(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CC30 RID: 52272 RVA: 0x004B35BC File Offset: 0x004B17BC
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC31 RID: 52273 RVA: 0x004B35C3 File Offset: 0x004B17C3
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CC32 RID: 52274 RVA: 0x004B35CC File Offset: 0x004B17CC
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_ShuttleLoot".Translate().Resolve();
		}

		// Token: 0x0400820E RID: 33294
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/CrashedShuttle", true);
	}
}
