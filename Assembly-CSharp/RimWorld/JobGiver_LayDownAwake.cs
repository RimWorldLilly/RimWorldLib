using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B1C RID: 2844
	public class JobGiver_LayDownAwake : ThinkNode_JobGiver
	{
		// Token: 0x0600540A RID: 21514 RVA: 0x001F45B4 File Offset: 0x001F27B4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.CanReach(pawn.mindState.duty.focusThird, PathEndMode.Touch, Danger.None, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.LayDownAwake, pawn.mindState.duty.focusThird);
		}
	}
}
