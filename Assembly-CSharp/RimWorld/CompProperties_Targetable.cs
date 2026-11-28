using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A02 RID: 6658
	public class CompProperties_Targetable : CompProperties_UseEffect
	{
		// Token: 0x0600A7A1 RID: 42913 RVA: 0x003BB302 File Offset: 0x003B9502
		public CompProperties_Targetable()
		{
			this.compClass = typeof(CompTargetable);
		}

		// Token: 0x040062A4 RID: 25252
		public bool psychicSensitiveTargetsOnly;

		// Token: 0x040062A5 RID: 25253
		public bool fleshCorpsesOnly;

		// Token: 0x040062A6 RID: 25254
		public bool nonDessicatedCorpsesOnly;

		// Token: 0x040062A7 RID: 25255
		public bool nonDownedPawnOnly;

		// Token: 0x040062A8 RID: 25256
		public bool ignoreQuestLodgerPawns;

		// Token: 0x040062A9 RID: 25257
		public bool ignorePlayerFactionPawns;

		// Token: 0x040062AA RID: 25258
		public MutantDef mutantFilter;

		// Token: 0x040062AB RID: 25259
		public ThingDef moteOnTarget;

		// Token: 0x040062AC RID: 25260
		public ThingDef moteConnecting;

		// Token: 0x040062AD RID: 25261
		public FleckDef fleckOnTarget;

		// Token: 0x040062AE RID: 25262
		public FleckDef fleckConnecting;

		// Token: 0x040062AF RID: 25263
		public HediffDef cannotHaveHediff;
	}
}
