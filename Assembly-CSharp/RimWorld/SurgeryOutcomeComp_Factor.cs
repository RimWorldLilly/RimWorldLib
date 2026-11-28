using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002013 RID: 8211
	public class SurgeryOutcomeComp_Factor : SurgeryOutcomeComp
	{
		// Token: 0x0600CFEF RID: 53231 RVA: 0x004C780C File Offset: 0x004C5A0C
		public override void AffectQuality(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill, ref float quality)
		{
			quality *= this.factor;
		}

		// Token: 0x04008441 RID: 33857
		public float factor = 1f;
	}
}
