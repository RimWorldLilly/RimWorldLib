using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A3D RID: 6717
	public class CompProperties_UseEffectInstallImplant : CompProperties_UseEffect
	{
		// Token: 0x0600A874 RID: 43124 RVA: 0x003BD892 File Offset: 0x003BBA92
		public CompProperties_UseEffectInstallImplant()
		{
			this.compClass = typeof(CompUseEffect_InstallImplant);
		}

		// Token: 0x040062F4 RID: 25332
		public HediffDef hediffDef;

		// Token: 0x040062F5 RID: 25333
		public BodyPartDef bodyPart;

		// Token: 0x040062F6 RID: 25334
		public bool canUpgrade;

		// Token: 0x040062F7 RID: 25335
		public bool allowNonColonists;

		// Token: 0x040062F8 RID: 25336
		public bool requiresExistingHediff;

		// Token: 0x040062F9 RID: 25337
		public float maxSeverity = float.MaxValue;

		// Token: 0x040062FA RID: 25338
		public float minSeverity;

		// Token: 0x040062FB RID: 25339
		public bool requiresPsychicallySensitive;
	}
}
