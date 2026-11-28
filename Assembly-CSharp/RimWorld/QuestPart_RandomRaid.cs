using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111E RID: 4382
	public class QuestPart_RandomRaid : QuestPart
	{
		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x06006D97 RID: 28055 RVA: 0x002665B0 File Offset: 0x002647B0
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_RandomRaid.<get_QuestLookTargets>d__15 <get_QuestLookTargets>d__ = new QuestPart_RandomRaid.<get_QuestLookTargets>d__15(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x06006D98 RID: 28056 RVA: 0x002665C0 File Offset: 0x002647C0
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_RandomRaid.<get_InvolvedFactions>d__17 <get_InvolvedFactions>d__ = new QuestPart_RandomRaid.<get_InvolvedFactions>d__17(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006D99 RID: 28057 RVA: 0x002665D0 File Offset: 0x002647D0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.fallbackToPlayerHomeMap && (this.mapParent == null || !this.mapParent.HasMap || !this.quest.IsParentSuitableForQuest(this.mapParent)))
			{
				MapParent mapParent = this.quest.TryFindNewSuitableMapParentForRetarget() ?? Find.AnyPlayerHomeMap.Parent;
				if (mapParent != null)
				{
					this.mapParent = mapParent;
				}
			}
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.forced = true;
				incidentParms.quest = this.quest;
				incidentParms.target = this.mapParent.Map;
				incidentParms.points = (this.useCurrentThreatPoints ? (StorytellerUtility.DefaultThreatPointsNow(this.mapParent.Map) * this.currentThreatPointsFactor) : this.pointsRange.RandomInRange);
				incidentParms.faction = this.faction;
				incidentParms.customLetterLabel = signal.args.GetFormattedText(this.customLetterLabel);
				incidentParms.customLetterText = signal.args.GetFormattedText(this.customLetterText).Resolve();
				incidentParms.attackTargets = this.attackTargets;
				incidentParms.generateFightersOnly = this.generateFightersOnly;
				incidentParms.sendLetter = this.sendLetter;
				if (this.arrivalMode != null)
				{
					incidentParms.raidArrivalMode = this.arrivalMode;
				}
				IncidentDef incidentDef;
				if (this.faction == null || this.faction.HostileTo(Faction.OfPlayer))
				{
					incidentDef = IncidentDefOf.RaidEnemy;
				}
				else
				{
					incidentDef = IncidentDefOf.RaidFriendly;
				}
				if (this.raidStrategy != null)
				{
					incidentParms.raidStrategy = this.raidStrategy;
				}
				if (this.faction != null)
				{
					incidentParms.points = Mathf.Max(incidentParms.points, this.faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null));
				}
				if (incidentDef.Worker.CanFireNow(incidentParms))
				{
					incidentDef.Worker.TryExecute(incidentParms);
				}
			}
		}

		// Token: 0x06006D9A RID: 28058 RVA: 0x002667D8 File Offset: 0x002649D8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<FloatRange>(ref this.pointsRange, "pointsRange", default(FloatRange), false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.useCurrentThreatPoints, "useCurrentThreatPoints", false, false);
			Scribe_Values.Look<float>(ref this.currentThreatPointsFactor, "currentThreatPointsFactor", 1f, false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<RaidStrategyDef>(ref this.raidStrategy, "raidStrategy");
			Scribe_Collections.Look<Thing>(ref this.attackTargets, "attackTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.generateFightersOnly, "generateFightersOnly", false, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", true, false);
			Scribe_Values.Look<bool>(ref this.fallbackToPlayerHomeMap, "fallbackToPlayerHomeMap", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.attackTargets != null)
			{
				this.attackTargets.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x06006D9B RID: 28059 RVA: 0x0026692C File Offset: 0x00264B2C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.pointsRange = new FloatRange(500f, 1500f);
			}
		}

		// Token: 0x040045D4 RID: 17876
		public string inSignal;

		// Token: 0x040045D5 RID: 17877
		public MapParent mapParent;

		// Token: 0x040045D6 RID: 17878
		public FloatRange pointsRange;

		// Token: 0x040045D7 RID: 17879
		public Faction faction;

		// Token: 0x040045D8 RID: 17880
		public bool useCurrentThreatPoints;

		// Token: 0x040045D9 RID: 17881
		public float currentThreatPointsFactor = 1f;

		// Token: 0x040045DA RID: 17882
		public PawnsArrivalModeDef arrivalMode;

		// Token: 0x040045DB RID: 17883
		public RaidStrategyDef raidStrategy;

		// Token: 0x040045DC RID: 17884
		public string customLetterLabel;

		// Token: 0x040045DD RID: 17885
		public string customLetterText;

		// Token: 0x040045DE RID: 17886
		public List<Thing> attackTargets;

		// Token: 0x040045DF RID: 17887
		public bool generateFightersOnly;

		// Token: 0x040045E0 RID: 17888
		public bool fallbackToPlayerHomeMap;

		// Token: 0x040045E1 RID: 17889
		public bool sendLetter = true;
	}
}
