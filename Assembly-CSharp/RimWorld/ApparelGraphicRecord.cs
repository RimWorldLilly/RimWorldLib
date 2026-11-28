using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002078 RID: 8312
	public struct ApparelGraphicRecord
	{
		// Token: 0x0600D176 RID: 53622 RVA: 0x004D00C0 File Offset: 0x004CE2C0
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}

		// Token: 0x040084EB RID: 34027
		public Graphic graphic;

		// Token: 0x040084EC RID: 34028
		public Apparel sourceApparel;
	}
}
