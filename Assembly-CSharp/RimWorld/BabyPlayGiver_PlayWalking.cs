using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BAB RID: 2987
	public class BabyPlayGiver_PlayWalking : BabyPlayGiver
	{
		// Token: 0x0600562C RID: 22060 RVA: 0x00200958 File Offset: 0x001FEB58
		public override bool CanDo(Pawn pawn, Pawn other)
		{
			return pawn.IsCarryingPawn(other) || pawn.CanReserveAndReach(other, PathEndMode.Touch, Danger.Some, 1, -1, null, false);
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x0020097C File Offset: 0x001FEB7C
		public override Job TryGiveJob(Pawn pawn, Pawn other)
		{
			IntVec3 c = JobDriver_PlayWalking.TryFindWanderCell(pawn, other.PositionHeld);
			if (!c.IsValid)
			{
				return null;
			}
			if (!pawn.CanReserveAndReach(c, PathEndMode.Touch, Danger.Some, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.def.jobDef, other, c);
			job.count = 1;
			return job;
		}
	}
}
