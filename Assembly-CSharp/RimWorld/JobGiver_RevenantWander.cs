using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B63 RID: 2915
	public class JobGiver_RevenantWander : ThinkNode_JobGiver
	{
		// Token: 0x060054F9 RID: 21753 RVA: 0x001F9DFC File Offset: 0x001F7FFC
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			CellFinder.TryFindRandomReachableNearbyCell(pawn.Position, pawn.Map, JobGiver_RevenantWander.WanderDist, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false), (IntVec3 x) => x.Standable(pawn.Map), null, out c, 999999);
			Job job = JobMaker.MakeJob(JobDefOf.RevenantWander, c);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			return job;
		}

		// Token: 0x040032B6 RID: 12982
		public static float WanderDist = 30f;
	}
}
