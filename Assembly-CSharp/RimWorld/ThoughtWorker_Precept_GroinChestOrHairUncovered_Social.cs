using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA8 RID: 3496
	public class ThoughtWorker_Precept_GroinChestOrHairUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06006004 RID: 24580 RVA: 0x0022E1AA File Offset: 0x0022C3AA
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinChestOrHairUncovered.HasUncoveredGroinChestOrHair(otherPawn);
		}
	}
}
