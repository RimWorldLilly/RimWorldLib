using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF8 RID: 3064
	public class JobDriver_ChangeTreeMode : JobDriver
	{
		// Token: 0x06005795 RID: 22421 RVA: 0x00207027 File Offset: 0x00205227
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005796 RID: 22422 RVA: 0x0020702A File Offset: 0x0020522A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ChangeTreeMode.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_ChangeTreeMode.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003335 RID: 13109
		private const int WaitTicks = 120;
	}
}
