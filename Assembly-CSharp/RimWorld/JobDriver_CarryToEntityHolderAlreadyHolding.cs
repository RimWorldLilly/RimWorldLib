using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A1C RID: 2588
	public class JobDriver_CarryToEntityHolderAlreadyHolding : JobDriver
	{
		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06004E34 RID: 20020 RVA: 0x001DD058 File Offset: 0x001DB258
		private Thing Takee
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06004E35 RID: 20021 RVA: 0x001DD07C File Offset: 0x001DB27C
		private CompEntityHolder DestHolder
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEntityHolder>();
			}
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x001DD0A2 File Offset: 0x001DB2A2
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.DestHolder.parent, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x001DD0CA File Offset: 0x001DB2CA
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CarryToEntityHolderAlreadyHolding.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_CarryToEntityHolderAlreadyHolding.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030A2 RID: 12450
		private const TargetIndex DestIndex = TargetIndex.A;

		// Token: 0x040030A3 RID: 12451
		private const TargetIndex TakeeIndex = TargetIndex.B;
	}
}
