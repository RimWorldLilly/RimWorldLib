using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCB RID: 3531
	public class ThoughtWorker_Precept_NonBlind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06006067 RID: 24679 RVA: 0x0022F2EE File Offset: 0x0022D4EE
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return !PawnUtility.IsBiologicallyBlind(otherPawn);
		}
	}
}
