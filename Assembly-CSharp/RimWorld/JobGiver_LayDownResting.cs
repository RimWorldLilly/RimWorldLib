using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B1D RID: 2845
	public class JobGiver_LayDownResting : ThinkNode_JobGiver
	{
		// Token: 0x0600540C RID: 21516 RVA: 0x001F45F8 File Offset: 0x001F27F8
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c2;
			if (!GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, (IntVec3 c) => pawn.CanReserveAndReach(c, PathEndMode.Touch, Danger.None, 1, -1, null, false), out c2))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.LayDownResting, c2);
			job.locomotionUrgency = LocomotionUrgency.Amble;
			return job;
		}
	}
}
