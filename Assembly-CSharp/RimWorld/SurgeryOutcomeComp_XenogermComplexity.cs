using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200201B RID: 8219
	public class SurgeryOutcomeComp_XenogermComplexity : SurgeryOutcomeComp_Curve
	{
		// Token: 0x0600D003 RID: 53251 RVA: 0x004C79E4 File Offset: 0x004C5BE4
		protected override float XGetter(RecipeDef recipe, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			Xenogerm xenogerm = bill.xenogerm;
			if (((xenogerm != null) ? xenogerm.GeneSet : null) == null)
			{
				return 0f;
			}
			return (float)xenogerm.GeneSet.ComplexityTotal;
		}
	}
}
