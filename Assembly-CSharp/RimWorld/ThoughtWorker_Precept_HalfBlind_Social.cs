using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC9 RID: 3529
	public class ThoughtWorker_Precept_HalfBlind_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06006063 RID: 24675 RVA: 0x0022F2C4 File Offset: 0x0022D4C4
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_HalfBlind.IsHalfBlind(otherPawn);
		}
	}
}
