using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173E RID: 5950
	public class CompProperties_RitualEffectIntervalSpawnCircle : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x06009439 RID: 37945 RVA: 0x00360714 File Offset: 0x0035E914
		public CompProperties_RitualEffectIntervalSpawnCircle()
		{
			this.compClass = typeof(CompRitualEffect_IntervalSpawnCircle);
		}

		// Token: 0x04005985 RID: 22917
		public IntVec2 area;

		// Token: 0x04005986 RID: 22918
		public float radius = 5f;

		// Token: 0x04005987 RID: 22919
		public float concentration = 1f;
	}
}
