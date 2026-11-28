using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001975 RID: 6517
	public class CompProperties_Shuttle : CompProperties
	{
		// Token: 0x0600A39D RID: 41885 RVA: 0x003A6AB0 File Offset: 0x003A4CB0
		public CompProperties_Shuttle()
		{
			this.compClass = typeof(CompShuttle);
		}

		// Token: 0x04006097 RID: 24727
		public TransportShipDef shipDef;
	}
}
