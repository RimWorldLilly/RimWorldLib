using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A88 RID: 2696
	public class JobDriver_UnnaturalCorpseSkip : JobDriver
	{
		// Token: 0x06005143 RID: 20803 RVA: 0x001E68BE File Offset: 0x001E4ABE
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x001E68C1 File Offset: 0x001E4AC1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UnnaturalCorpseSkip.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_UnnaturalCorpseSkip.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003185 RID: 12677
		private const TargetIndex DestIndex = TargetIndex.A;
	}
}
