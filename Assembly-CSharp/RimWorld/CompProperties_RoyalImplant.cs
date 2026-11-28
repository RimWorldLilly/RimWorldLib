using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195E RID: 6494
	public class CompProperties_RoyalImplant : CompProperties
	{
		// Token: 0x0600A30A RID: 41738 RVA: 0x003A3C9A File Offset: 0x003A1E9A
		public CompProperties_RoyalImplant()
		{
			this.compClass = typeof(CompRoyalImplant);
		}

		// Token: 0x04006054 RID: 24660
		public HediffDef implantHediff;
	}
}
