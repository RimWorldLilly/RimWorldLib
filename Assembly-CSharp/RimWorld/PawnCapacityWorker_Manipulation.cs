using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BF RID: 5311
	public class PawnCapacityWorker_Manipulation : PawnCapacityWorker
	{
		// Token: 0x060081F1 RID: 33265 RVA: 0x002E5EE4 File Offset: 0x002E40E4
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = 0f;
			return PawnCapacityUtility.CalculateLimbEfficiency(diffSet, BodyPartTagDefOf.ManipulationLimbCore, BodyPartTagDefOf.ManipulationLimbSegment, BodyPartTagDefOf.ManipulationLimbDigit, 0.8f, out num, impactors) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x060081F2 RID: 33266 RVA: 0x002E5F22 File Offset: 0x002E4122
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ManipulationLimbCore);
		}
	}
}
