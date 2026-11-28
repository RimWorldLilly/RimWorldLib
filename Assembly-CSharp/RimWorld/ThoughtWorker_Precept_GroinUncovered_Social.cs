using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA4 RID: 3492
	public class ThoughtWorker_Precept_GroinUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06005FFA RID: 24570 RVA: 0x0022DF84 File Offset: 0x0022C184
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinUncovered.HasUncoveredGroin(otherPawn);
		}
	}
}
