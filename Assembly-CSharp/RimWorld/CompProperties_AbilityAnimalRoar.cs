using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D6 RID: 5078
	public class CompProperties_AbilityAnimalRoar : CompProperties_AbilityEffect
	{
		// Token: 0x06007E62 RID: 32354 RVA: 0x002D297A File Offset: 0x002D0B7A
		public CompProperties_AbilityAnimalRoar()
		{
			this.compClass = typeof(CompAbilityEffect_AnimalRoar);
		}

		// Token: 0x04004F54 RID: 20308
		public SimpleCurve chanceFromHearingCurve = new SimpleCurve();
	}
}
