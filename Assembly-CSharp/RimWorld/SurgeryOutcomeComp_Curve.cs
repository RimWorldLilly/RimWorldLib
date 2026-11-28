using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002019 RID: 8217
	public abstract class SurgeryOutcomeComp_Curve : SurgeryOutcomeComp
	{
		// Token: 0x0600CFFE RID: 53246
		protected abstract float XGetter(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill);

		// Token: 0x0600CFFF RID: 53247 RVA: 0x004C7904 File Offset: 0x004C5B04
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality *= this.curve.Evaluate(this.XGetter(recipe, surgeon, patient, ingredients, part, bill));
		}

		// Token: 0x04008444 RID: 33860
		public SimpleCurve curve;
	}
}
