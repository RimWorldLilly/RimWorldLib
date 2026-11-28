using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B9F RID: 2975
	public class JobGiver_InduceSlaveToRebel : ThinkNode_JobGiver
	{
		// Token: 0x0600560C RID: 22028 RVA: 0x002000A8 File Offset: 0x001FE2A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = SlaveRebellionUtility.FindSlaveForRebellion(pawn);
			if (pawn2 == null || !pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InduceSlaveToRebel, pawn2);
			job.interaction = InteractionDefOf.SparkSlaveRebellion;
			return job;
		}
	}
}
