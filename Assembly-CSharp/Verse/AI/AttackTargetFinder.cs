using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000909 RID: 2313
	public static class AttackTargetFinder
	{
		// Token: 0x0600466F RID: 18031 RVA: 0x001B9904 File Offset: 0x001B7B04
		public static IAttackTarget BestAttackTarget(IAttackTargetSearcher searcher, TargetScanFlags flags, Predicate<Thing> validator = null, float minDist = 0f, float maxDist = 9999f, IntVec3 locus = default(IntVec3), float maxTravelRadiusFromLocus = 3.4028235E+38f, bool canBashDoors = false, bool canTakeTargetsCloserThanEffectiveMinRange = true, bool canBashFences = false, bool onlyRanged = false)
		{
			AttackTargetFinder.<>c__DisplayClass5_0 CS$<>8__locals1 = new AttackTargetFinder.<>c__DisplayClass5_0();
			CS$<>8__locals1.searcher = searcher;
			CS$<>8__locals1.canTakeTargetsCloserThanEffectiveMinRange = canTakeTargetsCloserThanEffectiveMinRange;
			CS$<>8__locals1.maxTravelRadiusFromLocus = maxTravelRadiusFromLocus;
			CS$<>8__locals1.locus = locus;
			CS$<>8__locals1.validator = validator;
			CS$<>8__locals1.flags = flags;
			CS$<>8__locals1.canBashDoors = canBashDoors;
			CS$<>8__locals1.canBashFences = canBashFences;
			CS$<>8__locals1.maxDist = maxDist;
			CS$<>8__locals1.searcherThing = CS$<>8__locals1.searcher.Thing;
			CS$<>8__locals1.searcherPawn = (CS$<>8__locals1.searcher as Pawn);
			CS$<>8__locals1.verb = CS$<>8__locals1.searcher.CurrentEffectiveVerb;
			if (CS$<>8__locals1.verb == null)
			{
				Log.Error("BestAttackTarget with " + CS$<>8__locals1.searcher.ToStringSafe<IAttackTargetSearcher>() + " who has no attack verb.");
				return null;
			}
			CS$<>8__locals1.onlyTargetMachines = CS$<>8__locals1.verb.IsEMP();
			CS$<>8__locals1.minDistSquared = minDist * minDist;
			float num = CS$<>8__locals1.maxTravelRadiusFromLocus + CS$<>8__locals1.verb.EffectiveRange;
			CS$<>8__locals1.maxLocusDistSquared = num * num;
			CS$<>8__locals1.losValidator = null;
			CompUniqueWeapon compUniqueWeapon;
			if ((CS$<>8__locals1.verb.EquipmentSource == null || !CS$<>8__locals1.verb.EquipmentSource.TryGetComp(out compUniqueWeapon) || !compUniqueWeapon.IgnoreAccuracyMaluses) && (CS$<>8__locals1.flags & TargetScanFlags.LOSBlockableByGas) != TargetScanFlags.None)
			{
				CS$<>8__locals1.losValidator = ((IntVec3 vec3) => !vec3.AnyGas(CS$<>8__locals1.searcherThing.Map, GasType.BlindSmoke));
			}
			CS$<>8__locals1.innerValidator = delegate(IAttackTarget t)
			{
				Thing thing = t.Thing;
				if (t == CS$<>8__locals1.searcher)
				{
					return false;
				}
				if (CS$<>8__locals1.minDistSquared > 0f && (float)(CS$<>8__locals1.searcherThing.Position - thing.Position).LengthHorizontalSquared < CS$<>8__locals1.minDistSquared)
				{
					return false;
				}
				if (!CS$<>8__locals1.canTakeTargetsCloserThanEffectiveMinRange)
				{
					float num2 = CS$<>8__locals1.verb.verbProps.EffectiveMinRange(thing, CS$<>8__locals1.searcherThing);
					if (num2 > 0f && (float)(CS$<>8__locals1.searcherThing.Position - thing.Position).LengthHorizontalSquared < num2 * num2)
					{
						return false;
					}
				}
				if (CS$<>8__locals1.maxTravelRadiusFromLocus < 9999f && (float)(thing.Position - CS$<>8__locals1.locus).LengthHorizontalSquared > CS$<>8__locals1.maxLocusDistSquared)
				{
					return false;
				}
				if (!CS$<>8__locals1.searcherThing.HostileTo(thing))
				{
					return false;
				}
				if (CS$<>8__locals1.validator != null && !CS$<>8__locals1.validator(thing))
				{
					return false;
				}
				if (CS$<>8__locals1.searcherPawn != null)
				{
					Lord lord = CS$<>8__locals1.searcherPawn.GetLord();
					if (lord != null && !lord.LordJob.ValidateAttackTarget(CS$<>8__locals1.searcherPawn, thing))
					{
						return false;
					}
				}
				if ((CS$<>8__locals1.flags & TargetScanFlags.NeedNotUnderThickRoof) != TargetScanFlags.None)
				{
					RoofDef roof = thing.Position.GetRoof(thing.Map);
					if (roof != null && roof.isThickRoof)
					{
						return false;
					}
				}
				if ((CS$<>8__locals1.flags & TargetScanFlags.NeedLOSToAll) != TargetScanFlags.None)
				{
					if (CS$<>8__locals1.losValidator != null && (!CS$<>8__locals1.losValidator(CS$<>8__locals1.searcherThing.Position) || !CS$<>8__locals1.losValidator(thing.Position)))
					{
						return false;
					}
					if (!CS$<>8__locals1.searcherThing.CanSee(thing, CS$<>8__locals1.losValidator))
					{
						if (t is Pawn)
						{
							if ((CS$<>8__locals1.flags & TargetScanFlags.NeedLOSToPawns) != TargetScanFlags.None)
							{
								return false;
							}
						}
						else if ((CS$<>8__locals1.flags & TargetScanFlags.NeedLOSToNonPawns) != TargetScanFlags.None)
						{
							return false;
						}
					}
				}
				if (((CS$<>8__locals1.flags & TargetScanFlags.NeedThreat) != TargetScanFlags.None || (CS$<>8__locals1.flags & TargetScanFlags.NeedAutoTargetable) != TargetScanFlags.None) && t.ThreatDisabled(CS$<>8__locals1.searcher))
				{
					return false;
				}
				if ((CS$<>8__locals1.flags & TargetScanFlags.NeedAutoTargetable) != TargetScanFlags.None && !AttackTargetFinder.IsAutoTargetable(t))
				{
					return false;
				}
				if ((CS$<>8__locals1.flags & TargetScanFlags.NeedActiveThreat) != TargetScanFlags.None && !GenHostility.IsActiveThreatTo(t, CS$<>8__locals1.searcher.Thing.Faction, true, false))
				{
					return false;
				}
				Pawn pawn = t as Pawn;
				if (CS$<>8__locals1.onlyTargetMachines && pawn != null && pawn.RaceProps.IsFlesh)
				{
					return false;
				}
				if ((CS$<>8__locals1.flags & TargetScanFlags.NeedNonBurning) != TargetScanFlags.None && thing.IsBurning())
				{
					return false;
				}
				if (CS$<>8__locals1.searcherThing.def.race != null && CS$<>8__locals1.searcherThing.def.race.intelligence >= Intelligence.Humanlike)
				{
					CompExplosive compExplosive = thing.TryGetComp<CompExplosive>();
					if (compExplosive != null && compExplosive.wickStarted)
					{
						return false;
					}
				}
				Pawn searcherPawn = CS$<>8__locals1.searcherPawn;
				bool flag4;
				if (searcherPawn == null || !searcherPawn.IsColonist)
				{
					Thing searcherThing = CS$<>8__locals1.searcherThing;
					flag4 = (((searcherThing != null) ? searcherThing.Faction : null) == Faction.OfPlayer);
				}
				else
				{
					flag4 = true;
				}
				if (flag4)
				{
					if (thing.def.size.x == 1 && thing.def.size.z == 1)
					{
						if (thing.Position.Fogged(thing.Map))
						{
							return false;
						}
					}
					else
					{
						bool flag5 = false;
						using (CellRect.Enumerator enumerator = thing.OccupiedRect().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (!enumerator.Current.Fogged(thing.Map))
								{
									flag5 = true;
									break;
								}
							}
						}
						if (!flag5)
						{
							return false;
						}
					}
				}
				return true;
			};
			Predicate<IAttackTarget> oldValidator;
			if ((AttackTargetFinder.HasRangedAttack(CS$<>8__locals1.searcher) || onlyRanged) && (CS$<>8__locals1.searcherPawn == null || !CS$<>8__locals1.searcherPawn.InAggroMentalState))
			{
				AttackTargetFinder.tmpTargets.Clear();
				AttackTargetFinder.tmpTargets.AddRange(CS$<>8__locals1.searcherThing.Map.attackTargetsCache.GetPotentialTargetsFor(CS$<>8__locals1.searcher));
				AttackTargetFinder.tmpTargets.RemoveAll((IAttackTarget t) => AttackTargetFinder.ShouldIgnoreNoncombatant(CS$<>8__locals1.searcherThing, t, CS$<>8__locals1.flags));
				if ((CS$<>8__locals1.flags & TargetScanFlags.NeedReachable) != TargetScanFlags.None)
				{
					Predicate<IAttackTarget> oldValidator = CS$<>8__locals1.innerValidator;
					CS$<>8__locals1.innerValidator = ((IAttackTarget t) => oldValidator(t) && AttackTargetFinder.CanReach(CS$<>8__locals1.searcherThing, t.Thing, CS$<>8__locals1.canBashDoors, CS$<>8__locals1.canBashFences));
				}
				bool flag = false;
				for (int i = 0; i < AttackTargetFinder.tmpTargets.Count; i++)
				{
					IAttackTarget attackTarget = AttackTargetFinder.tmpTargets[i];
					if (attackTarget.Thing.Position.InHorDistOf(CS$<>8__locals1.searcherThing.Position, CS$<>8__locals1.maxDist) && CS$<>8__locals1.innerValidator(attackTarget) && AttackTargetFinder.CanShootAtFromCurrentPosition(attackTarget, CS$<>8__locals1.searcher, CS$<>8__locals1.verb))
					{
						flag = true;
						break;
					}
				}
				IAttackTarget result;
				if (flag)
				{
					AttackTargetFinder.tmpTargets.RemoveAll((IAttackTarget x) => !x.Thing.Position.InHorDistOf(CS$<>8__locals1.searcherThing.Position, CS$<>8__locals1.maxDist) || !CS$<>8__locals1.innerValidator(x));
					result = AttackTargetFinder.GetRandomShootingTargetByScore(AttackTargetFinder.tmpTargets, CS$<>8__locals1.searcher, CS$<>8__locals1.verb);
				}
				else
				{
					bool flag2 = (CS$<>8__locals1.flags & TargetScanFlags.NeedReachableIfCantHitFromMyPos) > TargetScanFlags.None;
					bool flag3 = (CS$<>8__locals1.flags & TargetScanFlags.NeedReachable) > TargetScanFlags.None;
					Predicate<Thing> validator2;
					if (flag2 && !flag3)
					{
						validator2 = ((Thing t) => CS$<>8__locals1.innerValidator((IAttackTarget)t) && (AttackTargetFinder.CanReach(CS$<>8__locals1.searcherThing, t, CS$<>8__locals1.canBashDoors, CS$<>8__locals1.canBashFences) || AttackTargetFinder.CanShootAtFromCurrentPosition((IAttackTarget)t, CS$<>8__locals1.searcher, CS$<>8__locals1.verb)));
					}
					else
					{
						validator2 = ((Thing t) => CS$<>8__locals1.innerValidator((IAttackTarget)t));
					}
					result = (IAttackTarget)GenClosest.ClosestThing_Global(CS$<>8__locals1.searcherThing.Position, AttackTargetFinder.tmpTargets, CS$<>8__locals1.maxDist, validator2, null, false);
				}
				AttackTargetFinder.tmpTargets.Clear();
				return result;
			}
			if (CS$<>8__locals1.searcherPawn != null && CS$<>8__locals1.searcherPawn.mindState.duty != null && CS$<>8__locals1.searcherPawn.mindState.duty.radius > 0f && !CS$<>8__locals1.searcherPawn.InMentalState)
			{
				Predicate<IAttackTarget> oldValidator = CS$<>8__locals1.innerValidator;
				CS$<>8__locals1.innerValidator = ((IAttackTarget t) => oldValidator(t) && t.Thing.Position.InHorDistOf(CS$<>8__locals1.searcherPawn.mindState.duty.focus.Cell, CS$<>8__locals1.searcherPawn.mindState.duty.radius));
			}
			oldValidator = CS$<>8__locals1.innerValidator;
			CS$<>8__locals1.innerValidator = ((IAttackTarget t) => oldValidator(t) && !AttackTargetFinder.ShouldIgnoreNoncombatant(CS$<>8__locals1.searcherThing, t, CS$<>8__locals1.flags));
			IAttackTarget attackTarget2 = (IAttackTarget)GenClosest.ClosestThingReachable(CS$<>8__locals1.searcherThing.Position, CS$<>8__locals1.searcherThing.Map, ThingRequest.ForGroup(ThingRequestGroup.AttackTarget), PathEndMode.Touch, TraverseParms.For(CS$<>8__locals1.searcherPawn, Danger.Deadly, TraverseMode.ByPawn, CS$<>8__locals1.canBashDoors, false, CS$<>8__locals1.canBashFences, true), CS$<>8__locals1.maxDist, (Thing x) => CS$<>8__locals1.innerValidator((IAttackTarget)x), null, 0, (CS$<>8__locals1.maxDist > 800f) ? -1 : 40, false, RegionType.Set_Passable, false, false);
			if (attackTarget2 != null && PawnUtility.ShouldCollideWithPawns(CS$<>8__locals1.searcherPawn))
			{
				IAttackTarget attackTarget3 = AttackTargetFinder.FindBestReachableMeleeTarget(CS$<>8__locals1.innerValidator, CS$<>8__locals1.searcherPawn, CS$<>8__locals1.maxDist, CS$<>8__locals1.canBashDoors, CS$<>8__locals1.canBashFences);
				if (attackTarget3 != null)
				{
					float lengthHorizontal = (CS$<>8__locals1.searcherPawn.Position - attackTarget2.Thing.Position).LengthHorizontal;
					float lengthHorizontal2 = (CS$<>8__locals1.searcherPawn.Position - attackTarget3.Thing.Position).LengthHorizontal;
					if (Mathf.Abs(lengthHorizontal - lengthHorizontal2) < 50f)
					{
						attackTarget2 = attackTarget3;
					}
				}
			}
			return attackTarget2;
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x001B9DF0 File Offset: 0x001B7FF0
		private static bool ShouldIgnoreNoncombatant(Thing searcherThing, IAttackTarget t, TargetScanFlags flags)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && !pawn.IsCombatant() && ((flags & TargetScanFlags.IgnoreNonCombatants) != TargetScanFlags.None || !GenSight.LineOfSightToThing(searcherThing.Position, pawn, searcherThing.Map, false, null));
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x001B9E38 File Offset: 0x001B8038
		private static bool CanReach(Thing searcher, Thing target, bool canBashDoors, bool canBashFences)
		{
			Pawn pawn = searcher as Pawn;
			if (pawn != null)
			{
				if (!pawn.CanReach(target, PathEndMode.Touch, Danger.Some, canBashDoors, canBashFences, TraverseMode.ByPawn))
				{
					return false;
				}
			}
			else
			{
				TraverseMode mode = canBashDoors ? TraverseMode.PassDoors : TraverseMode.NoPassClosedDoors;
				if (!searcher.Map.reachability.CanReach(searcher.Position, target, PathEndMode.Touch, TraverseParms.For(mode, Danger.Deadly, false, false, false, true, false)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x001B9E9C File Offset: 0x001B809C
		private static IAttackTarget FindBestReachableMeleeTarget(Predicate<IAttackTarget> validator, Pawn searcherPawn, float maxTargDist, bool canBashDoors, bool canBashFences)
		{
			AttackTargetFinder.<>c__DisplayClass8_0 CS$<>8__locals1 = new AttackTargetFinder.<>c__DisplayClass8_0();
			CS$<>8__locals1.searcherPawn = searcherPawn;
			CS$<>8__locals1.validator = validator;
			CS$<>8__locals1.maxTargDist = maxTargDist;
			CS$<>8__locals1.canBashDoors = canBashDoors;
			CS$<>8__locals1.canBashFences = canBashFences;
			CS$<>8__locals1.maxTargDist = Mathf.Min(CS$<>8__locals1.maxTargDist, 30f);
			CS$<>8__locals1.reachableTarget = null;
			CS$<>8__locals1.searcherPawn.Map.floodFiller.FloodFill(CS$<>8__locals1.searcherPawn.Position, new Predicate<IntVec3>(CS$<>8__locals1.<FindBestReachableMeleeTarget>g__PassCheck|1), new Func<IntVec3, bool>(CS$<>8__locals1.<FindBestReachableMeleeTarget>g__Processor|2), int.MaxValue, false, null);
			return CS$<>8__locals1.reachableTarget;
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x001B9F38 File Offset: 0x001B8138
		private static bool HasRangedAttack(IAttackTargetSearcher t)
		{
			Verb currentEffectiveVerb = t.CurrentEffectiveVerb;
			return currentEffectiveVerb != null && !currentEffectiveVerb.verbProps.IsMeleeAttack;
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x001B9F5F File Offset: 0x001B815F
		private static bool CanShootAtFromCurrentPosition(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
		{
			return verb != null && verb.CanHitTargetFrom(searcher.Thing.Position, target.Thing);
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x001B9F84 File Offset: 0x001B8184
		private static IAttackTarget GetRandomShootingTargetByScore(List<IAttackTarget> targets, IAttackTargetSearcher searcher, Verb verb)
		{
			Pair<IAttackTarget, float> pair;
			if (AttackTargetFinder.GetAvailableShootingTargetsByScore(targets, searcher, verb).TryRandomElementByWeight((Pair<IAttackTarget, float> x) => x.Second, out pair))
			{
				return pair.First;
			}
			return null;
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x001B9FCC File Offset: 0x001B81CC
		private static List<Pair<IAttackTarget, float>> GetAvailableShootingTargetsByScore(List<IAttackTarget> rawTargets, IAttackTargetSearcher searcher, Verb verb)
		{
			AttackTargetFinder.availableShootingTargets.Clear();
			if (rawTargets.Count == 0)
			{
				return AttackTargetFinder.availableShootingTargets;
			}
			AttackTargetFinder.tmpTargetScores.Clear();
			AttackTargetFinder.tmpCanShootAtTarget.Clear();
			float num = 0f;
			IAttackTarget attackTarget = null;
			for (int i = 0; i < rawTargets.Count; i++)
			{
				AttackTargetFinder.tmpTargetScores.Add(float.MinValue);
				AttackTargetFinder.tmpCanShootAtTarget.Add(false);
				if (rawTargets[i] != searcher)
				{
					bool flag = AttackTargetFinder.CanShootAtFromCurrentPosition(rawTargets[i], searcher, verb);
					AttackTargetFinder.tmpCanShootAtTarget[i] = flag;
					if (flag)
					{
						float shootingTargetScore = AttackTargetFinder.GetShootingTargetScore(rawTargets[i], searcher, verb);
						AttackTargetFinder.tmpTargetScores[i] = shootingTargetScore;
						if (attackTarget == null || shootingTargetScore > num)
						{
							attackTarget = rawTargets[i];
							num = shootingTargetScore;
						}
					}
				}
			}
			if (num < 1f)
			{
				if (attackTarget != null)
				{
					AttackTargetFinder.availableShootingTargets.Add(new Pair<IAttackTarget, float>(attackTarget, 1f));
				}
			}
			else
			{
				float num2 = num - 30f;
				for (int j = 0; j < rawTargets.Count; j++)
				{
					if (rawTargets[j] != searcher && AttackTargetFinder.tmpCanShootAtTarget[j])
					{
						float num3 = AttackTargetFinder.tmpTargetScores[j];
						if (num3 >= num2)
						{
							float second = Mathf.InverseLerp(num - 30f, num, num3);
							AttackTargetFinder.availableShootingTargets.Add(new Pair<IAttackTarget, float>(rawTargets[j], second));
						}
					}
				}
			}
			return AttackTargetFinder.availableShootingTargets;
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x001BA130 File Offset: 0x001B8330
		private static float GetShootingTargetScore(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
		{
			float num = 60f;
			num -= Mathf.Min((target.Thing.Position - searcher.Thing.Position).LengthHorizontal, 40f);
			if (target.TargetCurrentlyAimingAt == searcher.Thing)
			{
				num += 10f;
			}
			if (searcher.LastAttackedTarget == target.Thing && Find.TickManager.TicksGame - searcher.LastAttackTargetTick <= 300)
			{
				num += 40f;
			}
			num -= CoverUtility.CalculateOverallBlockChance(target.Thing.Position, searcher.Thing.Position, searcher.Thing.Map) * 10f;
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				num -= AttackTargetFinder.NonCombatantScore(pawn);
				if (verb.verbProps.ai_TargetHasRangedAttackScoreOffset != 0f && pawn.CurrentEffectiveVerb != null && pawn.CurrentEffectiveVerb.verbProps.Ranged)
				{
					num += verb.verbProps.ai_TargetHasRangedAttackScoreOffset;
				}
				if (pawn.Downed)
				{
					num -= 50f;
				}
			}
			num += AttackTargetFinder.FriendlyFireBlastRadiusTargetScoreOffset(target, searcher, verb);
			num += AttackTargetFinder.FriendlyFireConeTargetScoreOffset(target, searcher, verb);
			return num * target.TargetPriorityFactor;
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x001BA27C File Offset: 0x001B847C
		private static float NonCombatantScore(Thing target)
		{
			Pawn pawn = target as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			if (!pawn.IsCombatant())
			{
				return 50f;
			}
			if (pawn.DevelopmentalStage.Juvenile())
			{
				return 25f;
			}
			return 0f;
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x001BA2C0 File Offset: 0x001B84C0
		private static float FriendlyFireBlastRadiusTargetScoreOffset(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
		{
			if (verb.verbProps.ai_AvoidFriendlyFireRadius <= 0f)
			{
				return 0f;
			}
			Map map = target.Thing.Map;
			IntVec3 position = target.Thing.Position;
			int num = GenRadial.NumCellsInRadius(verb.verbProps.ai_AvoidFriendlyFireRadius);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					bool flag = true;
					List<Thing> thingList = intVec.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j] is IAttackTarget && thingList[j] != target)
						{
							if (flag)
							{
								if (!GenSight.LineOfSight(position, intVec, map, true, null, 0, 0))
								{
									break;
								}
								flag = false;
							}
							float num3;
							if (thingList[j] == searcher)
							{
								num3 = 40f;
							}
							else if (thingList[j] is Pawn)
							{
								num3 = (thingList[j].def.race.Animal ? 7f : 18f);
							}
							else
							{
								num3 = 10f;
							}
							if (searcher.Thing.HostileTo(thingList[j]))
							{
								num2 += num3 * 0.6f;
							}
							else
							{
								num2 -= num3;
							}
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x001BA430 File Offset: 0x001B8630
		private static float FriendlyFireConeTargetScoreOffset(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
		{
			Pawn pawn = searcher.Thing as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			if (pawn.RaceProps.intelligence < Intelligence.ToolUser)
			{
				return 0f;
			}
			if (pawn.RaceProps.IsMechanoid)
			{
				return 0f;
			}
			Verb_Shoot verb_Shoot = verb as Verb_Shoot;
			if (verb_Shoot == null)
			{
				return 0f;
			}
			ThingDef defaultProjectile = verb_Shoot.verbProps.defaultProjectile;
			if (defaultProjectile == null)
			{
				return 0f;
			}
			if (defaultProjectile.projectile.flyOverhead)
			{
				return 0f;
			}
			Map map = pawn.Map;
			ShotReport report = ShotReport.HitReportFor(pawn, verb, (Thing)target);
			float radius = Mathf.Max(VerbUtility.CalculateAdjustedForcedMiss(verb.verbProps.ForcedMissRadius, report.ShootLine.Dest - report.ShootLine.Source), 1.5f);
			Func<IntVec3, bool> <>9__3;
			IEnumerable<IntVec3> enumerable = (from dest in GenRadial.RadialCellsAround(report.ShootLine.Dest, radius, true)
			where dest.InBounds(map)
			select new ShootLine(report.ShootLine.Source, dest)).SelectMany(delegate(ShootLine line)
			{
				IEnumerable<IntVec3> source = line.Points().Concat(line.Dest);
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__3) == null)
				{
					predicate = (<>9__3 = ((IntVec3 pos) => pos.CanBeSeenOverFast(map)));
				}
				return source.TakeWhile(predicate);
			}).Distinct<IntVec3>();
			float num = 0f;
			foreach (IntVec3 c in enumerable)
			{
				float num2 = VerbUtility.InterceptChanceFactorFromDistance(report.ShootLine.Source.ToVector3Shifted(), c);
				if (num2 > 0f)
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Thing thing = thingList[i];
						if (thing is IAttackTarget && thing != target)
						{
							float num3;
							if (thing == searcher)
							{
								num3 = 40f;
							}
							else if (thing is Pawn)
							{
								num3 = (thing.def.race.Animal ? 7f : 18f);
							}
							else
							{
								num3 = 10f;
							}
							num3 *= num2;
							if (searcher.Thing.HostileTo(thing))
							{
								num3 *= 0.6f;
							}
							else
							{
								num3 *= -1f;
							}
							num += num3;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x001BA6B8 File Offset: 0x001B88B8
		public static IAttackTarget BestShootTargetFromCurrentPosition(IAttackTargetSearcher searcher, TargetScanFlags flags, Predicate<Thing> validator = null, float minDistance = 0f, float maxDistance = 9999f)
		{
			Verb currentEffectiveVerb = searcher.CurrentEffectiveVerb;
			if (currentEffectiveVerb == null)
			{
				Log.Error("BestShootTargetFromCurrentPosition with " + searcher.ToStringSafe<IAttackTargetSearcher>() + " who has no attack verb.");
				return null;
			}
			return AttackTargetFinder.BestAttackTarget(searcher, flags, validator, Mathf.Max(minDistance, currentEffectiveVerb.verbProps.minRange), Mathf.Min(maxDistance, currentEffectiveVerb.EffectiveRange), default(IntVec3), float.MaxValue, false, false, false, false);
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x001BA724 File Offset: 0x001B8924
		public static bool CanSee(this Thing seer, Thing target, Func<IntVec3, bool> validator = null)
		{
			ShootLeanUtility.CalcShootableCellsOf(AttackTargetFinder.tempDestList, target, seer.Position);
			for (int i = 0; i < AttackTargetFinder.tempDestList.Count; i++)
			{
				if (GenSight.LineOfSight(seer.Position, AttackTargetFinder.tempDestList[i], seer.Map, true, validator, 0, 0))
				{
					return true;
				}
			}
			ShootLeanUtility.LeanShootingSourcesFromTo(seer.Position, target.Position, seer.Map, AttackTargetFinder.tempSourceList);
			for (int j = 0; j < AttackTargetFinder.tempSourceList.Count; j++)
			{
				if (AttackTargetFinder.tempSourceList[j].CanBeSeenOver(seer.Map))
				{
					for (int k = 0; k < AttackTargetFinder.tempDestList.Count; k++)
					{
						if (GenSight.LineOfSight(AttackTargetFinder.tempSourceList[j], AttackTargetFinder.tempDestList[k], seer.Map, true, validator, 0, 0))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x001BA804 File Offset: 0x001B8A04
		public static void DebugDrawAttackTargetScores_Update()
		{
			IAttackTargetSearcher attackTargetSearcher = Find.Selector.SingleSelectedThing as IAttackTargetSearcher;
			if (attackTargetSearcher == null)
			{
				return;
			}
			if (attackTargetSearcher.Thing.Map != Find.CurrentMap)
			{
				return;
			}
			Verb currentEffectiveVerb = attackTargetSearcher.CurrentEffectiveVerb;
			if (currentEffectiveVerb == null)
			{
				return;
			}
			AttackTargetFinder.tmpTargets.Clear();
			List<Thing> list = attackTargetSearcher.Thing.Map.listerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
			for (int i = 0; i < list.Count; i++)
			{
				AttackTargetFinder.tmpTargets.Add((IAttackTarget)list[i]);
			}
			List<Pair<IAttackTarget, float>> availableShootingTargetsByScore = AttackTargetFinder.GetAvailableShootingTargetsByScore(AttackTargetFinder.tmpTargets, attackTargetSearcher, currentEffectiveVerb);
			for (int j = 0; j < availableShootingTargetsByScore.Count; j++)
			{
				GenDraw.DrawLineBetween(attackTargetSearcher.Thing.DrawPos, availableShootingTargetsByScore[j].First.Thing.DrawPos);
			}
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x001BA8E0 File Offset: 0x001B8AE0
		public static void DebugDrawAttackTargetScores_OnGUI()
		{
			IAttackTargetSearcher attackTargetSearcher = Find.Selector.SingleSelectedThing as IAttackTargetSearcher;
			if (attackTargetSearcher == null)
			{
				return;
			}
			if (attackTargetSearcher.Thing.Map != Find.CurrentMap)
			{
				return;
			}
			Verb currentEffectiveVerb = attackTargetSearcher.CurrentEffectiveVerb;
			if (currentEffectiveVerb == null)
			{
				return;
			}
			List<Thing> list = attackTargetSearcher.Thing.Map.listerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing != attackTargetSearcher)
				{
					string text;
					Color red;
					if (!AttackTargetFinder.CanShootAtFromCurrentPosition((IAttackTarget)thing, attackTargetSearcher, currentEffectiveVerb))
					{
						text = "out of range";
						red = Color.red;
					}
					else
					{
						text = AttackTargetFinder.GetShootingTargetScore((IAttackTarget)thing, attackTargetSearcher, currentEffectiveVerb).ToString("F0");
						red = new Color(0.25f, 1f, 0.25f);
					}
					GenMapUI.DrawThingLabel(thing.DrawPos.MapToUIPosition(), text, red);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x001BA9DC File Offset: 0x001B8BDC
		public static void DebugDrawNonCombatantTimer_OnGUI()
		{
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Pawn);
			using (new TextBlock(new GameFont?(GameFont.Tiny), new TextAnchor?(TextAnchor.MiddleCenter), new bool?(false)))
			{
				foreach (Thing thing in list)
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && pawn.mindState != null)
					{
						int lastCombatantTick = pawn.mindState.lastCombatantTick;
						Vector2 vector = pawn.DrawPos.MapToUIPosition();
						if (pawn.IsCombatant())
						{
							int num = lastCombatantTick + 3600 - Find.TickManager.TicksGame;
							if (pawn.IsPermanentCombatant() || num == 3600)
							{
								GenMapUI.DrawThingLabel(vector, "combatant", Color.red);
							}
							else
							{
								Vector2 screenPos = vector;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 1);
								defaultInterpolatedStringHandler.AppendLiteral("combatant ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(num);
								GenMapUI.DrawThingLabel(screenPos, defaultInterpolatedStringHandler.ToStringAndClear(), Color.red);
							}
						}
						else
						{
							GenMapUI.DrawThingLabel(vector, "non-combatant", Color.green);
						}
					}
				}
			}
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x001BAB24 File Offset: 0x001B8D24
		public static bool IsAutoTargetable(IAttackTarget target)
		{
			CompCanBeDormant compCanBeDormant = target.Thing.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null && !compCanBeDormant.Awake)
			{
				return false;
			}
			CompInitiatable compInitiatable = target.Thing.TryGetComp<CompInitiatable>();
			if (compInitiatable != null && !compInitiatable.Initiated)
			{
				return false;
			}
			Pawn pawn = target.Thing as Pawn;
			return pawn == null || !pawn.RaceProps.IsMechanoid || pawn.Awake();
		}

		// Token: 0x04002DB0 RID: 11696
		private const float FriendlyFireScoreOffsetPerHumanlikeOrMechanoid = 18f;

		// Token: 0x04002DB1 RID: 11697
		private const float FriendlyFireScoreOffsetPerAnimal = 7f;

		// Token: 0x04002DB2 RID: 11698
		private const float FriendlyFireScoreOffsetPerNonPawn = 10f;

		// Token: 0x04002DB3 RID: 11699
		private const float FriendlyFireScoreOffsetSelf = 40f;

		// Token: 0x04002DB4 RID: 11700
		private static List<IAttackTarget> tmpTargets = new List<IAttackTarget>(128);

		// Token: 0x04002DB5 RID: 11701
		private static List<Pair<IAttackTarget, float>> availableShootingTargets = new List<Pair<IAttackTarget, float>>();

		// Token: 0x04002DB6 RID: 11702
		private static List<float> tmpTargetScores = new List<float>();

		// Token: 0x04002DB7 RID: 11703
		private static List<bool> tmpCanShootAtTarget = new List<bool>();

		// Token: 0x04002DB8 RID: 11704
		private static readonly List<IntVec3> tempDestList = new List<IntVec3>();

		// Token: 0x04002DB9 RID: 11705
		private static readonly List<IntVec3> tempSourceList = new List<IntVec3>();
	}
}
