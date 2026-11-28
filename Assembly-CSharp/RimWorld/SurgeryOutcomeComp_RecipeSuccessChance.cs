using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002016 RID: 8214
	public class SurgeryOutcomeComp_RecipeSuccessChance : SurgeryOutcomeComp
	{
		// Token: 0x0600CFF6 RID: 53238 RVA: 0x004C7889 File Offset: 0x004C5A89
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality *= recipe.surgerySuccessChanceFactor;
		}
	}
}
