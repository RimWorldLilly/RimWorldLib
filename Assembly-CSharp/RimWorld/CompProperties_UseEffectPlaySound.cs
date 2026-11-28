using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A44 RID: 6724
	public class CompProperties_UseEffectPlaySound : CompProperties_UseEffect
	{
		// Token: 0x0600A889 RID: 43145 RVA: 0x003BDE4E File Offset: 0x003BC04E
		public CompProperties_UseEffectPlaySound()
		{
			this.compClass = typeof(CompUseEffect_PlaySound);
		}

		// Token: 0x040062FF RID: 25343
		public SoundDef soundOnUsed;
	}
}
