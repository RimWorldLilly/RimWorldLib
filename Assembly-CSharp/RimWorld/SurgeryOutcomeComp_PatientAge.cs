using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200201C RID: 8220
	public class SurgeryOutcomeComp_PatientAge : SurgeryOutcomeComp_Curve
	{
		// Token: 0x0600D005 RID: 53253 RVA: 0x004C7A21 File Offset: 0x004C5C21
		protected override float XGetter(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			return patient.ageTracker.AgeBiologicalYearsFloat;
		}
	}
}
