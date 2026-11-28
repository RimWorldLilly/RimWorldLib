using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001461 RID: 5217
	public class CompProperties_Transmute : CompProperties_AbilityEffect
	{
		// Token: 0x06008026 RID: 32806 RVA: 0x002DB362 File Offset: 0x002D9562
		public CompProperties_Transmute()
		{
			this.compClass = typeof(CompAbilityEffect_Transmute);
		}

		// Token: 0x04005008 RID: 20488
		public List<CompProperties_Transmute.ElementRatio> elementRatios = new List<CompProperties_Transmute.ElementRatio>();

		// Token: 0x04005009 RID: 20489
		public List<ThingDef> outcomeItems = new List<ThingDef>();

		// Token: 0x0400500A RID: 20490
		public List<ThingDef> outcomeStuff = new List<ThingDef>();

		// Token: 0x0400500B RID: 20491
		[MustTranslate]
		public string failedMessage;

		// Token: 0x020032C0 RID: 12992
		public class ElementRatio
		{
			// Token: 0x0400CBDB RID: 52187
			public ThingDef sourceStuff;

			// Token: 0x0400CBDC RID: 52188
			public float ratio = 1f;
		}
	}
}
