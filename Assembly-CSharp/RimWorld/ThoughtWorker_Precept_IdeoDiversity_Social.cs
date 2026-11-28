using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB1 RID: 3505
	public class ThoughtWorker_Precept_IdeoDiversity_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06006019 RID: 24601 RVA: 0x0022E4F1 File Offset: 0x0022C6F1
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return p.Faction == otherPawn.Faction && p.Ideo != otherPawn.Ideo && !otherPawn.DevelopmentalStage.Baby();
		}
	}
}
