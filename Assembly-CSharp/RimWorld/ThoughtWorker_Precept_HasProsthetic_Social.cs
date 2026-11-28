using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAD RID: 3501
	public class ThoughtWorker_Precept_HasProsthetic_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06006011 RID: 24593 RVA: 0x0022E38B File Offset: 0x0022C58B
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_HasProsthetic.HasProsthetic(otherPawn);
		}
	}
}
