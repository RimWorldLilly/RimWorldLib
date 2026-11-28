using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200083F RID: 2111
	public static class Toils_Combat
	{
		// Token: 0x06004275 RID: 17013 RVA: 0x001A3918 File Offset: 0x001A1B18
		public static Toil TrySetJobToUseAttackVerb(TargetIndex targetInd)
		{
			Toil toil = ToilMaker.MakeToil("TrySetJobToUseAttackVerb");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				bool allowManualCastWeapons = !actor.IsColonist;
				Verb verb = actor.TryGetAttackVerb(curJob.GetTarget(targetInd).Thing, allowManualCastWeapons, false);
				if (verb == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				curJob.verbToUse = verb;
			};
			return toil;
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x001A3960 File Offset: 0x001A1B60
		public static Toil GotoCastPosition(TargetIndex targetInd, TargetIndex castPositionInd = TargetIndex.None, bool closeIfDowned = false, float maxRangeFactor = 1f)
		{
			Toil toil = ToilMaker.MakeToil("GotoCastPosition");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(targetInd).Thing;
				Pawn pawn = thing as Pawn;
				if (actor == thing)
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				if (thing == null)
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				CastPositionRequest newReq = default(CastPositionRequest);
				newReq.caster = toil.actor;
				newReq.target = thing;
				newReq.verb = curJob.verbToUse;
				newReq.maxRangeFromTarget = ((!closeIfDowned || pawn == null || !pawn.Downed) ? Mathf.Max(curJob.verbToUse.verbProps.range * maxRangeFactor, 1.42f) : Mathf.Min(curJob.verbToUse.verbProps.range, (float)pawn.RaceProps.executionRange));
				newReq.wantCoverFromTarget = false;
				if (castPositionInd != TargetIndex.None)
				{
					newReq.preferredCastPosition = new IntVec3?(curJob.GetTarget(castPositionInd).Cell);
				}
				IntVec3 intVec;
				if (!CastPositionFinder.TryFindCastPosition(newReq, out intVec))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				toil.actor.pather.StartPath(intVec, PathEndMode.OnCell);
				actor.Map.pawnDestinationReservationManager.Reserve(actor, curJob, intVec);
			};
			toil.FailOnDespawnedOrNull(targetInd);
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x001A39DA File Offset: 0x001A1BDA
		public static Toil CastVerb(TargetIndex targetInd, bool canHitNonTargetPawns = true)
		{
			return Toils_Combat.CastVerb(targetInd, TargetIndex.None, canHitNonTargetPawns);
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x001A39E4 File Offset: 0x001A1BE4
		public static Toil CastVerb(TargetIndex targetInd, TargetIndex destInd, bool canHitNonTargetPawns = true)
		{
			Toil toil = ToilMaker.MakeToil("CastVerb");
			toil.initAction = delegate()
			{
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(targetInd);
				LocalTargetInfo destTarg = (destInd != TargetIndex.None) ? toil.actor.jobs.curJob.GetTarget(destInd) : LocalTargetInfo.Invalid;
				toil.actor.jobs.curJob.verbToUse.TryStartCastOn(target, destTarg, false, canHitNonTargetPawns, toil.actor.jobs.curJob.preventFriendlyFire, false);
			};
			toil.defaultCompleteMode = ToilCompleteMode.FinishedBusy;
			toil.activeSkill = (() => Toils_Combat.GetActiveSkillForToil(toil));
			return toil;
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x001A3A5C File Offset: 0x001A1C5C
		public static SkillDef GetActiveSkillForToil(Toil toil)
		{
			Verb verbToUse = toil.actor.jobs.curJob.verbToUse;
			if (verbToUse != null && verbToUse.EquipmentSource != null)
			{
				if (verbToUse.EquipmentSource.def.IsMeleeWeapon)
				{
					return SkillDefOf.Melee;
				}
				if (verbToUse.EquipmentSource.def.IsRangedWeapon)
				{
					return SkillDefOf.Shooting;
				}
			}
			return null;
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x001A3ABB File Offset: 0x001A1CBB
		public static Toil FollowAndMeleeAttack(TargetIndex targetInd, Action hitAction)
		{
			return Toils_Combat.FollowAndMeleeAttack(targetInd, TargetIndex.None, hitAction);
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x001A3AC8 File Offset: 0x001A1CC8
		public static Toil FollowAndMeleeAttack(TargetIndex targetInd, TargetIndex standPositionInd, Action hitAction)
		{
			Toil followAndAttack = ToilMaker.MakeToil("FollowAndMeleeAttack");
			followAndAttack.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = followAndAttack.actor;
				Job curJob = actor.jobs.curJob;
				JobDriver curDriver = actor.jobs.curDriver;
				LocalTargetInfo target = curJob.GetTarget(targetInd);
				Thing thing = target.Thing;
				Pawn pawn = thing as Pawn;
				bool flag = actor.IsHashIntervalTick(250, delta);
				if (!thing.Spawned || (pawn != null && pawn.IsPsychologicallyInvisible()) || (flag && !actor.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)))
				{
					curDriver.ReadyForNextToil();
					return;
				}
				LocalTargetInfo localTargetInfo = target;
				PathEndMode peMode = PathEndMode.Touch;
				if (standPositionInd != TargetIndex.None)
				{
					LocalTargetInfo target2 = curJob.GetTarget(standPositionInd);
					if (target2.IsValid)
					{
						localTargetInfo = target2;
						peMode = PathEndMode.OnCell;
					}
				}
				if (!(localTargetInfo != actor.pather.Destination) && (actor.pather.Moving || actor.CanReachImmediate(target, PathEndMode.Touch)))
				{
					if (actor.CanReachImmediate(target, PathEndMode.Touch))
					{
						if (pawn != null)
						{
							bool flag2 = pawn.IsMutant && pawn.mutant.Def.canAttackWhileCrawling && !pawn.ThreatDisabled(null);
							if (pawn.Downed && !flag2 && !curJob.killIncappedTarget)
							{
								curDriver.ReadyForNextToil();
								return;
							}
						}
						hitAction();
					}
					return;
				}
				if (actor.CurJob.ensureReachable && !actor.CanReach(target, peMode, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(localTargetInfo, peMode);
			};
			followAndAttack.activeSkill = (() => SkillDefOf.Melee);
			followAndAttack.defaultCompleteMode = ToilCompleteMode.Never;
			return followAndAttack;
		}
	}
}
