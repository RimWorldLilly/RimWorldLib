using System;

namespace RimWorld
{
	// Token: 0x02001744 RID: 5956
	public class CompProperties_RitualEffectDrum : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x06009447 RID: 37959 RVA: 0x00360B18 File Offset: 0x0035ED18
		public CompProperties_RitualEffectDrum()
		{
			this.compClass = typeof(CompRitualEffect_Drum);
		}

		// Token: 0x0400598B RID: 22923
		public int maxDistance;

		// Token: 0x0400598C RID: 22924
		public float maxOffset;
	}
}
