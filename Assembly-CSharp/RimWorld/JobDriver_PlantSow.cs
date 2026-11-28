using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A91 RID: 2705
	public class JobDriver_PlantSow : JobDriver
	{
		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x0600517C RID: 20860 RVA: 0x001E7464 File Offset: 0x001E5664
		private Plant Plant
		{
			get
			{
				return (Plant)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x001E748A File Offset: 0x001E568A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x001E74A8 File Offset: 0x001E56A8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x001E74CB File Offset: 0x001E56CB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlantSow.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_PlantSow.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003192 RID: 12690
		private float sowWorkDone;
	}
}
