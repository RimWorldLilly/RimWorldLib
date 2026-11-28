using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001434 RID: 5172
	public class CompProperties_AbilityReleaseGas : CompProperties_AbilityEffect
	{
		// Token: 0x06007F8A RID: 32650 RVA: 0x002D8ADD File Offset: 0x002D6CDD
		public CompProperties_AbilityReleaseGas()
		{
			this.compClass = typeof(CompAbilityEffect_ReleaseGas);
		}

		// Token: 0x04004FD5 RID: 20437
		public GasType gasType;

		// Token: 0x04004FD6 RID: 20438
		public int cellsToFill;
	}
}
