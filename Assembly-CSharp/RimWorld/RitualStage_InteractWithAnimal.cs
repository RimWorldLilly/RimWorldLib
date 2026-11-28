using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200167A RID: 5754
	public class RitualStage_InteractWithAnimal : RitualStage
	{
		// Token: 0x0600904E RID: 36942 RVA: 0x0034FB78 File Offset: 0x0034DD78
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.assignments.Participants.FirstOrDefault((Pawn p) => p.RaceProps.Animal);
		}
	}
}
