using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BD7 RID: 3031
	public class JobDriver_MergeIntoGaumakerPod : JobDriver
	{
		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x0600571F RID: 22303 RVA: 0x00205138 File Offset: 0x00203338
		private CompTreeConnection TreeComp
		{
			get
			{
				return this.job.targetA.Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06005720 RID: 22304 RVA: 0x0020514F File Offset: 0x0020334F
		private CompGaumakerPod GaumakerPod
		{
			get
			{
				return this.job.targetB.Thing.TryGetComp<CompGaumakerPod>();
			}
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x00205166 File Offset: 0x00203366
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005722 RID: 22306 RVA: 0x00205169 File Offset: 0x00203369
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_MergeIntoGaumakerPod.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_MergeIntoGaumakerPod.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400332C RID: 13100
		private const int WaitTicks = 120;
	}
}
