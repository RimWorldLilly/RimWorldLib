using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B66 RID: 2918
	public class JobGiver_SelfShutdown : ThinkNode_JobGiver
	{
		// Token: 0x06005505 RID: 21765 RVA: 0x001FA22C File Offset: 0x001F842C
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (RCellFinder.TryFindNearbyMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out c, true))
			{
				Job job = JobMaker.MakeJob(JobDefOf.SelfShutdown, c);
				job.forceSleep = true;
				return job;
			}
			return null;
		}
	}
}
