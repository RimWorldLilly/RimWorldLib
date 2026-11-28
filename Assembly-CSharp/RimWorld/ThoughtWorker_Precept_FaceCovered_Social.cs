using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA2 RID: 3490
	public class ThoughtWorker_Precept_FaceCovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06005FF5 RID: 24565 RVA: 0x0022DEDC File Offset: 0x0022C0DC
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_FaceCovered.HasCoveredFace(otherPawn);
		}
	}
}
