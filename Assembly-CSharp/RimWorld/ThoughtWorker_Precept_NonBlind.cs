using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC7 RID: 3527
	public class ThoughtWorker_Precept_NonBlind : ThoughtWorker_Precept
	{
		// Token: 0x0600605F RID: 24671 RVA: 0x0022F297 File Offset: 0x0022D497
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return !PawnUtility.IsBiologicallyOrArtificiallyBlind(p);
		}
	}
}
