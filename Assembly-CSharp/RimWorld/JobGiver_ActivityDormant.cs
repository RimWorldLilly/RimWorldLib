using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B2E RID: 2862
	public class JobGiver_ActivityDormant : ThinkNode_JobGiver
	{
		// Token: 0x06005443 RID: 21571 RVA: 0x001F568A File Offset: 0x001F388A
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ActivityDormant);
			job.forceSleep = true;
			return job;
		}
	}
}
