using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B35 RID: 2869
	public class JobGiver_Deactivated : ThinkNode_JobGiver
	{
		// Token: 0x06005459 RID: 21593 RVA: 0x001F5A5D File Offset: 0x001F3C5D
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Deactivated, pawn.Position);
			job.forceSleep = true;
			return job;
		}
	}
}
