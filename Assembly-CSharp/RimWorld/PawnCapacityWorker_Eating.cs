using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BD RID: 5309
	public class PawnCapacityWorker_Eating : PawnCapacityWorker
	{
		// Token: 0x060081EB RID: 33259 RVA: 0x002E5E04 File Offset: 0x002E4004
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.EatingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.EatingPathway, 1f, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.Tongue, float.MaxValue, new FloatRange(0.5f, 1f), impactors, -1f) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x060081EC RID: 33260 RVA: 0x002E5E88 File Offset: 0x002E4088
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.EatingSource);
		}
	}
}
