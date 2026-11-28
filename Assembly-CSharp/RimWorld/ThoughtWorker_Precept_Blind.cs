using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC4 RID: 3524
	public class ThoughtWorker_Precept_Blind : ThoughtWorker_Precept
	{
		// Token: 0x06006058 RID: 24664 RVA: 0x0022F246 File Offset: 0x0022D446
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return PawnUtility.IsBiologicallyBlind(p);
		}
	}
}
