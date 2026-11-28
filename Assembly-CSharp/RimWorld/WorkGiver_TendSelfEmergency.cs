using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C50 RID: 3152
	public class WorkGiver_TendSelfEmergency : WorkGiver_TendSelf
	{
		// Token: 0x0600597F RID: 22911 RVA: 0x0020ED3C File Offset: 0x0020CF3C
		public override Job NonScanJob(Pawn pawn)
		{
			if (!this.HasJobOnThing(pawn, pawn, false) || !HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(pawn))
			{
				return null;
			}
			ThinkResult thinkResult = WorkGiver_TendSelfEmergency.jgp.TryIssueJobPackage(pawn, default(JobIssueParams));
			if (thinkResult.IsValid)
			{
				return thinkResult.Job;
			}
			return null;
		}

		// Token: 0x04003358 RID: 13144
		private static JobGiver_SelfTend jgp = new JobGiver_SelfTend();
	}
}
