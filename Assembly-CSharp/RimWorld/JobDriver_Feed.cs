using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A9E RID: 2718
	public class JobDriver_Feed : JobDriver
	{
		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x060051CD RID: 20941 RVA: 0x001E8232 File Offset: 0x001E6432
		protected Thing Food
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x060051CE RID: 20942 RVA: 0x001E8244 File Offset: 0x001E6444
		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x001E825B File Offset: 0x001E645B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x001E8283 File Offset: 0x001E6483
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Feed.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_Feed.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400319D RID: 12701
		private const TargetIndex IngestableInd = TargetIndex.A;

		// Token: 0x0400319E RID: 12702
		private const TargetIndex DelivereeInd = TargetIndex.B;

		// Token: 0x0400319F RID: 12703
		private const float FeedDurationMultiplier = 1.5f;
	}
}
