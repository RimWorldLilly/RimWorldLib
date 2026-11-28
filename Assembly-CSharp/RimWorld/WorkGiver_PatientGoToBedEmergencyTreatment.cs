using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C2E RID: 3118
	public class WorkGiver_PatientGoToBedEmergencyTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		// Token: 0x060058D1 RID: 22737 RVA: 0x0020D00B File Offset: 0x0020B20B
		public override Job NonScanJob(Pawn pawn)
		{
			if (!HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(pawn))
			{
				return null;
			}
			return base.NonScanJob(pawn);
		}
	}
}
