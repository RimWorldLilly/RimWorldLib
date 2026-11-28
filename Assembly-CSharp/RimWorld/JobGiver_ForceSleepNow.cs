using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B17 RID: 2839
	public class JobGiver_ForceSleepNow : ThinkNode_JobGiver
	{
		// Token: 0x060053FC RID: 21500 RVA: 0x001F3FCD File Offset: 0x001F21CD
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position);
			job.forceSleep = true;
			return job;
		}
	}
}
