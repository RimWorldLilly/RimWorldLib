using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B84 RID: 2948
	public class JoyGiver_VisitSickPawn : JoyGiver
	{
		// Token: 0x0600558D RID: 21901 RVA: 0x001FD16C File Offset: 0x001FB36C
		public override Job TryGiveJob(Pawn pawn)
		{
			if (!SocialInteractionUtility.CanInitiateInteraction(pawn, null))
			{
				return null;
			}
			Pawn pawn2 = SickPawnVisitUtility.FindRandomSickPawn(pawn, JoyCategory.Low);
			if (pawn2 == null)
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, pawn2, SickPawnVisitUtility.FindChair(pawn, pawn2));
		}
	}
}
