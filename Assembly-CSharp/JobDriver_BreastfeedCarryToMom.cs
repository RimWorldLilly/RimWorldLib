using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

// Token: 0x02000006 RID: 6
public class JobDriver_BreastfeedCarryToMom : JobDriver
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600001C RID: 28 RVA: 0x000024DF File Offset: 0x000006DF
	private Pawn Baby
	{
		get
		{
			return (Pawn)base.TargetThingA;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600001D RID: 29 RVA: 0x000024EC File Offset: 0x000006EC
	private Pawn Mom
	{
		get
		{
			return (Pawn)base.TargetThingB;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600001E RID: 30 RVA: 0x000024F9 File Offset: 0x000006F9
	protected virtual bool MomMustBeImmobile
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000024FC File Offset: 0x000006FC
	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return this.pawn.Reserve(this.Baby, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Mom, this.job, 1, -1, null, errorOnFailed, false);
	}

	// Token: 0x06000020 RID: 32 RVA: 0x0000254F File Offset: 0x0000074F
	protected override IEnumerable<Toil> MakeNewToils()
	{
		JobDriver_BreastfeedCarryToMom.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_BreastfeedCarryToMom.<MakeNewToils>d__9(-2);
		<MakeNewToils>d__.<>4__this = this;
		return <MakeNewToils>d__;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x0000255F File Offset: 0x0000075F
	private Toil StartBreastfeedJobOnMom()
	{
		Toil toil = ToilMaker.MakeToil("StartBreastfeedJobOnMom");
		toil.initAction = delegate()
		{
			this.Mom.jobs.SuspendCurrentJob(JobCondition.InterruptForced, true, new bool?(false));
			if (!this.pawn.carryTracker.innerContainer.TryTransferToContainer(this.pawn.carryTracker.CarriedThing, this.Mom.carryTracker.innerContainer, true))
			{
				this.Mom.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				base.EndJobWith(JobCondition.Incompletable);
				return;
			}
			Pawn_JobTracker jobs = this.Mom.jobs;
			Job newJob = ChildcareUtility.MakeBreastfeedJob(this.Baby, this.Mom.CurrentBed());
			JobCondition lastJobEndCondition = JobCondition.InterruptForced;
			ThinkNode jobGiver = null;
			bool resumeCurJobAfterwards = false;
			bool cancelBusyStances = true;
			ThinkTreeDef thinkTree = null;
			bool? keepCarryingThingOverride = new bool?(true);
			jobs.StartJob(newJob, lastJobEndCondition, jobGiver, resumeCurJobAfterwards, cancelBusyStances, thinkTree, null, false, false, keepCarryingThingOverride, false, true, false);
		};
		return toil;
	}

	// Token: 0x04000008 RID: 8
	private const TargetIndex BabyInd = TargetIndex.A;

	// Token: 0x04000009 RID: 9
	private const TargetIndex MomInd = TargetIndex.B;
}
