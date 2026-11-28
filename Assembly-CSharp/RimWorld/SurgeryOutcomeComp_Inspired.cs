using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002014 RID: 8212
	public class SurgeryOutcomeComp_Inspired : SurgeryOutcomeComp_Factor
	{
		// Token: 0x0600CFF1 RID: 53233 RVA: 0x004C782E File Offset: 0x004C5A2E
		public override bool Affects(RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			return surgeon.InspirationDef == this.inspirationDef && !patient.RaceProps.IsMechanoid;
		}

		// Token: 0x0600CFF2 RID: 53234 RVA: 0x004C784E File Offset: 0x004C5A4E
		public override void PreApply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			surgeon.mindState.inspirationHandler.EndInspiration(this.inspirationDef);
		}

		// Token: 0x04008442 RID: 33858
		public InspirationDef inspirationDef;
	}
}
