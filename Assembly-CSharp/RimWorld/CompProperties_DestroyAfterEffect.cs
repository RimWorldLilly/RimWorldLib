using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ECD RID: 3789
	public class CompProperties_DestroyAfterEffect : CompProperties
	{
		// Token: 0x06006382 RID: 25474 RVA: 0x0023BA5E File Offset: 0x00239C5E
		public CompProperties_DestroyAfterEffect()
		{
			this.compClass = typeof(CompDestroyAfterEffect);
		}

		// Token: 0x040037A7 RID: 14247
		public EffecterDef effecterDef;
	}
}
