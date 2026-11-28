using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008EB RID: 2283
	public class JobGiver_IdleError : ThinkNode_JobGiver
	{
		// Token: 0x06004609 RID: 17929 RVA: 0x001B82D9 File Offset: 0x001B64D9
		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.ErrorOnce(((pawn != null) ? pawn.ToString() : null) + " issued IdleError wait job. The behavior tree should never get here.", 532983);
			Job job = JobMaker.MakeJob(JobDefOf.Wait);
			job.expiryInterval = 100;
			return job;
		}

		// Token: 0x04002D7D RID: 11645
		private const int WaitTime = 100;
	}
}
