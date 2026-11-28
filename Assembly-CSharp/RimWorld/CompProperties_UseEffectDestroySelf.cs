using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A35 RID: 6709
	public class CompProperties_UseEffectDestroySelf : CompProperties_UseEffect
	{
		// Token: 0x0600A859 RID: 43097 RVA: 0x003BD206 File Offset: 0x003BB406
		public CompProperties_UseEffectDestroySelf()
		{
			this.compClass = typeof(CompUseEffect_DestroySelf);
		}

		// Token: 0x040062E9 RID: 25321
		public int delayTicks = -1;

		// Token: 0x040062EA RID: 25322
		public float orderPriority = -1000f;

		// Token: 0x040062EB RID: 25323
		public EffecterDef effecterDef;

		// Token: 0x040062EC RID: 25324
		public bool spawnKilledLeavings;

		// Token: 0x040062ED RID: 25325
		public List<ThingDefCountClass> leavings;
	}
}
