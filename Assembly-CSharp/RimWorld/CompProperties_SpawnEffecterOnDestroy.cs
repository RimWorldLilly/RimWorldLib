using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200197B RID: 6523
	public class CompProperties_SpawnEffecterOnDestroy : CompProperties
	{
		// Token: 0x0600A3D9 RID: 41945 RVA: 0x003A8CB7 File Offset: 0x003A6EB7
		public CompProperties_SpawnEffecterOnDestroy()
		{
			this.compClass = typeof(CompSpawnEffecterOnDestroy);
		}

		// Token: 0x040060CC RID: 24780
		public EffecterDef effect;
	}
}
