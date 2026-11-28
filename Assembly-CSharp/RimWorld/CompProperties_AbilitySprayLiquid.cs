using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200144A RID: 5194
	public class CompProperties_AbilitySprayLiquid : CompProperties_AbilityEffect
	{
		// Token: 0x06007FD5 RID: 32725 RVA: 0x002D9A6F File Offset: 0x002D7C6F
		public CompProperties_AbilitySprayLiquid()
		{
			this.compClass = typeof(CompAbilityEffect_SprayLiquid);
		}

		// Token: 0x04004FF3 RID: 20467
		public ThingDef projectileDef;

		// Token: 0x04004FF4 RID: 20468
		public int numCellsToHit;

		// Token: 0x04004FF5 RID: 20469
		public EffecterDef sprayEffecter;
	}
}
