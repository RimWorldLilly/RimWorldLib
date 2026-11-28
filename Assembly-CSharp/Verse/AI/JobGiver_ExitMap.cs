using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008E2 RID: 2274
	public abstract class JobGiver_ExitMap : ThinkNode_JobGiver
	{
		// Token: 0x060045F5 RID: 17909 RVA: 0x001B7DD8 File Offset: 0x001B5FD8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ExitMap jobGiver_ExitMap = (JobGiver_ExitMap)base.DeepCopy(resolve);
			jobGiver_ExitMap.defaultLocomotion = this.defaultLocomotion;
			jobGiver_ExitMap.jobMaxDuration = this.jobMaxDuration;
			jobGiver_ExitMap.canBash = this.canBash;
			jobGiver_ExitMap.forceCanDig = this.forceCanDig;
			jobGiver_ExitMap.forceCanDigIfAnyHostileActiveThreat = this.forceCanDigIfAnyHostileActiveThreat;
			jobGiver_ExitMap.forceCanDigIfCantReachMapEdge = this.forceCanDigIfCantReachMapEdge;
			jobGiver_ExitMap.failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan;
			jobGiver_ExitMap.canFlyOutOfMap = this.canFlyOutOfMap;
			return jobGiver_ExitMap;
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x001B7E54 File Offset: 0x001B6054
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed && !pawn.Crawling)
			{
				return null;
			}
			if (!pawn.MapHeld.CanEverExit)
			{
				if (pawn.MapHeld.IsPocketMap)
				{
					foreach (Thing t in pawn.MapHeld.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MapPortal)))
					{
						if (pawn.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							return JobMaker.MakeJob(JobDefOf.EnterPortal, t);
						}
					}
				}
				return null;
			}
			if (this.canFlyOutOfMap && pawn.RaceProps.canLeaveMapFlying && !pawn.Position.Roofed(pawn.Map) && pawn.Faction != Faction.OfPlayer && pawn.flight.CanEverFly && !pawn.IsQuestLodger())
			{
				return JobMaker.MakeJob(JobDefOf.ExitMapFlying);
			}
			bool flag = this.forceCanDig || (pawn.mindState.duty != null && pawn.mindState.duty.canDig && !pawn.CanReachMapEdge()) || (this.forceCanDigIfCantReachMapEdge && !pawn.CanReachMapEdge()) || (this.forceCanDigIfAnyHostileActiveThreat && pawn.Faction != null && GenHostility.AnyHostileActiveThreatTo(pawn.Map, pawn.Faction, true, false));
			IntVec3 c;
			if (!this.TryFindGoodExitDest(pawn, flag, this.canBash, out c))
			{
				return null;
			}
			if (flag)
			{
				using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, c, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false, false, false, true), null, PathEndMode.OnCell, null))
				{
					IntVec3 cellBeforeBlocker;
					Thing thing = pawnPath.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
					if (thing != null)
					{
						Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, true, true);
						if (job != null)
						{
							return job;
						}
					}
				}
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.Goto, c);
			job2.exitMapOnArrival = true;
			job2.failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan;
			job2.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.defaultLocomotion, LocomotionUrgency.Jog);
			job2.expiryInterval = this.jobMaxDuration;
			job2.canBashDoors = this.canBash;
			return job2;
		}

		// Token: 0x060045F7 RID: 17911
		protected abstract bool TryFindGoodExitDest(Pawn pawn, bool canDig, bool canBash, out IntVec3 dest);

		// Token: 0x04002D73 RID: 11635
		protected LocomotionUrgency defaultLocomotion;

		// Token: 0x04002D74 RID: 11636
		protected int jobMaxDuration = 999999;

		// Token: 0x04002D75 RID: 11637
		protected bool canBash;

		// Token: 0x04002D76 RID: 11638
		protected bool forceCanDig;

		// Token: 0x04002D77 RID: 11639
		protected bool forceCanDigIfAnyHostileActiveThreat;

		// Token: 0x04002D78 RID: 11640
		protected bool forceCanDigIfCantReachMapEdge;

		// Token: 0x04002D79 RID: 11641
		protected bool failIfCantJoinOrCreateCaravan;

		// Token: 0x04002D7A RID: 11642
		protected bool canFlyOutOfMap = true;
	}
}
