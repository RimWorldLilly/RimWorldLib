using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001978 RID: 6520
	public class CompProperties_SmokeCloudMaker : CompProperties
	{
		// Token: 0x0600A3CD RID: 41933 RVA: 0x003A875B File Offset: 0x003A695B
		public CompProperties_SmokeCloudMaker()
		{
			this.compClass = typeof(CompSmokeCloudMaker);
		}

		// Token: 0x040060C1 RID: 24769
		public EffecterDef sourceStreamEffect;

		// Token: 0x040060C2 RID: 24770
		public float cloudRadius;

		// Token: 0x040060C3 RID: 24771
		public FleckDef cloudFleck;

		// Token: 0x040060C4 RID: 24772
		public float fleckScale = 1f;

		// Token: 0x040060C5 RID: 24773
		public float fleckSpawnMTB;
	}
}
