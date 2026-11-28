using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C4 RID: 4548
	public class Storyteller : IExposable
	{
		// Token: 0x1700133C RID: 4924
		// (get) Token: 0x06007190 RID: 29072 RVA: 0x0027CC12 File Offset: 0x0027AE12
		[Obsolete("Use \"difficulty\" instead.")]
		public Difficulty difficultyValues
		{
			get
			{
				return this.difficulty;
			}
		}

		// Token: 0x1700133D RID: 4925
		// (get) Token: 0x06007191 RID: 29073 RVA: 0x0027CC1A File Offset: 0x0027AE1A
		public int LastIncidentTick
		{
			get
			{
				return this.lastIncidentTick;
			}
		}

		// Token: 0x1700133E RID: 4926
		// (get) Token: 0x06007192 RID: 29074 RVA: 0x0027CC24 File Offset: 0x0027AE24
		public List<IIncidentTarget> AllIncidentTargets
		{
			get
			{
				Storyteller.tmpAllIncidentTargets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Storyteller.tmpAllIncidentTargets.Add(maps[i]);
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					if (caravans[j].IsPlayerControlled)
					{
						Storyteller.tmpAllIncidentTargets.Add(caravans[j]);
					}
				}
				Storyteller.tmpAllIncidentTargets.Add(Find.World);
				return Storyteller.tmpAllIncidentTargets;
			}
		}

		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x06007193 RID: 29075 RVA: 0x0027CCB2 File Offset: 0x0027AEB2
		public static List<IncidentDef> AnomalyIncidents
		{
			get
			{
				List<IncidentDef> result;
				if ((result = Storyteller.anomalyIncidents) == null)
				{
					result = (Storyteller.anomalyIncidents = (from incident in DefDatabase<IncidentDef>.AllDefsListForReading
					where incident.IsAnomalyIncident
					select incident).ToList<IncidentDef>());
				}
				return result;
			}
		}

		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x06007194 RID: 29076 RVA: 0x0027CCF1 File Offset: 0x0027AEF1
		public static List<IncidentDef> NonAnomalyIncidents
		{
			get
			{
				List<IncidentDef> result;
				if ((result = Storyteller.nonAnomalyIncidents) == null)
				{
					result = (Storyteller.nonAnomalyIncidents = (from incident in DefDatabase<IncidentDef>.AllDefsListForReading
					where !incident.IsAnomalyIncident
					select incident).ToList<IncidentDef>());
				}
				return result;
			}
		}

		// Token: 0x17001341 RID: 4929
		// (get) Token: 0x06007195 RID: 29077 RVA: 0x0027CD30 File Offset: 0x0027AF30
		public float AnomalyIncidentChanceNow
		{
			get
			{
				return Mathf.Clamp01(Find.Anomaly.AnomalyThreatFractionNow);
			}
		}

		// Token: 0x17001342 RID: 4930
		// (get) Token: 0x06007196 RID: 29078 RVA: 0x0027CD44 File Offset: 0x0027AF44
		public float QuestGiverTraderChanceNow
		{
			get
			{
				if (Find.TickManager.TicksGame - this.lastQuestGiverTraderTick < 600000)
				{
					return 0f;
				}
				if (Find.TickManager.TicksGame - this.lastQuestGiverTraderTick > 3600000)
				{
					return 1f;
				}
				return 0.15f;
			}
		}

		// Token: 0x06007197 RID: 29079 RVA: 0x0027CD92 File Offset: 0x0027AF92
		public static void StorytellerStaticUpdate()
		{
			Storyteller.tmpAllIncidentTargets.Clear();
		}

		// Token: 0x06007198 RID: 29080 RVA: 0x0027CDA0 File Offset: 0x0027AFA0
		public Storyteller()
		{
		}

		// Token: 0x06007199 RID: 29081 RVA: 0x0027CE0B File Offset: 0x0027B00B
		public Storyteller(StorytellerDef def, DifficultyDef difficultyDef) : this(def, difficultyDef, new Difficulty(difficultyDef))
		{
		}

		// Token: 0x0600719A RID: 29082 RVA: 0x0027CE1C File Offset: 0x0027B01C
		public Storyteller(StorytellerDef def, DifficultyDef difficultyDef, Difficulty difficulty)
		{
			this.def = def;
			this.difficultyDef = difficultyDef;
			this.difficulty = difficulty;
			this.InitializeStorytellerComps();
		}

		// Token: 0x0600719B RID: 29083 RVA: 0x0027CEA4 File Offset: 0x0027B0A4
		private void InitializeStorytellerComps()
		{
			this.storytellerComps = new List<StorytellerComp>();
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				if (this.def.comps[i].Enabled)
				{
					StorytellerComp storytellerComp = (StorytellerComp)Activator.CreateInstance(this.def.comps[i].compClass);
					storytellerComp.props = this.def.comps[i];
					storytellerComp.Initialize();
					this.storytellerComps.Add(storytellerComp);
				}
			}
		}

		// Token: 0x0600719C RID: 29084 RVA: 0x0027CF3C File Offset: 0x0027B13C
		public void ExposeData()
		{
			Scribe_Defs.Look<StorytellerDef>(ref this.def, "def");
			Scribe_Defs.Look<DifficultyDef>(ref this.difficultyDef, "difficulty");
			Scribe_Deep.Look<IncidentQueue>(ref this.incidentQueue, "incidentQueue", Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.recentIncidentsAnomaly, "recentIncidentsAnomaly", LookMode.Value);
			Scribe_Values.Look<float>(ref this.recentAnomalyIncidentFactor, "recentAnomalyIncidentFactor", 0.5f, false);
			Scribe_Values.Look<int>(ref this.lastQuestGiverTraderTick, "lastQuestGiverTraderTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastIncidentTick, "lastIncidentTick", 0, false);
			if (this.difficultyDef == null)
			{
				Log.Error("Loaded storyteller without difficulty");
				this.difficultyDef = DifficultyDefOf.Rough;
			}
			if (this.difficultyDef.isCustom)
			{
				Scribe_Deep.Look<Difficulty>(ref this.difficulty, "customDifficulty", new object[]
				{
					this.difficultyDef
				});
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.difficulty = new Difficulty(this.difficultyDef);
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.InitializeStorytellerComps();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.Saving)
			{
				this.SetupNonCustomDifficultyAnomalySettings();
			}
		}

		// Token: 0x0600719D RID: 29085 RVA: 0x0027D054 File Offset: 0x0027B254
		private void SetupNonCustomDifficultyAnomalySettings()
		{
			if (!ModsConfig.AnomalyActive || this.difficultyDef.isCustom)
			{
				return;
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Scribe_Values.Look<float>(ref this.difficulty.anomalyThreatsInactiveFraction, "anomalyThreatsInactiveFraction", 0.08f, false);
				Scribe_Values.Look<float>(ref this.difficulty.anomalyThreatsActiveFraction, "anomalyThreatsActiveFraction", 0.3f, false);
				Scribe_Values.Look<float?>(ref this.difficulty.overrideAnomalyThreatsFraction, "overrideAnomalyThreatsFraction", null, false);
				Scribe_Values.Look<float>(ref this.difficulty.studyEfficiencyFactor, "studyEfficiencyFactor", 1f, false);
				Scribe_Defs.Look<AnomalyPlaystyleDef>(ref this.anomalyPlaystyleDef, "anomalyPlaystyleDef");
				this.difficulty.AnomalyPlaystyleDef = this.anomalyPlaystyleDef;
				return;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.anomalyThreatsInactiveFraction = this.difficulty.anomalyThreatsInactiveFraction;
				this.anomalyThreatsActiveFraction = this.difficulty.anomalyThreatsActiveFraction;
				this.overrideAnomalyThreatsFraction = this.difficulty.overrideAnomalyThreatsFraction;
				this.studyEfficiencyFactor = this.difficulty.studyEfficiencyFactor;
				this.anomalyPlaystyleDef = this.difficulty.AnomalyPlaystyleDef;
				Scribe_Values.Look<float>(ref this.anomalyThreatsInactiveFraction, "anomalyThreatsInactiveFraction", 0.08f, false);
				Scribe_Values.Look<float>(ref this.anomalyThreatsActiveFraction, "anomalyThreatsActiveFraction", 0.3f, false);
				Scribe_Values.Look<float?>(ref this.overrideAnomalyThreatsFraction, "overrideAnomalyThreatsFraction", null, false);
				Scribe_Values.Look<float>(ref this.studyEfficiencyFactor, "studyEfficiencyFactor", 1f, false);
				Scribe_Defs.Look<AnomalyPlaystyleDef>(ref this.anomalyPlaystyleDef, "anomalyPlaystyleDef");
			}
		}

		// Token: 0x0600719E RID: 29086 RVA: 0x0027D1E0 File Offset: 0x0027B3E0
		public void StorytellerTick()
		{
			this.incidentQueue.IncidentQueueTick();
			if (Find.TickManager.TicksGame % 1000 == 0)
			{
				if (!DebugSettings.enableStoryteller)
				{
					return;
				}
				foreach (FiringIncident fi in this.MakeIncidentsForInterval())
				{
					this.TryFire(fi, false);
				}
			}
		}

		// Token: 0x0600719F RID: 29087 RVA: 0x0027D254 File Offset: 0x0027B454
		public bool TryFire(FiringIncident fi, bool queued = false)
		{
			if (fi.def.Worker.CanFireNow(fi.parms) && fi.def.Worker.TryExecute(fi.parms))
			{
				fi.parms.target.StoryState.Notify_IncidentFired(fi);
				this.lastIncidentTick = GenTicks.TicksGame;
				return true;
			}
			return false;
		}

		// Token: 0x060071A0 RID: 29088 RVA: 0x0027D2B5 File Offset: 0x0027B4B5
		public IEnumerable<FiringIncident> MakeIncidentsForInterval()
		{
			Storyteller.<MakeIncidentsForInterval>d__48 <MakeIncidentsForInterval>d__ = new Storyteller.<MakeIncidentsForInterval>d__48(-2);
			<MakeIncidentsForInterval>d__.<>4__this = this;
			return <MakeIncidentsForInterval>d__;
		}

		// Token: 0x060071A1 RID: 29089 RVA: 0x0027D2C5 File Offset: 0x0027B4C5
		public IEnumerable<FiringIncident> MakeIncidentsForInterval(StorytellerComp comp, List<IIncidentTarget> targets)
		{
			Storyteller.<MakeIncidentsForInterval>d__49 <MakeIncidentsForInterval>d__ = new Storyteller.<MakeIncidentsForInterval>d__49(-2);
			<MakeIncidentsForInterval>d__.<>3__comp = comp;
			<MakeIncidentsForInterval>d__.<>3__targets = targets;
			return <MakeIncidentsForInterval>d__;
		}

		// Token: 0x060071A2 RID: 29090 RVA: 0x0027D2DC File Offset: 0x0027B4DC
		public void Notify_PawnEvent(Pawn pawn, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			Find.StoryWatcher.watcherAdaptation.Notify_PawnEvent(pawn, ev, dinfo);
			for (int i = 0; i < this.storytellerComps.Count; i++)
			{
				this.storytellerComps[i].Notify_PawnEvent(pawn, ev, dinfo);
			}
		}

		// Token: 0x060071A3 RID: 29091 RVA: 0x0027D328 File Offset: 0x0027B528
		public void Notify_DissolutionEvent(Thing thing)
		{
			for (int i = 0; i < this.storytellerComps.Count; i++)
			{
				this.storytellerComps[i].Notify_DissolutionEvent(thing);
			}
		}

		// Token: 0x060071A4 RID: 29092 RVA: 0x0027D35D File Offset: 0x0027B55D
		public void Notify_QuestGiverTraderSpawned()
		{
			this.lastQuestGiverTraderTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060071A5 RID: 29093 RVA: 0x0027D36F File Offset: 0x0027B56F
		public void Notify_DefChanged()
		{
			this.InitializeStorytellerComps();
		}

		// Token: 0x060071A6 RID: 29094 RVA: 0x0027D378 File Offset: 0x0027B578
		public void RecordIncidentFired(IncidentDef incident)
		{
			if (!ModsConfig.AnomalyActive || !incident.category.canUseAnomalyChance)
			{
				return;
			}
			if (this.recentIncidentsAnomaly == null)
			{
				this.recentIncidentsAnomaly = new Queue<bool>();
			}
			if (this.recentIncidentsAnomaly.Count >= 4)
			{
				this.recentIncidentsAnomaly.Dequeue();
			}
			this.recentIncidentsAnomaly.Enqueue(incident.IsAnomalyIncident);
			this.recentAnomalyIncidentFactor = this.recentIncidentsAnomaly.Average(delegate(bool b)
			{
				if (!b)
				{
					return 0f;
				}
				return 1f;
			});
		}

		// Token: 0x060071A7 RID: 29095 RVA: 0x0027D408 File Offset: 0x0027B608
		public static void ResetStaticData()
		{
			Storyteller.anomalyIncidents = null;
			Storyteller.nonAnomalyIncidents = null;
		}

		// Token: 0x060071A8 RID: 29096 RVA: 0x0027D418 File Offset: 0x0027B618
		public string DebugString()
		{
			if (Time.frameCount % 60 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("GLOBAL STORYTELLER STATS");
				stringBuilder.AppendLine("------------------------");
				stringBuilder.AppendLine("Storyteller: ".PadRight(40) + this.def.label);
				stringBuilder.AppendLine("Adaptation days: ".PadRight(40) + Find.StoryWatcher.watcherAdaptation.AdaptDays.ToString("F1"));
				stringBuilder.AppendLine("Adapt points factor: ".PadRight(40) + Find.StoryWatcher.watcherAdaptation.TotalThreatPointsFactor.ToString("F2"));
				stringBuilder.AppendLine("Time points factor: ".PadRight(40) + Find.Storyteller.def.pointsFactorFromDaysPassed.Evaluate((float)GenDate.DaysPassedSinceSettle).ToString("F2"));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Ally incident fraction (neutral or ally): ".PadRight(40) + StorytellerUtility.AllyIncidentFraction(false).ToString("F2"));
				stringBuilder.AppendLine("Ally incident fraction (ally only): ".PadRight(40) + StorytellerUtility.AllyIncidentFraction(true).ToString("F2"));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(StorytellerUtilityPopulation.DebugReadout().TrimEndNewlines());
				IIncidentTarget incidentTarget = (Find.WorldSelector.SingleSelectedObject as IIncidentTarget) ?? Find.CurrentMap;
				if (incidentTarget != null)
				{
					Map map = incidentTarget as Map;
					stringBuilder.AppendLine();
					StringBuilder stringBuilder2 = stringBuilder;
					string str = "STATS FOR INCIDENT TARGET: ";
					IIncidentTarget incidentTarget2 = incidentTarget;
					stringBuilder2.AppendLine(str + ((incidentTarget2 != null) ? incidentTarget2.ToString() : null));
					stringBuilder.AppendLine("------------------------");
					stringBuilder.AppendLine("Progress score: ".PadRight(40) + StorytellerUtility.GetProgressScore(incidentTarget).ToString("F2"));
					stringBuilder.AppendLine("Base points: ".PadRight(40) + StorytellerUtility.DefaultThreatPointsNow(incidentTarget).ToString("F0"));
					stringBuilder.AppendLine("Points factor random range: ".PadRight(40) + incidentTarget.IncidentPointsRandomFactorRange.ToString());
					stringBuilder.AppendLine("Wealth: ".PadRight(40) + incidentTarget.PlayerWealthForStoryteller.ToString("F0"));
					if (map != null)
					{
						if (Find.Storyteller.difficulty.fixedWealthMode)
						{
							StringBuilder stringBuilder3 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
							defaultInterpolatedStringHandler.AppendLiteral("- Wealth calculated using fixed model curve, time factor: ");
							defaultInterpolatedStringHandler.AppendFormatted<float>(Find.Storyteller.difficulty.fixedWealthTimeFactor, "F1");
							stringBuilder3.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
							stringBuilder.AppendLine("- Map age: ".PadRight(40) + map.AgeInDays.ToString("F1"));
						}
						stringBuilder.AppendLine("- Items: ".PadRight(40) + map.wealthWatcher.WealthItems.ToString("F0"));
						stringBuilder.AppendLine("- Buildings: ".PadRight(40) + map.wealthWatcher.WealthBuildings.ToString("F0"));
						stringBuilder.AppendLine("- Floors: ".PadRight(40) + map.wealthWatcher.WealthFloorsOnly.ToString("F0"));
						stringBuilder.AppendLine("- Pawns: ".PadRight(40) + map.wealthWatcher.WealthPawns.ToString("F0"));
					}
					stringBuilder.AppendLine("Pawn count human: ".PadRight(40) + incidentTarget.PlayerPawnsForStoryteller.Count((Pawn p) => p.def.race.Humanlike).ToString());
					stringBuilder.AppendLine("Pawn count animal: ".PadRight(40) + incidentTarget.PlayerPawnsForStoryteller.Count((Pawn p) => p.IsAnimal).ToString());
					if (map != null)
					{
						stringBuilder.AppendLine("StoryDanger: ".PadRight(40) + map.dangerWatcher.DangerRating.ToString());
						stringBuilder.AppendLine("FireDanger: ".PadRight(40) + map.fireWatcher.FireDanger.ToString("F2"));
						stringBuilder.AppendLine("LastThreatBigTick days ago: ".PadRight(40) + (Find.TickManager.TicksGame - map.storyState.LastThreatBigTick).ToStringTicksToDays("F1"));
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("LIST OF ALL INCIDENT TARGETS");
				stringBuilder.AppendLine("------------------------");
				for (int i = 0; i < this.AllIncidentTargets.Count; i++)
				{
					StringBuilder stringBuilder4 = stringBuilder;
					string str2 = i.ToString();
					string str3 = ". ";
					IIncidentTarget incidentTarget3 = this.AllIncidentTargets[i];
					stringBuilder4.AppendLine(str2 + str3 + ((incidentTarget3 != null) ? incidentTarget3.ToString() : null));
				}
				this.debugStringCached = stringBuilder.ToString();
			}
			return this.debugStringCached;
		}

		// Token: 0x040047D4 RID: 18388
		public StorytellerDef def;

		// Token: 0x040047D5 RID: 18389
		public DifficultyDef difficultyDef;

		// Token: 0x040047D6 RID: 18390
		public Difficulty difficulty = new Difficulty();

		// Token: 0x040047D7 RID: 18391
		public List<StorytellerComp> storytellerComps;

		// Token: 0x040047D8 RID: 18392
		private static List<IncidentDef> anomalyIncidents;

		// Token: 0x040047D9 RID: 18393
		private static List<IncidentDef> nonAnomalyIncidents;

		// Token: 0x040047DA RID: 18394
		public IncidentQueue incidentQueue = new IncidentQueue();

		// Token: 0x040047DB RID: 18395
		private Queue<bool> recentIncidentsAnomaly = new Queue<bool>();

		// Token: 0x040047DC RID: 18396
		private float recentAnomalyIncidentFactor = 0.5f;

		// Token: 0x040047DD RID: 18397
		private int lastQuestGiverTraderTick;

		// Token: 0x040047DE RID: 18398
		private int lastIncidentTick;

		// Token: 0x040047DF RID: 18399
		private float anomalyThreatsInactiveFraction = 0.08f;

		// Token: 0x040047E0 RID: 18400
		private float anomalyThreatsActiveFraction = 0.3f;

		// Token: 0x040047E1 RID: 18401
		private float? overrideAnomalyThreatsFraction;

		// Token: 0x040047E2 RID: 18402
		private float studyEfficiencyFactor = 1f;

		// Token: 0x040047E3 RID: 18403
		private AnomalyPlaystyleDef anomalyPlaystyleDef;

		// Token: 0x040047E4 RID: 18404
		public static readonly Vector2 PortraitSizeTiny = new Vector2(122f, 130f);

		// Token: 0x040047E5 RID: 18405
		public static readonly Vector2 PortraitSizeLarge = new Vector2(580f, 620f);

		// Token: 0x040047E6 RID: 18406
		public const int IntervalsPerDay = 60;

		// Token: 0x040047E7 RID: 18407
		public const int CheckInterval = 1000;

		// Token: 0x040047E8 RID: 18408
		private const int RecentAnomalyIncidentsToRecord = 4;

		// Token: 0x040047E9 RID: 18409
		private const float QuestGiverTraderChance = 0.15f;

		// Token: 0x040047EA RID: 18410
		private const int QuestGiverTraderMinIntervalTicks = 600000;

		// Token: 0x040047EB RID: 18411
		private const int QuestGiverTraderMaxIntervalTicks = 3600000;

		// Token: 0x040047EC RID: 18412
		private static readonly List<IIncidentTarget> tmpAllIncidentTargets = new List<IIncidentTarget>();

		// Token: 0x040047ED RID: 18413
		private string debugStringCached = "Generating data...";
	}
}
