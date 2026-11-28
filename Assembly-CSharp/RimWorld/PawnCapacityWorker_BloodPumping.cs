using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BA RID: 5306
	public class PawnCapacityWorker_BloodPumping : PawnCapacityWorker
	{
		// Token: 0x060081E2 RID: 33250 RVA: 0x002E5C34 File Offset: 0x002E3E34
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodPumpingSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x060081E3 RID: 33251 RVA: 0x002E5C62 File Offset: 0x002E3E62
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BloodPumpingSource);
		}
	}
}
