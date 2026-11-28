using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A09 RID: 2569
	public class JobDriver_NatureRunning : JobDriver
	{
		// Token: 0x06004DAB RID: 19883 RVA: 0x001DB590 File Offset: 0x001D9790
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x001DB593 File Offset: 0x001D9793
		private Toil FindInterestingThing()
		{
			Toil toil = ToilMaker.MakeToil("FindInterestingThing");
			toil.initAction = delegate()
			{
				LocalTargetInfo pack;
				if (NatureRunningUtility.TryFindNatureInterestTarget(this.pawn, out pack))
				{
					this.job.SetTarget(TargetIndex.A, pack);
					return;
				}
				base.EndJobWith(JobCondition.Incompletable);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x001DB5B8 File Offset: 0x001D97B8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_NatureRunning.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_NatureRunning.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003075 RID: 12405
		private static readonly IntRange WaitTicksRange = new IntRange(300, 600);
	}
}
