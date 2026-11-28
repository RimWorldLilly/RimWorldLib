using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA9 RID: 2985
	public class BabyPlayGiver_PlayStatic : BabyPlayGiver
	{
		// Token: 0x06005624 RID: 22052 RVA: 0x00200720 File Offset: 0x001FE920
		public override bool CanDo(Pawn pawn, Pawn other)
		{
			return pawn.IsCarryingPawn(other) || pawn.CanReserveAndReach(other, PathEndMode.Touch, Danger.Some, 1, -1, null, false);
		}

		// Token: 0x06005625 RID: 22053 RVA: 0x00200742 File Offset: 0x001FE942
		public override Job TryGiveJob(Pawn pawn, Pawn other)
		{
			Job job = JobMaker.MakeJob(this.def.jobDef, other);
			job.count = 1;
			return job;
		}
	}
}
