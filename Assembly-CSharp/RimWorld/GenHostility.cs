using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001608 RID: 5640
	public static class GenHostility
	{
		// Token: 0x06008C90 RID: 35984 RVA: 0x003313A8 File Offset: 0x0032F5A8
		public static bool HostileTo(this Thing a, Thing b)
		{
			if (a.Destroyed || b.Destroyed || a == b)
			{
				return false;
			}
			if ((a.Faction == null && a.TryGetComp<CompCauseGameCondition>() != null) || (b.Faction == null && b.TryGetComp<CompCauseGameCondition>() != null))
			{
				return true;
			}
			Pawn pawn = a as Pawn;
			Pawn pawn2 = b as Pawn;
			if (GenHostility.IsActivityDormant(pawn) || GenHostility.IsActivityDormant(pawn2))
			{
				return false;
			}
			if ((pawn != null && pawn.kindDef.hostileToAll) || (pawn2 != null && pawn2.kindDef.hostileToAll))
			{
				return true;
			}
			if (pawn != null && pawn2 != null && ((pawn.story != null && pawn.story.traits.DisableHostilityFrom(pawn2)) || (pawn2.story != null && pawn2.story.traits.DisableHostilityFrom(pawn))))
			{
				return false;
			}
			if ((pawn != null && pawn.MentalState != null && pawn.MentalState.ForceHostileTo(b)) || (pawn2 != null && pawn2.MentalState != null && pawn2.MentalState.ForceHostileTo(a)))
			{
				return true;
			}
			if (pawn != null && pawn2 != null && (GenHostility.IsPredatorHostileTo(pawn, pawn2) || GenHostility.IsPredatorHostileTo(pawn2, pawn)))
			{
				return true;
			}
			if ((a.Faction != null && pawn2 != null && pawn2.HostFaction == a.Faction && (pawn == null || pawn.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn2)) || (b.Faction != null && pawn != null && pawn.HostFaction == b.Faction && (pawn2 == null || pawn2.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn)))
			{
				return true;
			}
			if ((a.Faction != null && pawn2 != null && pawn2.IsSlave && pawn2.Faction == a.Faction && (pawn == null || !pawn.IsSlave) && SlaveRebellionUtility.IsRebelling(pawn2)) || (b.Faction != null && pawn != null && pawn.IsSlave && pawn.Faction == b.Faction && (pawn2 == null || !pawn2.IsSlave) && SlaveRebellionUtility.IsRebelling(pawn)))
			{
				return true;
			}
			if ((a.Faction != null && pawn2 != null && pawn2.HostFaction == a.Faction) || (b.Faction != null && pawn != null && pawn.HostFaction == b.Faction))
			{
				return false;
			}
			if (pawn != null && pawn.IsPrisoner && pawn2 != null && pawn2.IsPrisoner)
			{
				return false;
			}
			if (pawn != null && pawn.IsSlave && pawn2 != null && pawn2.IsSlave)
			{
				return false;
			}
			if (pawn != null && pawn2 != null && ((pawn.IsPrisoner && pawn.HostFaction == pawn2.HostFaction && !PrisonBreakUtility.IsPrisonBreaking(pawn)) || (pawn2.IsPrisoner && pawn2.HostFaction == pawn.HostFaction && !PrisonBreakUtility.IsPrisonBreaking(pawn2))))
			{
				return false;
			}
			if (pawn != null && pawn2 != null && ((pawn.HostFaction != null && pawn2.Faction != null && !pawn.HostFaction.HostileTo(pawn2.Faction) && !PrisonBreakUtility.IsPrisonBreaking(pawn)) || (pawn2.HostFaction != null && pawn.Faction != null && !pawn2.HostFaction.HostileTo(pawn.Faction) && !PrisonBreakUtility.IsPrisonBreaking(pawn2))))
			{
				return false;
			}
			if ((a.Faction != null && a.Faction.IsPlayer && pawn2 != null && pawn2.mindState.WillJoinColonyIfRescued) || (b.Faction != null && b.Faction.IsPlayer && pawn != null && pawn.mindState.WillJoinColonyIfRescued))
			{
				return false;
			}
			if (pawn != null && pawn2 != null && (pawn.ThreatDisabledBecauseNonAggressiveRoamer(pawn2) || pawn2.ThreatDisabledBecauseNonAggressiveRoamer(pawn)))
			{
				return false;
			}
			if ((pawn != null && MechanitorUtility.IsPlayerOverseerSubject(pawn) && !pawn.IsColonyMechPlayerControlled) || (pawn2 != null && MechanitorUtility.IsPlayerOverseerSubject(pawn2) && !pawn2.IsColonyMechPlayerControlled))
			{
				return false;
			}
			if ((pawn != null && pawn.Faction == null && pawn.RaceProps.Humanlike && b.Faction != null && b.Faction.def.hostileToFactionlessHumanlikes) || (pawn2 != null && pawn2.Faction == null && pawn2.RaceProps.Humanlike && a.Faction != null && a.Faction.def.hostileToFactionlessHumanlikes))
			{
				return true;
			}
			if (pawn != null && pawn2 != null && (pawn.IsShambler || pawn2.IsShambler))
			{
				return MutantUtility.CheckShamblerHostility(pawn, pawn2);
			}
			return (pawn == null || !(b is Building_Turret) || !pawn.IsPsychologicallyInvisible()) && (pawn2 == null || !(a is Building_Turret) || !pawn2.IsPsychologicallyInvisible()) && a.Faction != null && b.Faction != null && a.Faction.HostileTo(b.Faction);
		}

		// Token: 0x06008C91 RID: 35985 RVA: 0x003317E4 File Offset: 0x0032F9E4
		private static bool IsActivityDormant(Pawn pawn)
		{
			if (pawn == null)
			{
				return false;
			}
			CompCanBeDormant canBeDormant = pawn.canBeDormant;
			if (canBeDormant != null && !canBeDormant.Awake)
			{
				return false;
			}
			if (pawn.needs != null)
			{
				CompActivity activity = pawn.activity;
				return activity != null && activity.IsDormant;
			}
			return false;
		}

		// Token: 0x06008C92 RID: 35986 RVA: 0x00331828 File Offset: 0x0032FA28
		public static bool HostileTo(this Thing t, Faction fac)
		{
			if (t.Destroyed)
			{
				return false;
			}
			if (fac == null)
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				MentalState mentalState = pawn.MentalState;
				if (mentalState != null && mentalState.ForceHostileTo(fac))
				{
					return true;
				}
				if (GenHostility.IsPredatorHostileTo(pawn, fac))
				{
					return true;
				}
				if (pawn.HostFaction == fac && PrisonBreakUtility.IsPrisonBreaking(pawn))
				{
					return true;
				}
				if (pawn.Faction == fac && SlaveRebellionUtility.IsRebelling(pawn))
				{
					return true;
				}
				if (pawn.IsShambler && pawn.Faction == null)
				{
					return true;
				}
				if (pawn.HostFaction == fac)
				{
					return false;
				}
				if (pawn.HostFaction != null && !pawn.HostFaction.HostileTo(fac) && !PrisonBreakUtility.IsPrisonBreaking(pawn))
				{
					return false;
				}
				if (fac.IsPlayer && pawn.mindState.WillJoinColonyIfRescued)
				{
					return false;
				}
				if (GenHostility.IsActivityDormant(pawn))
				{
					return false;
				}
				if (pawn.kindDef.hostileToAll)
				{
					return true;
				}
				if (fac.def.hostileToFactionlessHumanlikes && pawn.Faction == null && pawn.RaceProps.Humanlike)
				{
					return true;
				}
			}
			else if (t.Faction == null && t.TryGetComp<CompCauseGameCondition>() != null)
			{
				return true;
			}
			return t.Faction != null && t.Faction.HostileTo(fac);
		}

		// Token: 0x06008C93 RID: 35987 RVA: 0x00331950 File Offset: 0x0032FB50
		private static bool IsPredatorHostileTo(Pawn predator, Pawn toPawn)
		{
			if (toPawn.Faction == null)
			{
				return false;
			}
			if (toPawn.Faction.HasPredatorRecentlyAttackedAnyone(predator))
			{
				return true;
			}
			Pawn preyOfMyFaction = GenHostility.GetPreyOfMyFaction(predator, toPawn.Faction);
			return preyOfMyFaction != null && predator.Position.InHorDistOf(preyOfMyFaction.Position, 12f);
		}

		// Token: 0x06008C94 RID: 35988 RVA: 0x003319A5 File Offset: 0x0032FBA5
		private static bool IsPredatorHostileTo(Pawn predator, Faction toFaction)
		{
			return toFaction.HasPredatorRecentlyAttackedAnyone(predator) || GenHostility.GetPreyOfMyFaction(predator, toFaction) != null;
		}

		// Token: 0x06008C95 RID: 35989 RVA: 0x003319C0 File Offset: 0x0032FBC0
		private static Pawn GetPreyOfMyFaction(Pawn predator, Faction myFaction)
		{
			Job curJob = predator.CurJob;
			if (curJob != null && curJob.def == JobDefOf.PredatorHunt && !predator.jobs.curDriver.ended)
			{
				Pawn pawn = curJob.GetTarget(TargetIndex.A).Thing as Pawn;
				if (pawn != null && !pawn.Dead && pawn.Faction == myFaction)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06008C96 RID: 35990 RVA: 0x00331A22 File Offset: 0x0032FC22
		public static bool AnyHostileActiveThreatToPlayer(Map map, bool countDormantPawnsAsHostile = false, bool canBeFogged = false)
		{
			return GenHostility.AnyHostileActiveThreatTo(map, Faction.OfPlayer, countDormantPawnsAsHostile, canBeFogged);
		}

		// Token: 0x06008C97 RID: 35991 RVA: 0x00331A34 File Offset: 0x0032FC34
		public static bool AnyHostileActiveThreatTo(Map map, Faction faction, bool countDormantPawnsAsHostile = false, bool canBeFogged = false)
		{
			IAttackTarget attackTarget;
			return GenHostility.AnyHostileActiveThreatTo(map, faction, out attackTarget, countDormantPawnsAsHostile, canBeFogged);
		}

		// Token: 0x06008C98 RID: 35992 RVA: 0x00331A4C File Offset: 0x0032FC4C
		public static bool AnyHostileActiveThreatTo(Map map, Faction faction, out IAttackTarget threat, bool countDormantPawnsAsHostile = false, bool canBeFogged = false)
		{
			foreach (IAttackTarget attackTarget in map.attackTargetsCache.TargetsHostileToFaction(faction))
			{
				if (GenHostility.IsActiveThreatTo(attackTarget, faction, true, canBeFogged))
				{
					threat = attackTarget;
					return true;
				}
				if (countDormantPawnsAsHostile && attackTarget.Thing.HostileTo(faction) && (canBeFogged || !attackTarget.Thing.Fogged()) && !attackTarget.ThreatDisabled(null))
				{
					Pawn pawn = attackTarget.Thing as Pawn;
					if (pawn != null)
					{
						CompCanBeDormant comp = pawn.GetComp<CompCanBeDormant>();
						if (comp != null && !comp.Awake)
						{
							threat = attackTarget;
							return true;
						}
					}
				}
			}
			threat = null;
			return false;
		}

		// Token: 0x06008C99 RID: 35993 RVA: 0x00331B10 File Offset: 0x0032FD10
		public static bool IsActiveThreatToPlayer(IAttackTarget target, bool canBeFogged = false)
		{
			return GenHostility.IsActiveThreatTo(target, Faction.OfPlayer, true, canBeFogged);
		}

		// Token: 0x06008C9A RID: 35994 RVA: 0x00331B20 File Offset: 0x0032FD20
		public static bool IsPotentialThreat(IAttackTarget target)
		{
			if (!(target.Thing is IAttackTargetSearcher))
			{
				return false;
			}
			if (target.ThreatDisabled(null))
			{
				return false;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null && (pawn.MentalStateDef == MentalStateDefOf.PanicFlee || pawn.IsPrisoner))
			{
				return false;
			}
			if (pawn != null && !pawn.Awake())
			{
				return false;
			}
			CompCanBeDormant compCanBeDormant = target.Thing.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null && !compCanBeDormant.Awake)
			{
				return false;
			}
			CompMechanoid compMechanoid = target.Thing.TryGetComp<CompMechanoid>();
			if (compMechanoid != null && compMechanoid.Deactivated)
			{
				return false;
			}
			CompInitiatable compInitiatable = target.Thing.TryGetComp<CompInitiatable>();
			if (compInitiatable != null && !compInitiatable.Initiated)
			{
				return false;
			}
			if (target.Thing.Spawned && target.Thing.Map.generatorDef.defeatRequiresCantReachUnfogged)
			{
				TraverseParms traverseParms = (pawn != null) ? TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true) : TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
				if (!target.Thing.Map.reachability.CanReachUnfogged(target.Thing.Position, traverseParms))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008C9B RID: 35995 RVA: 0x00331C30 File Offset: 0x0032FE30
		public static bool IsActiveThreatTo(IAttackTarget target, Faction faction, bool ignoreHives = true, bool canBeFogged = false)
		{
			if (!target.Thing.HostileTo(faction))
			{
				return false;
			}
			if (!canBeFogged && target.Thing.Fogged())
			{
				return false;
			}
			if (ignoreHives)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					Lord lord = pawn.GetLord();
					if (lord != null && lord.LordJob is LordJob_DefendAndExpandHive && (pawn.mindState.duty == null || pawn.mindState.duty.def != DutyDefOf.AssaultColony))
					{
						return false;
					}
				}
			}
			return GenHostility.IsPotentialThreat(target);
		}

		// Token: 0x06008C9C RID: 35996 RVA: 0x00331CB9 File Offset: 0x0032FEB9
		public static bool IsDefMechClusterThreat(ThingDef def)
		{
			return (def.building != null && (def.building.IsTurret || def.building.IsMortar)) || def.isMechClusterThreat;
		}

		// Token: 0x06008C9D RID: 35997 RVA: 0x00331CE5 File Offset: 0x0032FEE5
		public static void Notify_PawnLostForTutor(Pawn pawn, Map map)
		{
			if (!map.IsPlayerHome && map.mapPawns.FreeColonistsSpawnedCount != 0 && !GenHostility.AnyHostileActiveThreatToPlayer(map, false, false))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.ReformCaravan, OpportunityType.Important);
			}
		}
	}
}
