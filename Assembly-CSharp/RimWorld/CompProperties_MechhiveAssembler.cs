using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018EE RID: 6382
	public class CompProperties_MechhiveAssembler : CompProperties
	{
		// Token: 0x0600A008 RID: 40968 RVA: 0x0039548B File Offset: 0x0039368B
		public CompProperties_MechhiveAssembler()
		{
			this.compClass = typeof(CompMechhiveAssembler);
		}

		// Token: 0x04005EA4 RID: 24228
		public List<PawnGenOption> options = new List<PawnGenOption>();
	}
}
