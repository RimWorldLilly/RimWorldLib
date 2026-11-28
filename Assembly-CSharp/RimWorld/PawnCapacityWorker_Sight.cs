using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C2 RID: 5314
	public class PawnCapacityWorker_Sight : PawnCapacityWorker
	{
		// Token: 0x060081FA RID: 33274 RVA: 0x002E6088 File Offset: 0x002E4288
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef sightSource = BodyPartTagDefOf.SightSource;
			float maxValue = float.MaxValue;
			float partEfficiencySpecialWeight = PawnCapacityWorker_Sight.PartEfficiencySpecialWeight;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, sightSource, maxValue, default(FloatRange), impactors, partEfficiencySpecialWeight);
		}

		// Token: 0x060081FB RID: 33275 RVA: 0x002E60B8 File Offset: 0x002E42B8
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.SightSource);
		}

		// Token: 0x0400507F RID: 20607
		public static float PartEfficiencySpecialWeight = 0.75f;
	}
}
