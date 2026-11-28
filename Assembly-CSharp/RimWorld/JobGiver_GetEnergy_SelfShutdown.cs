using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B46 RID: 2886
	public class JobGiver_GetEnergy_SelfShutdown : JobGiver_GetEnergy
	{
		// Token: 0x0600548B RID: 21643 RVA: 0x001F6868 File Offset: 0x001F4A68
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!this.ShouldAutoRecharge(pawn))
			{
				return null;
			}
			IntVec3 c;
			if (RCellFinder.TryFindRandomMechSelfShutdownSpot(pawn.Position, pawn, pawn.Map, out c, false))
			{
				Job job = JobMaker.MakeJob(JobDefOf.SelfShutdown, c);
				job.checkOverrideOnExpire = true;
				job.expiryInterval = 500;
				return job;
			}
			return null;
		}
	}
}
