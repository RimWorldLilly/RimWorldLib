using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC8 RID: 3528
	public class ThoughtWorker_Precept_Blind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06006061 RID: 24673 RVA: 0x0022F2AF File Offset: 0x0022D4AF
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return PawnUtility.IsBiologicallyBlind(otherPawn);
		}
	}
}
