using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008E9 RID: 2281
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		// Token: 0x06004604 RID: 17924 RVA: 0x001B81FC File Offset: 0x001B63FC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x001B8218 File Offset: 0x001B6418
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait);
			job.expiryInterval = this.ticks;
			Pawn_MindState mindState = pawn.mindState;
			Rot4? rot;
			if (mindState == null)
			{
				rot = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				rot = ((duty != null) ? new Rot4?(duty.overrideFacing) : null);
			}
			job.overrideFacing = (rot ?? Rot4.Invalid);
			return job;
		}

		// Token: 0x04002D7B RID: 11643
		public int ticks = 50;
	}
}
