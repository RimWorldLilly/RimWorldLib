using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A46 RID: 2630
	public class JobDriver_InstallRelic : JobDriver
	{
		// Token: 0x06004F61 RID: 20321 RVA: 0x001E0B94 File Offset: 0x001DED94
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x001E0BE9 File Offset: 0x001DEDE9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_InstallRelic.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_InstallRelic.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x001E0C04 File Offset: 0x001DEE04
		[CompilerGenerated]
		private bool <MakeNewToils>g__ReliquaryFull|5_0()
		{
			CompRelicContainer compRelicContainer = this.pawn.jobs.curJob.GetTarget(TargetIndex.B).Thing.TryGetComp<CompRelicContainer>();
			return compRelicContainer == null || compRelicContainer.Full;
		}

		// Token: 0x040030F4 RID: 12532
		private const TargetIndex RelicInd = TargetIndex.A;

		// Token: 0x040030F5 RID: 12533
		private const TargetIndex ContainerInd = TargetIndex.B;

		// Token: 0x040030F6 RID: 12534
		private const TargetIndex ContainerInteractionCellInd = TargetIndex.C;

		// Token: 0x040030F7 RID: 12535
		private const int InstallTicks = 300;
	}
}
