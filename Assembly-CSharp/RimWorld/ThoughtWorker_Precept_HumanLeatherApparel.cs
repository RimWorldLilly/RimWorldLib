using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D96 RID: 3478
	public class ThoughtWorker_Precept_HumanLeatherApparel : ThoughtWorker_Precept
	{
		// Token: 0x06005FD1 RID: 24529 RVA: 0x0022D656 File Offset: 0x0022B856
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_HumanLeatherApparel.CurrentThoughtState(p);
		}
	}
}
