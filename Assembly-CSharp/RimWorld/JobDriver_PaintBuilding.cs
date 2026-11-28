using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A5C RID: 2652
	public class JobDriver_PaintBuilding : JobDriver
	{
		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06004FF1 RID: 20465 RVA: 0x001E279C File Offset: 0x001E099C
		private Thing PaintTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x001E27C0 File Offset: 0x001E09C0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x001E280E File Offset: 0x001E0A0E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PaintBuilding.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_PaintBuilding.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x001E281E File Offset: 0x001E0A1E
		private IEnumerable<Toil> CollectDyeToils()
		{
			return new JobDriver_PaintBuilding.<CollectDyeToils>d__8(-2);
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x001E2827 File Offset: 0x001E0A27
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.paintingTime, "paintingTime", 0f, false);
		}

		// Token: 0x04003118 RID: 12568
		private float paintingTime;

		// Token: 0x04003119 RID: 12569
		private const TargetIndex PaintTargetIndex = TargetIndex.A;

		// Token: 0x0400311A RID: 12570
		private const TargetIndex DyeIndex = TargetIndex.B;

		// Token: 0x0400311B RID: 12571
		private const float PaintTimeSecondsBase = 1f;
	}
}
