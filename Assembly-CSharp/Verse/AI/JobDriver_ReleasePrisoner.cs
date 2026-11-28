using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000808 RID: 2056
	public class JobDriver_ReleasePrisoner : JobDriver
	{
		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060040BD RID: 16573 RVA: 0x0019BDB8 File Offset: 0x00199FB8
		private Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x0019BDDE File Offset: 0x00199FDE
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Prisoner, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x0019BE01 File Offset: 0x0019A001
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ReleasePrisoner.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_ReleasePrisoner.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002AF4 RID: 10996
		private const TargetIndex PrisonerInd = TargetIndex.A;

		// Token: 0x04002AF5 RID: 10997
		private const TargetIndex ReleaseCellInd = TargetIndex.B;
	}
}
