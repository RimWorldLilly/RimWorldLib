using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDC RID: 3804
	public class CompProperties_Milkable : CompProperties
	{
		// Token: 0x060063A3 RID: 25507 RVA: 0x0023C00F File Offset: 0x0023A20F
		public CompProperties_Milkable()
		{
			this.compClass = typeof(CompMilkable);
		}

		// Token: 0x040037FC RID: 14332
		public int milkIntervalDays;

		// Token: 0x040037FD RID: 14333
		public int milkAmount = 1;

		// Token: 0x040037FE RID: 14334
		public ThingDef milkDef;

		// Token: 0x040037FF RID: 14335
		public bool milkFemaleOnly = true;
	}
}
