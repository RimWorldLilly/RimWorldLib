using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAC RID: 3500
	public class ThoughtWorker_Precept_HasProsthetic : ThoughtWorker_Precept
	{
		// Token: 0x0600600E RID: 24590 RVA: 0x0022E36B File Offset: 0x0022C56B
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_HasProsthetic.HasProsthetic(p);
		}

		// Token: 0x0600600F RID: 24591 RVA: 0x0022E378 File Offset: 0x0022C578
		public static bool HasProsthetic(Pawn p)
		{
			return ThoughtWorker_Precept_HasProsthetic_Count.ProstheticsCount(p) > 0;
		}
	}
}
