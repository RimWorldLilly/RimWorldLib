using System;

namespace RimWorld
{
	// Token: 0x020013FB RID: 5115
	public class CompProperties_AbilityFireBurst : CompProperties_AbilityEffect
	{
		// Token: 0x06007EDC RID: 32476 RVA: 0x002D59B0 File Offset: 0x002D3BB0
		public CompProperties_AbilityFireBurst()
		{
			this.compClass = typeof(CompAbilityEffect_FireBurst);
		}

		// Token: 0x04004F94 RID: 20372
		public float radius = 6f;
	}
}
