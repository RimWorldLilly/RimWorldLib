using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B9E RID: 2974
	public class JobGiver_InducePrisonerToEscape : ThinkNode_JobGiver
	{
		// Token: 0x0600560A RID: 22026 RVA: 0x00200058 File Offset: 0x001FE258
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = JailbreakerMentalStateUtility.FindPrisoner(pawn);
			if (pawn2 == null || !pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InducePrisonerToEscape, pawn2);
			job.interaction = InteractionDefOf.SparkJailbreak;
			return job;
		}
	}
}
