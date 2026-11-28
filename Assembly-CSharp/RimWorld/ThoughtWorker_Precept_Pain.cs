using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D94 RID: 3476
	public class ThoughtWorker_Precept_Pain : ThoughtWorker_Precept
	{
		// Token: 0x06005FCC RID: 24524 RVA: 0x0022D597 File Offset: 0x0022B797
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Pain.CurrentThoughtState(p);
		}
	}
}
