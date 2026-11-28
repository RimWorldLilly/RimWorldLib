using System;

namespace RimWorld
{
	// Token: 0x02000E09 RID: 3593
	public class Thought_TeetotalerVsChemicalInterest : Thought_SituationalSocial
	{
		// Token: 0x060060FD RID: 24829 RVA: 0x00231464 File Offset: 0x0022F664
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			int num = this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			if (num <= 0)
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
