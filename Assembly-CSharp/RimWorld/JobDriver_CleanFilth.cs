using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A1F RID: 2591
	public class JobDriver_CleanFilth : JobDriver
	{
		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06004E49 RID: 20041 RVA: 0x001DD290 File Offset: 0x001DB490
		private Filth Filth
		{
			get
			{
				return (Filth)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x001DD2B6 File Offset: 0x001DB4B6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x001DD2D9 File Offset: 0x001DB4D9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CleanFilth.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_CleanFilth.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x001DD2EC File Offset: 0x001DB4EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.cleaningWorkDone, "cleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkDone, "totalCleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkRequired, "totalCleaningWorkRequired", 0f, false);
		}

		// Token: 0x040030A6 RID: 12454
		private float cleaningWorkDone;

		// Token: 0x040030A7 RID: 12455
		private float totalCleaningWorkDone;

		// Token: 0x040030A8 RID: 12456
		private float totalCleaningWorkRequired;

		// Token: 0x040030A9 RID: 12457
		private const TargetIndex FilthInd = TargetIndex.A;
	}
}
