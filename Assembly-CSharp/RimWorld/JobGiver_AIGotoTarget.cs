using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ADC RID: 2780
	public class JobGiver_AIGotoTarget : ThinkNode_JobGiver
	{
		// Token: 0x0600533A RID: 21306 RVA: 0x001EFAAF File Offset: 0x001EDCAF
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIGotoTarget jobGiver_AIGotoTarget = (JobGiver_AIGotoTarget)base.DeepCopy(resolve);
			jobGiver_AIGotoTarget.ignoreNonCombatants = this.ignoreNonCombatants;
			jobGiver_AIGotoTarget.humanlikesOnly = this.humanlikesOnly;
			jobGiver_AIGotoTarget.overrideExpiryInterval = this.overrideExpiryInterval;
			return jobGiver_AIGotoTarget;
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x001EFAE4 File Offset: 0x001EDCE4
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_MindState mindState = pawn.mindState;
			Thing thing = (mindState != null) ? mindState.enemyTarget : null;
			if (thing == null)
			{
				return null;
			}
			if (thing.PositionHeld == pawn.PositionHeld || pawn.CanReachImmediate(thing, PathEndMode.Touch))
			{
				return null;
			}
			if (!this.IsTargetStillValid(thing, pawn))
			{
				pawn.mindState.enemyTarget = null;
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Goto, thing);
			if (this.overrideExpiryInterval > 0)
			{
				job.expiryInterval = this.overrideExpiryInterval;
			}
			else
			{
				job.intervalScalingTarget = TargetIndex.A;
			}
			job.checkOverrideOnExpire = true;
			job.collideWithPawns = true;
			return job;
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x001EFB84 File Offset: 0x001EDD84
		private bool IsTargetStillValid(Thing target, Pawn pawn)
		{
			Pawn pawn2 = target as Pawn;
			return target != null && (pawn2 == null || !pawn2.ThreatDisabled(pawn)) && pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x04003206 RID: 12806
		private bool ignoreNonCombatants;

		// Token: 0x04003207 RID: 12807
		private bool humanlikesOnly;

		// Token: 0x04003208 RID: 12808
		private int overrideExpiryInterval = -1;
	}
}
