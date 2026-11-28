using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A46 RID: 6726
	public class CompProperties_UseEffectPlayWarmupSound : CompProperties_UseEffect
	{
		// Token: 0x0600A88D RID: 43149 RVA: 0x003BDEB3 File Offset: 0x003BC0B3
		public CompProperties_UseEffectPlayWarmupSound()
		{
			this.compClass = typeof(CompUseEffect_PlayWarmupSound);
		}

		// Token: 0x04006300 RID: 25344
		public SoundDef warmupSound;
	}
}
