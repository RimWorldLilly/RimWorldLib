using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A4B RID: 2635
	public class JobDriver_LayDown : JobDriver
	{
		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06004F8B RID: 20363 RVA: 0x001E13F8 File Offset: 0x001DF5F8
		public Building_Bed Bed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Building_Bed;
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06004F8C RID: 20364 RVA: 0x001E1420 File Offset: 0x001DF620
		public IntVec3 Cell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06004F8D RID: 20365 RVA: 0x001E1441 File Offset: 0x001DF641
		public virtual bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06004F8E RID: 20366 RVA: 0x001E1444 File Offset: 0x001DF644
		public virtual bool CanRest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06004F8F RID: 20367 RVA: 0x001E1447 File Offset: 0x001DF647
		public virtual bool LookForOtherJobs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x001E144C File Offset: 0x001DF64C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Bed != null)
			{
				if (!this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed, false))
				{
					return false;
				}
			}
			else if (this.job.targetA.Cell.IsValid && !this.pawn.ageTracker.CurLifeStage.alwaysDowned && !this.job.forceSleep && !this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x001E1501 File Offset: 0x001DF701
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x001E151A File Offset: 0x001DF71A
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.canMoveOrCrawl = (!this.pawn.Downed || this.pawn.health.CanCrawl);
			this.hasSavedValues = true;
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x001E154F File Offset: 0x001DF74F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_LayDown.<MakeNewToils>d__16 <MakeNewToils>d__ = new JobDriver_LayDown.<MakeNewToils>d__16(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x001E155F File Offset: 0x001DF75F
		public virtual Toil LayDownToil(bool hasBed)
		{
			return Toils_LayDown.LayDown(TargetIndex.A, hasBed, this.LookForOtherJobs, this.CanSleep, this.CanRest, PawnPosture.LayingOnGroundNormal, false);
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x001E157C File Offset: 0x001DF77C
		public override string GetReport()
		{
			string reportStringOverride = base.GetReportStringOverride();
			if (!reportStringOverride.NullOrEmpty())
			{
				return reportStringOverride;
			}
			if (this.asleep)
			{
				return "ReportSleeping".Translate();
			}
			Thing spawnedParentOrMe = this.pawn.SpawnedParentOrMe;
			if (spawnedParentOrMe != null && this.pawn != spawnedParentOrMe)
			{
				return JobDriver_Carried.GetReport(this.pawn, spawnedParentOrMe);
			}
			if (this.pawn.health.hediffSet.InLabor(false))
			{
				return "GivingBirth".Translate();
			}
			return "ReportResting".Translate();
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x001E160E File Offset: 0x001DF80E
		public override bool IsContinuation(Job j)
		{
			return !(this.job.GetTarget(TargetIndex.A) != j.GetTarget(TargetIndex.A)) && base.IsContinuation(j);
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x001E1634 File Offset: 0x001DF834
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.canMoveOrCrawl, "canMoveOrCrawl", false, false);
			Scribe_Values.Look<bool>(ref this.hasSavedValues, "hasSavedValues", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.hasSavedValues)
			{
				this.canMoveOrCrawl = (!this.pawn.Downed || this.pawn.health.CanCrawl);
				this.hasSavedValues = true;
			}
			base.ExposeData();
		}

		// Token: 0x040030FD RID: 12541
		private bool canMoveOrCrawl;

		// Token: 0x040030FE RID: 12542
		private bool hasSavedValues;

		// Token: 0x040030FF RID: 12543
		public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;
	}
}
