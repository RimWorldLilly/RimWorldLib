using System;

namespace RimWorld
{
	// Token: 0x02000E0B RID: 3595
	public class Thought_ChemicalInterestVsTeetotaler : Thought_SituationalSocial
	{
		// Token: 0x06006101 RID: 24833 RVA: 0x0023155C File Offset: 0x0022F75C
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			int num = this.pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			if (this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) >= 0)
			{
				return 0f;
			}
			if (num == 1)
			{
				return -20f;
			}
			return -30f;
		}
	}
}
