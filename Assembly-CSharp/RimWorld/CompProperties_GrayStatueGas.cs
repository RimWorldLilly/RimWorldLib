using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019EE RID: 6638
	public class CompProperties_GrayStatueGas : CompProperties_GrayStatue
	{
		// Token: 0x0600A72E RID: 42798 RVA: 0x003B9066 File Offset: 0x003B7266
		public CompProperties_GrayStatueGas()
		{
			this.compClass = typeof(CompGrayStatueGas);
		}

		// Token: 0x0400627A RID: 25210
		public GasType gas = GasType.DeadlifeDust;
	}
}
