using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001938 RID: 6456
	public class CompProperties_PolluteOverTime : CompProperties
	{
		// Token: 0x0600A1EE RID: 41454 RVA: 0x0039DF10 File Offset: 0x0039C110
		public CompProperties_PolluteOverTime()
		{
			this.compClass = typeof(CompPolluteOverTime);
		}

		// Token: 0x04005FA7 RID: 24487
		public int cellsToPollutePerDay;
	}
}
