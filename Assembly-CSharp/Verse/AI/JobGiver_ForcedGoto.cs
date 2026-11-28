using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008E8 RID: 2280
	public class JobGiver_ForcedGoto : ThinkNode_JobGiver
	{
		// Token: 0x06004602 RID: 17922 RVA: 0x001B8194 File Offset: 0x001B6394
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 forcedGotoPosition = pawn.mindState.forcedGotoPosition;
			if (!forcedGotoPosition.IsValid)
			{
				return null;
			}
			if (!pawn.CanReach(forcedGotoPosition, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Goto, forcedGotoPosition);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			return job;
		}
	}
}
