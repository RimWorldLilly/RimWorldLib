using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A33 RID: 6707
	public class CompProperties_Useable_CallBossgroup : CompProperties_UseEffect
	{
		// Token: 0x0600A84B RID: 43083 RVA: 0x003BCD81 File Offset: 0x003BAF81
		public CompProperties_Useable_CallBossgroup()
		{
			this.compClass = typeof(CompUseEffect_CallBossgroup);
		}

		// Token: 0x0600A84C RID: 43084 RVA: 0x003BCDAB File Offset: 0x003BAFAB
		public override void Notify_PostUnlockedByResearch(ThingDef parent)
		{
			if (Find.TickManager.TicksGame > 0 && !this.unlockedLetterLabelKey.NullOrEmpty() && !this.unlockedLetterTextKey.NullOrEmpty())
			{
				this.SendBossgroupDetailsLetter(this.unlockedLetterLabelKey, this.unlockedLetterTextKey, parent);
			}
		}

		// Token: 0x0600A84D RID: 43085 RVA: 0x003BCDE8 File Offset: 0x003BAFE8
		public void SendBossgroupDetailsLetter(string labelKey, string textKey, ThingDef parent)
		{
			List<ThingDef> list = new List<ThingDef>
			{
				parent
			};
			list.AddRange(from t in this.bossgroupDef.boss.kindDef.race.killedLeavingsPlayerHostile
			select t.thingDef);
			Find.LetterStack.ReceiveLetter(this.FormatLetterLabel(labelKey), this.FormatLetterText(textKey, parent), LetterDefOf.NeutralEvent, null, null, null, list, null, 0, true);
		}

		// Token: 0x0600A84E RID: 43086 RVA: 0x003BCE75 File Offset: 0x003BB075
		public string FormatLetterLabel(string label)
		{
			return label.Translate(this.bossgroupDef.boss.kindDef.Named("LEADER"));
		}

		// Token: 0x0600A84F RID: 43087 RVA: 0x003BCE9C File Offset: 0x003BB09C
		public string FormatLetterText(string text, ThingDef parent)
		{
			string arg = (from r in this.bossgroupDef.boss.kindDef.race.killedLeavingsPlayerHostile
			select r.Label + " x" + r.count.ToString()).ToLineList("- ", false);
			return text.Translate(parent.Named("PARENT"), this.bossgroupDef.boss.kindDef.Named("LEADER"), arg.Named("REWARDSLIST"));
		}

		// Token: 0x040062DF RID: 25311
		public BossgroupDef bossgroupDef;

		// Token: 0x040062E0 RID: 25312
		public EffecterDef effecterDef;

		// Token: 0x040062E1 RID: 25313
		public EffecterDef prepareEffecterDef;

		// Token: 0x040062E2 RID: 25314
		[NoTranslate]
		public string spawnLetterTextKey;

		// Token: 0x040062E3 RID: 25315
		[NoTranslate]
		public string spawnLetterLabelKey;

		// Token: 0x040062E4 RID: 25316
		[NoTranslate]
		public string unlockedLetterTextKey;

		// Token: 0x040062E5 RID: 25317
		[NoTranslate]
		public string unlockedLetterLabelKey;

		// Token: 0x040062E6 RID: 25318
		public int delayTicks = -1;

		// Token: 0x040062E7 RID: 25319
		private List<ThingDef> tmpMechsUsingRewards = new List<ThingDef>();
	}
}
