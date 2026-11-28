using System;

namespace RimWorld
{
	// Token: 0x02000E05 RID: 3589
	public class Thought_HardWorkerVsLazy : Thought_SituationalSocial
	{
		// Token: 0x060060F5 RID: 24821 RVA: 0x00231278 File Offset: 0x0022F478
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			int num = this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.Industriousness);
			if (num > 0)
			{
				return 0f;
			}
			if (num == 0)
			{
				return -5f;
			}
			if (num == -1)
			{
				return -20f;
			}
			return -30f;
		}
	}
}
