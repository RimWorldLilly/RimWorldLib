using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAF RID: 3503
	public class ThoughtWorker_Precept_HasNoProsthetic_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06006015 RID: 24597 RVA: 0x0022E3B8 File Offset: 0x0022C5B8
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return !ThoughtWorker_Precept_HasProsthetic.HasProsthetic(otherPawn);
		}
	}
}
