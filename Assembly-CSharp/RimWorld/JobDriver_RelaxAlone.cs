using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009FC RID: 2556
	public class JobDriver_RelaxAlone : JobDriver
	{
		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06004D53 RID: 19795 RVA: 0x001DA288 File Offset: 0x001D8488
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing is Building_Bed;
			}
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06004D54 RID: 19796 RVA: 0x001DA2B1 File Offset: 0x001D84B1
		protected virtual bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x001DA2B4 File Offset: 0x001D84B4
		public override bool CanBeginNowWhileLyingDown()
		{
			return this.FromBed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x001DA2D8 File Offset: 0x001D84D8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.FromBed)
			{
				if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, ((Building_Bed)this.job.GetTarget(TargetIndex.A).Thing).SleepingSlotsCount, 0, null, errorOnFailed, false))
				{
					return false;
				}
			}
			else if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x001DA357 File Offset: 0x001D8557
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RelaxAlone.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_RelaxAlone.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x001DA368 File Offset: 0x001D8568
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.faceDir, "faceDir", default(Rot4), false);
		}

		// Token: 0x04003063 RID: 12387
		protected Rot4 faceDir = Rot4.Invalid;

		// Token: 0x04003064 RID: 12388
		protected const TargetIndex SpotOrBedInd = TargetIndex.A;
	}
}
