using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023BD RID: 9149
	[StaticConstructorOnStartup]
	public class Site : MapParent
	{
		// Token: 0x17002823 RID: 10275
		// (get) Token: 0x0600E40E RID: 58382 RVA: 0x0054C0E0 File Offset: 0x0054A2E0
		public override string Label
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel;
				}
				if (this.MainSitePartDef == SitePartDefOf.PreciousLump && this.MainSitePart.parms.preciousLumpResources != null)
				{
					return "PreciousLumpLabel".Translate(this.MainSitePart.parms.preciousLumpResources.label);
				}
				return this.MainSitePartDef.label;
			}
		}

		// Token: 0x17002824 RID: 10276
		// (get) Token: 0x0600E40F RID: 58383 RVA: 0x0054C155 File Offset: 0x0054A355
		public override Texture2D ExpandingIcon
		{
			get
			{
				return this.MainSitePartDef.ExpandingIconTexture;
			}
		}

		// Token: 0x17002825 RID: 10277
		// (get) Token: 0x0600E410 RID: 58384 RVA: 0x0054C162 File Offset: 0x0054A362
		public override bool HandlesConditionCausers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002826 RID: 10278
		// (get) Token: 0x0600E411 RID: 58385 RVA: 0x0054C168 File Offset: 0x0054A368
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (this.MainSitePartDef.applyFactionColorToSiteTexture && base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.MainSitePartDef.siteTexture, ShaderDatabase.WorldOverlayTransparentLit, color, 3550);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x17002827 RID: 10279
		// (get) Token: 0x0600E412 RID: 58386 RVA: 0x0054C1D3 File Offset: 0x0054A3D3
		public override bool AppendFactionToInspectString
		{
			get
			{
				return this.MainSitePartDef.applyFactionColorToSiteTexture || this.MainSitePartDef.showFactionInInspectString;
			}
		}

		// Token: 0x17002828 RID: 10280
		// (get) Token: 0x0600E413 RID: 58387 RVA: 0x0054C1F0 File Offset: 0x0054A3F0
		private SitePart MainSitePart
		{
			get
			{
				if (!this.parts.Any<SitePart>())
				{
					Log.ErrorOnce("Site without any SitePart at " + base.Tile.ToString(), this.ID ^ 93890909);
					return null;
				}
				if (this.parts[0].hidden)
				{
					Log.ErrorOnce("Site with first SitePart hidden at " + base.Tile.ToString(), this.ID ^ 48471239);
					return this.parts[0];
				}
				return this.parts[0];
			}
		}

		// Token: 0x17002829 RID: 10281
		// (get) Token: 0x0600E414 RID: 58388 RVA: 0x0054C296 File Offset: 0x0054A496
		public SitePartDef MainSitePartDef
		{
			get
			{
				return this.MainSitePart.def;
			}
		}

		// Token: 0x1700282A RID: 10282
		// (get) Token: 0x0600E415 RID: 58389 RVA: 0x0054C2A3 File Offset: 0x0054A4A3
		public override bool GravShipCanLandOn
		{
			get
			{
				return this.MainSitePartDef.gravShipsCanLandOn && !base.HasMap;
			}
		}

		// Token: 0x1700282B RID: 10283
		// (get) Token: 0x0600E416 RID: 58390 RVA: 0x0054C2BD File Offset: 0x0054A4BD
		public override IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				Site.<get_ExtraGenStepDefs>d__29 <get_ExtraGenStepDefs>d__ = new Site.<get_ExtraGenStepDefs>d__29(-2);
				<get_ExtraGenStepDefs>d__.<>4__this = this;
				return <get_ExtraGenStepDefs>d__;
			}
		}

		// Token: 0x1700282C RID: 10284
		// (get) Token: 0x0600E417 RID: 58391 RVA: 0x0054C2D0 File Offset: 0x0054A4D0
		public string ApproachOrderString
		{
			get
			{
				return this.MainSitePartDef.approachOrderString.NullOrEmpty() ? "ApproachSite".Translate(this.Label) : this.MainSitePartDef.approachOrderString.Formatted(this.Label);
			}
		}

		// Token: 0x1700282D RID: 10285
		// (get) Token: 0x0600E418 RID: 58392 RVA: 0x0054C328 File Offset: 0x0054A528
		public string ApproachingReportString
		{
			get
			{
				return this.MainSitePartDef.approachingReportString.NullOrEmpty() ? "ApproachingSite".Translate(this.Label) : this.MainSitePartDef.approachingReportString.Formatted(this.Label);
			}
		}

		// Token: 0x1700282E RID: 10286
		// (get) Token: 0x0600E419 RID: 58393 RVA: 0x0054C380 File Offset: 0x0054A580
		public float ActualThreatPoints
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.parts.Count; i++)
				{
					num += this.parts[i].parms.threatPoints;
				}
				return num;
			}
		}

		// Token: 0x1700282F RID: 10287
		// (get) Token: 0x0600E41A RID: 58394 RVA: 0x0054C3C4 File Offset: 0x0054A5C4
		public bool IncreasesPopulation
		{
			get
			{
				if (base.HasMap)
				{
					return false;
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].def.Worker.IncreasesPopulation(this.parts[i].parms))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002830 RID: 10288
		// (get) Token: 0x0600E41B RID: 58395 RVA: 0x0054C424 File Offset: 0x0054A624
		public bool BadEvenIfNoMap
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].def.badEvenIfNoMap)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002831 RID: 10289
		// (get) Token: 0x0600E41C RID: 58396 RVA: 0x0054C462 File Offset: 0x0054A662
		public bool HasWorldObjectTimeout
		{
			get
			{
				return this.WorldObjectTimeoutTicksLeft != -1;
			}
		}

		// Token: 0x17002832 RID: 10290
		// (get) Token: 0x0600E41D RID: 58397 RVA: 0x0054C470 File Offset: 0x0054A670
		public int WorldObjectTimeoutTicksLeft
		{
			get
			{
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					Quest quest = questsListForReading[i];
					if (quest.State == QuestState.Ongoing)
					{
						for (int j = 0; j < quest.PartsListForReading.Count; j++)
						{
							QuestPart_WorldObjectTimeout questPart_WorldObjectTimeout = quest.PartsListForReading[j] as QuestPart_WorldObjectTimeout;
							if (questPart_WorldObjectTimeout != null && questPart_WorldObjectTimeout.State == QuestPartState.Enabled && questPart_WorldObjectTimeout.worldObject == this)
							{
								return questPart_WorldObjectTimeout.TicksLeft;
							}
						}
					}
				}
				return -1;
			}
		}

		// Token: 0x17002833 RID: 10291
		// (get) Token: 0x0600E41E RID: 58398 RVA: 0x0054C4F4 File Offset: 0x0054A6F4
		public IntVec3 PreferredMapSize
		{
			get
			{
				IntVec3 defaultMapSize = Site.DefaultMapSize;
				for (int i = 0; i < this.parts.Count; i++)
				{
					SitePart sitePart = this.parts[i];
					if (sitePart.def.minMapSize != null)
					{
						IntVec3 value = sitePart.def.minMapSize.Value;
						defaultMapSize.x = Mathf.Max(value.x, defaultMapSize.x);
						defaultMapSize.y = Mathf.Max(value.y, defaultMapSize.y);
						defaultMapSize.z = Mathf.Max(value.z, defaultMapSize.z);
					}
				}
				return defaultMapSize;
			}
		}

		// Token: 0x17002834 RID: 10292
		// (get) Token: 0x0600E41F RID: 58399 RVA: 0x0054C59C File Offset: 0x0054A79C
		public override AcceptanceReport CanBeSettled
		{
			get
			{
				foreach (Quest quest in Find.QuestManager.QuestsListForReading)
				{
					if (quest.State == QuestState.Ongoing)
					{
						foreach (QuestPart questPart in quest.PartsListForReading)
						{
							QuestPart_SpawnWorldObject questPart_SpawnWorldObject = questPart as QuestPart_SpawnWorldObject;
							if (questPart_SpawnWorldObject != null && questPart_SpawnWorldObject.worldObject == this)
							{
								return false;
							}
						}
					}
				}
				return true;
			}
		}

		// Token: 0x0600E420 RID: 58400 RVA: 0x0054C654 File Offset: 0x0054A854
		public override void Destroy()
		{
			base.Destroy();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostDestroy(this.parts[i]);
			}
			for (int j = 0; j < this.parts.Count; j++)
			{
				this.parts[j].PostDestroy();
			}
		}

		// Token: 0x0600E421 RID: 58401 RVA: 0x0054C6CC File Offset: 0x0054A8CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
			Scribe_Deep.Look<SiteCoreBackCompat>(ref this.coreBackCompat, "core", Array.Empty<object>());
			Scribe_Collections.Look<SitePart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.anyEnemiesInitially, "anyEnemiesInitially", false, false);
			Scribe_Values.Look<bool>(ref this.caravanAssaultSuccessfulTaleRecorded, "caravanAssaultSuccessfulTaleRecorded", false, false);
			Scribe_Values.Look<bool>(ref this.allEnemiesDefeatedSignalSent, "allEnemiesDefeatedSignalSent", false, false);
			Scribe_Values.Look<bool>(ref this.factionMustRemainHostile, "factionMustRemainHostile", false, false);
			Scribe_Values.Look<float>(ref this.desiredThreatPoints, "desiredThreatPoints", 0f, false);
			Scribe_Values.Look<bool>(ref this.preventGravshipLanding, "preventGravshipLanding", false, false);
			Scribe_Values.Look<bool>(ref this.firedNoActiveThreatsSignal, "firedNoActiveThreatsSignal", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.coreBackCompat != null && this.coreBackCompat.def != null)
				{
					this.parts.Insert(0, new SitePart(this, this.coreBackCompat.def, this.coreBackCompat.parms));
					this.coreBackCompat = null;
				}
				if (this.parts.RemoveAll((SitePart x) => x == null || x.def == null) != 0)
				{
					Log.Error("Some site parts were null after loading.");
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					this.parts[i].site = this;
				}
				BackCompatibility.PostExposeData(this);
			}
		}

		// Token: 0x0600E422 RID: 58402 RVA: 0x0054C854 File Offset: 0x0054AA54
		public void AddPart(SitePart part)
		{
			if (!part.def.forceMutators.NullOrEmpty<TileMutatorDef>())
			{
				foreach (TileMutatorDef mutator in part.def.forceMutators)
				{
					base.Tile.Tile.AddMutator(mutator);
				}
			}
			this.parts.Add(part);
			part.def.Worker.Init(this, part);
		}

		// Token: 0x0600E423 RID: 58403 RVA: 0x0054C8EC File Offset: 0x0054AAEC
		protected override void Tick()
		{
			base.Tick();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.SitePartWorkerTick(this.parts[i]);
			}
			if (base.HasMap && !this.firedNoActiveThreatsSignal && !GenHostility.AnyHostileActiveThreatToPlayer(base.Map, true, !this.CanReformFoggedEnemies))
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "NoActiveThreats");
				this.firedNoActiveThreatsSignal = true;
			}
		}

		// Token: 0x0600E424 RID: 58404 RVA: 0x0054C97C File Offset: 0x0054AB7C
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].SitePartTickInterval(delta);
			}
			if (base.HasMap)
			{
				this.CheckRecordAssaultSuccessfulTale();
				this.CheckAllEnemiesDefeated();
			}
		}

		// Token: 0x0600E425 RID: 58405 RVA: 0x0054C9CC File Offset: 0x0054ABCC
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			Map map = base.Map;
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostMapGenerate(map);
			}
			float num = 0f;
			for (int j = 0; j < this.parts.Count; j++)
			{
				num = Mathf.Max(num, this.parts[j].def.forceExitAndRemoveMapCountdownDurationDays);
			}
			num *= MapParentTuning.SiteDetectionCountdownMultiplier.RandomInRange;
			if (!this.parts.Any((SitePart p) => p.def.disallowsAutomaticDetectionTimerStart) && !map.wasSpawnedViaGravShipLanding)
			{
				int ticks = Mathf.RoundToInt(num * 60000f);
				base.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(ticks, -1);
			}
			this.allEnemiesDefeatedSignalSent = false;
		}

		// Token: 0x0600E426 RID: 58406 RVA: 0x0054CABC File Offset: 0x0054ACBC
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostDrawExtraSelectionOverlays(this.parts[i]);
			}
		}

		// Token: 0x0600E427 RID: 58407 RVA: 0x0054CB0C File Offset: 0x0054AD0C
		public override void Notify_MyMapAboutToBeRemoved()
		{
			base.Notify_MyMapAboutToBeRemoved();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.Notify_SiteMapAboutToBeRemoved(this.parts[i]);
			}
		}

		// Token: 0x0600E428 RID: 58408 RVA: 0x0054CB5C File Offset: 0x0054AD5C
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps.ToList<PocketMapParent>())
			{
				if (pocketMapParent.sourceMap == base.Map && pocketMapParent.Map.mapPawns.AnyPawnBlockingMapRemoval)
				{
					return false;
				}
			}
			if (base.Map.AnyBuildingBlockingMapRemoval)
			{
				return false;
			}
			alsoRemoveWorldObject = !this.parts.Any((SitePart x) => (x.def.Worker is SitePartWorker_ConditionCauser && x.conditionCauser != null && !x.conditionCauser.Destroyed) || (x.def.Worker is SitePartWorker_RaidSource && GenHostility.AnyHostileActiveThreatToPlayer(base.Map, true, false)));
			if (this.parts.Any(delegate(SitePart x)
			{
				SitePartWorker_AncientAltar sitePartWorker_AncientAltar = x.def.Worker as SitePartWorker_AncientAltar;
				return sitePartWorker_AncientAltar != null && sitePartWorker_AncientAltar.ShouldKeepMapForRelic(x);
			}))
			{
				alsoRemoveWorldObject = false;
			}
			return !TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map);
		}

		// Token: 0x0600E429 RID: 58409 RVA: 0x0054CC5C File Offset: 0x0054AE5C
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			for (int i = 0; i < this.parts.Count; i++)
			{
				outChildren.Add(this.parts[i]);
			}
		}

		// Token: 0x0600E42A RID: 58410 RVA: 0x0054CC98 File Offset: 0x0054AE98
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			Site.<GetFloatMenuOptions>d__59 <GetFloatMenuOptions>d__ = new Site.<GetFloatMenuOptions>d__59(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__caravan = caravan;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600E42B RID: 58411 RVA: 0x0054CCAF File Offset: 0x0054AEAF
		public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			Site.<GetTransportersFloatMenuOptions>d__60 <GetTransportersFloatMenuOptions>d__ = new Site.<GetTransportersFloatMenuOptions>d__60(-2);
			<GetTransportersFloatMenuOptions>d__.<>4__this = this;
			<GetTransportersFloatMenuOptions>d__.<>3__pods = pods;
			<GetTransportersFloatMenuOptions>d__.<>3__launchAction = launchAction;
			return <GetTransportersFloatMenuOptions>d__;
		}

		// Token: 0x0600E42C RID: 58412 RVA: 0x0054CCCD File Offset: 0x0054AECD
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			Site.<GetShuttleFloatMenuOptions>d__61 <GetShuttleFloatMenuOptions>d__ = new Site.<GetShuttleFloatMenuOptions>d__61(-2);
			<GetShuttleFloatMenuOptions>d__.<>4__this = this;
			<GetShuttleFloatMenuOptions>d__.<>3__pods = pods;
			<GetShuttleFloatMenuOptions>d__.<>3__launchAction = launchAction;
			return <GetShuttleFloatMenuOptions>d__;
		}

		// Token: 0x0600E42D RID: 58413 RVA: 0x0054CCEB File Offset: 0x0054AEEB
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Site.<GetGizmos>d__62 <GetGizmos>d__ = new Site.<GetGizmos>d__62(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600E42E RID: 58414 RVA: 0x0054CCFC File Offset: 0x0054AEFC
		private void CheckRecordAssaultSuccessfulTale()
		{
			if (this.anyEnemiesInitially && !this.caravanAssaultSuccessfulTaleRecorded && !GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false, false))
			{
				this.caravanAssaultSuccessfulTaleRecorded = true;
				if (base.Map.mapPawns.FreeColonists.Any<Pawn>())
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, new object[]
					{
						base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
					});
				}
			}
		}

		// Token: 0x0600E42F RID: 58415 RVA: 0x0054CD70 File Offset: 0x0054AF70
		private void CheckAllEnemiesDefeated()
		{
			if (this.allEnemiesDefeatedSignalSent || !base.HasMap || GenHostility.AnyHostileActiveThreatToPlayer(base.Map, true, true))
			{
				return;
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated", this.Named("SUBJECT"));
			this.allEnemiesDefeatedSignalSent = true;
		}

		// Token: 0x0600E430 RID: 58416 RVA: 0x0054CDC0 File Offset: 0x0054AFC0
		public override bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			Site site = other as Site;
			return site != null && site.MainSitePartDef == this.MainSitePartDef;
		}

		// Token: 0x0600E431 RID: 58417 RVA: 0x0054CDE8 File Offset: 0x0054AFE8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			Site.tmpSitePartsLabels.Clear();
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (!this.parts[i].hidden && this.parts[i].def.displayOnInspectPane)
				{
					if (this.MainSitePart == this.parts[i] && !this.parts[i].def.mainPartAllThreatsLabel.NullOrEmpty() && this.ActualThreatPoints > 0f)
					{
						stringBuilder.Length = 0;
						stringBuilder.Append(this.parts[i].def.mainPartAllThreatsLabel.CapitalizeFirst());
						break;
					}
					string postProcessedThreatLabel = this.parts[i].def.Worker.GetPostProcessedThreatLabel(this, this.parts[i]);
					if (!postProcessedThreatLabel.NullOrEmpty())
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(postProcessedThreatLabel.CapitalizeFirst());
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600E432 RID: 58418 RVA: 0x0054CF1C File Offset: 0x0054B11C
		public override string GetDescription()
		{
			string text = this.MainSitePartDef.description;
			string description = base.GetDescription();
			if (!description.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += description;
			}
			return text;
		}

		// Token: 0x04009127 RID: 37159
		public string customLabel;

		// Token: 0x04009128 RID: 37160
		public List<SitePart> parts = new List<SitePart>();

		// Token: 0x04009129 RID: 37161
		public bool sitePartsKnown = true;

		// Token: 0x0400912A RID: 37162
		public bool factionMustRemainHostile;

		// Token: 0x0400912B RID: 37163
		public float desiredThreatPoints;

		// Token: 0x0400912C RID: 37164
		private SiteCoreBackCompat coreBackCompat;

		// Token: 0x0400912D RID: 37165
		private bool anyEnemiesInitially;

		// Token: 0x0400912E RID: 37166
		private bool caravanAssaultSuccessfulTaleRecorded;

		// Token: 0x0400912F RID: 37167
		private bool allEnemiesDefeatedSignalSent;

		// Token: 0x04009130 RID: 37168
		public bool preventGravshipLanding;

		// Token: 0x04009131 RID: 37169
		public bool firedNoActiveThreatsSignal;

		// Token: 0x04009132 RID: 37170
		private Material cachedMat;

		// Token: 0x04009133 RID: 37171
		private static readonly IntVec3 DefaultMapSize = new IntVec3(200, 1, 200);

		// Token: 0x04009134 RID: 37172
		private static List<string> tmpSitePartsLabels = new List<string>();
	}
}
