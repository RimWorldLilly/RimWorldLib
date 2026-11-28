using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001916 RID: 6422
	public class CompProperties_NoiseSource : CompProperties
	{
		// Token: 0x0600A109 RID: 41225 RVA: 0x003994A9 File Offset: 0x003976A9
		public CompProperties_NoiseSource()
		{
			this.compClass = typeof(CompNoiseSource);
		}

		// Token: 0x04005F21 RID: 24353
		public float radius;
	}
}
