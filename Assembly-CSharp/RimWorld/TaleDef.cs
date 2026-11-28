using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FFC RID: 4092
	public class TaleDef : Def
	{
		// Token: 0x06006737 RID: 26423 RVA: 0x00248E14 File Offset: 0x00247014
		public override IEnumerable<string> ConfigErrors()
		{
			TaleDef.<ConfigErrors>d__15 <ConfigErrors>d__ = new TaleDef.<ConfigErrors>d__15(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06006738 RID: 26424 RVA: 0x00248E24 File Offset: 0x00247024
		public static TaleDef Named(string str)
		{
			return DefDatabase<TaleDef>.GetNamed(str, true);
		}

		// Token: 0x040040F0 RID: 16624
		public TaleType type;

		// Token: 0x040040F1 RID: 16625
		public Type taleClass;

		// Token: 0x040040F2 RID: 16626
		public bool usableForArt = true;

		// Token: 0x040040F3 RID: 16627
		public bool colonistOnly = true;

		// Token: 0x040040F4 RID: 16628
		public bool usableWithChildren = true;

		// Token: 0x040040F5 RID: 16629
		public int maxPerPawn = -1;

		// Token: 0x040040F6 RID: 16630
		public float ignoreChance;

		// Token: 0x040040F7 RID: 16631
		public float expireDays = -1f;

		// Token: 0x040040F8 RID: 16632
		public RulePack rulePack;

		// Token: 0x040040F9 RID: 16633
		[NoTranslate]
		public string firstPawnSymbol;

		// Token: 0x040040FA RID: 16634
		[NoTranslate]
		public string secondPawnSymbol;

		// Token: 0x040040FB RID: 16635
		[NoTranslate]
		public string defSymbol;

		// Token: 0x040040FC RID: 16636
		public Type defType = typeof(ThingDef);

		// Token: 0x040040FD RID: 16637
		public float baseInterest;

		// Token: 0x040040FE RID: 16638
		public Color historyGraphColor = Color.white;
	}
}
