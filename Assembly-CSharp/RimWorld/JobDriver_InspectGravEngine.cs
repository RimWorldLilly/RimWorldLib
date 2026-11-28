using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A44 RID: 2628
	public class JobDriver_InspectGravEngine : JobDriver
	{
		// Token: 0x06004F57 RID: 20311 RVA: 0x001E0969 File Offset: 0x001DEB69
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(base.TargetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x001E0987 File Offset: 0x001DEB87
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_InspectGravEngine.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_InspectGravEngine.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030F0 RID: 12528
		private const TargetIndex GravEngineIndex = TargetIndex.A;
	}
}
