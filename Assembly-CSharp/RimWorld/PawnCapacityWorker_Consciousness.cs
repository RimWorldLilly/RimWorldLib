using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BC RID: 5308
	public class PawnCapacityWorker_Consciousness : PawnCapacityWorker
	{
		// Token: 0x060081E8 RID: 33256 RVA: 0x002E5D00 File Offset: 0x002E3F00
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.ConsciousnessSource, float.MaxValue, default(FloatRange), impactors, -1f);
			float num2 = Mathf.Clamp(GenMath.LerpDouble(0.1f, 1f, 0f, 0.4f, diffSet.PainTotal), 0f, 0.4f);
			if ((double)num2 >= 0.01)
			{
				num -= num2;
				if (impactors != null)
				{
					impactors.Add(new PawnCapacityUtility.CapacityImpactorPain());
				}
			}
			num = Mathf.Lerp(num, num * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.BloodPumping, impactors), 1f), 0.2f);
			num = Mathf.Lerp(num, num * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Breathing, impactors), 1f), 0.2f);
			return Mathf.Lerp(num, num * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.BloodFiltration, impactors), 1f), 0.1f);
		}

		// Token: 0x060081E9 RID: 33257 RVA: 0x002E5DED File Offset: 0x002E3FED
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ConsciousnessSource);
		}
	}
}
