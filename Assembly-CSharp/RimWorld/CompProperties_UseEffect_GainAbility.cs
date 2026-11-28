using System;

namespace RimWorld
{
	// Token: 0x02001A39 RID: 6713
	public class CompProperties_UseEffect_GainAbility : CompProperties_UseEffect
	{
		// Token: 0x0600A868 RID: 43112 RVA: 0x003BD567 File Offset: 0x003BB767
		public CompProperties_UseEffect_GainAbility()
		{
			this.compClass = typeof(CompUseEffect_GainAbility);
		}

		// Token: 0x040062EF RID: 25327
		public AbilityDef ability;
	}
}
