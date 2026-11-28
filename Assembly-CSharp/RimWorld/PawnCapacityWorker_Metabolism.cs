using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C0 RID: 5312
	public class PawnCapacityWorker_Metabolism : PawnCapacityWorker
	{
		// Token: 0x060081F4 RID: 33268 RVA: 0x002E5F38 File Offset: 0x002E4138
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.MetabolismSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x060081F5 RID: 33269 RVA: 0x002E5F66 File Offset: 0x002E4166
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MetabolismSource);
		}
	}
}
