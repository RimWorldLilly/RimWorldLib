using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C2F RID: 3119
	public class WorkGiver_PatientGoToBedRecuperate : WorkGiver
	{
		// Token: 0x060058D3 RID: 22739 RVA: 0x0020D028 File Offset: 0x0020B228
		public override Job NonScanJob(Pawn pawn)
		{
			ThinkResult thinkResult = WorkGiver_PatientGoToBedRecuperate.jgp.TryIssueJobPackage(pawn, default(JobIssueParams));
			if (thinkResult.IsValid)
			{
				return thinkResult.Job;
			}
			return null;
		}

		// Token: 0x04003354 RID: 13140
		private static JobGiver_PatientGoToBed jgp = new JobGiver_PatientGoToBed
		{
			respectTimetable = false
		};
	}
}
