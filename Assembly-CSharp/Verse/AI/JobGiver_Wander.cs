using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008F6 RID: 2294
	public abstract class JobGiver_Wander : ThinkNode_JobGiver
	{
		// Token: 0x0600463A RID: 17978 RVA: 0x001B8B44 File Offset: 0x001B6D44
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Wander jobGiver_Wander = (JobGiver_Wander)base.DeepCopy(resolve);
			jobGiver_Wander.wanderRadius = this.wanderRadius;
			jobGiver_Wander.wanderDestValidator = this.wanderDestValidator;
			jobGiver_Wander.ticksBetweenWandersRange = this.ticksBetweenWandersRange;
			jobGiver_Wander.locomotionUrgency = this.locomotionUrgency;
			jobGiver_Wander.locomotionUrgencyOutsideRadius = this.locomotionUrgencyOutsideRadius;
			jobGiver_Wander.maxDanger = this.maxDanger;
			jobGiver_Wander.expiryInterval = this.expiryInterval;
			jobGiver_Wander.canBashDoors = this.canBashDoors;
			jobGiver_Wander.expireOnNearbyEnemy = this.expireOnNearbyEnemy;
			jobGiver_Wander.canEatMealsFromInventory = this.canEatMealsFromInventory;
			return jobGiver_Wander;
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x001B8BD8 File Offset: 0x001B6DD8
		protected override Job TryGiveJob(Pawn pawn)
		{
			bool flag = pawn.CurJob != null && pawn.CurJob.def == JobDefOf.GotoWander;
			bool flying = pawn.Flying;
			bool flag2 = pawn.mindState.nextMoveOrderIsWait && !flying;
			if (!flag)
			{
				pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
			}
			if (flag2 && !flag)
			{
				return this.<TryGiveJob>g__GetWaitJob|12_0();
			}
			IntVec3 exactWanderDest = this.GetExactWanderDest(pawn);
			if (exactWanderDest == pawn.Position && !flag)
			{
				return this.<TryGiveJob>g__GetWaitJob|12_0();
			}
			if (!exactWanderDest.IsValid)
			{
				pawn.mindState.nextMoveOrderIsWait = false;
				return null;
			}
			LocomotionUrgency value = this.locomotionUrgency;
			if (this.locomotionUrgencyOutsideRadius != null && !pawn.Position.InHorDistOf(this.GetWanderRoot(pawn), this.wanderRadius))
			{
				value = this.locomotionUrgencyOutsideRadius.Value;
			}
			Job job = JobMaker.MakeJob(JobDefOf.GotoWander, exactWanderDest);
			job.locomotionUrgency = value;
			job.expiryInterval = this.expiryInterval;
			job.checkOverrideOnExpire = true;
			job.reportStringOverride = this.reportStringOverride;
			job.canBashDoors = this.canBashDoors;
			if (this.expireOnNearbyEnemy)
			{
				job.expiryInterval = 30;
				job.checkOverrideOnExpire = true;
			}
			this.DecorateGotoJob(job);
			return job;
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x001B8D28 File Offset: 0x001B6F28
		protected virtual IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 wanderRoot = this.GetWanderRoot(pawn);
			if (!wanderRoot.IsValid)
			{
				return IntVec3.Invalid;
			}
			float value = this.wanderRadius;
			PawnDuty duty = pawn.mindState.duty;
			if (duty != null && duty.wanderRadius != null)
			{
				value = duty.wanderRadius.Value;
			}
			return RCellFinder.RandomWanderDestFor(pawn, wanderRoot, value, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), this.canBashDoors);
		}

		// Token: 0x0600463D RID: 17981
		protected abstract IntVec3 GetWanderRoot(Pawn pawn);

		// Token: 0x0600463E RID: 17982 RVA: 0x001B8D9B File Offset: 0x001B6F9B
		protected virtual void DecorateGotoJob(Job job)
		{
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x001B8DCC File Offset: 0x001B6FCC
		[CompilerGenerated]
		private Job <TryGiveJob>g__GetWaitJob|12_0()
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
			job.expiryInterval = this.ticksBetweenWandersRange.RandomInRange;
			job.reportStringOverride = this.reportStringOverride;
			if (this.expireOnNearbyEnemy)
			{
				job.expiryInterval = 30;
				job.checkOverrideOnExpire = true;
			}
			return job;
		}

		// Token: 0x04002D91 RID: 11665
		protected float wanderRadius;

		// Token: 0x04002D92 RID: 11666
		protected Func<Pawn, IntVec3, IntVec3, bool> wanderDestValidator;

		// Token: 0x04002D93 RID: 11667
		protected IntRange ticksBetweenWandersRange = new IntRange(20, 100);

		// Token: 0x04002D94 RID: 11668
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x04002D95 RID: 11669
		protected LocomotionUrgency? locomotionUrgencyOutsideRadius;

		// Token: 0x04002D96 RID: 11670
		protected Danger maxDanger = Danger.None;

		// Token: 0x04002D97 RID: 11671
		protected int expiryInterval = -1;

		// Token: 0x04002D98 RID: 11672
		protected bool canBashDoors;

		// Token: 0x04002D99 RID: 11673
		protected bool expireOnNearbyEnemy;

		// Token: 0x04002D9A RID: 11674
		protected bool canEatMealsFromInventory;

		// Token: 0x04002D9B RID: 11675
		private const int EnemyScanCheckTicks = 30;
	}
}
