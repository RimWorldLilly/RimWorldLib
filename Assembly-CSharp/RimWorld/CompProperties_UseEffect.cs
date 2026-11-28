using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EEB RID: 3819
	public class CompProperties_UseEffect : CompProperties
	{
		// Token: 0x060063C9 RID: 25545 RVA: 0x0023C5F3 File Offset: 0x0023A7F3
		public CompProperties_UseEffect()
		{
			this.compClass = typeof(CompUseEffect);
		}

		// Token: 0x0400383D RID: 14397
		public bool doCameraShake;

		// Token: 0x0400383E RID: 14398
		public ThingDef moteOnUsed;

		// Token: 0x0400383F RID: 14399
		public float moteOnUsedScale = 1f;

		// Token: 0x04003840 RID: 14400
		public FleckDef fleckOnUsed;

		// Token: 0x04003841 RID: 14401
		public float fleckOnUsedScale = 1f;

		// Token: 0x04003842 RID: 14402
		public EffecterDef effecterOnUsed;

		// Token: 0x04003843 RID: 14403
		public EffecterDef warmupEffecter;
	}
}
