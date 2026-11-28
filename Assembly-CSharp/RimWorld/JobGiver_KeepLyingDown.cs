using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B4E RID: 2894
	public class JobGiver_KeepLyingDown : ThinkNode_JobGiver
	{
		// Token: 0x060054AE RID: 21678 RVA: 0x001F77C8 File Offset: 0x001F59C8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.GetPosture().Laying() && pawn.CurJob != null)
			{
				return pawn.CurJob;
			}
			return JobMaker.MakeJob(JobDefOf.LayDown, (pawn.InBed() && this.useBed) ? new LocalTargetInfo(pawn.CurrentBed()) : pawn.Position);
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x001F7823 File Offset: 0x001F5A23
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_KeepLyingDown jobGiver_KeepLyingDown = (JobGiver_KeepLyingDown)base.DeepCopy(resolve);
			jobGiver_KeepLyingDown.useBed = this.useBed;
			return jobGiver_KeepLyingDown;
		}

		// Token: 0x04003294 RID: 12948
		public bool useBed;
	}
}
