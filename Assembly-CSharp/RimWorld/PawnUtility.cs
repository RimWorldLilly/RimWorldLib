using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020014F1 RID: 5361
	public static class PawnUtility
	{
		// Token: 0x0600831B RID: 33563 RVA: 0x002F1310 File Offset: 0x002EF510
		public static Faction GetFactionLeaderFaction(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].leader == pawn)
				{
					return allFactionsListForReading[i];
				}
			}
			return null;
		}

		// Token: 0x0600831C RID: 33564 RVA: 0x002F1351 File Offset: 0x002EF551
		public static bool IsFactionLeader(Pawn pawn)
		{
			return PawnUtility.GetFactionLeaderFaction(pawn) != null;
		}

		// Token: 0x0600831D RID: 33565 RVA: 0x002F135C File Offset: 0x002EF55C
		public static bool IsInteractionBlocked(this Pawn pawn, InteractionDef interaction, bool isInitiator, bool isRandom)
		{
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			if (mentalStateDef == null)
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].def.blocksSocialInteraction)
					{
						return true;
					}
				}
				Lord lord = pawn.GetLord();
				return lord != null && lord.BlocksSocialInteraction(pawn);
			}
			if (isRandom)
			{
				return mentalStateDef.blockRandomInteraction;
			}
			if (interaction == null)
			{
				return false;
			}
			List<InteractionDef> list = isInitiator ? mentalStateDef.blockInteractionInitiationExcept : mentalStateDef.blockInteractionRecipientExcept;
			return list != null && !list.Contains(interaction);
		}

		// Token: 0x0600831E RID: 33566 RVA: 0x002F13F4 File Offset: 0x002EF5F4
		public static bool IsKidnappedPawn(Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600831F RID: 33567 RVA: 0x002F1439 File Offset: 0x002EF639
		public static bool IsTravelingInTransportPodWorldObject(Pawn pawn)
		{
			return (pawn.IsWorldPawn() && ThingOwnerUtility.AnyParentIs<ActiveTransporterInfo>(pawn)) || ThingOwnerUtility.AnyParentIs<TravellingTransporters>(pawn);
		}

		// Token: 0x06008320 RID: 33568 RVA: 0x002F1453 File Offset: 0x002EF653
		public static bool ForSaleBySettlement(Pawn pawn)
		{
			return pawn.ParentHolder is Settlement_TraderTracker;
		}

		// Token: 0x06008321 RID: 33569 RVA: 0x002F1463 File Offset: 0x002EF663
		public static bool IsCarrying(this Pawn pawn)
		{
			return !pawn.Destroyed && pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null;
		}

		// Token: 0x06008322 RID: 33570 RVA: 0x002F1485 File Offset: 0x002EF685
		public static bool IsCarryingPawn(this Pawn pawn, Pawn carryPawn = null)
		{
			return pawn.carryTracker != null && pawn.carryTracker.CarriedThing is Pawn && (carryPawn == null || pawn.carryTracker.CarriedThing == carryPawn);
		}

		// Token: 0x06008323 RID: 33571 RVA: 0x002F14B6 File Offset: 0x002EF6B6
		public static bool IsCarryingThing(this Pawn pawn, Thing carriedThing)
		{
			return pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.CarriedThing == carriedThing;
		}

		// Token: 0x06008324 RID: 33572 RVA: 0x002F14DD File Offset: 0x002EF6DD
		public static void TryDestroyStartingColonistFamily(Pawn pawn)
		{
			if (!pawn.relations.RelatedPawns.Any((Pawn x) => Find.GameInitData.startingAndOptionalPawns.Contains(x)))
			{
				PawnUtility.DestroyStartingColonistFamily(pawn);
			}
		}

		// Token: 0x06008325 RID: 33573 RVA: 0x002F1518 File Offset: 0x002EF718
		public static void DestroyStartingColonistFamily(Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.relations.RelatedPawns.ToList<Pawn>())
			{
				if (!Find.GameInitData.startingAndOptionalPawns.Contains(pawn2))
				{
					WorldPawnSituation situation = Find.WorldPawns.GetSituation(pawn2);
					if (situation == WorldPawnSituation.Free || situation == WorldPawnSituation.Dead)
					{
						Find.WorldPawns.RemovePawn(pawn2);
						Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
					}
				}
			}
		}

		// Token: 0x06008326 RID: 33574 RVA: 0x002F15AC File Offset: 0x002EF7AC
		public static bool EnemiesAreNearby(Pawn pawn, int regionsToScan = 9, bool passDoors = false, float maxDistance = -1f, int maxCount = 1, bool invisible = false)
		{
			TraverseParms tp = passDoors ? TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false) : TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			int count = 0;
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(tp, false), delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				int count;
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if ((maxDistance <= 0f || thing.Position.InHorDistOf(pawn.Position, maxDistance)) && thing.HostileTo(pawn))
					{
						if (!invisible)
						{
							Pawn pawn2 = thing as Pawn;
							if (pawn2 != null && pawn2.IsPsychologicallyInvisible())
							{
								goto IL_83;
							}
						}
						count = count;
						count++;
					}
					IL_83:;
				}
				return count >= maxCount;
			}, regionsToScan, RegionType.Set_Passable);
			return count >= maxCount;
		}

		// Token: 0x06008327 RID: 33575 RVA: 0x002F1654 File Offset: 0x002EF854
		public static bool WillSoonHaveBasicNeed(Pawn p, float thresholdOffset = 0.05f)
		{
			return p.needs != null && ((p.needs.rest != null && p.needs.rest.CurLevel < 0.28f + thresholdOffset) || (p.needs.food != null && p.needs.food.CurLevelPercentage < p.needs.food.PercentageThreshHungry + thresholdOffset));
		}

		// Token: 0x06008328 RID: 33576 RVA: 0x002F16C8 File Offset: 0x002EF8C8
		public static bool CanCasuallyInteractNow(this Pawn p, bool twoWayInteraction = false, bool canInteractWhileSleeping = false, bool canInteractWhileRoaming = false, bool canInteractWhileDrafted = false)
		{
			if (p.Drafted && !canInteractWhileDrafted)
			{
				return false;
			}
			if (p.IsPsychologicallyInvisible())
			{
				return false;
			}
			if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(p))
			{
				return false;
			}
			if (p.InAggroMentalState)
			{
				return false;
			}
			if (p.InMentalState && p.MentalStateDef == MentalStateDefOf.Roaming && !canInteractWhileRoaming)
			{
				return false;
			}
			if (!p.Awake() && !canInteractWhileSleeping)
			{
				return false;
			}
			if (p.IsFormingCaravan())
			{
				return false;
			}
			Job curJob = p.CurJob;
			return curJob == null || !twoWayInteraction || (curJob.def.casualInterruptible && curJob.playerForced);
		}

		// Token: 0x06008329 RID: 33577 RVA: 0x002F1757 File Offset: 0x002EF957
		public static IEnumerable<Pawn> SpawnedMasteredPawns(Pawn master)
		{
			PawnUtility.<SpawnedMasteredPawns>d__14 <SpawnedMasteredPawns>d__ = new PawnUtility.<SpawnedMasteredPawns>d__14(-2);
			<SpawnedMasteredPawns>d__.<>3__master = master;
			return <SpawnedMasteredPawns>d__;
		}

		// Token: 0x0600832A RID: 33578 RVA: 0x002F1767 File Offset: 0x002EF967
		public static bool InValidState(Pawn p)
		{
			return p.health != null && (p.Dead || (p.stances != null && p.mindState != null && p.needs != null && p.ageTracker != null));
		}

		// Token: 0x0600832B RID: 33579 RVA: 0x002F17A0 File Offset: 0x002EF9A0
		public static PawnPosture GetPosture(this Pawn p)
		{
			PawnPosture pawnPosture;
			if (p.Dead)
			{
				pawnPosture = PawnPosture.LayingOnGroundNormal;
			}
			else if (ModsConfig.BiotechActive && p.IsCharging())
			{
				pawnPosture = PawnPosture.Standing;
			}
			else
			{
				IThingHolderWithDrawnPawn thingHolderWithDrawnPawn = p.ParentHolder as IThingHolderWithDrawnPawn;
				if (thingHolderWithDrawnPawn != null)
				{
					pawnPosture = thingHolderWithDrawnPawn.HeldPawnPosture;
				}
				else if (p.Downed)
				{
					if (p.jobs != null && p.jobs.posture.Laying())
					{
						pawnPosture = p.jobs.posture;
					}
					else
					{
						pawnPosture = PawnPosture.LayingOnGroundNormal;
					}
				}
				else if (p.jobs == null)
				{
					pawnPosture = PawnPosture.Standing;
				}
				else
				{
					pawnPosture = p.jobs.posture;
				}
			}
			Pawn_MindState mindState = p.mindState;
			bool flag;
			if (mindState == null)
			{
				flag = false;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				bool? flag2;
				if (duty == null)
				{
					flag2 = null;
				}
				else
				{
					DutyDef def = duty.def;
					flag2 = ((def != null) ? new bool?(def.forceFaceUpPosture) : null);
				}
				bool? flag3 = flag2;
				flag = flag3.GetValueOrDefault();
			}
			if (flag && pawnPosture != PawnPosture.Standing)
			{
				pawnPosture |= PawnPosture.FaceUpMask;
			}
			return pawnPosture;
		}

		// Token: 0x0600832C RID: 33580 RVA: 0x002F1884 File Offset: 0x002EFA84
		public static void ForceWait(Pawn pawn, int ticks, Thing faceTarget = null, bool maintainPosture = false, bool maintainSleep = false)
		{
			if (ticks <= 0)
			{
				Log.ErrorOnce("Forcing a wait for zero ticks", 47045639);
			}
			JobDef def = maintainPosture ? JobDefOf.Wait_MaintainPosture : JobDefOf.Wait;
			if (pawn.IsDeactivated())
			{
				def = JobDefOf.Deactivated;
			}
			if (pawn.IsSelfShutdown())
			{
				def = JobDefOf.SelfShutdown;
			}
			else if (pawn.InBed())
			{
				def = (pawn.Awake() ? JobDefOf.LayDownAwake : JobDefOf.LayDown);
			}
			else if (!pawn.health.capacities.CanBeAwake)
			{
				def = JobDefOf.Wait_Downed;
			}
			else if (maintainSleep && !pawn.Awake())
			{
				def = JobDefOf.Wait_Asleep;
			}
			Job job = JobMaker.MakeJob(def, faceTarget);
			if (maintainSleep && !pawn.Awake())
			{
				job.forceSleep = true;
				job.targetA = pawn.Position;
			}
			if (pawn.InBed())
			{
				job.targetA = pawn.CurrentBed();
			}
			job.expiryInterval = ticks;
			pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
		}

		// Token: 0x0600832D RID: 33581 RVA: 0x002F1998 File Offset: 0x002EFB98
		public static void GainComfortFromCellIfPossible(this Pawn p, int delta, bool chairsOnly = false)
		{
			if (!p.Spawned)
			{
				return;
			}
			if (p.IsHashIntervalTick(15, delta))
			{
				Building edifice = p.Position.GetEdifice(p.Map);
				if (edifice != null && (!chairsOnly || (edifice.def.category == ThingCategory.Building && edifice.def.building.isSittable)))
				{
					PawnUtility.GainComfortFromThingIfPossible(p, edifice, delta);
				}
			}
		}

		// Token: 0x0600832E RID: 33582 RVA: 0x002F19FC File Offset: 0x002EFBFC
		public static void GainComfortFromThingIfPossible(Pawn p, Thing from, int delta)
		{
			if (p.IsHashIntervalTick(15, delta))
			{
				float statValue = from.GetStatValue(StatDefOf.Comfort, true, 100);
				if (statValue >= 0f && p.needs != null && p.needs.comfort != null)
				{
					p.needs.comfort.ComfortUsed(statValue);
				}
			}
		}

		// Token: 0x0600832F RID: 33583 RVA: 0x002F1A54 File Offset: 0x002EFC54
		public static float BodyResourceGrowthSpeed(Pawn pawn)
		{
			Pawn_NeedsTracker needs = pawn.needs;
			float? num;
			if (needs == null)
			{
				num = null;
			}
			else
			{
				Need_Food food = needs.food;
				num = ((food != null) ? new float?(food.CurCategory.HungerMultiplier()) : null);
			}
			float? num2 = num;
			return num2.GetValueOrDefault(1f);
		}

		// Token: 0x06008330 RID: 33584 RVA: 0x002F1AA8 File Offset: 0x002EFCA8
		public static bool FertileMateTarget(Pawn male, Pawn female)
		{
			if (female.gender != Gender.Female || female.Sterile())
			{
				return false;
			}
			CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
			return compEggLayer == null || !compEggLayer.FullyFertilized;
		}

		// Token: 0x06008331 RID: 33585 RVA: 0x002F1AE0 File Offset: 0x002EFCE0
		public static void Mated(Pawn male, Pawn female)
		{
			if (female.Sterile() || male.Sterile())
			{
				return;
			}
			CompEggLayer compEggLayer = female.TryGetComp<CompEggLayer>();
			if (compEggLayer != null)
			{
				compEggLayer.Fertilize(male);
				return;
			}
			if (Rand.Value < 0.5f)
			{
				Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, female, null);
				hediff_Pregnant.SetParents(null, male, null);
				female.health.AddHediff(hediff_Pregnant, null, null, null);
			}
		}

		// Token: 0x06008332 RID: 33586 RVA: 0x002F1B50 File Offset: 0x002EFD50
		public static bool PlayerForcedJobNowOrSoon(Pawn pawn)
		{
			if (pawn.jobs == null)
			{
				return false;
			}
			Job curJob = pawn.CurJob;
			if (curJob != null)
			{
				return curJob.playerForced;
			}
			return pawn.jobs.jobQueue.Count > 0 && pawn.jobs.jobQueue.Peek().job.playerForced;
		}

		// Token: 0x06008333 RID: 33587 RVA: 0x002F1BA8 File Offset: 0x002EFDA8
		public static bool TrySpawnHatchedOrBornPawn(Pawn pawn, Thing motherOrEgg, IntVec3? positionOverride = null)
		{
			if (motherOrEgg.SpawnedOrAnyParentSpawned)
			{
				return GenSpawn.Spawn(pawn, positionOverride ?? motherOrEgg.PositionHeld, motherOrEgg.MapHeld, WipeMode.Vanish) != null;
			}
			Pawn pawn2 = motherOrEgg as Pawn;
			if (pawn2 != null)
			{
				if (pawn2.IsCaravanMember())
				{
					pawn2.GetCaravan().AddPawn(pawn, true);
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return true;
				}
				IThingHolder parentHolder = pawn2.ParentHolder;
				if (parentHolder != null)
				{
					if (!parentHolder.GetDirectlyHeldThings().TryAdd(pawn, true))
					{
						return false;
					}
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return true;
				}
				else if (pawn2.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return true;
				}
			}
			else if (motherOrEgg.ParentHolder != null)
			{
				Pawn_InventoryTracker pawn_InventoryTracker = motherOrEgg.ParentHolder as Pawn_InventoryTracker;
				if (pawn_InventoryTracker != null)
				{
					if (pawn_InventoryTracker.pawn.IsCaravanMember())
					{
						pawn_InventoryTracker.pawn.GetCaravan().AddPawn(pawn, true);
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return true;
					}
					if (pawn_InventoryTracker.pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008334 RID: 33588 RVA: 0x002F1CB4 File Offset: 0x002EFEB4
		public static bool TryGetAvoidGrid(this Pawn p, out AvoidGrid grid, bool onlyIfLordAllows = true)
		{
			grid = null;
			if (!p.Spawned)
			{
				return false;
			}
			if (p.Faction == null)
			{
				return false;
			}
			if (!p.Faction.def.canUseAvoidGrid)
			{
				return false;
			}
			if (p.Faction == Faction.OfPlayer || !p.Faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (p.kindDef.canUseAvoidGrid)
			{
				grid = p.Map.avoidGrid;
				return true;
			}
			if (!onlyIfLordAllows)
			{
				grid = p.Map.avoidGrid;
				return true;
			}
			Lord lord = p.GetLord();
			LordToil lordToil = (lord != null) ? lord.CurLordToil : null;
			if (lordToil != null && lordToil.useAvoidGrid)
			{
				grid = lord.Map.avoidGrid;
				return true;
			}
			return false;
		}

		// Token: 0x06008335 RID: 33589 RVA: 0x002F1D6C File Offset: 0x002EFF6C
		public static bool ShouldCollideWithPawns(Pawn p)
		{
			return p != null && !p.Downed && !p.Dead && !p.IsSelfShutdown() && (p.IsShambler || (p.mindState.anyCloseHostilesRecently && !p.IsPsychologicallyInvisible() && p.kindDef.collidesWithPawns));
		}

		// Token: 0x06008336 RID: 33590 RVA: 0x002F1DC9 File Offset: 0x002EFFC9
		public static bool AnyPawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false, bool useId = false)
		{
			return PawnUtility.PawnBlockingPathAt(c, forPawn, actAsIfHadCollideWithPawnsJob, collideOnlyWithStandingPawns, forPathFinder, useId) != null;
		}

		// Token: 0x06008337 RID: 33591 RVA: 0x002F1DDC File Offset: 0x002EFFDC
		public static Pawn PawnBlockingPathAt(IntVec3 c, Pawn forPawn, bool actAsIfHadCollideWithPawnsJob = false, bool collideOnlyWithStandingPawns = false, bool forPathFinder = false, bool useId = false)
		{
			List<Thing> thingList = c.GetThingList(forPawn.Map);
			if (thingList.Count == 0)
			{
				return null;
			}
			bool collideWithNonHostile = false;
			if (actAsIfHadCollideWithPawnsJob)
			{
				collideWithNonHostile = true;
			}
			else
			{
				Job curJob = forPawn.CurJob;
				if (curJob != null && (curJob.collideWithPawns || curJob.def.collideWithPawns || forPawn.jobs.curDriver.collideWithPawns))
				{
					collideWithNonHostile = true;
				}
			}
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (PawnUtility.PawnBlockedBy(forPawn, pawn, collideOnlyWithStandingPawns, collideWithNonHostile, forPathFinder, useId))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06008338 RID: 33592 RVA: 0x002F1E70 File Offset: 0x002F0070
		public static bool PawnBlockedBy(Pawn forPawn, Pawn other, bool collideOnlyWithStandingPawns = false, bool collideWithNonHostile = false, bool forPathFinder = false, bool useId = false)
		{
			if (!PawnUtility.ShouldCollideWithPawns(other))
			{
				return false;
			}
			if (useId && forPawn.thingIDNumber < other.thingIDNumber && !other.stances.FullBodyBusyRecently)
			{
				return false;
			}
			if (other == forPawn)
			{
				return false;
			}
			if (collideOnlyWithStandingPawns)
			{
				if (other.pather.MovingNow)
				{
					return false;
				}
				if (other.pather.Moving && other.pather.MovedRecently(60))
				{
					return false;
				}
			}
			if (PawnUtility.PawnsCanShareCellBecauseOfBodySize(other, forPawn))
			{
				return false;
			}
			if (other.IsPsychologicallyInvisible())
			{
				return false;
			}
			if (!other.kindDef.collidesWithPawns)
			{
				return false;
			}
			if (other.HostileTo(forPawn))
			{
				return true;
			}
			if (forPawn.IsShambler && !MutantUtility.ShamblerShouldCollideWith(forPawn, other))
			{
				return false;
			}
			if (collideWithNonHostile)
			{
				if (!forPathFinder && forPawn.Drafted && other.RaceProps.Animal)
				{
					return false;
				}
				Job curJob = other.CurJob;
				if (curJob != null && (curJob.collideWithPawns || curJob.def.collideWithPawns || other.jobs.curDriver.collideWithPawns))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008339 RID: 33593 RVA: 0x002F1F74 File Offset: 0x002F0174
		private static bool PawnsCanShareCellBecauseOfBodySize(Pawn p1, Pawn p2)
		{
			float bodySize = p1.BodySize;
			float bodySize2 = p2.BodySize;
			if (bodySize >= 1.5f || bodySize2 >= 1.5f)
			{
				return false;
			}
			float num = bodySize / bodySize2;
			if (num < 1f)
			{
				num = 1f / num;
			}
			return num > 3.57f;
		}

		// Token: 0x0600833A RID: 33594 RVA: 0x002F1FC0 File Offset: 0x002F01C0
		public static bool KnownDangerAt(IntVec3 c, Map map, Pawn forPawn)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.IsDangerousFor(forPawn);
		}

		// Token: 0x0600833B RID: 33595 RVA: 0x002F1FE4 File Offset: 0x002F01E4
		[Obsolete("Lord and job report display validation is now checked separately.")]
		public static bool ShouldDisplayActionInInspectString(Pawn p)
		{
			return (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer) && !p.InMentalState && (!p.IsMutant || !p.mutant.Def.overrideInspectString);
		}

		// Token: 0x0600833C RID: 33596 RVA: 0x002F2034 File Offset: 0x002F0234
		public static bool ShouldDisplayLordReport(Pawn pawn)
		{
			return PawnUtility.ShouldShowCultistLordReport(pawn) || PawnUtility.ShouldShowActionReportToPlayer(pawn);
		}

		// Token: 0x0600833D RID: 33597 RVA: 0x002F2048 File Offset: 0x002F0248
		public static bool ShouldDisplayJobReport(Pawn pawn)
		{
			return (!pawn.IsMutant || !pawn.mutant.Def.overrideInspectString) && ((pawn.CurJobDef != null && pawn.CurJobDef.alwaysShowReport) || (ModsConfig.AnomalyActive && pawn.Faction == Faction.OfHoraxCult && pawn.CurJobDef == JobDefOf.HateChanting) || PawnUtility.ShouldShowCultistLordReport(pawn) || PawnUtility.ShouldShowActionReportToPlayer(pawn));
		}

		// Token: 0x0600833E RID: 33598 RVA: 0x002F20BC File Offset: 0x002F02BC
		private static bool ShouldShowActionReportToPlayer(Pawn p)
		{
			return (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer) && !p.InMentalState && (!p.IsMutant || !p.mutant.Def.overrideInspectString);
		}

		// Token: 0x0600833F RID: 33599 RVA: 0x002F210C File Offset: 0x002F030C
		public static bool ShouldDisplayFactionInInspectString(Pawn p)
		{
			return !p.IsMutant || !p.mutant.Def.overrideInspectString;
		}

		// Token: 0x06008340 RID: 33600 RVA: 0x002F212B File Offset: 0x002F032B
		private static bool ShouldShowCultistLordReport(Pawn pawn)
		{
			if (ModsConfig.AnomalyActive && pawn.Faction == Faction.OfHoraxCult)
			{
				PawnDuty duty = pawn.mindState.duty;
				if (((duty != null) ? duty.def : null) == DutyDefOf.Invoke)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008341 RID: 33601 RVA: 0x002F2164 File Offset: 0x002F0364
		public static bool ShouldSendNotificationAbout(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (p == null)
			{
				return false;
			}
			if (PawnGenerator.IsBeingGenerated(p))
			{
				return false;
			}
			if (p.IsWorldPawn() && (!p.IsCaravanMember() || !p.GetCaravan().IsPlayerControlled) && !PawnUtility.IsTravelingInTransportPodWorldObject(p) && !p.IsBorrowedByAnyFaction() && p.Corpse.DestroyedOrNull())
			{
				return false;
			}
			if (p.IsSubhuman)
			{
				return false;
			}
			if (ModsConfig.AnomalyActive && p.Corpse is UnnaturalCorpse)
			{
				return false;
			}
			if (p.Faction != Faction.OfPlayer)
			{
				if (p.HostFaction != Faction.OfPlayer)
				{
					return false;
				}
				if (p.RaceProps.Humanlike && p.guest.Released && !p.Downed && !p.InBed())
				{
					return false;
				}
				if (p.CurJob != null && p.CurJob.exitMapOnArrival && !PrisonBreakUtility.IsPrisonBreaking(p))
				{
					return false;
				}
				if (PawnUtility.IsExitingMap(p))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008342 RID: 33602 RVA: 0x002F2258 File Offset: 0x002F0458
		public static bool ShouldGetThoughtAbout(Pawn pawn, Pawn subject)
		{
			return !pawn.IsSubhuman && !subject.IsSubhuman && (pawn.Faction == subject.Faction || (!subject.IsWorldPawn() && !pawn.IsWorldPawn()));
		}

		// Token: 0x06008343 RID: 33603 RVA: 0x002F2290 File Offset: 0x002F0490
		public static bool IsTeetotaler(this Pawn pawn)
		{
			return !new HistoryEvent(HistoryEventDefOf.IngestedDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo() || !new HistoryEvent(HistoryEventDefOf.IngestedRecreationalDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo() || (pawn.story != null && pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) < 0);
		}

		// Token: 0x06008344 RID: 33604 RVA: 0x002F22FC File Offset: 0x002F04FC
		public static bool CanTakeDrug(this Pawn pawn, ThingDef drug)
		{
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			if (compProperties == null)
			{
				return false;
			}
			if (PawnUtility.CanTakeDrugForDependency(pawn, drug))
			{
				return true;
			}
			if (!compProperties.teetotalerCanConsume && pawn.IsTeetotaler())
			{
				return false;
			}
			if (ModsConfig.IdeologyActive)
			{
				if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.IngestedDrug, pawn))
				{
					return false;
				}
				if (drug.IsNonMedicalDrug && !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.IngestedRecreationalDrug, pawn))
				{
					return false;
				}
				if (drug.ingestible != null && drug.ingestible.drugCategory == DrugCategory.Hard && !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.IngestedHardDrug, pawn))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008345 RID: 33605 RVA: 0x002F2388 File Offset: 0x002F0588
		public static bool CanTakeDrugForDependency(Pawn pawn, ThingDef drug)
		{
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				return false;
			}
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			if (compProperties == null)
			{
				return false;
			}
			foreach (Gene gene in pawn.genes.GenesListForReading)
			{
				Gene_ChemicalDependency gene_ChemicalDependency = gene as Gene_ChemicalDependency;
				if (gene_ChemicalDependency != null && gene.Active && gene_ChemicalDependency.def.chemical == compProperties.chemical)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008346 RID: 33606 RVA: 0x002F2424 File Offset: 0x002F0624
		public static bool TryGetChemicalDependencyGene(Pawn pawn, out Gene_ChemicalDependency gene)
		{
			gene = null;
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				return false;
			}
			foreach (Gene gene2 in pawn.genes.GenesListForReading)
			{
				Gene_ChemicalDependency gene_ChemicalDependency = gene2 as Gene_ChemicalDependency;
				if (gene_ChemicalDependency != null)
				{
					gene = gene_ChemicalDependency;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008347 RID: 33607 RVA: 0x002F249C File Offset: 0x002F069C
		public static bool PawnWouldBeUnhappyTakingDrug(this Pawn pawn, ThingDef drug)
		{
			return pawn.IsTeetotaler() && drug.GetCompProperties<CompProperties_Drug>() != null;
		}

		// Token: 0x06008348 RID: 33608 RVA: 0x002F24B1 File Offset: 0x002F06B1
		public static bool IsProsthophobe(this Pawn pawn)
		{
			return pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.BodyPurist);
		}

		// Token: 0x06008349 RID: 33609 RVA: 0x002F24D2 File Offset: 0x002F06D2
		public static bool IsPrisonerInPrisonCell(this Pawn pawn)
		{
			return pawn.IsPrisoner && pawn.Spawned && pawn.Position.IsInPrisonCell(pawn.Map);
		}

		// Token: 0x0600834A RID: 33610 RVA: 0x002F24F8 File Offset: 0x002F06F8
		public static bool IsBeingArrested(Pawn pawn)
		{
			if (pawn.Map == null)
			{
				return false;
			}
			foreach (Pawn pawn2 in pawn.Map.mapPawns.AllPawnsSpawned)
			{
				if (pawn2 != pawn && pawn2.CurJobDef == JobDefOf.Arrest && pawn2.CurJob.AnyTargetIs(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600834B RID: 33611 RVA: 0x002F2580 File Offset: 0x002F0780
		public static string PawnKindsToCommaList(IEnumerable<Pawn> pawns, bool useAnd = false)
		{
			PawnUtility.tmpPawns.Clear();
			PawnUtility.tmpPawns.AddRange(pawns);
			if (PawnUtility.tmpPawns.Count >= 2)
			{
				PawnUtility.tmpPawns.SortBy((Pawn x) => !x.RaceProps.Humanlike, (Pawn x) => x.GetKindLabelPlural(-1));
			}
			PawnUtility.tmpAddedPawnKinds.Clear();
			PawnUtility.tmpPawnKindsStr.Clear();
			for (int i = 0; i < PawnUtility.tmpPawns.Count; i++)
			{
				if (!PawnUtility.tmpAddedPawnKinds.Contains(PawnUtility.tmpPawns[i].kindDef))
				{
					PawnUtility.tmpAddedPawnKinds.Add(PawnUtility.tmpPawns[i].kindDef);
					int num = 0;
					for (int j = 0; j < PawnUtility.tmpPawns.Count; j++)
					{
						if (PawnUtility.tmpPawns[j].kindDef == PawnUtility.tmpPawns[i].kindDef)
						{
							num++;
						}
					}
					if (num == 1)
					{
						PawnUtility.tmpPawnKindsStr.Add("1 " + PawnUtility.tmpPawns[i].KindLabel);
					}
					else
					{
						PawnUtility.tmpPawnKindsStr.Add(num.ToString() + " " + PawnUtility.tmpPawns[i].GetKindLabelPlural(num));
					}
				}
			}
			PawnUtility.tmpPawns.Clear();
			return PawnUtility.tmpPawnKindsStr.ToCommaList(useAnd, false);
		}

		// Token: 0x0600834C RID: 33612 RVA: 0x002F270C File Offset: 0x002F090C
		public static List<string> PawnKindsToList(IEnumerable<PawnKindDef> pawnKinds)
		{
			PawnUtility.tmpPawnKinds.Clear();
			PawnUtility.tmpPawnKinds.AddRange(pawnKinds);
			if (PawnUtility.tmpPawnKinds.Count >= 2)
			{
				PawnUtility.tmpPawnKinds.SortBy((PawnKindDef x) => !x.RaceProps.Humanlike, (PawnKindDef x) => GenLabel.BestKindLabel(x, Gender.None, true, -1));
			}
			PawnUtility.tmpAddedPawnKinds.Clear();
			PawnUtility.tmpPawnKindsStr.Clear();
			for (int i = 0; i < PawnUtility.tmpPawnKinds.Count; i++)
			{
				if (!PawnUtility.tmpAddedPawnKinds.Contains(PawnUtility.tmpPawnKinds[i]))
				{
					PawnUtility.tmpAddedPawnKinds.Add(PawnUtility.tmpPawnKinds[i]);
					int num = 0;
					for (int j = 0; j < PawnUtility.tmpPawnKinds.Count; j++)
					{
						if (PawnUtility.tmpPawnKinds[j] == PawnUtility.tmpPawnKinds[i])
						{
							num++;
						}
					}
					if (num == 1)
					{
						PawnUtility.tmpPawnKindsStr.Add("1 " + GenLabel.BestKindLabel(PawnUtility.tmpPawnKinds[i], Gender.None, false, -1));
					}
					else
					{
						PawnUtility.tmpPawnKindsStr.Add(num.ToString() + " " + GenLabel.BestKindLabel(PawnUtility.tmpPawnKinds[i], Gender.None, true, num));
					}
				}
			}
			return PawnUtility.tmpPawnKindsStr;
		}

		// Token: 0x0600834D RID: 33613 RVA: 0x002F2875 File Offset: 0x002F0A75
		public static string PawnKindsToLineList(IEnumerable<PawnKindDef> pawnKinds, string prefix)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			return PawnUtility.tmpPawnKindsStr.ToLineList(prefix);
		}

		// Token: 0x0600834E RID: 33614 RVA: 0x002F288C File Offset: 0x002F0A8C
		public static string PawnKindsToLineList(IEnumerable<PawnKindDef> pawnKinds, string prefix, Color color)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			for (int i = 0; i < PawnUtility.tmpPawnKindsStr.Count; i++)
			{
				PawnUtility.tmpPawnKindsStr[i] = PawnUtility.tmpPawnKindsStr[i].Colorize(color);
			}
			return PawnUtility.tmpPawnKindsStr.ToLineList(prefix);
		}

		// Token: 0x0600834F RID: 33615 RVA: 0x002F28DC File Offset: 0x002F0ADC
		public static string PawnKindsToCommaList(IEnumerable<PawnKindDef> pawnKinds, bool useAnd = false)
		{
			PawnUtility.PawnKindsToList(pawnKinds);
			return PawnUtility.tmpPawnKindsStr.ToCommaList(useAnd, false);
		}

		// Token: 0x06008350 RID: 33616 RVA: 0x002F28F1 File Offset: 0x002F0AF1
		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority)
		{
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.locomotion != LocomotionUrgency.None)
			{
				return pawn.mindState.duty.locomotion;
			}
			return secondPriority;
		}

		// Token: 0x06008351 RID: 33617 RVA: 0x002F292C File Offset: 0x002F0B2C
		public static LocomotionUrgency ResolveLocomotion(Pawn pawn, LocomotionUrgency secondPriority, LocomotionUrgency thirdPriority)
		{
			LocomotionUrgency locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, secondPriority);
			if (locomotionUrgency != LocomotionUrgency.None)
			{
				return locomotionUrgency;
			}
			return thirdPriority;
		}

		// Token: 0x06008352 RID: 33618 RVA: 0x002F2947 File Offset: 0x002F0B47
		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority)
		{
			if (!pawn.Dead && pawn.mindState.duty != null && pawn.mindState.duty.maxDanger != Danger.Unspecified)
			{
				return pawn.mindState.duty.maxDanger;
			}
			return secondPriority;
		}

		// Token: 0x06008353 RID: 33619 RVA: 0x002F2984 File Offset: 0x002F0B84
		public static Danger ResolveMaxDanger(Pawn pawn, Danger secondPriority, Danger thirdPriority)
		{
			Danger danger = PawnUtility.ResolveMaxDanger(pawn, secondPriority);
			if (danger != Danger.Unspecified)
			{
				return danger;
			}
			return thirdPriority;
		}

		// Token: 0x06008354 RID: 33620 RVA: 0x002F299F File Offset: 0x002F0B9F
		public static bool IsPermanentCombatant(this Pawn pawn)
		{
			return ((pawn != null) ? pawn.mindState : null) == null || ((!pawn.IsAnimal || pawn.Faction == null) && !pawn.DevelopmentalStage.Juvenile());
		}

		// Token: 0x06008355 RID: 33621 RVA: 0x002F29D3 File Offset: 0x002F0BD3
		public static bool IsCombatant(this Pawn pawn)
		{
			return pawn.IsPermanentCombatant() || pawn.mindState.CombatantRecently;
		}

		// Token: 0x06008356 RID: 33622 RVA: 0x002F29EC File Offset: 0x002F0BEC
		public static bool IsFighting(this Pawn pawn)
		{
			return pawn.CurJob != null && (pawn.CurJob.def == JobDefOf.AttackMelee || pawn.CurJob.def == JobDefOf.AttackStatic || pawn.CurJob.def == JobDefOf.Wait_Combat || pawn.CurJob.def == JobDefOf.PredatorHunt || pawn.CurJob.def == JobDefOf.ManTurret);
		}

		// Token: 0x06008357 RID: 33623 RVA: 0x002F2A60 File Offset: 0x002F0C60
		public static bool IsAttacking(this Pawn pawn)
		{
			if (pawn.CurJobDef == JobDefOf.AttackMelee || pawn.CurJobDef == JobDefOf.AttackStatic)
			{
				return true;
			}
			if (pawn.CurJobDef == JobDefOf.Wait_Combat)
			{
				Stance_Busy stance_Busy = pawn.stances.curStance as Stance_Busy;
				if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008358 RID: 33624 RVA: 0x002F2ABA File Offset: 0x002F0CBA
		public static Hediff_Psylink GetMainPsylinkSource(this Pawn pawn)
		{
			return (Hediff_Psylink)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
		}

		// Token: 0x06008359 RID: 33625 RVA: 0x002F2AD8 File Offset: 0x002F0CD8
		public static int GetPsylinkLevel(this Pawn pawn)
		{
			int num = 0;
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				Hediff_Psylink hediff_Psylink = hediff as Hediff_Psylink;
				if (hediff_Psylink != null)
				{
					num += hediff_Psylink.level;
				}
			}
			return num;
		}

		// Token: 0x0600835A RID: 33626 RVA: 0x002F2B44 File Offset: 0x002F0D44
		public static int GetMaxPsylinkLevel(this Pawn pawn)
		{
			return (int)HediffDefOf.PsychicAmplifier.maxSeverity;
		}

		// Token: 0x0600835B RID: 33627 RVA: 0x002F2B54 File Offset: 0x002F0D54
		public static RoyalTitle GetMaxPsylinkLevelTitle(this Pawn pawn)
		{
			if (pawn.royalty == null)
			{
				return null;
			}
			int num = 0;
			RoyalTitle result = null;
			foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
			{
				if (num < royalTitle.def.maxPsylinkLevel)
				{
					num = royalTitle.def.maxPsylinkLevel;
					result = royalTitle;
				}
			}
			return result;
		}

		// Token: 0x0600835C RID: 33628 RVA: 0x002F2BD0 File Offset: 0x002F0DD0
		public static int GetMaxPsylinkLevelByTitle(this Pawn pawn)
		{
			RoyalTitle maxPsylinkLevelTitle = pawn.GetMaxPsylinkLevelTitle();
			if (maxPsylinkLevelTitle == null)
			{
				return 0;
			}
			return maxPsylinkLevelTitle.def.maxPsylinkLevel;
		}

		// Token: 0x0600835D RID: 33629 RVA: 0x002F2BF4 File Offset: 0x002F0DF4
		public static void ChangePsylinkLevel(this Pawn pawn, int levelOffset, bool sendLetter = true)
		{
			Hediff_Psylink hediff_Psylink = pawn.GetMainPsylinkSource();
			if (hediff_Psylink == null)
			{
				hediff_Psylink = (Hediff_Psylink)HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, pawn, null);
				try
				{
					hediff_Psylink.suppressPostAddLetter = !sendLetter;
					pawn.health.AddHediff(hediff_Psylink, pawn.health.hediffSet.GetBrain(), null, null);
					return;
				}
				finally
				{
					hediff_Psylink.suppressPostAddLetter = false;
				}
			}
			hediff_Psylink.ChangeLevel(levelOffset, sendLetter);
		}

		// Token: 0x0600835E RID: 33630 RVA: 0x002F2C70 File Offset: 0x002F0E70
		public static void GiveAllStartingPlayerPawnsThought(ThoughtDef thought)
		{
			foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
			{
				if (pawn.needs.mood != null)
				{
					if (thought.IsSocial)
					{
						using (List<Pawn>.Enumerator enumerator2 = Find.GameInitData.startingAndOptionalPawns.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Pawn pawn2 = enumerator2.Current;
								if (pawn2 != pawn)
								{
									pawn.needs.mood.thoughts.memories.TryGainMemory(thought, pawn2, null);
								}
							}
							continue;
						}
					}
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought, null, null);
				}
			}
		}

		// Token: 0x0600835F RID: 33631 RVA: 0x002F2D58 File Offset: 0x002F0F58
		public static IntVec3 DutyLocation(this Pawn pawn)
		{
			Pawn_RopeTracker roping = pawn.roping;
			if (roping != null && roping.IsRopedToSpot)
			{
				return pawn.roping.RopedToSpot;
			}
			if (pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid)
			{
				return pawn.mindState.duty.focus.Cell;
			}
			return pawn.Position;
		}

		// Token: 0x06008360 RID: 33632 RVA: 0x002F2DC5 File Offset: 0x002F0FC5
		public static bool EverBeenColonistOrTameAnimal(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsColonistOrColonyAnimal) > 0;
		}

		// Token: 0x06008361 RID: 33633 RVA: 0x002F2DDA File Offset: 0x002F0FDA
		public static bool EverBeenPrisoner(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsPrisoner) > 0;
		}

		// Token: 0x06008362 RID: 33634 RVA: 0x002F2DEF File Offset: 0x002F0FEF
		public static bool EverBeenQuestLodger(Pawn pawn)
		{
			return pawn.records.GetAsInt(RecordDefOf.TimeAsQuestLodger) > 0;
		}

		// Token: 0x06008363 RID: 33635 RVA: 0x002F2E04 File Offset: 0x002F1004
		public static void RecoverFromUnwalkablePositionOrKill(IntVec3 c, Map map)
		{
			if (!c.InBounds(map) || c.Walkable(map))
			{
				return;
			}
			PawnUtility.tmpThings.Clear();
			PawnUtility.tmpThings.AddRange(c.GetThingList(map));
			for (int i = 0; i < PawnUtility.tmpThings.Count; i++)
			{
				Pawn pawn = PawnUtility.tmpThings[i] as Pawn;
				if (pawn != null)
				{
					IntVec3 position;
					if (CellFinder.TryFindBestPawnStandCell(pawn, out position, false))
					{
						pawn.Position = position;
						pawn.Notify_Teleported(true, false);
					}
					else
					{
						DamageInfo damageInfo = new DamageInfo(DamageDefOf.Crush, 99999f, 999f, -1f, null, pawn.health.hediffSet.GetBrain(), null, DamageInfo.SourceCategory.Collapse, null, true, true, QualityCategory.Normal, true, false);
						damageInfo.SetIgnoreInstantKillProtection(true);
						pawn.TakeDamage(damageInfo);
						if (!pawn.Dead)
						{
							pawn.Kill(new DamageInfo?(damageInfo), null);
						}
					}
				}
			}
		}

		// Token: 0x06008364 RID: 33636 RVA: 0x002F2EE4 File Offset: 0x002F10E4
		public static float GetManhunterOnDamageChance(Pawn pawn, Thing instigator = null, float distance = -1f)
		{
			float num = PawnUtility.GetManhunterOnDamageChance(pawn.def);
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				num += 0.5f;
			}
			if (instigator != null)
			{
				num *= GenMath.LerpDoubleClamped(1f, 30f, 3f, 1f, distance);
				num *= 1f - instigator.GetStatValue(StatDefOf.HuntingStealth, true, -1);
				Pawn pawn2 = instigator as Pawn;
				if (pawn2 != null)
				{
					num *= PawnUtility.GetManhunterChanceFactorForInstigator(pawn2);
				}
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x06008365 RID: 33637 RVA: 0x002F2F6C File Offset: 0x002F116C
		public static float GetManhunterOnDamageChance(ThingDef def)
		{
			return def.race.manhunterOnDamageChance * Find.Storyteller.difficulty.manhunterChanceOnDamageFactor;
		}

		// Token: 0x06008366 RID: 33638 RVA: 0x002F2F8C File Offset: 0x002F118C
		public static string GetManhunterOnDamageChanceExplanation(ThingDef def, Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("HarmedRevengeChanceExplanation".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(def.LabelCap + ": " + def.race.manhunterOnDamageChance.ToStringPercent());
			stringBuilder.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.manhunterChanceOnDamageFactor.ToStringPercent());
			if (pawn != null && pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				stringBuilder.AppendLine(HediffDefOf.Scaria.LabelCap + ": " + 0.5f.ToStringPercentSigned());
			}
			stringBuilder.AppendLine();
			float f = (pawn == null) ? PawnUtility.GetManhunterOnDamageChance(def) : PawnUtility.GetManhunterOnDamageChance(pawn, null, -1f);
			stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + f.ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x06008367 RID: 33639 RVA: 0x002F30D4 File Offset: 0x002F12D4
		public static float GetManhunterChanceFactorForInstigator(Pawn instigator)
		{
			if (ModsConfig.AnomalyActive && ((instigator != null) ? instigator.Faction : null) == Faction.OfEntities)
			{
				return 0f;
			}
			float num = 1f;
			if (ModsConfig.IdeologyActive && ((instigator != null) ? instigator.Ideo : null) != null)
			{
				Precept_Role role = instigator.Ideo.GetRole(instigator);
				RoleEffect roleEffect;
				if (role == null)
				{
					roleEffect = null;
				}
				else
				{
					List<RoleEffect> roleEffects = role.def.roleEffects;
					if (roleEffects == null)
					{
						roleEffect = null;
					}
					else
					{
						roleEffect = roleEffects.FirstOrDefault((RoleEffect eff) => eff is RoleEffect_HuntingRevengeChanceFactor);
					}
				}
				RoleEffect roleEffect2 = roleEffect;
				if (roleEffect2 != null)
				{
					num *= ((RoleEffect_HuntingRevengeChanceFactor)roleEffect2).factor;
				}
			}
			return num;
		}

		// Token: 0x06008368 RID: 33640 RVA: 0x002F3177 File Offset: 0x002F1377
		public static float GetManhunterOnTameFailChance(Pawn pawn)
		{
			return Mathf.Clamp01(PawnUtility.GetManhunterOnTameFailChance(pawn.def));
		}

		// Token: 0x06008369 RID: 33641 RVA: 0x002F3189 File Offset: 0x002F1389
		public static float GetManhunterOnTameFailChance(ThingDef def)
		{
			return def.race.manhunterOnTameFailChance;
		}

		// Token: 0x0600836A RID: 33642 RVA: 0x002F3198 File Offset: 0x002F1398
		public static string GetManhunterOnTameFailChanceExplanation(ThingDef def, Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Stat_Race_Animal_TameFailedRevengeChance_Desc".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(def.LabelCap + ": " + def.race.manhunterOnTameFailChance.ToStringPercent());
			stringBuilder.AppendLine();
			float f = (pawn == null) ? PawnUtility.GetManhunterOnTameFailChance(def) : PawnUtility.GetManhunterOnTameFailChance(pawn);
			stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + f.ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x0600836B RID: 33643 RVA: 0x002F3241 File Offset: 0x002F1441
		public static MentalStateDef ManhunterStateFor(Pawn pawn)
		{
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				return MentalStateDefOf.ManhunterPermanent;
			}
			return MentalStateDefOf.Manhunter;
		}

		// Token: 0x0600836C RID: 33644 RVA: 0x002F3268 File Offset: 0x002F1468
		public static bool PlayerHasReproductivePair(PawnKindDef pawnKindDef)
		{
			if (!pawnKindDef.RaceProps.Animal)
			{
				return false;
			}
			List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction.Count; i++)
			{
				Pawn pawn = allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction[i];
				if (pawn.kindDef == pawnKindDef && pawn.ageTracker.CurLifeStage.reproductive)
				{
					if (pawn.gender == Gender.Male)
					{
						flag = true;
					}
					else if (pawn.gender == Gender.Female)
					{
						flag2 = true;
					}
					if (flag && flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600836D RID: 33645 RVA: 0x002F32E8 File Offset: 0x002F14E8
		public static float PlayerAnimalBodySizePerCapita()
		{
			float num = 0f;
			int num2 = 0;
			List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction;
			for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction.Count; i++)
			{
				Pawn pawn = allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction[i];
				if (pawn.IsFreeColonist && !pawn.IsQuestLodger())
				{
					num2++;
				}
				if (pawn.IsAnimal)
				{
					num += pawn.BodySize;
				}
			}
			if (num2 <= 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x0600836E RID: 33646 RVA: 0x002F3354 File Offset: 0x002F1554
		private static List<Pawn> PawnsOfFactionOnMapOrInCaravan(Pawn pawn)
		{
			List<Pawn> result;
			if (pawn.Spawned)
			{
				result = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			}
			else
			{
				Caravan caravan = pawn.GetCaravan();
				if (caravan == null)
				{
					return null;
				}
				result = caravan.PawnsListForReading;
			}
			return result;
		}

		// Token: 0x0600836F RID: 33647 RVA: 0x002F3398 File Offset: 0x002F1598
		public static float PlayerVeneratedAnimalBodySizePerCapitaOnMapOrCaravan(Pawn pawn)
		{
			if (pawn.Ideo == null || pawn.Faction == null)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = 0;
			List<Pawn> list = PawnUtility.PawnsOfFactionOnMapOrInCaravan(pawn);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == pawn.Faction && !pawn.IsQuestLodger())
				{
					if (list[i].IsAnimal && pawn.Ideo.IsVeneratedAnimal(list[i]))
					{
						num += list[i].BodySize;
					}
					else if (list[i].RaceProps.Humanlike)
					{
						num2++;
					}
				}
			}
			return Mathf.Round(((num2 > 0) ? (num / (float)num2) : 0f) * 100f) / 100f;
		}

		// Token: 0x06008370 RID: 33648 RVA: 0x002F3464 File Offset: 0x002F1664
		public static Pawn FirstVeneratedAnimalOnMapOrCaravan(Pawn pawn)
		{
			if (pawn.Ideo == null || pawn.Faction == null)
			{
				return null;
			}
			List<Pawn> list = PawnUtility.PawnsOfFactionOnMapOrInCaravan(pawn);
			for (int i = 0; i < list.Count; i++)
			{
				if (pawn.Faction == list[i].Faction && pawn.Ideo.IsVeneratedAnimal(list[i]))
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06008371 RID: 33649 RVA: 0x002F34CC File Offset: 0x002F16CC
		public static bool HasClothingNotRequiredByKind(Pawn p)
		{
			if (p.apparel == null)
			{
				return false;
			}
			List<Apparel> wornApparel = p.apparel.WornApparel;
			if (wornApparel.Count > 0 && p.kindDef.apparelRequired.NullOrEmpty<ThingDef>())
			{
				return true;
			}
			for (int i = 0; i < wornApparel.Count; i++)
			{
				Apparel apparel = wornApparel[i];
				if (apparel.def.apparel.countsAsClothingForNudity && !p.kindDef.apparelRequired.Contains(apparel.def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008372 RID: 33650 RVA: 0x002F3552 File Offset: 0x002F1752
		public static IEnumerable<PawnKindDef> GetCombatPawnKindsForPoints(Func<PawnKindDef, bool> selector, float points, Func<PawnKindDef, float> selectionWeight = null)
		{
			PawnUtility.<GetCombatPawnKindsForPoints>d__92 <GetCombatPawnKindsForPoints>d__ = new PawnUtility.<GetCombatPawnKindsForPoints>d__92(-2);
			<GetCombatPawnKindsForPoints>d__.<>3__selector = selector;
			<GetCombatPawnKindsForPoints>d__.<>3__points = points;
			<GetCombatPawnKindsForPoints>d__.<>3__selectionWeight = selectionWeight;
			return <GetCombatPawnKindsForPoints>d__;
		}

		// Token: 0x06008373 RID: 33651 RVA: 0x002F3570 File Offset: 0x002F1770
		public static int GetMaxAllowedToPickUp(Pawn pawn, ThingDef thingDef)
		{
			int maxAllowedToPickUp = PawnUtility.GetMaxAllowedToPickUp(thingDef, pawn.Map);
			if (maxAllowedToPickUp <= 0)
			{
				return 0;
			}
			int num = pawn.inventory.Count((Thing t) => t.def.orderedTakeGroup == thingDef.orderedTakeGroup);
			return Math.Max(maxAllowedToPickUp - num, 0);
		}

		// Token: 0x06008374 RID: 33652 RVA: 0x002F35C3 File Offset: 0x002F17C3
		public static int GetMaxAllowedToPickUp(ThingDef thingDef, Map map = null)
		{
			if (map != null && !map.IsPlayerHome)
			{
				return int.MaxValue;
			}
			if (thingDef.orderedTakeGroup == null)
			{
				return 0;
			}
			return thingDef.orderedTakeGroup.max;
		}

		// Token: 0x06008375 RID: 33653 RVA: 0x002F35EB File Offset: 0x002F17EB
		public static bool CanPickUp(Pawn pawn, ThingDef thingDef)
		{
			return !pawn.Map.IsPlayerHome || (pawn.inventory != null && thingDef.orderedTakeGroup != null && thingDef.orderedTakeGroup.max > 0);
		}

		// Token: 0x06008376 RID: 33654 RVA: 0x002F361C File Offset: 0x002F181C
		public static bool ShouldBeSlaughtered(this Pawn pawn)
		{
			return pawn.Spawned && pawn.IsAnimal && (pawn.Map.designationManager.DesignationOn(pawn, DesignationDefOf.Slaughter) != null || pawn.Map.autoSlaughterManager.AnimalsToSlaughter.Contains(pawn)) && pawn.Map.designationManager.DesignationOn(pawn, DesignationDefOf.ReleaseAnimalToWild) == null;
		}

		// Token: 0x06008377 RID: 33655 RVA: 0x002F3688 File Offset: 0x002F1888
		public static bool CanBeBuried(this Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse == null || corpse.MapHeld == null || corpse.MapHeld.designationManager.DesignationOn(corpse, DesignationDefOf.ExtractSkull) == null;
		}

		// Token: 0x06008378 RID: 33656 RVA: 0x002F36C4 File Offset: 0x002F18C4
		public static bool PawnHadFuneral(Pawn pawn)
		{
			Pawn_IdeoTracker ideo = pawn.ideo;
			object obj;
			if (ideo == null)
			{
				obj = null;
			}
			else
			{
				Ideo ideo2 = ideo.Ideo;
				obj = ((ideo2 != null) ? ideo2.GetPrecept(PreceptDefOf.Funeral) : null);
			}
			Precept_Ritual precept_Ritual = (Precept_Ritual)obj;
			return precept_Ritual != null && !precept_Ritual.completedObligations.NullOrEmpty<RitualObligation>() && precept_Ritual.completedObligations.Any((RitualObligation o) => o.FirstValidTarget.Thing == pawn);
		}

		// Token: 0x06008379 RID: 33657 RVA: 0x002F3735 File Offset: 0x002F1935
		public static bool IsBiologicallyOrArtificiallyBlind(Pawn pawn)
		{
			return PawnUtility.IsBiologicallyBlind(pawn) || PawnUtility.IsArtificiallyBlind(pawn);
		}

		// Token: 0x0600837A RID: 33658 RVA: 0x002F3747 File Offset: 0x002F1947
		public static bool IsBiologicallyBlind(Pawn pawn)
		{
			return !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}

		// Token: 0x0600837B RID: 33659 RVA: 0x002F3764 File Offset: 0x002F1964
		public static bool IsArtificiallyBlind(Pawn p)
		{
			if (PawnUtility.IsBiologicallyBlind(p))
			{
				return false;
			}
			if (p.apparel != null)
			{
				using (List<Apparel>.Enumerator enumerator = p.apparel.WornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.apparel.blocksVision)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600837C RID: 33660 RVA: 0x002F37E0 File Offset: 0x002F19E0
		public static bool IsWorkTypeDisabledByAge(this Pawn pawn, WorkTypeDef workType, out int minAgeRequired)
		{
			for (int i = 0; i < pawn.RaceProps.lifeStageWorkSettings.Count; i++)
			{
				LifeStageWorkSettings lifeStageWorkSettings = pawn.RaceProps.lifeStageWorkSettings[i];
				if (lifeStageWorkSettings.workType == workType && lifeStageWorkSettings.IsDisabled(pawn))
				{
					minAgeRequired = lifeStageWorkSettings.minAge;
					return true;
				}
			}
			minAgeRequired = 0;
			return false;
		}

		// Token: 0x0600837D RID: 33661 RVA: 0x002F383C File Offset: 0x002F1A3C
		public static bool DutyActiveWhenDown(this Pawn pawn, bool onlyInBed = false)
		{
			if (onlyInBed && !pawn.InBed())
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			bool? flag;
			if (lord == null)
			{
				flag = null;
			}
			else
			{
				LordJob lordJob = lord.LordJob;
				flag = ((lordJob != null) ? new bool?(lordJob.DutyActiveWhenDown(pawn)) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x0600837E RID: 33662 RVA: 0x002F3892 File Offset: 0x002F1A92
		public static bool IsPotentiallyPredator(this Pawn pawn)
		{
			return pawn.RaceProps.predator || (pawn.IsWildMan() && !pawn.IsPrisoner && !pawn.WorkTypeIsDisabled(WorkTypeDefOf.Hunting));
		}

		// Token: 0x0600837F RID: 33663 RVA: 0x002F38C4 File Offset: 0x002F1AC4
		public static bool IsExitingMap(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && (lord.LordJob is LordJob_ExitMapBest || lord.LordJob is LordJob_ExitMapNear || lord.LordJob is LordJob_ExitOnShuttle || lord.LordJob is LordJob_TravelAndExit);
		}

		// Token: 0x06008380 RID: 33664 RVA: 0x002F3914 File Offset: 0x002F1B14
		public static void ForceEjectFromContainer(Pawn pawn)
		{
			Pawn_CarryTracker pawn_CarryTracker = pawn.ParentHolder as Pawn_CarryTracker;
			if (pawn_CarryTracker != null)
			{
				Thing thing;
				pawn_CarryTracker.TryDropCarriedThing(pawn_CarryTracker.pawn.Position, ThingPlaceMode.Near, out thing, null);
				pawn_CarryTracker.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			Building_Enterable building_Enterable = pawn.ParentHolder as Building_Enterable;
			if (building_Enterable != null)
			{
				Thing thing;
				building_Enterable.innerContainer.TryDrop(pawn, building_Enterable.InteractionCell, building_Enterable.Map, ThingPlaceMode.Near, out thing, null, null, true);
			}
			Building_Casket building_Casket = pawn.ParentHolder as Building_Casket;
			if (building_Casket != null)
			{
				building_Casket.EjectContents();
			}
			Building_HoldingPlatform building_HoldingPlatform = pawn.ParentHolder as Building_HoldingPlatform;
			if (building_HoldingPlatform != null)
			{
				building_HoldingPlatform.EjectContents();
			}
			CompBiosculpterPod compBiosculpterPod = pawn.ParentHolder as CompBiosculpterPod;
			if (compBiosculpterPod != null)
			{
				compBiosculpterPod.EjectContents(true, true, null);
			}
		}

		// Token: 0x04005134 RID: 20788
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04005135 RID: 20789
		private static List<string> tmpPawnKindsStr = new List<string>();

		// Token: 0x04005136 RID: 20790
		private static HashSet<PawnKindDef> tmpAddedPawnKinds = new HashSet<PawnKindDef>();

		// Token: 0x04005137 RID: 20791
		private static List<PawnKindDef> tmpPawnKinds = new List<PawnKindDef>();

		// Token: 0x04005138 RID: 20792
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
