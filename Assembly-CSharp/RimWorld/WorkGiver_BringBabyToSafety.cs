using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF2 RID: 3058
	public class WorkGiver_BringBabyToSafety : WorkGiver
	{
		// Token: 0x06005782 RID: 22402 RVA: 0x00206E10 File Offset: 0x00205010
		public override Job NonScanJob(Pawn pawn)
		{
			Pawn pawn2 = ChildcareUtility.FindUnsafeBaby(pawn, AutofeedMode.Childcare);
			if (pawn2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.BringBabyToSafetyUnforced, pawn2);
			job.count = 1;
			return job;
		}
	}
}
