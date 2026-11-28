using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189F RID: 6303
	public class CompProperties_GenepackContainer : CompProperties
	{
		// Token: 0x06009DDA RID: 40410 RVA: 0x0038B1BC File Offset: 0x003893BC
		public CompProperties_GenepackContainer()
		{
			this.compClass = typeof(CompGenepackContainer);
		}

		// Token: 0x04005D6C RID: 23916
		public int maxCapacity;
	}
}
