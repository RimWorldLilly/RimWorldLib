using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BB RID: 5307
	public class PawnCapacityWorker_Breathing : PawnCapacityWorker
	{
		// Token: 0x060081E5 RID: 33253 RVA: 0x002E5C78 File Offset: 0x002E3E78
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingPathway, 1f, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingSourceCage, 1f, default(FloatRange), impactors, -1f);
		}

		// Token: 0x060081E6 RID: 33254 RVA: 0x002E5CEA File Offset: 0x002E3EEA
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BreathingSource);
		}
	}
}
