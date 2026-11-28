using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A5D RID: 2653
	public class JobDriver_RemovePaintBuilding : JobDriver
	{
		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06004FFB RID: 20475 RVA: 0x001E29A0 File Offset: 0x001E0BA0
		private Thing PaintTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x001E29C1 File Offset: 0x001E0BC1
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06004FFD RID: 20477 RVA: 0x001E29E4 File Offset: 0x001E0BE4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RemovePaintBuilding.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_RemovePaintBuilding.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x001E29F4 File Offset: 0x001E0BF4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.removalTime, "removalTime", 0f, false);
		}

		// Token: 0x0400311C RID: 12572
		private float removalTime;

		// Token: 0x0400311D RID: 12573
		private const TargetIndex PaintTargetIndex = TargetIndex.A;

		// Token: 0x0400311E RID: 12574
		private const float PaintTimeSecondsBase = 1f;
	}
}
