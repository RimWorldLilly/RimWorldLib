using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9E RID: 3486
	public class ThoughtWorker_Precept_AnyBodyPartButGroinCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06005FEB RID: 24555 RVA: 0x0022DD12 File Offset: 0x0022BF12
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_AnyBodyPartButGroinCovered.HasCoveredBodyPartsButGroin(otherPawn);
		}
	}
}
