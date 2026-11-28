using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000819 RID: 2073
	public class JobDriver_Follow : JobDriver
	{
		// Token: 0x06004135 RID: 16693 RVA: 0x0019E4E0 File Offset: 0x0019C6E0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x0019E4E3 File Offset: 0x0019C6E3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Follow.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_Follow.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x0019E4F3 File Offset: 0x0019C6F3
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x04002B21 RID: 11041
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x04002B22 RID: 11042
		private const int Distance = 4;
	}
}
