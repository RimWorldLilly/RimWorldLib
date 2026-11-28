using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA6 RID: 3494
	public class ThoughtWorker_Precept_GroinOrChestUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06005FFF RID: 24575 RVA: 0x0022E06B File Offset: 0x0022C26B
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinOrChestUncovered.HasUncoveredGroinOrChest(otherPawn);
		}
	}
}
