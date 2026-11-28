using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ADB RID: 2779
	public class JobGiver_AIGotoNearestHostile : ThinkNode_JobGiver
	{
		// Token: 0x06005337 RID: 21303 RVA: 0x001EF8C7 File Offset: 0x001EDAC7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIGotoNearestHostile jobGiver_AIGotoNearestHostile = (JobGiver_AIGotoNearestHostile)base.DeepCopy(resolve);
			jobGiver_AIGotoNearestHostile.ignoreNonCombatants = this.ignoreNonCombatants;
			jobGiver_AIGotoNearestHostile.humanlikesOnly = this.humanlikesOnly;
			jobGiver_AIGotoNearestHostile.overrideExpiryInterval = this.overrideExpiryInterval;
			return jobGiver_AIGotoNearestHostile;
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x001EF8FC File Offset: 0x001EDAFC
		protected override Job TryGiveJob(Pawn pawn)
		{
			float num = float.MaxValue;
			Thing thing = null;
			List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				IAttackTarget attackTarget = potentialTargetsFor[i];
				if (!attackTarget.ThreatDisabled(pawn) && AttackTargetFinder.IsAutoTargetable(attackTarget))
				{
					if (this.humanlikesOnly)
					{
						Pawn pawn2 = attackTarget as Pawn;
						if (pawn2 != null && !pawn2.RaceProps.Humanlike)
						{
							goto IL_112;
						}
					}
					Pawn pawn3 = attackTarget.Thing as Pawn;
					if ((pawn3 == null || pawn3.IsCombatant() || (!this.ignoreNonCombatants && GenSight.LineOfSightToThing(pawn.Position, pawn3, pawn.Map, false, null))) && (pawn.Faction == null || !pawn.Faction.IsPlayer || !attackTarget.Thing.Position.Fogged(pawn.Map)))
					{
						Thing thing2 = (Thing)attackTarget;
						int num2 = thing2.Position.DistanceToSquared(pawn.Position);
						if ((float)num2 < num && pawn.CanReach(thing2, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							num = (float)num2;
							thing = thing2;
						}
					}
				}
				IL_112:;
			}
			if (thing == null)
			{
				return null;
			}
			if (thing.PositionHeld == pawn.PositionHeld || pawn.CanReachImmediate(thing, PathEndMode.Touch))
			{
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
			job.expireRequiresEnemiesNearby = true;
			job.collideWithPawns = true;
			return job;
		}

		// Token: 0x04003203 RID: 12803
		private bool ignoreNonCombatants;

		// Token: 0x04003204 RID: 12804
		private bool humanlikesOnly;

		// Token: 0x04003205 RID: 12805
		private int overrideExpiryInterval = -1;
	}
}
