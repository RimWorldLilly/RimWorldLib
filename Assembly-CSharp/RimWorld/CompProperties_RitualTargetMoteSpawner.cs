using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A29 RID: 6697
	public class CompProperties_RitualTargetMoteSpawner : CompProperties
	{
		// Token: 0x0600A82B RID: 43051 RVA: 0x003BC879 File Offset: 0x003BAA79
		public CompProperties_RitualTargetMoteSpawner()
		{
			this.compClass = typeof(CompRitualTargetMoteSpawner);
		}

		// Token: 0x040062D5 RID: 25301
		public ThingDef mote;
	}
}
