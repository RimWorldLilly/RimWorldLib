using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022D1 RID: 8913
	[StaticConstructorOnStartup]
	public class Reward_ReimplantXenogerm : Reward
	{
		// Token: 0x17002639 RID: 9785
		// (get) Token: 0x0600DBD1 RID: 56273 RVA: 0x0051E748 File Offset: 0x0051C948
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_ReimplantXenogerm.<get_StackElements>d__3 <get_StackElements>d__ = new Reward_ReimplantXenogerm.<get_StackElements>d__3(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600DBD2 RID: 56274 RVA: 0x0051E758 File Offset: 0x0051C958
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DBD3 RID: 56275 RVA: 0x0051E75F File Offset: 0x0051C95F
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			return new Reward_ReimplantXenogerm.<GenerateQuestParts>d__5(-2);
		}

		// Token: 0x0600DBD4 RID: 56276 RVA: 0x0051E768 File Offset: 0x0051C968
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_ReimplantXenogerm_Desc".Translate().Resolve();
		}

		// Token: 0x04008D1B RID: 36123
		private static readonly CachedTexture Icon = new CachedTexture("UI/Icons/Genes/Gene_XenogermReimplanter");

		// Token: 0x04008D1C RID: 36124
		public const int ReimplantWaitDurationTicks = 15000;
	}
}
