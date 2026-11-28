using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCA RID: 3530
	public class ThoughtWorker_Precept_ArtificialBlind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06006065 RID: 24677 RVA: 0x0022F2D9 File Offset: 0x0022D4D9
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return PawnUtility.IsArtificiallyBlind(otherPawn);
		}
	}
}
