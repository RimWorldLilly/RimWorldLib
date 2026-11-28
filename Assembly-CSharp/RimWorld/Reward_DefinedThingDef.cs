using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F39 RID: 7993
	public class Reward_DefinedThingDef : Reward
	{
		// Token: 0x0600CBE2 RID: 52194 RVA: 0x004B265B File Offset: 0x004B085B
		public Reward_DefinedThingDef()
		{
		}

		// Token: 0x0600CBE3 RID: 52195 RVA: 0x004B2663 File Offset: 0x004B0863
		public Reward_DefinedThingDef(ThingDef thingDef)
		{
			this.thingDef = thingDef;
		}

		// Token: 0x1700252E RID: 9518
		// (get) Token: 0x0600CBE4 RID: 52196 RVA: 0x004B2672 File Offset: 0x004B0872
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_DefinedThingDef.<get_StackElements>d__4 <get_StackElements>d__ = new Reward_DefinedThingDef.<get_StackElements>d__4(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CBE5 RID: 52197 RVA: 0x004B2682 File Offset: 0x004B0882
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			valueActuallyUsed = rewardValue;
		}

		// Token: 0x0600CBE6 RID: 52198 RVA: 0x004B2687 File Offset: 0x004B0887
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			return new Reward_DefinedThingDef.<GenerateQuestParts>d__6(-2);
		}

		// Token: 0x0600CBE7 RID: 52199 RVA: 0x004B2690 File Offset: 0x004B0890
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return this.thingDef.description;
		}

		// Token: 0x0600CBE8 RID: 52200 RVA: 0x004B269D File Offset: 0x004B089D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x040081FC RID: 33276
		public ThingDef thingDef;
	}
}
