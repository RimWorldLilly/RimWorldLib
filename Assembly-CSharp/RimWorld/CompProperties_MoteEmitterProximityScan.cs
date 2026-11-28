using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001910 RID: 6416
	public class CompProperties_MoteEmitterProximityScan : CompProperties_MoteEmitter
	{
		// Token: 0x0600A0D6 RID: 41174 RVA: 0x0039828F File Offset: 0x0039648F
		public CompProperties_MoteEmitterProximityScan()
		{
			this.compClass = typeof(CompMoteEmitterProximityScan);
		}

		// Token: 0x04005EF6 RID: 24310
		public float warmupPulseFadeInTime;

		// Token: 0x04005EF7 RID: 24311
		public float warmupPulseSolidTime;

		// Token: 0x04005EF8 RID: 24312
		public float warmupPulseFadeOutTime;

		// Token: 0x04005EF9 RID: 24313
		public SoundDef soundEmitting;
	}
}
