using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001679 RID: 5753
	public class RitualStage_InteractWithPrisoner : RitualStage
	{
		// Token: 0x0600904C RID: 36940 RVA: 0x0034FB3A File Offset: 0x0034DD3A
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.assignments.Participants.FirstOrDefault((Pawn p) => p.IsPrisonerOfColony);
		}
	}
}
