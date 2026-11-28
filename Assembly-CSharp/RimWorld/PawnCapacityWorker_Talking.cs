using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C3 RID: 5315
	public class PawnCapacityWorker_Talking : PawnCapacityWorker
	{
		// Token: 0x060081FE RID: 33278 RVA: 0x002E60DC File Offset: 0x002E42DC
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.TalkingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.TalkingPathway, 1f, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.Tongue, 1f, default(FloatRange), impactors, -1f) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x060081FF RID: 33279 RVA: 0x002E615C File Offset: 0x002E435C
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.TalkingSource);
		}
	}
}
