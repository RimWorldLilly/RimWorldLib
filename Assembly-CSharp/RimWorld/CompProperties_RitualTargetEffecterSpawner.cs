using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A2B RID: 6699
	public class CompProperties_RitualTargetEffecterSpawner : CompProperties
	{
		// Token: 0x0600A831 RID: 43057 RVA: 0x003BC949 File Offset: 0x003BAB49
		public CompProperties_RitualTargetEffecterSpawner()
		{
			this.compClass = typeof(CompRitualTargetEffecterSpawner);
		}

		// Token: 0x040062D7 RID: 25303
		public EffecterDef effecter;

		// Token: 0x040062D8 RID: 25304
		public float minRitualProgress;
	}
}
