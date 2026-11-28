using System;

namespace Verse.AI
{
	// Token: 0x02000830 RID: 2096
	public class JobDriver_CastVerbOnceStaticReserve : JobDriver_CastVerbOnceStatic
	{
		// Token: 0x060041A1 RID: 16801 RVA: 0x0019FB6F File Offset: 0x0019DD6F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}
	}
}
