using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B1A RID: 2842
	public class JobGiver_Kidnap : ThinkNode_JobGiver
	{
		// Token: 0x06005406 RID: 21510 RVA: 0x001F4418 File Offset: 0x001F2618
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn, true))
			{
				return null;
			}
			Pawn t;
			if (KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 18f, out t, null) && !GenAI.InDangerousCombat(pawn))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Kidnap);
				job.targetA = t;
				job.targetB = c;
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x04003254 RID: 12884
		public const float VictimSearchRadiusInitial = 8f;

		// Token: 0x04003255 RID: 12885
		private const float VictimSearchRadiusOngoing = 18f;
	}
}
