using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B9 RID: 5305
	public class PawnCapacityWorker_BloodFiltration : PawnCapacityWorker
	{
		// Token: 0x060081DF RID: 33247 RVA: 0x002E5B74 File Offset: 0x002E3D74
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			if (diffSet.pawn.RaceProps.body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney))
			{
				return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodFiltrationKidney, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodFiltrationLiver, float.MaxValue, default(FloatRange), impactors, -1f);
			}
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodFiltrationSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x060081E0 RID: 33248 RVA: 0x002E5C02 File Offset: 0x002E3E02
		public override bool CanHaveCapacity(BodyDef body)
		{
			return (body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney) && body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationLiver)) || body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationSource);
		}
	}
}
