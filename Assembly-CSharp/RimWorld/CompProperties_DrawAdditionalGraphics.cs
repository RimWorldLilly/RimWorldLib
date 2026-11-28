using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001868 RID: 6248
	public class CompProperties_DrawAdditionalGraphics : CompProperties
	{
		// Token: 0x06009C51 RID: 40017 RVA: 0x003846AA File Offset: 0x003828AA
		public CompProperties_DrawAdditionalGraphics()
		{
			this.compClass = typeof(CompDrawAdditionalGraphics);
		}

		// Token: 0x04005CDF RID: 23775
		public List<GraphicData> graphics;
	}
}
