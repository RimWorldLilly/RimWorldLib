using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A60 RID: 2656
	public class JobDriver_PilotConsole : JobDriver
	{
		// Token: 0x06005018 RID: 20504 RVA: 0x001E2EAB File Offset: 0x001E10AB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x001E2ECF File Offset: 0x001E10CF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PilotConsole.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_PilotConsole.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003126 RID: 12582
		private const TargetIndex ConsoleInd = TargetIndex.A;
	}
}
