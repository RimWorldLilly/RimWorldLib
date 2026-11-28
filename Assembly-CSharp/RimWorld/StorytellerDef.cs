using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FEC RID: 4076
	public class StorytellerDef : Def
	{
		// Token: 0x0600670F RID: 26383 RVA: 0x002488CC File Offset: 0x00246ACC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.portraitTiny.NullOrEmpty())
				{
					this.portraitTinyTex = ContentFinder<Texture2D>.Get(this.portraitTiny, true);
					this.portraitLargeTex = ContentFinder<Texture2D>.Get(this.portraitLarge, true);
				}
			});
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		// Token: 0x06006710 RID: 26384 RVA: 0x00248918 File Offset: 0x00246B18
		public override IEnumerable<string> ConfigErrors()
		{
			StorytellerDef.<ConfigErrors>d__23 <ConfigErrors>d__ = new StorytellerDef.<ConfigErrors>d__23(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040040A5 RID: 16549
		public int listOrder = 9999;

		// Token: 0x040040A6 RID: 16550
		public bool listVisible = true;

		// Token: 0x040040A7 RID: 16551
		public bool tutorialMode;

		// Token: 0x040040A8 RID: 16552
		public bool disableAdaptiveTraining;

		// Token: 0x040040A9 RID: 16553
		public bool disableAlerts;

		// Token: 0x040040AA RID: 16554
		public bool disablePermadeath;

		// Token: 0x040040AB RID: 16555
		public DifficultyDef forcedDifficulty;

		// Token: 0x040040AC RID: 16556
		[NoTranslate]
		private string portraitLarge;

		// Token: 0x040040AD RID: 16557
		[NoTranslate]
		private string portraitTiny;

		// Token: 0x040040AE RID: 16558
		public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

		// Token: 0x040040AF RID: 16559
		public SimpleCurve populationIntentFactorFromPopCurve;

		// Token: 0x040040B0 RID: 16560
		public SimpleCurve populationIntentFactorFromPopAdaptDaysCurve;

		// Token: 0x040040B1 RID: 16561
		public SimpleCurve pointsFactorFromDaysPassed;

		// Token: 0x040040B2 RID: 16562
		public float adaptDaysMin;

		// Token: 0x040040B3 RID: 16563
		public float adaptDaysMax = 100f;

		// Token: 0x040040B4 RID: 16564
		public float adaptDaysGameStartGraceDays;

		// Token: 0x040040B5 RID: 16565
		public SimpleCurve pointsFactorFromAdaptDays;

		// Token: 0x040040B6 RID: 16566
		public SimpleCurve adaptDaysLossFromColonistLostByPostPopulation;

		// Token: 0x040040B7 RID: 16567
		public SimpleCurve adaptDaysLossFromColonistViolentlyDownedByPopulation;

		// Token: 0x040040B8 RID: 16568
		public SimpleCurve adaptDaysGrowthRateCurve;

		// Token: 0x040040B9 RID: 16569
		[Unsaved(false)]
		public Texture2D portraitLargeTex;

		// Token: 0x040040BA RID: 16570
		[Unsaved(false)]
		public Texture2D portraitTinyTex;
	}
}
