using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AD6 RID: 2774
	public abstract class JobGiver_AIFightEnemy : ThinkNode_JobGiver
	{
		// Token: 0x06005310 RID: 21264
		protected abstract bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null);

		// Token: 0x06005311 RID: 21265 RVA: 0x001EEF96 File Offset: 0x001ED196
		protected virtual float GetFlagRadius(Pawn pawn)
		{
			return 999999f;
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06005312 RID: 21266 RVA: 0x001EEF9D File Offset: 0x001ED19D
		protected virtual bool OnlyUseAbilityVerbs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06005313 RID: 21267 RVA: 0x001EEFA0 File Offset: 0x001ED1A0
		protected virtual bool DisableAbilityVerbs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06005314 RID: 21268 RVA: 0x001EEFA3 File Offset: 0x001ED1A3
		protected virtual bool OnlyUseRangedSearch
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x001EEFA6 File Offset: 0x001ED1A6
		protected virtual IntVec3 GetFlagPosition(Pawn pawn)
		{
			return IntVec3.Invalid;
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06005316 RID: 21270 RVA: 0x001EEFAD File Offset: 0x001ED1AD
		protected virtual IntRange ExpiryInterval_Melee
		{
			get
			{
				return new IntRange(360, 480);
			}
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06005317 RID: 21271 RVA: 0x001EEFBE File Offset: 0x001ED1BE
		protected virtual IntRange ExpiryInterval_Ability
		{
			get
			{
				return new IntRange(30, 30);
			}
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x001EEFCC File Offset: 0x001ED1CC
		protected virtual bool ExtraTargetValidator(Pawn pawn, Thing target)
		{
			if (pawn.IsColonyMechPlayerControlled && target.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (this.humanlikesOnly)
			{
				Pawn pawn2 = target as Pawn;
				if (pawn2 != null && !pawn2.RaceProps.Humanlike)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x001EF014 File Offset: 0x001ED214
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIFightEnemy jobGiver_AIFightEnemy = (JobGiver_AIFightEnemy)base.DeepCopy(resolve);
			jobGiver_AIFightEnemy.targetAcquireRadius = this.targetAcquireRadius;
			jobGiver_AIFightEnemy.targetKeepRadius = this.targetKeepRadius;
			jobGiver_AIFightEnemy.needLOSToAcquireNonPawnTargets = this.needLOSToAcquireNonPawnTargets;
			jobGiver_AIFightEnemy.chaseTarget = this.chaseTarget;
			jobGiver_AIFightEnemy.allowTurrets = this.allowTurrets;
			jobGiver_AIFightEnemy.ignoreNonCombatants = this.ignoreNonCombatants;
			jobGiver_AIFightEnemy.humanlikesOnly = this.humanlikesOnly;
			return jobGiver_AIFightEnemy;
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x001EF084 File Offset: 0x001ED284
		protected override Job TryGiveJob(Pawn pawn)
		{
			if ((pawn.IsColonist || pawn.IsColonySubhuman) && pawn.playerSettings.hostilityResponse != HostilityResponseMode.Attack)
			{
				Lord lord = pawn.GetLord();
				LordJob_Ritual_Duel lordJob_Ritual_Duel = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual_Duel;
				if (lordJob_Ritual_Duel == null || !lordJob_Ritual_Duel.duelists.Contains(pawn))
				{
					return null;
				}
			}
			this.UpdateEnemyTarget(pawn);
			Thing enemyTarget = pawn.mindState.enemyTarget;
			if (enemyTarget == null)
			{
				return null;
			}
			Pawn pawn2 = enemyTarget as Pawn;
			if (pawn2 != null && pawn2.IsPsychologicallyInvisible())
			{
				return null;
			}
			bool flag = !pawn.IsColonist && !pawn.IsColonySubhuman && !this.DisableAbilityVerbs;
			if (flag)
			{
				Job abilityJob = JobGiver_AIFightEnemy.GetAbilityJob(pawn, enemyTarget);
				if (abilityJob != null)
				{
					return abilityJob;
				}
			}
			if (this.OnlyUseAbilityVerbs)
			{
				IntVec3 intVec;
				if (!this.TryFindShootingPosition(pawn, out intVec, null))
				{
					return null;
				}
				if (intVec == pawn.Position)
				{
					Pawn_PathFollower pather = pawn.pather;
					if (pather != null)
					{
						pather.StopDead();
					}
					return JobMaker.MakeJob(JobDefOf.Wait_Combat, this.ExpiryInterval_Ability.RandomInRange, true);
				}
				Job job = JobMaker.MakeJob(JobDefOf.Goto, intVec);
				job.expiryInterval = this.ExpiryInterval_Ability.RandomInRange;
				job.checkOverrideOnExpire = true;
				return job;
			}
			else
			{
				Verb verb = pawn.TryGetAttackVerb(enemyTarget, flag, this.allowTurrets);
				if (verb == null)
				{
					return null;
				}
				if (verb.verbProps.IsMeleeAttack)
				{
					return this.MeleeAttackJob(pawn, enemyTarget);
				}
				bool flag2 = CoverUtility.CalculateOverallBlockChance(pawn, enemyTarget.Position, pawn.Map) > 0.01f;
				bool flag3 = pawn.Position.WalkableBy(pawn.Map, pawn) && pawn.Map.pawnDestinationReservationManager.CanReserve(pawn.Position, pawn, pawn.Drafted);
				bool flag4 = verb.CanHitTarget(enemyTarget);
				bool flag5 = (pawn.Position - enemyTarget.Position).LengthHorizontalSquared < 25;
				if ((flag2 && flag3 && flag4) || (flag5 && flag4))
				{
					Pawn_PathFollower pather2 = pawn.pather;
					if (pather2 != null)
					{
						pather2.StopDead();
					}
					return JobMaker.MakeJob(JobDefOf.Wait_Combat, JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange, true);
				}
				IntVec3 intVec2;
				if (!this.TryFindShootingPosition(pawn, out intVec2, null))
				{
					return null;
				}
				if (intVec2 == pawn.Position)
				{
					Pawn_PathFollower pather3 = pawn.pather;
					if (pather3 != null)
					{
						pather3.StopDead();
					}
					return JobMaker.MakeJob(JobDefOf.Wait_Combat, JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange, true);
				}
				Job job2 = JobMaker.MakeJob(JobDefOf.Goto, intVec2);
				job2.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
				job2.checkOverrideOnExpire = true;
				return job2;
			}
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x001EF314 File Offset: 0x001ED514
		public static Job GetAbilityJob(Pawn pawn, Thing enemyTarget)
		{
			if (pawn.abilities == null)
			{
				return null;
			}
			List<Ability> list = pawn.abilities.AICastableAbilities(enemyTarget, true);
			if (list.NullOrEmpty<Ability>())
			{
				return null;
			}
			if (pawn.Position.WalkableBy(pawn.Map, pawn) && pawn.Map.pawnDestinationReservationManager.CanReserve(pawn.Position, pawn, pawn.Drafted))
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].verb.CanHitTarget(enemyTarget))
					{
						return list[i].GetJob(enemyTarget, enemyTarget);
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					LocalTargetInfo localTargetInfo = list[j].AIGetAOETarget();
					if (localTargetInfo.IsValid)
					{
						return list[j].GetJob(localTargetInfo, localTargetInfo);
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k].verb.targetParams.canTargetSelf)
					{
						return list[k].GetJob(pawn, pawn);
					}
				}
			}
			return null;
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x001EF444 File Offset: 0x001ED644
		protected virtual Job MeleeAttackJob(Pawn pawn, Thing enemyTarget)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, enemyTarget);
			job.expiryInterval = this.ExpiryInterval_Melee.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
			return job;
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x0600531D RID: 21277 RVA: 0x001EF483 File Offset: 0x001ED683
		protected virtual int TicksSinceEngageToLoseTarget
		{
			get
			{
				return 400;
			}
		}

		// Token: 0x0600531E RID: 21278 RVA: 0x001EF48C File Offset: 0x001ED68C
		protected virtual void UpdateEnemyTarget(Pawn pawn)
		{
			Thing thing = pawn.mindState.enemyTarget;
			if (thing != null && this.ShouldLoseTarget(pawn))
			{
				thing = null;
			}
			if (thing == null)
			{
				thing = this.FindAttackTargetIfPossible(pawn);
				if (thing != null)
				{
					pawn.mindState.Notify_EngagedTarget();
					Lord lord = pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnAcquiredTarget(pawn, thing);
					}
				}
			}
			else
			{
				Thing thing2 = this.FindAttackTargetIfPossible(pawn);
				if (thing2 == null && !this.chaseTarget)
				{
					thing = null;
				}
				else if (thing2 != null && thing2 != thing)
				{
					pawn.mindState.Notify_EngagedTarget();
					thing = thing2;
				}
			}
			pawn.mindState.enemyTarget = thing;
			Pawn pawn2 = thing as Pawn;
			if (pawn2 != null && thing.Faction == Faction.OfPlayer && pawn.Position.InHorDistOf(thing.Position, 40f) && !pawn2.IsShambler && !pawn.IsPsychologicallyInvisible())
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x001EF569 File Offset: 0x001ED769
		protected Thing FindAttackTargetIfPossible(Pawn pawn)
		{
			if (pawn.TryGetAttackVerb(null, !pawn.IsColonist, false) == null)
			{
				return null;
			}
			return this.FindAttackTarget(pawn);
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x001EF588 File Offset: 0x001ED788
		protected virtual bool ShouldLoseTarget(Pawn pawn)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			if (!enemyTarget.Destroyed && Find.TickManager.TicksGame - pawn.mindState.lastEngageTargetTick <= this.TicksSinceEngageToLoseTarget && pawn.CanReach(enemyTarget, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (float)(pawn.Position - enemyTarget.Position).LengthHorizontalSquared <= this.targetKeepRadius * this.targetKeepRadius)
			{
				IAttackTarget attackTarget = enemyTarget as IAttackTarget;
				return attackTarget != null && attackTarget.ThreatDisabled(pawn);
			}
			return true;
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x001EF618 File Offset: 0x001ED818
		protected virtual Thing FindAttackTarget(Pawn pawn)
		{
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
			if (this.needLOSToAcquireNonPawnTargets)
			{
				targetScanFlags |= TargetScanFlags.NeedLOSToNonPawns;
			}
			if (this.PrimaryVerbIsIncendiary(pawn))
			{
				targetScanFlags |= TargetScanFlags.NeedNonBurning;
			}
			if (this.ignoreNonCombatants)
			{
				targetScanFlags |= TargetScanFlags.IgnoreNonCombatants;
			}
			return (Thing)AttackTargetFinder.BestAttackTarget(pawn, targetScanFlags, (Thing x) => this.ExtraTargetValidator(pawn, x), 0f, this.targetAcquireRadius, this.GetFlagPosition(pawn), this.GetFlagRadius(pawn), false, true, false, this.OnlyUseRangedSearch);
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x001EF6B8 File Offset: 0x001ED8B8
		private bool PrimaryVerbIsIncendiary(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			if (((equipment != null) ? equipment.Primary : null) != null)
			{
				List<Verb> allVerbs = pawn.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i].verbProps.isPrimary)
					{
						return allVerbs[i].IsIncendiary_Ranged();
					}
				}
			}
			return false;
		}

		// Token: 0x040031F8 RID: 12792
		protected float targetAcquireRadius = 56f;

		// Token: 0x040031F9 RID: 12793
		protected float targetKeepRadius = 65f;

		// Token: 0x040031FA RID: 12794
		private bool needLOSToAcquireNonPawnTargets;

		// Token: 0x040031FB RID: 12795
		protected bool chaseTarget;

		// Token: 0x040031FC RID: 12796
		protected bool allowTurrets;

		// Token: 0x040031FD RID: 12797
		protected bool ignoreNonCombatants;

		// Token: 0x040031FE RID: 12798
		protected bool humanlikesOnly;

		// Token: 0x040031FF RID: 12799
		public static readonly IntRange ExpiryInterval_ShooterSucceeded = new IntRange(450, 550);

		// Token: 0x04003200 RID: 12800
		private const int MinTargetDistanceToMove = 5;
	}
}
