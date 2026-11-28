using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002022 RID: 8226
	public class SurgeryOutcome_HediffWithDuration : SurgeryOutcome
	{
		// Token: 0x0600D017 RID: 53271 RVA: 0x004C7C70 File Offset: 0x004C5E70
		public override bool Apply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			if (Rand.Chance(this.chance))
			{
				Hediff hd = HediffMaker.MakeHediff(this.hediff, patient, this.applyEffectsToPart ? part : null);
				hd.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.RoundToInt(this.qualityToDurationDaysCurve.Evaluate(quality) * 60000f);
				patient.health.AddHediff(hd, null, null, null);
				return true;
			}
			return false;
		}

		// Token: 0x0400844D RID: 33869
		public HediffDef hediff;

		// Token: 0x0400844E RID: 33870
		public SimpleCurve qualityToDurationDaysCurve;
	}
}
