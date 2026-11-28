using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A14 RID: 6676
	public class CompProperties_TargetEffectInstallImplantInOtherPawn : CompProperties
	{
		// Token: 0x0600A7E5 RID: 42981 RVA: 0x003BBBFE File Offset: 0x003B9DFE
		public CompProperties_TargetEffectInstallImplantInOtherPawn()
		{
			this.compClass = typeof(CompTargetEffect_InstallImplantInOtherPawn);
		}

		// Token: 0x040062B7 RID: 25271
		public HediffDef hediffDef;

		// Token: 0x040062B8 RID: 25272
		public BodyPartDef bodyPart;

		// Token: 0x040062B9 RID: 25273
		public bool canUpgrade;

		// Token: 0x040062BA RID: 25274
		public bool requiresExistingHediff;

		// Token: 0x040062BB RID: 25275
		public SoundDef soundOnUsed;
	}
}
