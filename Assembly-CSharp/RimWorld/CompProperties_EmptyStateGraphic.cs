using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001872 RID: 6258
	public class CompProperties_EmptyStateGraphic : CompProperties
	{
		// Token: 0x06009C93 RID: 40083 RVA: 0x00385E92 File Offset: 0x00384092
		public CompProperties_EmptyStateGraphic()
		{
			this.compClass = typeof(CompEmptyStateGraphic);
		}

		// Token: 0x04005CF5 RID: 23797
		public GraphicData graphicData;

		// Token: 0x04005CF6 RID: 23798
		public bool alwaysDrawParent;
	}
}
