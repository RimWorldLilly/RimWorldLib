using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019DA RID: 6618
	public class CompProperties_WasteProducer : CompProperties
	{
		// Token: 0x0600A6A2 RID: 42658 RVA: 0x003B6766 File Offset: 0x003B4966
		public CompProperties_WasteProducer()
		{
			this.compClass = typeof(CompWasteProducer);
		}

		// Token: 0x0400623A RID: 25146
		public bool showContentsInInspectPane = true;
	}
}
