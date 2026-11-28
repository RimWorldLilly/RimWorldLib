using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B33 RID: 2867
	public class JobGiver_BringBabyToSafety : ThinkNode_JobGiver
	{
		// Token: 0x06005455 RID: 21589 RVA: 0x001F597C File Offset: 0x001F3B7C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = ChildcareUtility.FindUnsafeBaby(pawn, AutofeedMode.Urgent);
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
