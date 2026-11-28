using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BD9 RID: 3033
	public class JobDriver_ReturnToGauranlenTree : JobDriver
	{
		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06005728 RID: 22312 RVA: 0x00205260 File Offset: 0x00203460
		private CompTreeConnection TreeComp
		{
			get
			{
				return this.job.targetA.Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x00205277 File Offset: 0x00203477
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x0020527A File Offset: 0x0020347A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ReturnToGauranlenTree.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_ReturnToGauranlenTree.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400332D RID: 13101
		private const int WaitTicks = 180;
	}
}
