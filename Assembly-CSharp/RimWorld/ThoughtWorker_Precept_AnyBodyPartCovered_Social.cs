using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9C RID: 3484
	public class ThoughtWorker_Precept_AnyBodyPartCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06005FE6 RID: 24550 RVA: 0x0022DBCA File Offset: 0x0022BDCA
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_AnyBodyPartCovered.HasUnnecessarilyCoveredBodyParts(otherPawn);
		}
	}
}
