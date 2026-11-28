using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193D RID: 6461
	public class CompProperties_ProducesBioferrite : CompProperties
	{
		// Token: 0x0600A205 RID: 41477 RVA: 0x0039E5BD File Offset: 0x0039C7BD
		public CompProperties_ProducesBioferrite()
		{
			this.compClass = typeof(CompProducesBioferrite);
		}

		// Token: 0x04005FB2 RID: 24498
		public float bioferriteDensity = 1f;
	}
}
