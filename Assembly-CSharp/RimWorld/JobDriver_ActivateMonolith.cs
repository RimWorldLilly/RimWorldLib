using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A10 RID: 2576
	public class JobDriver_ActivateMonolith : JobDriver
	{
		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06004DD1 RID: 19921 RVA: 0x001DBC78 File Offset: 0x001D9E78
		private Building_VoidMonolith VoidMonolith
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Building_VoidMonolith;
			}
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x001DBCA0 File Offset: 0x001D9EA0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (base.TargetThingC != null)
			{
				this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			}
			return (base.TargetThingB == null || this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false)) && this.pawn.Reserve(this.VoidMonolith, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.ReserveSittableOrSpot(this.VoidMonolith.InteractionCell, this.job, errorOnFailed);
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x001DBD4A File Offset: 0x001D9F4A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ActivateMonolith.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_ActivateMonolith.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003081 RID: 12417
		private const int ActivateSeconds = 10;

		// Token: 0x04003082 RID: 12418
		private const TargetIndex MonolithIndex = TargetIndex.A;

		// Token: 0x04003083 RID: 12419
		private const TargetIndex ActivationItemIndex = TargetIndex.B;
	}
}
