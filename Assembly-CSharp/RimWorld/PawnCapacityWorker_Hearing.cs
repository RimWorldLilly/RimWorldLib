using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BE RID: 5310
	public class PawnCapacityWorker_Hearing : PawnCapacityWorker
	{
		// Token: 0x060081EE RID: 33262 RVA: 0x002E5EA0 File Offset: 0x002E40A0
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.HearingSource, float.MaxValue, default(FloatRange), impactors, 0.75f);
		}

		// Token: 0x060081EF RID: 33263 RVA: 0x002E5ECE File Offset: 0x002E40CE
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.HearingSource);
		}
	}
}
