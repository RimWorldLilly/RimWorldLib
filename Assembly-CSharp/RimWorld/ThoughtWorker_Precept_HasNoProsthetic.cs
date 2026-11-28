using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAE RID: 3502
	public class ThoughtWorker_Precept_HasNoProsthetic : ThoughtWorker_Precept
	{
		// Token: 0x06006013 RID: 24595 RVA: 0x0022E3A0 File Offset: 0x0022C5A0
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return !ThoughtWorker_Precept_HasProsthetic.HasProsthetic(p);
		}
	}
}
