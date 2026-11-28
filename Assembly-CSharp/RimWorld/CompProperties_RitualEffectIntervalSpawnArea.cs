using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173A RID: 5946
	public class CompProperties_RitualEffectIntervalSpawnArea : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x0600942F RID: 37935 RVA: 0x003604A9 File Offset: 0x0035E6A9
		public CompProperties_RitualEffectIntervalSpawnArea()
		{
			this.compClass = typeof(CompRitualEffect_IntervalSpawnArea);
		}

		// Token: 0x04005981 RID: 22913
		public IntVec2 area;

		// Token: 0x04005982 RID: 22914
		public bool smoothEdges = true;
	}
}
