using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA0 RID: 3488
	public class ThoughtWorker_Precept_AnyBodyPartButHairOrFaceCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06005FF0 RID: 24560 RVA: 0x0022DE36 File Offset: 0x0022C036
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_AnyBodyPartButHairOrFaceCovered.HasCoveredBodyPartsButHairOrFace(otherPawn);
		}
	}
}
