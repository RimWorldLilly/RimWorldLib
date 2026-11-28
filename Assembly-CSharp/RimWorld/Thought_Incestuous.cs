using System;

namespace RimWorld
{
	// Token: 0x02000E02 RID: 3586
	public class Thought_Incestuous : Thought_SituationalSocial
	{
		// Token: 0x060060EF RID: 24815 RVA: 0x002310FC File Offset: 0x0022F2FC
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			return LovePartnerRelationUtility.IncestOpinionOffsetFor(this.otherPawn, this.pawn);
		}
	}
}
