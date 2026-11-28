using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EAF RID: 7855
	[DefOf]
	public static class ResearchProjectDefOf
	{
		// Token: 0x0600CA6A RID: 51818 RVA: 0x004A5FCC File Offset: 0x004A41CC
		static ResearchProjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ResearchProjectDefOf));
		}

		// Token: 0x04007E91 RID: 32401
		public static ResearchProjectDef CarpetMaking;

		// Token: 0x04007E92 RID: 32402
		public static ResearchProjectDef MicroelectronicsBasics;

		// Token: 0x04007E93 RID: 32403
		public static ResearchProjectDef Batteries;

		// Token: 0x04007E94 RID: 32404
		public static ResearchProjectDef ColoredLights;

		// Token: 0x04007E95 RID: 32405
		[MayRequireBiotech]
		public static ResearchProjectDef BasicMechtech;

		// Token: 0x04007E96 RID: 32406
		[MayRequireBiotech]
		public static ResearchProjectDef Archogenetics;

		// Token: 0x04007E97 RID: 32407
		[MayRequireAnomaly]
		public static ResearchProjectDef BasicPsychicRituals;

		// Token: 0x04007E98 RID: 32408
		[MayRequireAnomaly]
		public static ResearchProjectDef AdvancedPsychicRituals;

		// Token: 0x04007E99 RID: 32409
		[MayRequireAnomaly]
		public static ResearchProjectDef BioferriteExtraction;

		// Token: 0x04007E9A RID: 32410
		[MayRequireOdyssey]
		public static ResearchProjectDef Fishing;

		// Token: 0x04007E9B RID: 32411
		[MayRequireOdyssey]
		public static ResearchProjectDef BasicGravtech;
	}
}
