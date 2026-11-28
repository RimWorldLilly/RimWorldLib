using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002025 RID: 8229
	public class GameComponent_Anomaly : GameComponent
	{
		// Token: 0x17002569 RID: 9577
		// (get) Token: 0x0600D01E RID: 53278 RVA: 0x004C7D72 File Offset: 0x004C5F72
		public bool MonolithSpawned
		{
			get
			{
				return this.monolith != null && this.monolith.Spawned;
			}
		}

		// Token: 0x1700256A RID: 9578
		// (get) Token: 0x0600D01F RID: 53279 RVA: 0x004C7D89 File Offset: 0x004C5F89
		public LargeBuildingSpawnParms MonolithSpawnParms
		{
			get
			{
				return this.monolithSpawnParms;
			}
		}

		// Token: 0x1700256B RID: 9579
		// (get) Token: 0x0600D020 RID: 53280 RVA: 0x004C7D91 File Offset: 0x004C5F91
		public int Level
		{
			get
			{
				if (!this.MonolithSpawned)
				{
					return 0;
				}
				return this.level;
			}
		}

		// Token: 0x1700256C RID: 9580
		// (get) Token: 0x0600D021 RID: 53281 RVA: 0x004C7DA3 File Offset: 0x004C5FA3
		public int HighestLevelReached
		{
			get
			{
				return this.highestLevelReached;
			}
		}

		// Token: 0x1700256D RID: 9581
		// (get) Token: 0x0600D022 RID: 53282 RVA: 0x004C7DAB File Offset: 0x004C5FAB
		public MonolithLevelDef LevelDef
		{
			get
			{
				if (!this.MonolithSpawned)
				{
					return MonolithLevelDefOf.Inactive;
				}
				return this.levelDef ?? MonolithLevelDefOf.Inactive;
			}
		}

		// Token: 0x1700256E RID: 9582
		// (get) Token: 0x0600D023 RID: 53283 RVA: 0x004C7DCA File Offset: 0x004C5FCA
		public MonolithLevelDef NextLevelDef
		{
			get
			{
				if (!this.LevelDef.advanceThroughActivation)
				{
					return null;
				}
				return DefDatabase<MonolithLevelDef>.AllDefs.FirstOrDefault((MonolithLevelDef x) => x.level == this.Level + 1);
			}
		}

		// Token: 0x1700256F RID: 9583
		// (get) Token: 0x0600D024 RID: 53284 RVA: 0x004C7DF1 File Offset: 0x004C5FF1
		public bool QuestlineEnded
		{
			get
			{
				return this.LevelDef == MonolithLevelDefOf.Embraced || this.LevelDef == MonolithLevelDefOf.Disrupted;
			}
		}

		// Token: 0x17002570 RID: 9584
		// (get) Token: 0x0600D025 RID: 53285 RVA: 0x004C7E0F File Offset: 0x004C600F
		public int TicksSinceLastLevelChange
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastLevelChangeTick;
			}
		}

		// Token: 0x17002571 RID: 9585
		// (get) Token: 0x0600D026 RID: 53286 RVA: 0x004C7E22 File Offset: 0x004C6022
		private int TicksSinceLastMetalhorrorBio
		{
			get
			{
				return Find.TickManager.TicksGame - this.newestMetalhorrorBiosignatureTick;
			}
		}

		// Token: 0x17002572 RID: 9586
		// (get) Token: 0x0600D027 RID: 53287 RVA: 0x004C7E35 File Offset: 0x004C6035
		public bool CanNewMetalhorrorBiosignatureImplantOccur
		{
			get
			{
				return this.TicksSinceLastMetalhorrorBio >= 1800000;
			}
		}

		// Token: 0x17002573 RID: 9587
		// (get) Token: 0x0600D028 RID: 53288 RVA: 0x004C7E47 File Offset: 0x004C6047
		public IReadOnlyList<ChoiceLetter> MonolithLetters
		{
			get
			{
				return this.monolithLetters;
			}
		}

		// Token: 0x17002574 RID: 9588
		// (get) Token: 0x0600D029 RID: 53289 RVA: 0x004C7E4F File Offset: 0x004C604F
		public bool MonolithStudyCompleted
		{
			get
			{
				return this.monolithLetters.Count == 3;
			}
		}

		// Token: 0x17002575 RID: 9589
		// (get) Token: 0x0600D02A RID: 53290 RVA: 0x004C7E5F File Offset: 0x004C605F
		public int MonolithNextIndex
		{
			get
			{
				return this.monolithNextIndex;
			}
		}

		// Token: 0x17002576 RID: 9590
		// (get) Token: 0x0600D02B RID: 53291 RVA: 0x004C7E67 File Offset: 0x004C6067
		public int MonolithStudyProgress
		{
			get
			{
				return this.monolithStudyProgress;
			}
		}

		// Token: 0x17002577 RID: 9591
		// (get) Token: 0x0600D02C RID: 53292 RVA: 0x004C7E6F File Offset: 0x004C606F
		public bool GenerateMonolith
		{
			get
			{
				return Find.Storyteller.difficulty.AnomalyPlaystyleDef.generateMonolith;
			}
		}

		// Token: 0x17002578 RID: 9592
		// (get) Token: 0x0600D02D RID: 53293 RVA: 0x004C7E85 File Offset: 0x004C6085
		public bool AmbientHorrorMode
		{
			get
			{
				return Find.Storyteller.difficulty.AnomalyPlaystyleDef == AnomalyPlaystyleDefOf.AmbientHorror;
			}
		}

		// Token: 0x17002579 RID: 9593
		// (get) Token: 0x0600D02E RID: 53294 RVA: 0x004C7E9D File Offset: 0x004C609D
		public bool AnomalyStudyEnabled
		{
			get
			{
				return ModsConfig.AnomalyActive && Find.Storyteller.difficulty.AnomalyPlaystyleDef.enableAnomalyContent && (this.HighestLevelReached > 0 || !this.GenerateMonolith);
			}
		}

		// Token: 0x1700257A RID: 9594
		// (get) Token: 0x0600D02F RID: 53295 RVA: 0x004C7ED8 File Offset: 0x004C60D8
		public float AnomalyThreatFractionNow
		{
			get
			{
				AnomalyPlaystyleDef anomalyPlaystyleDef = Find.Storyteller.difficulty.AnomalyPlaystyleDef;
				if (!anomalyPlaystyleDef.enableAnomalyContent)
				{
					return 0f;
				}
				if (anomalyPlaystyleDef.overrideThreatFraction && Find.Storyteller.difficulty.overrideAnomalyThreatsFraction != null)
				{
					return Find.Storyteller.difficulty.overrideAnomalyThreatsFraction.Value;
				}
				MonolithLevelDef monolithLevelDef = this.LevelDef;
				if (monolithLevelDef.useInactiveAnomalyThreatFraction)
				{
					return Find.Storyteller.difficulty.anomalyThreatsInactiveFraction * monolithLevelDef.anomalyThreatFractionFactor;
				}
				if (monolithLevelDef.useActiveAnomalyThreatFraction)
				{
					return Find.Storyteller.difficulty.anomalyThreatsActiveFraction * monolithLevelDef.anomalyThreatFractionFactor;
				}
				return this.LevelDef.anomalyThreatFraction * monolithLevelDef.anomalyThreatFractionFactor;
			}
		}

		// Token: 0x0600D030 RID: 53296 RVA: 0x004C7F90 File Offset: 0x004C6190
		public GameComponent_Anomaly(Game game)
		{
			if (ModsConfig.AnomalyActive)
			{
				this.levelDef = DefDatabase<MonolithLevelDef>.AllDefs.FirstOrDefault((MonolithLevelDef x) => x.level == this.level);
				this.level = this.levelDef.level;
				this.monolithSpawnParms = new LargeBuildingSpawnParms
				{
					minDistanceToColonyBuilding = 30f,
					minDistToEdge = 10,
					attemptSpawnLocationType = SpawnLocationType.Outdoors,
					attemptNotUnderBuildings = true,
					canSpawnOnImpassable = false,
					allowFogged = true,
					overrideSize = new IntVec2?(new IntVec2(ThingDefOf.VoidMonolith.size.x + 2, ThingDefOf.VoidMonolith.size.z + 2))
				};
			}
		}

		// Token: 0x0600D031 RID: 53297 RVA: 0x004C80E8 File Offset: 0x004C62E8
		public override void StartedNewGame()
		{
			base.StartedNewGame();
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			this.Notify_LevelChanged(true);
			this.nextHarbingerTreeCheckTick = Find.TickManager.TicksGame + Mathf.RoundToInt(GameComponent_Anomaly.HarbingerTreeCheckIntervalHours.RandomInRange * 2500f);
			this.monolith = (Find.AnyPlayerHomeMap.listerThings.ThingsOfDef(ThingDefOf.VoidMonolith).FirstOrDefault<Thing>() as Building_VoidMonolith);
		}

		// Token: 0x0600D032 RID: 53298 RVA: 0x004C8158 File Offset: 0x004C6358
		public override void GameComponentTick()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (this.monolith != null && this.monolith.quest == null && this.Level > 0)
			{
				this.monolith.CheckAndGenerateQuest();
			}
			if (this.monolith != null && !this.monolith.Spawned)
			{
				this.monolith = null;
			}
			if (Find.TickManager.TicksGame > this.nextHarbingerTreeCheckTick)
			{
				this.TrySpawnHarbingerTrees();
				this.nextHarbingerTreeCheckTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + GameComponent_Anomaly.HarbingerTreeCheckIntervalHours.RandomInRange * 2500f);
			}
			this.UpdateCorpseTrackers();
			this.UpdateHypnotized();
			if (this.fireLevelIncidentTick > 0 && Find.TickManager.TicksGame > this.fireLevelIncidentTick)
			{
				if (this.levelDef.incidentsOnReached != null)
				{
					foreach (IncidentDef def in this.levelDef.incidentsOnReached)
					{
						IncidentParms parms = new IncidentParms
						{
							target = Find.AnyPlayerHomeMap,
							points = StorytellerUtility.DefaultThreatPointsNow(this.monolith.Map),
							forced = true
						};
						Find.Storyteller.incidentQueue.Add(def, Find.TickManager.TicksGame, parms, 0);
					}
				}
				this.fireLevelIncidentTick = -99999;
			}
			if (this.fireGrayPallTick > 0 && GenTicks.TicksGame > this.fireGrayPallTick)
			{
				if (this.monolith != null)
				{
					int duration = Mathf.CeilToInt(GameComponent_Anomaly.GrayPallConditionDaysRange.RandomInRange * 60000f);
					GameCondition cond = GameConditionMaker.MakeCondition(GameConditionDefOf.GrayPall, duration);
					this.monolith.MapHeld.GameConditionManager.RegisterCondition(cond);
					TaggedString label = "LetterLabelGrayPallDescending".Translate();
					TaggedString text = "LetterGrayPallDescending".Translate();
					Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, null, 0, true);
				}
				this.fireGrayPallTick = -99999;
			}
			if (!this.metalHellPawns.NullOrEmpty<Pawn>() && Find.TickManager.TicksGame > this.metalHellReturnTick)
			{
				Map map = this.monolith.Map;
				Predicate<IntVec3> <>9__0;
				Predicate<IntVec3> <>9__1;
				foreach (Pawn pawn in this.metalHellPawns)
				{
					IntVec3 position = this.monolith.Position;
					Map map3 = this.monolith.Map;
					int squareRadius = 5;
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((IntVec3 c) => !c.Fogged(map) && c.Standable(map)));
					}
					IntVec3 intVec;
					if (CellFinder.TryFindRandomCellNear(position, map3, squareRadius, validator, out intVec, -1))
					{
						SkipUtility.SkipTo(pawn, intVec, this.monolith.Map);
					}
					else
					{
						Map map2 = this.monolith.Map;
						Predicate<IntVec3> validator2;
						if ((validator2 = <>9__1) == null)
						{
							validator2 = (<>9__1 = ((IntVec3 c) => !c.Fogged(map) && c.Standable(map) && map.reachability.CanReachColony(c)));
						}
						if (CellFinder.TryFindRandomCell(map2, validator2, out intVec))
						{
							SkipUtility.SkipTo(pawn, intVec, this.monolith.Map);
						}
					}
					this.monolith.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(intVec, this.monolith.Map, 1f), intVec, 60);
					SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(intVec, this.monolith.Map, false));
					Messages.Message("VoidNodeReturnedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
				}
				Find.LetterStack.ReceiveLetter("VoidNodeReturnedLabel".Translate(this.voidNodeActivator.Named("PAWN")), this.metalHellReturnLetterText, LetterDefOf.PositiveEvent, this.voidNodeActivator, null, null, null, null, 0, true);
				this.metalHellPawns.Clear();
				this.metalHellReturnTick = -99999;
				this.metalHellReturnLetterText = null;
			}
			if (Find.TickManager.TicksGame == this.metalHellClosedTick + 60 && this.levelDef == MonolithLevelDefOf.Disrupted)
			{
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (faction != Faction.OfPlayer && faction.def.humanlikeFaction && !faction.def.permanentEnemy)
					{
						faction.TryAffectGoodwillWith(Faction.OfPlayer, 50, true, true, HistoryEventDefOf.DestroyedVoidMonolith, null);
					}
				}
			}
		}

		// Token: 0x0600D033 RID: 53299 RVA: 0x004C8644 File Offset: 0x004C6844
		public void Notify_NewMetalhorrorBiosignatureImplanted()
		{
			this.newestMetalhorrorBiosignatureTick = GenTicks.TicksGame;
		}

		// Token: 0x0600D034 RID: 53300 RVA: 0x004C8651 File Offset: 0x004C6851
		public void Notify_MonolithStudyIncreased(ChoiceLetter letter, int nextIndex, int studyProgress)
		{
			this.monolithLetters.Add(letter);
			this.monolithNextIndex = nextIndex;
			this.monolithStudyProgress = studyProgress;
		}

		// Token: 0x0600D035 RID: 53301 RVA: 0x004C8670 File Offset: 0x004C6870
		private void UpdateHypnotized()
		{
			foreach (KeyValuePair<Pawn, int> keyValuePair in this.hypnotisedPawns)
			{
				Pawn pawn;
				int num;
				keyValuePair.Deconstruct(out pawn, out num);
				Pawn item = pawn;
				int num2 = num;
				if (GenTicks.TicksGame > num2)
				{
					GameComponent_Anomaly.toRemove.Add(item);
				}
			}
			foreach (Pawn key in GameComponent_Anomaly.toRemove)
			{
				this.hypnotisedPawns.Remove(key);
			}
			GameComponent_Anomaly.toRemove.Clear();
		}

		// Token: 0x0600D036 RID: 53302 RVA: 0x004C8734 File Offset: 0x004C6934
		private void UpdateCorpseTrackers()
		{
			this.ElevateLooseCorpseTrackers();
			foreach (UnnaturalCorpseTracker unnaturalCorpseTracker in this.looseCorpseTrackers)
			{
				unnaturalCorpseTracker.CorpseTick();
				if (unnaturalCorpseTracker.ShouldDisappear)
				{
					unnaturalCorpseTracker.Notify_Finished();
					GameComponent_Anomaly.tmpTrackers.Add(unnaturalCorpseTracker);
				}
			}
			foreach (KeyValuePair<Pawn, UnnaturalCorpseTracker> keyValuePair in this.corpseTrackers)
			{
				Pawn pawn;
				UnnaturalCorpseTracker unnaturalCorpseTracker2;
				keyValuePair.Deconstruct(out pawn, out unnaturalCorpseTracker2);
				Pawn item = pawn;
				UnnaturalCorpseTracker unnaturalCorpseTracker3 = unnaturalCorpseTracker2;
				unnaturalCorpseTracker3.CorpseTick();
				if (unnaturalCorpseTracker3.ShouldDisappear)
				{
					unnaturalCorpseTracker3.Notify_Finished();
					GameComponent_Anomaly.toRemove.Add(item);
				}
			}
			foreach (Pawn key in GameComponent_Anomaly.toRemove)
			{
				this.corpseTrackers.Remove(key);
			}
			foreach (UnnaturalCorpseTracker item2 in GameComponent_Anomaly.tmpTrackers)
			{
				this.looseCorpseTrackers.Remove(item2);
			}
			GameComponent_Anomaly.toRemove.Clear();
			GameComponent_Anomaly.tmpTrackers.Clear();
		}

		// Token: 0x0600D037 RID: 53303 RVA: 0x004C88BC File Offset: 0x004C6ABC
		public void RemoveCorpseTracker(Pawn pawn)
		{
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!this.corpseTrackers.TryGetValue(pawn, out unnaturalCorpseTracker))
			{
				return;
			}
			unnaturalCorpseTracker.Notify_Finished();
			this.corpseTrackers.Remove(pawn);
		}

		// Token: 0x0600D038 RID: 53304 RVA: 0x004C88ED File Offset: 0x004C6AED
		public void IncrementLevel()
		{
			if (this.LevelDef == MonolithLevelDefOf.Inactive)
			{
				this.level = 0;
			}
			this.level++;
			this.Notify_LevelChanged(false);
		}

		// Token: 0x0600D039 RID: 53305 RVA: 0x004C8918 File Offset: 0x004C6B18
		public void Hypnotize(Pawn pawn, Pawn instigator, int ticks)
		{
			Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_Hypnotized, instigator));
			this.hypnotisedPawns[pawn] = GenTicks.TicksGame + ticks;
		}

		// Token: 0x0600D03A RID: 53306 RVA: 0x004C8943 File Offset: 0x004C6B43
		public void EndHypnotize(Pawn pawn)
		{
			this.hypnotisedPawns.Remove(pawn);
		}

		// Token: 0x0600D03B RID: 53307 RVA: 0x004C8952 File Offset: 0x004C6B52
		public bool IsPawnHypnotized(Pawn pawn)
		{
			return this.hypnotisedPawns.ContainsKey(pawn);
		}

		// Token: 0x0600D03C RID: 53308 RVA: 0x004C8960 File Offset: 0x004C6B60
		public void SetLevel(MonolithLevelDef levelDef, bool silent = false)
		{
			int num = this.level;
			this.level = levelDef.level;
			if (num != this.level)
			{
				this.Notify_LevelChanged(silent);
			}
		}

		// Token: 0x0600D03D RID: 53309 RVA: 0x004C8984 File Offset: 0x004C6B84
		private void Notify_LevelChanged(bool silent = false)
		{
			if (!ModLister.CheckAnomaly("Monolith level"))
			{
				return;
			}
			this.highestLevelReached = Mathf.Max(this.highestLevelReached, this.level);
			this.lastLevelChangeTick = Find.TickManager.TicksGame;
			this.levelDef = DefDatabase<MonolithLevelDef>.AllDefs.FirstOrDefault((MonolithLevelDef x) => x.level == this.level);
			Building_VoidMonolith building_VoidMonolith = this.monolith;
			if (building_VoidMonolith != null)
			{
				building_VoidMonolith.SetLevel(this.levelDef);
			}
			Find.ResearchManager.Notify_MonolithLevelChanged(this.level);
			if (!silent)
			{
				if (!this.levelDef.incidentsOnReached.NullOrEmpty<IncidentDef>())
				{
					this.fireLevelIncidentTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + GameComponent_Anomaly.MonolithLevelIncidentDelayRangeHours.RandomInRange * 2500f);
				}
				if (this.Level > 0)
				{
					Find.CameraDriver.shaker.DoShake(0.05f, 300);
				}
				if (this.LevelDef.triggersGrayPall)
				{
					this.TriggerGrayPall();
				}
				if (this.LevelDef == MonolithLevelDefOf.VoidAwakened)
				{
					this.TriggerVoidAwakening();
				}
			}
			Find.SignalManager.SendSignal(new Signal("MonolithLevelChanged", true));
			if (this.level == 1)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.StudyingEntities, OpportunityType.Important);
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.EntityCodex, OpportunityType.GoodToKnow);
				IncidentParms parms = new IncidentParms
				{
					target = this.monolith.Map,
					points = StorytellerUtility.DefaultThreatPointsNow(this.monolith.Map),
					forced = true
				};
				Find.Storyteller.incidentQueue.Add(IncidentDefOf.VoidCuriosity, Find.TickManager.TicksGame + Mathf.RoundToInt(GameComponent_Anomaly.VoidCuriosityIncidentDelayRangeDays.RandomInRange * 60000f), parms, 0);
			}
		}

		// Token: 0x0600D03E RID: 53310 RVA: 0x004C8B37 File Offset: 0x004C6D37
		private void TriggerGrayPall()
		{
			this.fireGrayPallTick = GenTicks.TicksGame + 300;
		}

		// Token: 0x0600D03F RID: 53311 RVA: 0x004C8B4C File Offset: 0x004C6D4C
		private void TriggerVoidAwakening()
		{
			Slate slate = new Slate();
			slate.Set<Map>("map", this.monolith.Map, false);
			QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.EndGame_VoidAwakening, slate);
		}

		// Token: 0x0600D040 RID: 53312 RVA: 0x004C8B84 File Offset: 0x004C6D84
		public bool TryGetCellForMonolithSpawn(Map map, out IntVec3 cell, bool allowFogged = false)
		{
			LargeBuildingSpawnParms largeBuildingSpawnParms = this.MonolithSpawnParms.ForThing(ThingDefOf.VoidMonolith);
			LargeBuildingSpawnParms parms = largeBuildingSpawnParms;
			parms.minDistanceToColonyBuilding = 1f;
			largeBuildingSpawnParms.allowFogged = allowFogged;
			parms.allowFogged = allowFogged;
			return LargeBuildingCellFinder.TryFindCell(out cell, map, largeBuildingSpawnParms, null, (IntVec3 c) => ScattererValidator_AvoidSpecialThings.IsValid(c, map), false) || LargeBuildingCellFinder.TryFindCell(out cell, map, parms, null, (IntVec3 c) => ScattererValidator_AvoidSpecialThings.IsValid(c, map), true);
		}

		// Token: 0x0600D041 RID: 53313 RVA: 0x004C8C10 File Offset: 0x004C6E10
		public Building_VoidMonolith SpawnNewMonolith(IntVec3 cell, Map map)
		{
			this.monolith = (ThingMaker.MakeThing(ThingDefOf.VoidMonolith, null) as Building_VoidMonolith);
			this.levelDef = DefDatabase<MonolithLevelDef>.AllDefs.FirstOrDefault((MonolithLevelDef x) => x.level == this.level);
			GenSpawn.Spawn(this.monolith, cell, map, WipeMode.Vanish);
			EffecterDefOf.VoidStructureSpawningSkipSequence.SpawnMaintained(this.monolith, map, 1f);
			if (this.Level > 0)
			{
				this.monolith.CheckAndGenerateQuest();
			}
			return this.monolith;
		}

		// Token: 0x0600D042 RID: 53314 RVA: 0x004C8C8F File Offset: 0x004C6E8F
		public void ResetMonolith()
		{
			this.SetLevel(MonolithLevelDefOf.Inactive, false);
			Building_VoidMonolith building_VoidMonolith = this.monolith;
			if (building_VoidMonolith != null)
			{
				building_VoidMonolith.Reset();
			}
			this.fireLevelIncidentTick = -99999;
			this.lastLevelChangeTick = -99999;
		}

		// Token: 0x0600D043 RID: 53315 RVA: 0x004C8CC4 File Offset: 0x004C6EC4
		public bool VoidAwakeningActive()
		{
			foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
			{
				if (!quest.Historical && quest.root == QuestScriptDefOf.EndGame_VoidAwakening)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D044 RID: 53316 RVA: 0x004C8D30 File Offset: 0x004C6F30
		public bool PawnHasUnnaturalCorpse(Pawn pawn)
		{
			return this.corpseTrackers.ContainsKey(pawn);
		}

		// Token: 0x0600D045 RID: 53317 RVA: 0x004C8D3E File Offset: 0x004C6F3E
		public bool TryGetUnnaturalCorpseTrackerForHaunted(Pawn pawn, out UnnaturalCorpseTracker tracker)
		{
			return this.corpseTrackers.TryGetValue(pawn, out tracker);
		}

		// Token: 0x0600D046 RID: 53318 RVA: 0x004C8D50 File Offset: 0x004C6F50
		public bool TryGetUnnaturalCorpseTrackerForAwoken(Pawn pawn, out UnnaturalCorpseTracker tracker)
		{
			foreach (KeyValuePair<Pawn, UnnaturalCorpseTracker> keyValuePair in this.corpseTrackers)
			{
				Pawn pawn2;
				UnnaturalCorpseTracker unnaturalCorpseTracker;
				keyValuePair.Deconstruct(out pawn2, out unnaturalCorpseTracker);
				UnnaturalCorpseTracker unnaturalCorpseTracker2 = unnaturalCorpseTracker;
				if (unnaturalCorpseTracker2.AwokenPawn == pawn)
				{
					tracker = unnaturalCorpseTracker2;
					return true;
				}
			}
			tracker = null;
			return false;
		}

		// Token: 0x0600D047 RID: 53319 RVA: 0x004C8DC4 File Offset: 0x004C6FC4
		public bool HasActiveAwokenCorpse()
		{
			foreach (KeyValuePair<Pawn, UnnaturalCorpseTracker> keyValuePair in this.corpseTrackers)
			{
				Pawn pawn;
				UnnaturalCorpseTracker unnaturalCorpseTracker;
				keyValuePair.Deconstruct(out pawn, out unnaturalCorpseTracker);
				UnnaturalCorpseTracker unnaturalCorpseTracker2 = unnaturalCorpseTracker;
				if (!unnaturalCorpseTracker2.AwokenPawn.DestroyedOrNull() && unnaturalCorpseTracker2.AwokenPawn.Spawned && !unnaturalCorpseTracker2.Haunted.DestroyedOrNull() && !unnaturalCorpseTracker2.Haunted.Dead && unnaturalCorpseTracker2.Haunted.MapHeld == unnaturalCorpseTracker2.AwokenPawn.Map)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D048 RID: 53320 RVA: 0x004C8E74 File Offset: 0x004C7074
		public void RegisterUnnaturalCorpse(Pawn pawn, UnnaturalCorpse corpse)
		{
			if (this.corpseTrackers.ContainsKey(pawn))
			{
				Log.Error("Attempted to register a pawn (" + pawn.LabelShort + ") which already has an unnatural corpse");
				return;
			}
			this.corpseTrackers[pawn] = new UnnaturalCorpseTracker(pawn, corpse);
			Find.EntityCodex.SetDiscovered(EntityCodexEntryDefOf.UnnaturalCorpse, null, null);
		}

		// Token: 0x0600D049 RID: 53321 RVA: 0x004C8ECE File Offset: 0x004C70CE
		public void DevRemoveUnnaturalCorpse(Pawn pawn)
		{
			if (this.corpseTrackers.ContainsKey(pawn))
			{
				this.corpseTrackers[pawn].Notify_Finished();
				this.corpseTrackers.Remove(pawn);
			}
		}

		// Token: 0x0600D04A RID: 53322 RVA: 0x004C8EFC File Offset: 0x004C70FC
		public void Notify_MapRemoved(Map map)
		{
			if (this.monolith == null || this.monolith.Map != map)
			{
				return;
			}
			if (this.monolith.quest != null && !this.monolith.quest.Historical)
			{
				this.monolith.quest.End(QuestEndOutcome.Unknown, true, true);
			}
			if (this.levelDef == MonolithLevelDefOf.VoidAwakened || this.levelDef == MonolithLevelDefOf.Gleaming)
			{
				this.SetLevel(MonolithLevelDefOf.Waking, true);
			}
			foreach (Pawn pawn in this.metalHellPawns)
			{
				pawn.Destroy(DestroyMode.Vanish);
			}
			this.metalHellPawns.Clear();
			this.metalHellReturnTick = -99999;
			this.monolith = null;
			this.fireLevelIncidentTick = -99999;
		}

		// Token: 0x0600D04B RID: 53323 RVA: 0x004C8FE4 File Offset: 0x004C71E4
		public void Notify_PawnDied(Pawn pawn)
		{
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (this.corpseTrackers.TryGetValue(pawn, out unnaturalCorpseTracker))
			{
				unnaturalCorpseTracker.Notify_PawnDied();
			}
		}

		// Token: 0x0600D04C RID: 53324 RVA: 0x004C9008 File Offset: 0x004C7208
		public void Notify_PawnKilledViaAwoken(Pawn pawn)
		{
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (this.corpseTrackers.TryGetValue(pawn, out unnaturalCorpseTracker))
			{
				unnaturalCorpseTracker.Notify_PawnKilledViaAwoken();
			}
		}

		// Token: 0x0600D04D RID: 53325 RVA: 0x004C902C File Offset: 0x004C722C
		private void TrySpawnHarbingerTrees()
		{
			IncidentParms parms = new IncidentParms
			{
				target = Find.AnyPlayerHomeMap,
				forced = true
			};
			Find.Storyteller.incidentQueue.Add(IncidentDefOf.HarbingerTreeSpawn, Find.TickManager.TicksGame, parms, 0);
		}

		// Token: 0x0600D04E RID: 53326 RVA: 0x004C9074 File Offset: 0x004C7274
		public void Notify_HarbingerTreeSpawned()
		{
			this.nextHarbingerTreeCheckTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + GameComponent_Anomaly.HarbingerTreeSpawnIntervalDays.RandomInRange * 60000f + GameComponent_Anomaly.HarbingerTreeCheckIntervalHours.RandomInRange * 2500f);
		}

		// Token: 0x0600D04F RID: 53327 RVA: 0x004C90C0 File Offset: 0x004C72C0
		public void Notify_HarbingerTreeDied()
		{
			this.nextHarbingerTreeCheckTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + 480000f + GameComponent_Anomaly.HarbingerTreeCheckIntervalHours.RandomInRange * 2500f);
		}

		// Token: 0x0600D050 RID: 53328 RVA: 0x004C90FD File Offset: 0x004C72FD
		public override void AppendDebugString(StringBuilder sb)
		{
			sb.AppendLine("  " + base.GetType().Name + ":");
			sb.AppendLine("    level:  " + this.level.ToString());
		}

		// Token: 0x0600D051 RID: 53329 RVA: 0x004C913C File Offset: 0x004C733C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.ElevateLooseCorpseTrackers();
				foreach (KeyValuePair<Pawn, int> keyValuePair in (from kvp in this.hypnotisedPawns
				where kvp.Key.DestroyedOrNull() || kvp.Key.Dead
				select kvp).ToList<KeyValuePair<Pawn, int>>())
				{
					Pawn pawn;
					int num;
					keyValuePair.Deconstruct(out pawn, out num);
					Pawn key = pawn;
					this.hypnotisedPawns.Remove(key);
				}
			}
			Scribe_Values.Look<int>(ref this.level, "level", 0, false);
			Scribe_Values.Look<int>(ref this.highestLevelReached, "highestLevelReached", 0, false);
			Scribe_Defs.Look<MonolithLevelDef>(ref this.levelDef, "levelDef");
			Scribe_References.Look<Building_VoidMonolith>(ref this.monolith, "monolith", false);
			Scribe_Values.Look<int>(ref this.nextHarbingerTreeCheckTick, "nextHarbingerTreeCheckTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastLevelChangeTick, "lastLevelChangeTick", 0, false);
			Scribe_Values.Look<bool>(ref this.hasSeenGrayFlesh, "hasSeenGrayFlesh", false, false);
			Scribe_Values.Look<bool>(ref this.hasPerformedVoidProvocation, "hasPerformedVoidProvocation", false, false);
			Scribe_Values.Look<int>(ref this.lastLevelActivationLetterSent, "lastLevelActivationLetterSent", -1, false);
			Scribe_References.Look<Pawn>(ref this.voidNodeActivator, "voidNodeActivator", false);
			Scribe_Collections.Look<Pawn>(ref this.metalHellPawns, "metalHellPawns", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.metalHellReturnTick, "metalHellReturnTick", 0, false);
			Scribe_Values.Look<string>(ref this.metalHellReturnLetterText, "metalHellReturnLetterText", null, false);
			Scribe_Values.Look<int>(ref this.fireGrayPallTick, "fireGrayPallTick", 0, false);
			Scribe_Values.Look<int>(ref this.newestMetalhorrorBiosignatureTick, "newestMetalhorrorBiosignatureTick", 0, false);
			Scribe_Values.Look<bool>(ref this.hasBuiltHoldingPlatform, "hasBuiltHoldingPlatform", false, false);
			Scribe_Collections.Look<Pawn, UnnaturalCorpseTracker>(ref this.corpseTrackers, "corpses", LookMode.Reference, LookMode.Deep, ref this.workingPawnList, ref this.workingTrackerList, true, false, false);
			Scribe_Collections.Look<UnnaturalCorpseTracker>(ref this.looseCorpseTrackers, "looseCorpseTrackers", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn, int>(ref this.hypnotisedPawns, "hypnotisedPawns", LookMode.Reference, LookMode.Value, ref this.workingHypnotizedList, ref this.workingHypnotizedTickList, true, false, false);
			Scribe_Collections.Look<int>(ref this.emergedBiosignatures, "emergedBiosignatures", LookMode.Value);
			Scribe_Collections.Look<ChoiceLetter>(ref this.monolithLetters, "monolithLetters", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.monolithStudyProgress, "monolithStudyProgress", 0, false);
			Scribe_Values.Look<int>(ref this.monolithNextIndex, "monolithNextIndex", 0, false);
			Scribe_Values.Look<float>(ref this.monolithAnomalyKnowledge, "monolithAnomalyKnowledge", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.looseCorpseTrackers == null)
			{
				this.looseCorpseTrackers = new List<UnnaturalCorpseTracker>();
			}
		}

		// Token: 0x0600D052 RID: 53330 RVA: 0x004C93D4 File Offset: 0x004C75D4
		private void ElevateLooseCorpseTrackers()
		{
			foreach (KeyValuePair<Pawn, UnnaturalCorpseTracker> keyValuePair in this.corpseTrackers)
			{
				Pawn pawn;
				UnnaturalCorpseTracker unnaturalCorpseTracker;
				keyValuePair.Deconstruct(out pawn, out unnaturalCorpseTracker);
				Pawn pawn2 = pawn;
				if (pawn2.Dead || pawn2.DestroyedOrNull())
				{
					GameComponent_Anomaly.toRemove.Add(pawn2);
				}
			}
			foreach (Pawn key in GameComponent_Anomaly.toRemove)
			{
				this.looseCorpseTrackers.Add(this.corpseTrackers[key]);
				this.corpseTrackers.Remove(key);
			}
			GameComponent_Anomaly.toRemove.Clear();
		}

		// Token: 0x0400846F RID: 33903
		public const string MonolithLevelChangedSignal = "MonolithLevelChanged";

		// Token: 0x04008470 RID: 33904
		private const int GrayPallDelayTicks = 300;

		// Token: 0x04008471 RID: 33905
		private const int LevelChangeScreenShakeDuration = 300;

		// Token: 0x04008472 RID: 33906
		private const int MinTimeBetweenNewMetalhorrorBiosignatureImplants = 1800000;

		// Token: 0x04008473 RID: 33907
		private const int MonolithStudyNoteLetters = 3;

		// Token: 0x04008474 RID: 33908
		private const float LevelChangeScreenShakeMagnitude = 0.05f;

		// Token: 0x04008475 RID: 33909
		private const float HarbingerTreeRespawnIntervalDays = 8f;

		// Token: 0x04008476 RID: 33910
		public const int PostEndGameReliefPeriod = 300000;

		// Token: 0x04008477 RID: 33911
		private static readonly FloatRange MonolithLevelIncidentDelayRangeHours = new FloatRange(12f, 36f);

		// Token: 0x04008478 RID: 33912
		private static readonly FloatRange HarbingerTreeCheckIntervalHours = new FloatRange(6f, 18f);

		// Token: 0x04008479 RID: 33913
		private static readonly FloatRange HarbingerTreeSpawnIntervalDays = new FloatRange(2f, 5f);

		// Token: 0x0400847A RID: 33914
		private static readonly FloatRange GrayPallConditionDaysRange = new FloatRange(1f, 3f);

		// Token: 0x0400847B RID: 33915
		private static readonly FloatRange VoidCuriosityIncidentDelayRangeDays = new FloatRange(15f, 20f);

		// Token: 0x0400847C RID: 33916
		private int level;

		// Token: 0x0400847D RID: 33917
		private int highestLevelReached;

		// Token: 0x0400847E RID: 33918
		private MonolithLevelDef levelDef;

		// Token: 0x0400847F RID: 33919
		public Building_VoidMonolith monolith;

		// Token: 0x04008480 RID: 33920
		private int fireLevelIncidentTick = -99999;

		// Token: 0x04008481 RID: 33921
		private int nextHarbingerTreeCheckTick = -99999;

		// Token: 0x04008482 RID: 33922
		private int lastLevelChangeTick = -99999;

		// Token: 0x04008483 RID: 33923
		private int fireGrayPallTick = -99999;

		// Token: 0x04008484 RID: 33924
		private int newestMetalhorrorBiosignatureTick = -1800000;

		// Token: 0x04008485 RID: 33925
		public bool hasBuiltHoldingPlatform;

		// Token: 0x04008486 RID: 33926
		public bool hasSeenGrayFlesh;

		// Token: 0x04008487 RID: 33927
		private Dictionary<Pawn, UnnaturalCorpseTracker> corpseTrackers = new Dictionary<Pawn, UnnaturalCorpseTracker>();

		// Token: 0x04008488 RID: 33928
		private List<UnnaturalCorpseTracker> looseCorpseTrackers = new List<UnnaturalCorpseTracker>();

		// Token: 0x04008489 RID: 33929
		private Dictionary<Pawn, int> hypnotisedPawns = new Dictionary<Pawn, int>();

		// Token: 0x0400848A RID: 33930
		public HashSet<int> emergedBiosignatures = new HashSet<int>();

		// Token: 0x0400848B RID: 33931
		public bool hasPerformedVoidProvocation;

		// Token: 0x0400848C RID: 33932
		public int lastLevelActivationLetterSent = -1;

		// Token: 0x0400848D RID: 33933
		private List<ChoiceLetter> monolithLetters = new List<ChoiceLetter>();

		// Token: 0x0400848E RID: 33934
		private int monolithNextIndex;

		// Token: 0x0400848F RID: 33935
		private int monolithStudyProgress;

		// Token: 0x04008490 RID: 33936
		public float monolithAnomalyKnowledge;

		// Token: 0x04008491 RID: 33937
		public Pawn voidNodeActivator;

		// Token: 0x04008492 RID: 33938
		public List<Pawn> metalHellPawns = new List<Pawn>();

		// Token: 0x04008493 RID: 33939
		public int metalHellReturnTick = -99999;

		// Token: 0x04008494 RID: 33940
		public int metalHellClosedTick = -99999;

		// Token: 0x04008495 RID: 33941
		public string metalHellReturnLetterText;

		// Token: 0x04008496 RID: 33942
		private LargeBuildingSpawnParms monolithSpawnParms;

		// Token: 0x04008497 RID: 33943
		private static readonly List<Pawn> toRemove = new List<Pawn>();

		// Token: 0x04008498 RID: 33944
		private static readonly List<UnnaturalCorpseTracker> tmpTrackers = new List<UnnaturalCorpseTracker>();

		// Token: 0x04008499 RID: 33945
		private List<Pawn> workingPawnList;

		// Token: 0x0400849A RID: 33946
		private List<UnnaturalCorpseTracker> workingTrackerList;

		// Token: 0x0400849B RID: 33947
		private List<Pawn> workingHypnotizedList;

		// Token: 0x0400849C RID: 33948
		private List<int> workingHypnotizedTickList;
	}
}
