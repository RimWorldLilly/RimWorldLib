using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F36 RID: 7990
	[StaticConstructorOnStartup]
	public class Reward_ArchonexusMap : Reward
	{
		// Token: 0x1700252A RID: 9514
		// (get) Token: 0x0600CBC9 RID: 52169 RVA: 0x004B2399 File Offset: 0x004B0599
		private static Texture2D Icon
		{
			get
			{
				if (Reward_ArchonexusMap.icon == null)
				{
					Reward_ArchonexusMap.icon = ContentFinder<Texture2D>.Get("UI/Icons/ArchonexusMapPart", true);
				}
				return Reward_ArchonexusMap.icon;
			}
		}

		// Token: 0x1700252B RID: 9515
		// (get) Token: 0x0600CBCA RID: 52170 RVA: 0x004B23BD File Offset: 0x004B05BD
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_ArchonexusMap.<get_StackElements>d__6 <get_StackElements>d__ = new Reward_ArchonexusMap.<get_StackElements>d__6(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CBCB RID: 52171 RVA: 0x004B23CD File Offset: 0x004B05CD
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CBCC RID: 52172 RVA: 0x004B23D4 File Offset: 0x004B05D4
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600CBCD RID: 52173 RVA: 0x004B23DB File Offset: 0x004B05DB
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_ArchonexusMapPart".Translate(this.currentPart, 3);
		}

		// Token: 0x0600CBCE RID: 52174 RVA: 0x004B23FD File Offset: 0x004B05FD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.currentPart, "currentPart", 0, false);
		}

		// Token: 0x040081F2 RID: 33266
		public int currentPart = 1;

		// Token: 0x040081F3 RID: 33267
		private const int totalParts = 3;

		// Token: 0x040081F4 RID: 33268
		private static Texture2D icon;
	}
}
