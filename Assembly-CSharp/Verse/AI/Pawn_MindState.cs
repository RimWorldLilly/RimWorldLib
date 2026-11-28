using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020008B2 RID: 2226
	public class Pawn_MindState : IExposable
	{
		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06004518 RID: 17688 RVA: 0x001B300D File Offset: 0x001B120D
		public bool InRoamingCooldown
		{
			get
			{
				return this.lastStartRoamCooldownTick != null && this.lastStartRoamCooldownTick.Value + 30000 > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004519 RID: 17689 RVA: 0x001B303B File Offset: 0x001B123B
		public bool AvailableForGoodwillReward
		{
			get
			{
				return Find.TickManager.TicksGame >= this.noAidRelationsGainUntilTick;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x0600451A RID: 17690 RVA: 0x001B3052 File Offset: 0x001B1252
		// (set) Token: 0x0600451B RID: 17691 RVA: 0x001B305A File Offset: 0x001B125A
		public bool Active
		{
			get
			{
				return this.activeInt;
			}
			set
			{
				if (value != this.activeInt)
				{
					this.activeInt = value;
					if (this.pawn.Spawned)
					{
						this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
					}
				}
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x0600451C RID: 17692 RVA: 0x001B3094 File Offset: 0x001B1294
		public bool IsIdle
		{
			get
			{
				return !this.pawn.Downed && this.pawn.Spawned && this.lastJobTag == JobTag.Idle;
			}
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x0600451D RID: 17693 RVA: 0x001B30C0 File Offset: 0x001B12C0
		public bool MeleeThreatStillThreat
		{
			get
			{
				return this.meleeThreat != null && this.meleeThreat.Spawned && !this.meleeThreat.ThreatDisabled(this.pawn) && this.meleeThreat.Awake() && this.pawn.Spawned && Find.TickManager.TicksGame <= this.lastMeleeThreatHarmTick + 400 && (float)(this.pawn.Position - this.meleeThreat.Position).LengthHorizontalSquared <= 9f && GenSight.LineOfSight(this.pawn.Position, this.meleeThreat.Position, this.pawn.Map);
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x0600451E RID: 17694 RVA: 0x001B3183 File Offset: 0x001B1383
		// (set) Token: 0x0600451F RID: 17695 RVA: 0x001B318B File Offset: 0x001B138B
		public bool WildManEverReachedOutside
		{
			get
			{
				return this.wildManEverReachedOutsideInt;
			}
			set
			{
				if (this.wildManEverReachedOutsideInt == value)
				{
					return;
				}
				this.wildManEverReachedOutsideInt = value;
				ReachabilityUtility.ClearCacheFor(this.pawn);
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06004520 RID: 17696 RVA: 0x001B31A9 File Offset: 0x001B13A9
		// (set) Token: 0x06004521 RID: 17697 RVA: 0x001B31B1 File Offset: 0x001B13B1
		public bool WillJoinColonyIfRescued
		{
			get
			{
				return this.willJoinColonyIfRescuedInt;
			}
			set
			{
				if (this.willJoinColonyIfRescuedInt == value)
				{
					return;
				}
				this.willJoinColonyIfRescuedInt = value;
				if (this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06004522 RID: 17698 RVA: 0x001B31EC File Offset: 0x001B13EC
		public bool AnythingPreventsJoiningColonyIfRescued
		{
			get
			{
				return this.pawn.Faction == Faction.OfPlayer || (this.pawn.IsPrisoner && !this.pawn.HostFaction.HostileTo(Faction.OfPlayer)) || (!this.pawn.IsPrisoner && this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer) && !this.pawn.Downed);
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06004523 RID: 17699 RVA: 0x001B3274 File Offset: 0x001B1474
		public bool CombatantRecently
		{
			get
			{
				return this.lastCombatantTick >= 0 && Find.TickManager.TicksGame <= this.lastCombatantTick + 3600;
			}
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x001B329C File Offset: 0x001B149C
		public bool WasRecentlyCombatantTicks(int ticksDuration)
		{
			return this.lastCombatantTick >= 0 && Find.TickManager.TicksGame <= this.lastCombatantTick + ticksDuration;
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x001B32C0 File Offset: 0x001B14C0
		public Pawn_MindState()
		{
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x001B344C File Offset: 0x001B164C
		public Pawn_MindState(Pawn pawn)
		{
			this.pawn = pawn;
			this.mentalStateHandler = new MentalStateHandler(pawn);
			this.mentalBreaker = new MentalBreaker(pawn);
			this.mentalFitGenerator = new MentalFitGenerator(pawn);
			this.inspirationHandler = new InspirationHandler(pawn);
			this.priorityWork = new PriorityWork(pawn);
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x001B361B File Offset: 0x001B181B
		public void Reset(bool clearInspiration = false, bool clearMentalState = true)
		{
			this.Reset(clearInspiration, clearMentalState, false);
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x001B3628 File Offset: 0x001B1828
		public void Reset(bool clearInspiration = false, bool clearMentalState = true, bool wasDowned = false)
		{
			if (clearMentalState)
			{
				this.mentalStateHandler.Reset();
				this.mentalBreaker.Reset();
				this.mentalFitGenerator.Reset();
			}
			if (clearInspiration)
			{
				this.inspirationHandler.Reset();
			}
			this.activeInt = true;
			this.lastJobTag = JobTag.Misc;
			this.lastIngestTick = -99999;
			this.nextApparelOptimizeTick = -99999;
			this.canFleeIndividual = true;
			this.exitMapAfterTick = -99999;
			this.lastDisturbanceTick = -99999;
			this.forcedGotoPosition = IntVec3.Invalid;
			this.knownExploder = null;
			this.wantsToTradeWithColony = false;
			this.traderDismissed = false;
			this.hasQuest = false;
			this.lastMannedThing = null;
			this.canLovinTick = -99999;
			this.canSleepTick = -99999;
			this.meleeThreat = null;
			this.lastMeleeThreatHarmTick = -99999;
			this.lastEngageTargetTick = -99999;
			this.lastAttackTargetTick = -99999;
			this.lastAttackedTarget = LocalTargetInfo.Invalid;
			this.enemyTarget = null;
			this.breachingTarget = null;
			this.duty = null;
			this.thinkData.Clear();
			this.lastAssignedInteractTime = -99999;
			this.interactionsToday = 0;
			this.lastInventoryRawFoodUseTick = 0;
			this.priorityWork.Clear();
			this.nextMoveOrderIsWait = true;
			this.lastTakeCombatEnhancingDrugTick = -99999;
			this.lastHarmTick = -99999;
			this.lastRangedHarmTick = -99999;
			this.anyCloseHostilesRecently = false;
			this.WillJoinColonyIfRescued = false;
			this.WildManEverReachedOutside = false;
			this.lastSelfTendTick = -99999;
			this.spawnedByInfestationThingComp = false;
			this.lastPredatorHuntingPlayerNotificationTick = -99999;
			this.lastSlaveSuppressedTick = -99999;
			this.lastStartRoamCooldownTick = null;
			this.nextInventoryStockTick = -99999;
			this.returnToHealingPod = false;
			this.resurrectTarget = null;
			this.lastRotStinkTick = -99999;
			this.lastSwamTick = -99999;
			if (!wasDowned)
			{
				this.timesGuestTendedToByPlayer = 0;
			}
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x001B380D File Offset: 0x001B1A0D
		public void Notify_PawnRedressed()
		{
			this.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame - 60000;
			this.SetupLastHumanMeatTick();
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x001B382C File Offset: 0x001B1A2C
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.meleeThreat, "meleeThreat", false);
			Scribe_References.Look<Thing>(ref this.enemyTarget, "enemyTarget", false);
			Scribe_References.Look<Thing>(ref this.knownExploder, "knownExploder", false);
			Scribe_References.Look<Thing>(ref this.lastMannedThing, "lastMannedThing", false);
			Scribe_References.Look<Thing>(ref this.droppedWeapon, "droppedWeapon", false);
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Collections.Look<int, int>(ref this.thinkData, "thinkData", LookMode.Value, LookMode.Value);
			Scribe_Values.Look<bool>(ref this.activeInt, "active", true, false);
			Scribe_Values.Look<JobTag>(ref this.lastJobTag, "lastJobTag", JobTag.Misc, false);
			Scribe_Values.Look<int>(ref this.lastIngestTick, "lastIngestTick", -99999, false);
			Scribe_Values.Look<int>(ref this.nextApparelOptimizeTick, "nextApparelOptimizeTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastEngageTargetTick, "lastEngageTargetTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastAttackTargetTick, "lastAttackTargetTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canFleeIndividual, "canFleeIndividual", false, false);
			Scribe_Values.Look<int>(ref this.exitMapAfterTick, "exitMapAfterTick", -99999, false);
			Scribe_Values.Look<IntVec3>(ref this.forcedGotoPosition, "forcedGotoPosition", IntVec3.Invalid, false);
			Scribe_Values.Look<int>(ref this.lastMeleeThreatHarmTick, "lastMeleeThreatHarmTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastAssignedInteractTime, "lastAssignedInteractTime", -99999, false);
			Scribe_Values.Look<int>(ref this.interactionsToday, "interactionsToday", 0, false);
			Scribe_Values.Look<int>(ref this.lastInventoryRawFoodUseTick, "lastInventoryRawFoodUseTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastDisturbanceTick, "lastDisturbanceTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.wantsToTradeWithColony, "wantsToTradeWithColony", false, false);
			Scribe_Values.Look<bool>(ref this.hasQuest, "hasQuest", false, false);
			Scribe_Values.Look<int>(ref this.canLovinTick, "canLovinTick", -99999, false);
			Scribe_Values.Look<int>(ref this.canSleepTick, "canSleepTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.nextMoveOrderIsWait, "nextMoveOrderIsWait", true, false);
			Scribe_Values.Look<int>(ref this.lastTakeCombatEnhancingDrugTick, "lastTakeCombatEnhancingDrugTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastTakeRecreationalDrugTick, "lastTakeRecreationalDrugTick", -60000, false);
			Scribe_Values.Look<int>(ref this.lastHarmTick, "lastHarmTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.anyCloseHostilesRecently, "anyCloseHostilesRecently", false, false);
			Scribe_Deep.Look<PawnDuty>(ref this.duty, "duty", Array.Empty<object>());
			Scribe_Deep.Look<MentalStateHandler>(ref this.mentalStateHandler, "mentalStateHandler", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<MentalBreaker>(ref this.mentalBreaker, "mentalBreaker", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<MentalFitGenerator>(ref this.mentalFitGenerator, "mentalFitGenerator", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<InspirationHandler>(ref this.inspirationHandler, "inspirationHandler", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<PriorityWork>(ref this.priorityWork, "priorityWork", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.applyBedThoughtsTick, "applyBedThoughtsTick", 0, false);
			Scribe_Values.Look<int>(ref this.applyThroneThoughtsTick, "applyThroneThoughtsTick", 0, false);
			Scribe_Values.Look<bool>(ref this.applyBedThoughtsOnLeave, "applyBedThoughtsOnLeave", false, false);
			Scribe_Values.Look<bool>(ref this.willJoinColonyIfRescuedInt, "willJoinColonyIfRescued", false, false);
			Scribe_Values.Look<bool>(ref this.wildManEverReachedOutsideInt, "wildManEverReachedOutside", false, false);
			Scribe_Values.Look<int>(ref this.timesGuestTendedToByPlayer, "timesGuestTendedToByPlayer", 0, false);
			Scribe_Values.Look<int>(ref this.noAidRelationsGainUntilTick, "noAidRelationsGainUntilTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastSelfTendTick, "lastSelfTendTick", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnedByInfestationThingComp, "spawnedByInfestationThingComp", false, false);
			Scribe_Values.Look<int>(ref this.lastPredatorHuntingPlayerNotificationTick, "lastPredatorHuntingPlayerNotificationTick", -99999, false);
			Scribe_Deep.Look<BreachingTargetData>(ref this.breachingTarget, "breachingTarget", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastSlaveSuppressedTick, "lastSlaveSuppressedTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastHumanMeatIngestedTick, "lastHumanMeatIngestedTick", -99999, false);
			Scribe_Values.Look<int?>(ref this.lastStartRoamCooldownTick, "lastStartRoamCooldownTick", null, false);
			Scribe_Values.Look<int>(ref this.nextInventoryStockTick, "nextInventoryStockTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.returnToHealingPod, "returnToHealingPod", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.lastBedDefSleptIn, "lastBedDefSleptIn");
			Scribe_Collections.Look<Pawn, AutofeedMode>(ref this.autoFeeders, "babyAutoBreastfeedMoms", LookMode.Reference, LookMode.Value, ref this.tmpLoadPawns, ref this.tmpLoadAutofeedMode, false, false, false);
			Scribe_Collections.Look<Pawn, float>(ref this.babyCaravanBreastfeed, "babyCaravanBreastfeed", LookMode.Reference, LookMode.Value, ref this.tmpLoadPawns, ref this.tmpLoadFloat, false, false, false);
			Scribe_Deep.Look<ResurrectCorpseData>(ref this.resurrectTarget, "resurrectTarget", null);
			Scribe_Values.Look<int>(ref this.lastRangedHarmTick, "lastRangedHarmTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastRotStinkTick, "lastRotStinkTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastBroughtToSafeTemperatureTick, "lastBroughtToSafeTemperatureTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastBecameVisibleTick, "lastBecameVisibleTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastBecameInvisibleTick, "lastBecameInvisibleTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastForcedVisibleTick, "lastForcedVisibleTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastCombatantTick, "lastCombatantTick", -99999, false);
			Scribe_Values.Look<int>(ref this.entityTicksInCaptivity, "entityTicksInCaptivity", 0, false);
			Scribe_Values.Look<int>(ref this.hibernationEndedTick, "hibernationEndedTick", -99999, false);
			Scribe_Values.Look<int>(ref this.lastDayInteractionTick, "lastDayInteractionTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastSwamTick, "lastSwamTick", -99999, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.autoFeeders == null)
			{
				this.autoFeeders = new Dictionary<Pawn, AutofeedMode>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.babyCaravanBreastfeed == null)
			{
				this.babyCaravanBreastfeed = new Dictionary<Pawn, float>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.mentalFitGenerator == null)
			{
				this.mentalFitGenerator = new MentalFitGenerator(this.pawn);
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x001B3E14 File Offset: 0x001B2014
		public void MindStateTickInterval(int delta)
		{
			if (this.wantsToTradeWithColony)
			{
				TradeUtility.CheckInteractWithTradersTeachOpportunity(this.pawn);
			}
			if (this.meleeThreat != null && !this.MeleeThreatStillThreat)
			{
				this.meleeThreat = null;
			}
			this.mentalStateHandler.MentalStateHandlerTickInterval(delta);
			this.mentalBreaker.MentalBreakerTickInterval(delta);
			this.mentalFitGenerator.TickInterval(delta);
			this.inspirationHandler.InspirationHandlerTickInterval(delta);
			if (!this.pawn.GetPosture().Laying())
			{
				this.applyBedThoughtsTick = 0;
			}
			if (this.pawn.IsHashIntervalTick(100, delta))
			{
				if (this.pawn.Spawned)
				{
					int regionsToScan = this.anyCloseHostilesRecently ? 24 : 18;
					this.anyCloseHostilesRecently = PawnUtility.EnemiesAreNearby(this.pawn, regionsToScan, true, -1f, 1, false);
				}
				else
				{
					this.anyCloseHostilesRecently = false;
				}
			}
			if (this.WillJoinColonyIfRescued && this.AnythingPreventsJoiningColonyIfRescued)
			{
				this.WillJoinColonyIfRescued = false;
			}
			if (this.pawn.Spawned && this.pawn.IsWildMan() && !this.WildManEverReachedOutside && this.pawn.GetDistrict(RegionType.Set_Passable) != null && this.pawn.GetDistrict(RegionType.Set_Passable).TouchesMapEdge)
			{
				this.WildManEverReachedOutside = true;
			}
			if (this.pawn.Spawned && this.pawn.RaceProps.IsFlesh && this.pawn.needs.mood != null && this.pawn.IsHashIntervalTick(120, delta))
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (this.CanGainGainThoughtNow(terrain.traversedThought))
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(terrain.traversedThought, null);
				}
				WeatherDef curWeatherLerped = this.pawn.Map.weatherManager.CurWeatherLerped;
				if (this.CanGainGainThoughtNow(curWeatherLerped.weatherThought))
				{
					bool flag = this.pawn.Position.Roofed(this.pawn.Map);
					bool flag2 = curWeatherLerped.weatherThought.stages.Count == 1;
					if (!flag || !flag2)
					{
						int stage = (flag2 || flag) ? 0 : 1;
						this.pawn.needs.mood.thoughts.memories.TryGainMemoryFast(curWeatherLerped.weatherThought, stage, null);
					}
				}
				if (this.pawn.Position.GasDensity(this.pawn.Map, GasType.RotStink) > 0 && GasUtility.IsAffectedByExposure(this.pawn))
				{
					this.lastRotStinkTick = Find.TickManager.TicksGame;
				}
			}
			if (this.droppedWeapon != null && !this.droppedWeapon.Spawned)
			{
				this.droppedWeapon = null;
			}
			int num = GenLocalDate.DayTick(this.pawn);
			if (num < this.lastDayInteractionTick)
			{
				this.interactionsToday = 0;
			}
			this.lastDayInteractionTick = num;
			if (this.pawn.IsFighting())
			{
				Job curJob = this.pawn.CurJob;
				if (((curJob != null) ? curJob.def : null) != JobDefOf.Wait_Combat)
				{
					goto IL_319;
				}
			}
			Pawn_EquipmentTracker equipment = this.pawn.equipment;
			if (((equipment != null) ? equipment.Primary : null) == null)
			{
				goto IL_329;
			}
			IL_319:
			this.lastCombatantTick = Find.TickManager.TicksGame;
			IL_329:
			Pawn pawn = this.enemyTarget as Pawn;
			if (pawn != null && pawn.mindState != null)
			{
				pawn.mindState.lastCombatantTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x001B4176 File Offset: 0x001B2376
		private bool CanGainGainThoughtNow(ThoughtDef thought)
		{
			return thought != null && (thought != ThoughtDefOf.SoakingWet || GenTicks.TicksGame - this.lastSwamTick >= 7500);
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x001B419C File Offset: 0x001B239C
		public void JoinColonyBecauseRescuedBy(Pawn by)
		{
			this.WillJoinColonyIfRescued = false;
			if (this.AnythingPreventsJoiningColonyIfRescued)
			{
				return;
			}
			InteractionWorker_RecruitAttempt.DoRecruit(by, this.pawn, false);
			if (this.pawn.needs != null && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null, null);
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMeByOfferingHelp, by, null);
			}
			TaggedString taggedString = "LetterRescueQuestFinished".Translate(this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst();
			if (!this.pawn.Map.wasSpawnedViaGravShipLanding)
			{
				taggedString += " " + "LetterRescueQuestFinishedCaravanExtra".Translate(this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
			}
			Find.LetterStack.ReceiveLetter("LetterLabelRescueQuestFinished".Translate(), taggedString, LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x001B42E4 File Offset: 0x001B24E4
		public void ResetLastDisturbanceTick()
		{
			this.lastDisturbanceTick = -9999999;
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x001B42F4 File Offset: 0x001B24F4
		public void SetupLastHumanMeatTick()
		{
			if (this.pawn.Ideo != null && this.pawn.Ideo.IdeoCausesHumanMeatCravings())
			{
				this.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
				this.lastHumanMeatIngestedTick -= Pawn_MindState.LastHumanMeatEatenTicksRange.RandomInRange;
			}
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x001B434A File Offset: 0x001B254A
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_MindState.<GetGizmos>d__121 <GetGizmos>d__ = new Pawn_MindState.<GetGizmos>d__121(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x001B435A File Offset: 0x001B255A
		public void SetNoAidRelationsGainUntilTick(int tick)
		{
			if (tick > this.noAidRelationsGainUntilTick)
			{
				this.noAidRelationsGainUntilTick = tick;
			}
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x001B436C File Offset: 0x001B256C
		public void Notify_OutfitChanged()
		{
			this.nextApparelOptimizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x001B4380 File Offset: 0x001B2580
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			this.mentalStateHandler.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn))
			{
				this.lastHarmTick = Find.TickManager.TicksGame;
				if (dinfo.Def.isRanged)
				{
					this.lastRangedHarmTick = Find.TickManager.TicksGame;
				}
				if (this.pawn.Spawned)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (!this.mentalStateHandler.InMentalState && dinfo.Instigator != null && (pawn != null || dinfo.Instigator is Building_Turret) && dinfo.Instigator.Faction != null && (!ModsConfig.AnomalyActive || dinfo.Instigator.Faction != Faction.OfEntities) && (dinfo.Instigator.Faction.def.humanlikeFaction || (pawn != null && pawn.def.race.intelligence >= Intelligence.ToolUser)) && this.pawn.Faction == null && (this.pawn.IsAnimal || this.pawn.IsWildMan()) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.PredatorHunt || dinfo.Instigator != ((JobDriver_PredatorHunt)this.pawn.jobs.curDriver).Prey) && Rand.Chance(PawnUtility.GetManhunterOnDamageChance(this.pawn, dinfo.Instigator, -1f)))
					{
						this.StartManhunterBecauseOfPawnAction(pawn, "AnimalManhunterFromDamage", true);
					}
					else if (dinfo.Instigator != null && dinfo.Def.makesAnimalsFlee && FleeUtility.ShouldAnimalFleeDanger(this.pawn))
					{
						this.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
					}
				}
				if (this.pawn.GetPosture() != PawnPosture.Standing)
				{
					this.lastDisturbanceTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x001B457C File Offset: 0x001B277C
		public void Notify_ClamorImpact(Thing instigator)
		{
			this.canSleepTick = Find.TickManager.TicksGame + 1000;
			if (this.pawn.IsAnimal && instigator is Projectile && ((Projectile)instigator).AnimalsFleeImpact && (this.pawn.playerSettings == null || this.pawn.playerSettings.Master == null) && Rand.Chance(0.4f) && FleeUtility.ShouldAnimalFleeDanger(this.pawn))
			{
				this.StartFleeingBecauseOfPawnAction(((Projectile)instigator).Launcher);
			}
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x001B460A File Offset: 0x001B280A
		internal void Notify_EngagedTarget()
		{
			this.lastEngageTargetTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x001B461C File Offset: 0x001B281C
		internal void Notify_AttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x001B4638 File Offset: 0x001B2838
		internal bool CheckStartMentalStateBecauseRecruitAttempted(Pawn tamer)
		{
			if (!this.pawn.IsAnimal && (!this.pawn.IsWildMan() || this.pawn.IsPrisoner))
			{
				return false;
			}
			if (!this.mentalStateHandler.InMentalState && this.pawn.Faction == null && Rand.Chance(PawnUtility.GetManhunterOnTameFailChance(this.pawn)))
			{
				this.StartManhunterBecauseOfPawnAction(tamer, "AnimalManhunterFromTaming", false);
				return true;
			}
			return false;
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x001B46AA File Offset: 0x001B28AA
		internal void Notify_DangerousExploderAboutToExplode(Thing exploder)
		{
			if (this.pawn.RaceProps.intelligence >= Intelligence.Humanlike && !this.pawn.Drafted)
			{
				this.knownExploder = exploder;
				this.pawn.jobs.CheckForJobOverride(0f, true);
			}
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x001B46EC File Offset: 0x001B28EC
		public void Notify_Explosion(Explosion explosion)
		{
			if (this.pawn.Faction != null)
			{
				return;
			}
			if (explosion.radius < 3.5f || !this.pawn.Position.InHorDistOf(explosion.Position, explosion.radius + 7f))
			{
				return;
			}
			if (FleeUtility.ShouldAnimalFleeDanger(this.pawn))
			{
				this.StartFleeingBecauseOfPawnAction(explosion);
			}
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x001B4750 File Offset: 0x001B2950
		public void Notify_TuckedIntoBed()
		{
			if (this.pawn.IsWildMan())
			{
				this.WildManEverReachedOutside = false;
			}
			this.ResetLastDisturbanceTick();
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x001B476C File Offset: 0x001B296C
		public void Notify_SelfTended()
		{
			this.lastSelfTendTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x001B477E File Offset: 0x001B297E
		public void Notify_PredatorHuntingPlayerNotification()
		{
			this.lastPredatorHuntingPlayerNotificationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x001B4790 File Offset: 0x001B2990
		private IEnumerable<Pawn> GetPackmates(Pawn pawn, float radius)
		{
			Pawn_MindState.<GetPackmates>d__134 <GetPackmates>d__ = new Pawn_MindState.<GetPackmates>d__134(-2);
			<GetPackmates>d__.<>3__pawn = pawn;
			<GetPackmates>d__.<>3__radius = radius;
			return <GetPackmates>d__;
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x001B47A8 File Offset: 0x001B29A8
		private void StartManhunterBecauseOfPawnAction(Pawn instigator, string letterTextKey, bool causedByDamage = false)
		{
			if (!this.mentalStateHandler.TryStartMentalState(PawnUtility.ManhunterStateFor(this.pawn), null, false, false, false, null, false, false, false))
			{
				return;
			}
			string text = letterTextKey.Translate(this.pawn.Label, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
			GlobalTargetInfo target = this.pawn;
			float num = 0.5f;
			if (causedByDamage)
			{
				num *= PawnUtility.GetManhunterChanceFactorForInstigator(instigator);
			}
			int num2 = 1;
			if (Find.Storyteller.difficulty.allowBigThreats && Rand.Value < num)
			{
				PawnKindDef pawnKindDef = this.pawn.RaceProps.manhunterPackUseLabelFrom ?? this.pawn.kindDef;
				Pawn value = this.pawn;
				foreach (Pawn pawn in this.GetPackmates(this.pawn, 24f))
				{
					if (pawn.mindState.mentalStateHandler.TryStartMentalState(PawnUtility.ManhunterStateFor(pawn), null, false, false, false, null, false, causedByDamage, false))
					{
						num2++;
						if (pawn.kindDef == pawnKindDef)
						{
							value = pawn;
						}
					}
				}
				if (num2 > 1)
				{
					target = new TargetInfo(this.pawn.Position, this.pawn.Map, false);
					text += "\n\n";
					text += "AnimalManhunterOthers".Translate(pawnKindDef.GetLabelPlural(-1), value);
				}
			}
			string value2 = this.pawn.IsAnimal ? this.pawn.Label : this.pawn.def.label;
			string str = "LetterLabelAnimalManhunterRevenge".Translate(value2).CapitalizeFirst();
			Find.LetterStack.ReceiveLetter(str, text, (num2 == 1) ? LetterDefOf.ThreatSmall : LetterDefOf.ThreatBig, target, null, null, null, null, 0, true);
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x001B49DC File Offset: 0x001B2BDC
		public void StartFleeingBecauseOfPawnAction(Thing instigator)
		{
			if (this.pawn.RaceProps.canLeaveMapFlying && this.pawn.Faction != Faction.OfPlayer && !this.pawn.Position.Roofed(this.pawn.Map) && !this.pawn.IsQuestLodger() && this.pawn.flight.CanEverFly && Rand.Chance(this.pawn.RaceProps.leaveMapOnFleeChance))
			{
				this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.ExitMapFlying), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
				return;
			}
			List<Thing> threats = new List<Thing>
			{
				instigator
			};
			IntVec3 fleeDest = CellFinderLoose.GetFleeDest(this.pawn, threats, this.pawn.Position.DistanceTo(instigator.Position) + 28f);
			if (fleeDest != this.pawn.Position)
			{
				Vector3 lhs = (fleeDest - this.pawn.Position).ToVector3();
				Vector3 rhs = (this.pawn.Map.Center - this.pawn.Position).ToVector3();
				bool flag = Vector3.Dot(lhs, rhs) < 0f;
				IntVec3 c;
				if (this.pawn.IsAnimal && this.pawn.Faction == null && flag && Rand.Chance(0.5f) && CellFinderLoose.GetFleeExitPosition(this.pawn, 10f, out c))
				{
					Job job = JobMaker.MakeJob(JobDefOf.Flee, c, instigator);
					job.exitMapOnArrival = true;
					this.pawn.jobs.StartJob(job, JobCondition.InterruptOptional, null, false, true, null, null, false, false, null, false, true, false);
				}
				else
				{
					this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Flee, fleeDest, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false, false, null, false, true, false);
				}
			}
			if (this.pawn.RaceProps.herdAnimal && Rand.Chance(0.1f))
			{
				foreach (Pawn pawn in this.GetPackmates(this.pawn, 24f))
				{
					if (FleeUtility.ShouldAnimalFleeDanger(pawn))
					{
						IntVec3 fleeDest2 = CellFinderLoose.GetFleeDest(pawn, threats, pawn.Position.DistanceTo(instigator.Position) + 28f);
						if (fleeDest2 != pawn.Position)
						{
							pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Flee, fleeDest2, instigator), JobCondition.InterruptOptional, null, false, true, null, null, false, false, null, false, true, false);
						}
					}
				}
			}
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x001B4D04 File Offset: 0x001B2F04
		public void SetAutofeeder(Pawn feeder, AutofeedMode setting)
		{
			if (setting == AutofeedMode.Childcare)
			{
				this.autoFeeders.Remove(feeder);
			}
			else
			{
				this.autoFeeders[feeder] = setting;
			}
			if (setting == AutofeedMode.Childcare)
			{
				if (feeder.WorkTypeIsDisabled(WorkTypeDefOf.Childcare))
				{
					Messages.Message("MessageChildcareDisabled".Translate(feeder.Named("FEEDER")), feeder, MessageTypeDefOf.CautionInput, false);
					return;
				}
				if (!feeder.workSettings.WorkIsActive(WorkTypeDefOf.Childcare))
				{
					Messages.Message("MessageChildcareNotAssigned".Translate(feeder.Named("FEEDER")), feeder, MessageTypeDefOf.CautionInput, false);
				}
			}
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x001B4DAB File Offset: 0x001B2FAB
		public AutofeedMode AutofeedSetting(Pawn feeder)
		{
			if (!this.pawn.DevelopmentalStage.Baby())
			{
				return AutofeedMode.Never;
			}
			return this.autoFeeders.TryGetValue(feeder, AutofeedMode.Childcare);
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x001B4DD0 File Offset: 0x001B2FD0
		public IEnumerable<Pawn> Autofeeders()
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(this.pawn, out breastfeedFailReason))
			{
				return Enumerable.Empty<Pawn>();
			}
			return this.autoFeeders.Keys;
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x001B4E00 File Offset: 0x001B3000
		public bool AnyAutofeeder(AutofeedMode autofeed, Predicate<Pawn, Pawn> feederPredicate, List<Pawn> possibleFeeders = null)
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(this.pawn, out breastfeedFailReason))
			{
				return false;
			}
			possibleFeeders = (possibleFeeders ?? this.pawn.MapHeld.mapPawns.FreeHumanlikesOfFaction(this.pawn.Faction));
			foreach (Pawn pawn in possibleFeeders)
			{
				if (pawn != this.pawn && autofeed == this.AutofeedSetting(pawn) && feederPredicate(this.pawn, pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x001B4EA8 File Offset: 0x001B30A8
		public void ClearBreastfeedCaravan()
		{
			this.babyCaravanBreastfeed.Clear();
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x001B4EB8 File Offset: 0x001B30B8
		public void BreastfeedCaravan(Pawn baby, float amountPct)
		{
			if (this.pawn.GetCaravan() == null)
			{
				Log.Warning(this.pawn.Label + " tried to caravan breastfeed while not in a caravan");
			}
			if (!this.babyCaravanBreastfeed.ContainsKey(baby))
			{
				this.babyCaravanBreastfeed.Add(baby, 0f);
			}
			float num = this.babyCaravanBreastfeed[baby] + amountPct;
			if (num >= 0.6f)
			{
				num -= 0.6f;
				baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BreastfedMe, this.pawn, null);
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BreastfedBaby, baby, null);
			}
			this.babyCaravanBreastfeed[baby] = num;
		}

		// Token: 0x04002CEC RID: 11500
		public Pawn pawn;

		// Token: 0x04002CED RID: 11501
		public MentalStateHandler mentalStateHandler;

		// Token: 0x04002CEE RID: 11502
		public MentalBreaker mentalBreaker;

		// Token: 0x04002CEF RID: 11503
		public MentalFitGenerator mentalFitGenerator;

		// Token: 0x04002CF0 RID: 11504
		public InspirationHandler inspirationHandler;

		// Token: 0x04002CF1 RID: 11505
		public PriorityWork priorityWork;

		// Token: 0x04002CF2 RID: 11506
		private bool activeInt = true;

		// Token: 0x04002CF3 RID: 11507
		public JobTag lastJobTag;

		// Token: 0x04002CF4 RID: 11508
		public int lastIngestTick = -99999;

		// Token: 0x04002CF5 RID: 11509
		public int nextApparelOptimizeTick = -99999;

		// Token: 0x04002CF6 RID: 11510
		public bool canFleeIndividual = true;

		// Token: 0x04002CF7 RID: 11511
		public int exitMapAfterTick = -99999;

		// Token: 0x04002CF8 RID: 11512
		public int lastDisturbanceTick = -99999;

		// Token: 0x04002CF9 RID: 11513
		public int hibernationEndedTick = -99999;

		// Token: 0x04002CFA RID: 11514
		public IntVec3 forcedGotoPosition = IntVec3.Invalid;

		// Token: 0x04002CFB RID: 11515
		public Thing knownExploder;

		// Token: 0x04002CFC RID: 11516
		public bool wantsToTradeWithColony;

		// Token: 0x04002CFD RID: 11517
		public bool traderDismissed;

		// Token: 0x04002CFE RID: 11518
		public bool hasQuest;

		// Token: 0x04002CFF RID: 11519
		public Thing lastMannedThing;

		// Token: 0x04002D00 RID: 11520
		public int canLovinTick = -99999;

		// Token: 0x04002D01 RID: 11521
		public int canSleepTick = -99999;

		// Token: 0x04002D02 RID: 11522
		public Pawn meleeThreat;

		// Token: 0x04002D03 RID: 11523
		public int lastMeleeThreatHarmTick = -99999;

		// Token: 0x04002D04 RID: 11524
		public int lastEngageTargetTick = -99999;

		// Token: 0x04002D05 RID: 11525
		public int lastAttackTargetTick = -99999;

		// Token: 0x04002D06 RID: 11526
		public LocalTargetInfo lastAttackedTarget;

		// Token: 0x04002D07 RID: 11527
		public Thing enemyTarget;

		// Token: 0x04002D08 RID: 11528
		public BreachingTargetData breachingTarget;

		// Token: 0x04002D09 RID: 11529
		public PawnDuty duty;

		// Token: 0x04002D0A RID: 11530
		public Dictionary<int, int> thinkData = new Dictionary<int, int>();

		// Token: 0x04002D0B RID: 11531
		public int lastAssignedInteractTime = -99999;

		// Token: 0x04002D0C RID: 11532
		public int interactionsToday;

		// Token: 0x04002D0D RID: 11533
		public int lastDayInteractionTick;

		// Token: 0x04002D0E RID: 11534
		public int lastInventoryRawFoodUseTick;

		// Token: 0x04002D0F RID: 11535
		public bool nextMoveOrderIsWait;

		// Token: 0x04002D10 RID: 11536
		public bool nextMoveOrderIsCrawlBreak;

		// Token: 0x04002D11 RID: 11537
		public int lastTakeCombatEnhancingDrugTick = -99999;

		// Token: 0x04002D12 RID: 11538
		public int lastTakeRecreationalDrugTick = -60000;

		// Token: 0x04002D13 RID: 11539
		public int lastHarmTick = -99999;

		// Token: 0x04002D14 RID: 11540
		public int noAidRelationsGainUntilTick = -99999;

		// Token: 0x04002D15 RID: 11541
		public bool anyCloseHostilesRecently;

		// Token: 0x04002D16 RID: 11542
		public int applyBedThoughtsTick;

		// Token: 0x04002D17 RID: 11543
		public int applyThroneThoughtsTick;

		// Token: 0x04002D18 RID: 11544
		public bool applyBedThoughtsOnLeave;

		// Token: 0x04002D19 RID: 11545
		public bool willJoinColonyIfRescuedInt;

		// Token: 0x04002D1A RID: 11546
		private bool wildManEverReachedOutsideInt;

		// Token: 0x04002D1B RID: 11547
		public int timesGuestTendedToByPlayer;

		// Token: 0x04002D1C RID: 11548
		public int lastSelfTendTick = -99999;

		// Token: 0x04002D1D RID: 11549
		public bool spawnedByInfestationThingComp;

		// Token: 0x04002D1E RID: 11550
		public int lastPredatorHuntingPlayerNotificationTick = -99999;

		// Token: 0x04002D1F RID: 11551
		public int lastSlaveSuppressedTick = -99999;

		// Token: 0x04002D20 RID: 11552
		public ThingDef lastBedDefSleptIn;

		// Token: 0x04002D21 RID: 11553
		public int lastHumanMeatIngestedTick = -99999;

		// Token: 0x04002D22 RID: 11554
		public int? lastStartRoamCooldownTick;

		// Token: 0x04002D23 RID: 11555
		public int nextInventoryStockTick = -99999;

		// Token: 0x04002D24 RID: 11556
		public bool returnToHealingPod;

		// Token: 0x04002D25 RID: 11557
		private Dictionary<Pawn, AutofeedMode> autoFeeders = new Dictionary<Pawn, AutofeedMode>();

		// Token: 0x04002D26 RID: 11558
		private Dictionary<Pawn, float> babyCaravanBreastfeed = new Dictionary<Pawn, float>();

		// Token: 0x04002D27 RID: 11559
		public int nextSleepingBreastfeedStrictTick = -99999;

		// Token: 0x04002D28 RID: 11560
		public ResurrectCorpseData resurrectTarget;

		// Token: 0x04002D29 RID: 11561
		public int lastRangedHarmTick;

		// Token: 0x04002D2A RID: 11562
		public Thing droppedWeapon;

		// Token: 0x04002D2B RID: 11563
		public int lastRotStinkTick = -99999;

		// Token: 0x04002D2C RID: 11564
		public int lastBroughtToSafeTemperatureTick = -99999;

		// Token: 0x04002D2D RID: 11565
		public int lastBecameVisibleTick = -99999;

		// Token: 0x04002D2E RID: 11566
		public int lastBecameInvisibleTick = -99999;

		// Token: 0x04002D2F RID: 11567
		public int lastForcedVisibleTick = -99999;

		// Token: 0x04002D30 RID: 11568
		public int lastCombatantTick = -99999;

		// Token: 0x04002D31 RID: 11569
		public int entityTicksInCaptivity;

		// Token: 0x04002D32 RID: 11570
		public int lastSwamTick = -99999;

		// Token: 0x04002D33 RID: 11571
		public float maxDistToSquadFlag = -1f;

		// Token: 0x04002D34 RID: 11572
		private List<Pawn> tmpLoadPawns;

		// Token: 0x04002D35 RID: 11573
		private List<AutofeedMode> tmpLoadAutofeedMode;

		// Token: 0x04002D36 RID: 11574
		private List<float> tmpLoadFloat;

		// Token: 0x04002D37 RID: 11575
		private const int UpdateAnyCloseHostilesRecentlyEveryTicks = 100;

		// Token: 0x04002D38 RID: 11576
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToActivate = 18;

		// Token: 0x04002D39 RID: 11577
		private const int AnyCloseHostilesRecentlyRegionsToScan_ToDeactivate = 24;

		// Token: 0x04002D3A RID: 11578
		private const float HarmForgetDistance = 3f;

		// Token: 0x04002D3B RID: 11579
		private const int MeleeHarmForgetDelay = 400;

		// Token: 0x04002D3C RID: 11580
		private const float ClamorImpactFleeChance = 0.4f;

		// Token: 0x04002D3D RID: 11581
		private const float PawnActionFleeOffMapChance = 0.5f;

		// Token: 0x04002D3E RID: 11582
		private const int RoamingCooldownTicks = 30000;

		// Token: 0x04002D3F RID: 11583
		private static readonly IntRange LastHumanMeatEatenTicksRange = new IntRange(0, 60000);

		// Token: 0x04002D40 RID: 11584
		public static readonly IntRange NextSleepingBreastfeedTickCheck = new IntRange(300, 500);

		// Token: 0x04002D41 RID: 11585
		public const int DefaultCombatantCooldown = 3600;

		// Token: 0x04002D42 RID: 11586
		public const int WeatherThoughInterval = 120;

		// Token: 0x04002D43 RID: 11587
		public const int SwimSoakingWetImmuneDuration = 7500;
	}
}
