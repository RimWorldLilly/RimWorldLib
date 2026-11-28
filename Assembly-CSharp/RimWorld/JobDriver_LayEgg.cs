using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009B4 RID: 2484
	public class JobDriver_LayEgg : JobDriver
	{
		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06004B35 RID: 19253 RVA: 0x001D2CC8 File Offset: 0x001D0EC8
		public CompEggContainer EggBoxComp
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEggContainer>();
			}
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x001D2CEE File Offset: 0x001D0EEE
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x001D2CF1 File Offset: 0x001D0EF1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_LayEgg.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_LayEgg.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002FB9 RID: 12217
		private const int LayEgg = 500;

		// Token: 0x04002FBA RID: 12218
		private const TargetIndex LaySpotOrEggBoxInd = TargetIndex.A;
	}
}
