using System;

namespace RimWorld
{
	// Token: 0x02001740 RID: 5952
	public class CompProperties_RitualEffectIntervalSpawnDividedCircle : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x0600943D RID: 37949 RVA: 0x003607FA File Offset: 0x0035E9FA
		public CompProperties_RitualEffectIntervalSpawnDividedCircle()
		{
			this.compClass = typeof(CompRitualEffect_IntervalSpawnDividedCircleEffecter);
		}

		// Token: 0x04005988 RID: 22920
		public float radius = 5f;

		// Token: 0x04005989 RID: 22921
		public int numCopies = 5;
	}
}
