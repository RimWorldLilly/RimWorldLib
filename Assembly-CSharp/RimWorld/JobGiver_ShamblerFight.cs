using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B68 RID: 2920
	public class JobGiver_ShamblerFight : JobGiver_AIFightEnemy
	{
		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06005509 RID: 21769 RVA: 0x001FA312 File Offset: 0x001F8512
		protected override IntRange ExpiryInterval_Melee
		{
			get
			{
				return JobGiver_ShamblerFight.CheckForBetterTargetInterval;
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x0600550A RID: 21770 RVA: 0x001FA319 File Offset: 0x001F8519
		protected override int TicksSinceEngageToLoseTarget
		{
			get
			{
				return 4200;
			}
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x0600550B RID: 21771 RVA: 0x001FA320 File Offset: 0x001F8520
		protected override bool DisableAbilityVerbs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600550C RID: 21772 RVA: 0x001FA323 File Offset: 0x001F8523
		public JobGiver_ShamblerFight()
		{
			this.targetAcquireRadius = 20f;
			this.targetKeepRadius = 40f;
		}

		// Token: 0x0600550D RID: 21773 RVA: 0x001FA341 File Offset: 0x001F8541
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.enemyTarget == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x0600550E RID: 21774 RVA: 0x001FA35C File Offset: 0x001F855C
		protected override void UpdateEnemyTarget(Pawn pawn)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			if (enemyTarget == null)
			{
				return;
			}
			Thing thing = base.FindAttackTargetIfPossible(pawn);
			if (thing != null && pawn.Position.DistanceTo(thing.Position) < pawn.Position.DistanceTo(enemyTarget.Position) / 2f)
			{
				pawn.mindState.enemyTarget = thing;
				pawn.mindState.Notify_EngagedTarget();
			}
			if (this.ShouldLoseTarget(pawn))
			{
				pawn.mindState.enemyTarget = thing;
			}
			Pawn pawn2 = thing as Pawn;
			if (pawn2 != null && thing.Faction == Faction.OfPlayer && pawn.Position.InHorDistOf(thing.Position, 40f) && !pawn2.IsShambler && !pawn.IsPsychologicallyInvisible())
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x001FA42B File Offset: 0x001F862B
		protected override Thing FindAttackTarget(Pawn pawn)
		{
			return MutantUtility.FindShamblerTarget(pawn);
		}

		// Token: 0x06005510 RID: 21776 RVA: 0x001FA434 File Offset: 0x001F8634
		protected override Job MeleeAttackJob(Pawn pawn, Thing enemyTarget)
		{
			if (pawn.Faction == Faction.OfPlayer)
			{
				return base.MeleeAttackJob(pawn, enemyTarget);
			}
			Job result;
			using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, enemyTarget.Position, TraverseParms.For(TraverseMode.PassAllDestroyablePlayerOwnedThings, Danger.Deadly, false, false, false, true, false), new PathFinderCostTuning?(JobGiver_ShamblerFight.tuning), PathEndMode.OnCell, null))
			{
				if (!pawnPath.Found)
				{
					Job job = JobMaker.MakeJob(JobDefOf.Wait);
					job.expiryInterval = JobGiver_ShamblerFight.CheckForBetterTargetInterval.RandomInRange;
					result = job;
				}
				else
				{
					IntVec3 intVec;
					Thing thing = pawnPath.FirstBlockingBuilding(out intVec, pawn);
					result = base.MeleeAttackJob(pawn, thing ?? enemyTarget);
				}
			}
			return result;
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x001FA4F0 File Offset: 0x001F86F0
		protected override bool ShouldLoseTarget(Pawn pawn)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			return enemyTarget == null || enemyTarget.Destroyed || !enemyTarget.Spawned || enemyTarget.Map != pawn.Map || ((IAttackTarget)enemyTarget).ThreatDisabled(pawn) || (Find.TickManager.TicksGame - pawn.mindState.lastEngageTargetTick > this.TicksSinceEngageToLoseTarget && !pawn.CanReach(enemyTarget, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.PassAllDestroyablePlayerOwnedThings));
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x001FA56E File Offset: 0x001F876E
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			dest = IntVec3.Invalid;
			return false;
		}

		// Token: 0x040032BB RID: 12987
		private static readonly IntRange CheckForBetterTargetInterval = new IntRange(240, 540);

		// Token: 0x040032BC RID: 12988
		private static readonly PathFinderCostTuning tuning = new PathFinderCostTuning
		{
			costBlockedDoor = 75,
			costBlockedWallBase = 75,
			costBlockedWallExtraForNaturalWalls = 0,
			costBlockedDoorPerHitPoint = 0f,
			costBlockedWallExtraPerHitPoint = 0f
		};
	}
}
