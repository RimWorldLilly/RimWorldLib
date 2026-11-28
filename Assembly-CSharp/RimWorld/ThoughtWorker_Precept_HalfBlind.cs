using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC5 RID: 3525
	public class ThoughtWorker_Precept_HalfBlind : ThoughtWorker_Precept
	{
		// Token: 0x0600605A RID: 24666 RVA: 0x0022F25B File Offset: 0x0022D45B
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_HalfBlind.IsHalfBlind(p);
		}

		// Token: 0x0600605B RID: 24667 RVA: 0x0022F268 File Offset: 0x0022D468
		public static bool IsHalfBlind(Pawn p)
		{
			return !PawnUtility.IsBiologicallyOrArtificiallyBlind(p) && HealthUtility.IsMissingSightBodyPart(p);
		}
	}
}
