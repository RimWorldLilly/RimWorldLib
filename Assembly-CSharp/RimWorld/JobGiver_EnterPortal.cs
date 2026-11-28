using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B14 RID: 2836
	public class JobGiver_EnterPortal : ThinkNode_JobGiver
	{
		// Token: 0x060053F2 RID: 21490 RVA: 0x001F3CB8 File Offset: 0x001F1EB8
		protected override Job TryGiveJob(Pawn pawn)
		{
			MapPortal mapPortal = pawn.mindState.duty.focus.Thing as MapPortal;
			if (mapPortal == null || mapPortal.Map != pawn.Map || !pawn.CanReach(mapPortal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.EnterPortal, mapPortal);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, LocomotionUrgency.Jog);
			return job;
		}
	}
}
