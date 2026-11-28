using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B27 RID: 2855
	public class JobGiver_TakeWoundedGuest : ThinkNode_JobGiver
	{
		// Token: 0x0600542E RID: 21550 RVA: 0x001F5060 File Offset: 0x001F3260
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn, true))
			{
				return null;
			}
			Pawn pawn2 = KidnapAIUtility.ReachableWoundedGuest(pawn);
			if (pawn2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Kidnap);
			job.targetA = pawn2;
			job.targetB = c;
			job.count = 1;
			return job;
		}
	}
}
