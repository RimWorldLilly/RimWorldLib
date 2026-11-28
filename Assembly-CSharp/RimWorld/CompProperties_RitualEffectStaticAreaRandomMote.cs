using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173C RID: 5948
	public class CompProperties_RitualEffectStaticAreaRandomMote : CompProperties_RitualEffectIntervalSpawnArea
	{
		// Token: 0x06009433 RID: 37939 RVA: 0x00360590 File Offset: 0x0035E790
		public CompProperties_RitualEffectStaticAreaRandomMote()
		{
			this.compClass = typeof(CompRitualEffect_StaticAreaRandomMote);
		}

		// Token: 0x04005983 RID: 22915
		public float minDist = 1.5f;

		// Token: 0x04005984 RID: 22916
		public List<ThingDef> moteDefs;
	}
}
