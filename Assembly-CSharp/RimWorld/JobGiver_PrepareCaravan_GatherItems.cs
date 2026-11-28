using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AED RID: 2797
	public class JobGiver_PrepareCaravan_GatherItems : ThinkNode_JobGiver
	{
		// Token: 0x0600537B RID: 21371 RVA: 0x001F16C0 File Offset: 0x001EF8C0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(pawn, lord);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.PrepareCaravan_GatherItems, thing);
			job.lord = lord;
			return job;
		}
	}
}
