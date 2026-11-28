using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B67 RID: 2919
	public class JobGiver_SelfTend : ThinkNode_JobGiver
	{
		// Token: 0x06005507 RID: 21767 RVA: 0x001FA274 File Offset: 0x001F8474
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike || !pawn.health.HasHediffsNeedingTend(false) || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || pawn.InAggroMentalState)
			{
				return null;
			}
			if (pawn.IsColonist && pawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
			{
				return null;
			}
			if (!pawn.Map.reservationManager.CanReserve(pawn, pawn, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.TendPatient, pawn);
			job.endAfterTendedOnce = true;
			return job;
		}
	}
}
