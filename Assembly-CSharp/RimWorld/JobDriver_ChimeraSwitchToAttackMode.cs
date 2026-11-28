using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A1E RID: 2590
	public class JobDriver_ChimeraSwitchToAttackMode : JobDriver
	{
		// Token: 0x06004E45 RID: 20037 RVA: 0x001DD23C File Offset: 0x001DB43C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x001DD23F File Offset: 0x001DB43F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ChimeraSwitchToAttackMode.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_ChimeraSwitchToAttackMode.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
