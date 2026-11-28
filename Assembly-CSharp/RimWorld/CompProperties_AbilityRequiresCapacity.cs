using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001470 RID: 5232
	public class CompProperties_AbilityRequiresCapacity : AbilityCompProperties
	{
		// Token: 0x0600806D RID: 32877 RVA: 0x002DCC9E File Offset: 0x002DAE9E
		public CompProperties_AbilityRequiresCapacity()
		{
			this.compClass = typeof(CompAbility_RequiresCapacity);
		}

		// Token: 0x0400501E RID: 20510
		public PawnCapacityDef capacity;
	}
}
