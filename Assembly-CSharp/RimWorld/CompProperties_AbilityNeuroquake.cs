using System;

namespace RimWorld
{
	// Token: 0x02001421 RID: 5153
	public class CompProperties_AbilityNeuroquake : CompProperties_AbilityEffect
	{
		// Token: 0x06007F50 RID: 32592 RVA: 0x002D7968 File Offset: 0x002D5B68
		public CompProperties_AbilityNeuroquake()
		{
			this.compClass = typeof(CompAbilityEffect_Neuroquake);
		}

		// Token: 0x04004FC1 RID: 20417
		public int goodwillImpactForNeuroquake;

		// Token: 0x04004FC2 RID: 20418
		public int goodwillImpactForBerserk;

		// Token: 0x04004FC3 RID: 20419
		public int worldRangeTiles;

		// Token: 0x04004FC4 RID: 20420
		public float mentalStateRadius;
	}
}
