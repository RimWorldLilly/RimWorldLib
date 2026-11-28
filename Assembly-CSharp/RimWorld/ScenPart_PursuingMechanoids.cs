using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B7 RID: 6071
	public class ScenPart_PursuingMechanoids : ScenPart
	{
		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x06009751 RID: 38737 RVA: 0x0036E830 File Offset: 0x0036CA30
		private Alert_MechThreat AlertCached
		{
			get
			{
				if (this.Disabled)
				{
					return null;
				}
				if (this.cachedAlertMap != Find.CurrentMap)
				{
					this.alertCached = null;
				}
				if (this.alertCached != null && Find.TickManager.TicksGame > this.TimerIntervalTick(this.alertCached.raidTick + 30000))
				{
					this.alertCached = null;
				}
				if (this.alertCached != null)
				{
					return this.alertCached;
				}
				int timer;
				int num;
				if (this.mapWarningTimers.TryGetValue(Find.CurrentMap, out timer) && Find.TickManager.TicksGame > this.TimerIntervalTick(timer) && this.mapRaidTimers.TryGetValue(Find.CurrentMap, out num) && Find.TickManager.TicksGame < this.TimerIntervalTick(num + 30000))
				{
					this.alertCached = new Alert_MechThreat
					{
						raidTick = this.mapRaidTimers[Find.CurrentMap]
					};
					this.cachedAlertMap = Find.CurrentMap;
				}
				return this.alertCached;
			}
		}

		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x06009752 RID: 38738 RVA: 0x0036E922 File Offset: 0x0036CB22
		private bool Disabled
		{
			get
			{
				return this.questCompleted;
			}
		}

		// Token: 0x170019B7 RID: 6583
		// (get) Token: 0x06009753 RID: 38739 RVA: 0x0036E92A File Offset: 0x0036CB2A
		public override bool OverrideDangerMusic
		{
			get
			{
				return this.onStartMap;
			}
		}

		// Token: 0x06009754 RID: 38740 RVA: 0x0036E934 File Offset: 0x0036CB34
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				foreach (Map map in this.mapWarningTimers.Keys.ToList<Map>())
				{
					if (((map != null) ? map.Parent : null) == null || map.Parent.Destroyed)
					{
						this.mapWarningTimers.Remove(map);
						this.mapRaidTimers.Remove(map);
					}
				}
			}
			Scribe_Values.Look<bool>(ref this.onStartMap, "initialMap", false, false);
			Scribe_Collections.Look<Map, int>(ref this.mapWarningTimers, "mapWarningTimers", LookMode.Reference, LookMode.Value, ref this.tmpWarningKeys, ref this.tmpWarningValues, true, false, false);
			Scribe_Collections.Look<Map, int>(ref this.mapRaidTimers, "mapRaidTimers", LookMode.Reference, LookMode.Value, ref this.tmpRaidKeys, ref this.tmpRaidValues, true, false, false);
			Scribe_Values.Look<bool>(ref this.questCompleted, "questCompleted", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.mapWarningTimers == null)
				{
					this.mapWarningTimers = new Dictionary<Map, int>();
				}
				if (this.mapRaidTimers == null)
				{
					this.mapRaidTimers = new Dictionary<Map, int>();
				}
			}
		}

		// Token: 0x06009755 RID: 38741 RVA: 0x0036EA64 File Offset: 0x0036CC64
		public override void PostWorldGenerate()
		{
			this.onStartMap = true;
			this.mapWarningTimers.Clear();
			this.mapRaidTimers.Clear();
		}

		// Token: 0x06009756 RID: 38742 RVA: 0x0036EA83 File Offset: 0x0036CC83
		public override void PostMapGenerate(Map map)
		{
			if (this.onStartMap)
			{
				this.StartTimers(map);
				this.onStartMap = false;
			}
		}

		// Token: 0x06009757 RID: 38743 RVA: 0x0036EA9B File Offset: 0x0036CC9B
		public override void PostGravshipLanded(Map map)
		{
			this.onStartMap = false;
			this.StartTimers(map);
		}

		// Token: 0x06009758 RID: 38744 RVA: 0x0036EAAB File Offset: 0x0036CCAB
		public override void MapRemoved(Map map)
		{
			if (this.mapWarningTimers.Remove(map))
			{
				this.mapRaidTimers.Remove(map);
				this.onStartMap = false;
			}
		}

		// Token: 0x06009759 RID: 38745 RVA: 0x0036EACF File Offset: 0x0036CCCF
		private int TimerIntervalTick(int timer)
		{
			return (timer + 2500 - 1) / 2500 * 2500;
		}

		// Token: 0x0600975A RID: 38746 RVA: 0x0036EAE8 File Offset: 0x0036CCE8
		public override void Tick()
		{
			if (Find.TickManager.TicksGame % 2500 != 0)
			{
				return;
			}
			this.tmpMaps.Clear();
			this.tmpMaps.AddRange(this.mapWarningTimers.Keys);
			foreach (Map map in this.tmpMaps)
			{
				if (this.Disabled || GravshipUtility.GetPlayerGravEngine_NewTemp(map) == null)
				{
					this.mapWarningTimers.Remove(map);
					this.mapRaidTimers.Remove(map);
				}
				else
				{
					if (Find.TickManager.TicksGame == this.TimerIntervalTick(this.mapWarningTimers[map]))
					{
						Thing t = map.listerThings.ThingsOfDef(ThingDefOf.PilotConsole).FirstOrDefault<Thing>();
						Find.LetterStack.ReceiveLetter("LetterLabelMechanoidThreat".Translate(), "LetterTextMechanoidThreat".Translate(), LetterDefOf.ThreatSmall, t, null, null, null, null, 0, true);
					}
					if (Find.TickManager.TicksGame == this.TimerIntervalTick(this.mapRaidTimers[map]))
					{
						this.FireRaid_NewTemp(map, 1.5f, 2000f);
					}
					if (Find.TickManager.TicksGame == this.TimerIntervalTick(this.mapRaidTimers[map] + 30000))
					{
						this.FireRaid_NewTemp(map, 2f, 8000f);
					}
				}
			}
		}

		// Token: 0x0600975B RID: 38747 RVA: 0x0036EC78 File Offset: 0x0036CE78
		private void StartTimers(Map map)
		{
			if (map.generatorDef == MapGeneratorDefOf.Mechhive)
			{
				return;
			}
			if (this.onStartMap)
			{
				this.mapWarningTimers[map] = Find.TickManager.TicksGame + 2700;
				this.mapRaidTimers[map] = Find.TickManager.TicksGame + 30000;
				return;
			}
			this.mapWarningTimers[map] = Find.TickManager.TicksGame + ScenPart_PursuingMechanoids.WarningDelayRange.RandomInRange;
			this.mapRaidTimers[map] = Find.TickManager.TicksGame + ScenPart_PursuingMechanoids.RaidDelayRange.RandomInRange;
		}

		// Token: 0x0600975C RID: 38748 RVA: 0x0036ED16 File Offset: 0x0036CF16
		public void Notify_QuestCompleted()
		{
			this.questCompleted = true;
		}

		// Token: 0x0600975D RID: 38749 RVA: 0x0036ED1F File Offset: 0x0036CF1F
		private void FireRaid(Map map)
		{
			this.FireRaid_NewTemp(map, 1.5f, 5000f);
		}

		// Token: 0x0600975E RID: 38750 RVA: 0x0036ED34 File Offset: 0x0036CF34
		private void FireRaid_NewTemp(Map map, float pointsMultiplier, float minPoints)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = map;
			incidentParms.points = Mathf.Max(minPoints, StorytellerUtility.DefaultThreatPointsNow(map) * pointsMultiplier);
			incidentParms.faction = Faction.OfMechanoids;
			incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.RandomDrop;
			incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
		}

		// Token: 0x0600975F RID: 38751 RVA: 0x0036ED9B File Offset: 0x0036CF9B
		public override IEnumerable<Alert> GetAlerts()
		{
			ScenPart_PursuingMechanoids.<GetAlerts>d__34 <GetAlerts>d__ = new ScenPart_PursuingMechanoids.<GetAlerts>d__34(-2);
			<GetAlerts>d__.<>4__this = this;
			return <GetAlerts>d__;
		}

		// Token: 0x04005AA3 RID: 23203
		private bool onStartMap = true;

		// Token: 0x04005AA4 RID: 23204
		private Dictionary<Map, int> mapWarningTimers = new Dictionary<Map, int>();

		// Token: 0x04005AA5 RID: 23205
		private Dictionary<Map, int> mapRaidTimers = new Dictionary<Map, int>();

		// Token: 0x04005AA6 RID: 23206
		private bool questCompleted;

		// Token: 0x04005AA7 RID: 23207
		private const int InitialWarningDelay = 2700;

		// Token: 0x04005AA8 RID: 23208
		private const int InitialRaidDelay = 30000;

		// Token: 0x04005AA9 RID: 23209
		private static IntRange WarningDelayRange = new IntRange(840000, 960000);

		// Token: 0x04005AAA RID: 23210
		private static IntRange RaidDelayRange = new IntRange(1080000, 2100000);

		// Token: 0x04005AAB RID: 23211
		private const int TickInterval = 2500;

		// Token: 0x04005AAC RID: 23212
		private const int SecondRaidDelay = 30000;

		// Token: 0x04005AAD RID: 23213
		private Map cachedAlertMap;

		// Token: 0x04005AAE RID: 23214
		private Alert_MechThreat alertCached;

		// Token: 0x04005AAF RID: 23215
		private List<Map> tmpWarningKeys;

		// Token: 0x04005AB0 RID: 23216
		private List<int> tmpWarningValues;

		// Token: 0x04005AB1 RID: 23217
		private List<Map> tmpRaidKeys;

		// Token: 0x04005AB2 RID: 23218
		private List<int> tmpRaidValues;

		// Token: 0x04005AB3 RID: 23219
		private List<Map> tmpMaps = new List<Map>();
	}
}
