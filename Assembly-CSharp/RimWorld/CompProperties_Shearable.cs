using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE6 RID: 3814
	public class CompProperties_Shearable : CompProperties
	{
		// Token: 0x060063C2 RID: 25538 RVA: 0x0023C49D File Offset: 0x0023A69D
		public CompProperties_Shearable()
		{
			this.compClass = typeof(CompShearable);
		}

		// Token: 0x0400381D RID: 14365
		public int shearIntervalDays;

		// Token: 0x0400381E RID: 14366
		public int woolAmount = 1;

		// Token: 0x0400381F RID: 14367
		public ThingDef woolDef;
	}
}
