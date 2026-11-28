using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A24 RID: 2596
	public class JobDriver_Deathrest : JobDriver
	{
		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06004E6F RID: 20079 RVA: 0x001DDAD0 File Offset: 0x001DBCD0
		private Building_Bed Bed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Building_Bed;
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06004E70 RID: 20080 RVA: 0x001DDAF6 File Offset: 0x001DBCF6
		public override bool PlayerInterruptable
		{
			get
			{
				return !base.OnLastToil;
			}
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x001DDB01 File Offset: 0x001DBD01
		public override string GetReport()
		{
			return this.ReportStringProcessed(SanguophageUtility.DeathrestJobReport(this.pawn));
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x001DDB14 File Offset: 0x001DBD14
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.Bed == null || this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed, false);
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x001DDB4B File Offset: 0x001DBD4B
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x001DDB64 File Offset: 0x001DBD64
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Deathrest.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_Deathrest.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030B5 RID: 12469
		private const TargetIndex BedIndex = TargetIndex.A;

		// Token: 0x040030B6 RID: 12470
		private const int MoteIntervalTicks = 160;
	}
}
