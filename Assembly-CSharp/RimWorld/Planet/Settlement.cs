using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023B7 RID: 9143
	[StaticConstructorOnStartup]
	public class Settlement : MapParent, ITrader, ITraderRestockingInfoProvider, INameableWorldObject
	{
		// Token: 0x17002801 RID: 10241
		// (get) Token: 0x0600E3B4 RID: 58292 RVA: 0x0054AA1E File Offset: 0x00548C1E
		// (set) Token: 0x0600E3B5 RID: 58293 RVA: 0x0054AA26 File Offset: 0x00548C26
		public string Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x17002802 RID: 10242
		// (get) Token: 0x0600E3B6 RID: 58294 RVA: 0x0054AA2F File Offset: 0x00548C2F
		public override Texture2D ExpandingIcon
		{
			get
			{
				return base.Faction.def.FactionIcon;
			}
		}

		// Token: 0x17002803 RID: 10243
		// (get) Token: 0x0600E3B7 RID: 58295 RVA: 0x0054AA41 File Offset: 0x00548C41
		public override string Label
		{
			get
			{
				return this.nameInt ?? base.Label;
			}
		}

		// Token: 0x17002804 RID: 10244
		// (get) Token: 0x0600E3B8 RID: 58296 RVA: 0x0054AA53 File Offset: 0x00548C53
		public override bool HasName
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x17002805 RID: 10245
		// (get) Token: 0x0600E3B9 RID: 58297 RVA: 0x0054AA63 File Offset: 0x00548C63
		protected override bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return !this.Attackable;
			}
		}

		// Token: 0x17002806 RID: 10246
		// (get) Token: 0x0600E3BA RID: 58298 RVA: 0x0054AA70 File Offset: 0x00548C70
		public virtual bool Visitable
		{
			get
			{
				return base.Faction != Faction.OfPlayer && (base.Faction == null || !base.Faction.HostileTo(Faction.OfPlayer)) && !base.Tile.LayerDef.isSpace;
			}
		}

		// Token: 0x17002807 RID: 10247
		// (get) Token: 0x0600E3BB RID: 58299 RVA: 0x0054AABC File Offset: 0x00548CBC
		public virtual bool Attackable
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x17002808 RID: 10248
		// (get) Token: 0x0600E3BC RID: 58300 RVA: 0x0054AACE File Offset: 0x00548CCE
		public override bool ShowRelatedQuests
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x17002809 RID: 10249
		// (get) Token: 0x0600E3BD RID: 58301 RVA: 0x0054AAE0 File Offset: 0x00548CE0
		public override bool GravShipCanLandOn
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x1700280A RID: 10250
		// (get) Token: 0x0600E3BE RID: 58302 RVA: 0x0054AAF2 File Offset: 0x00548CF2
		public override AcceptanceReport CanBeSettled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700280B RID: 10251
		// (get) Token: 0x0600E3BF RID: 58303 RVA: 0x0054AAFC File Offset: 0x00548CFC
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					this.cachedMat = MaterialPool.MatFrom(base.Faction.def.settlementTexturePath, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, 3550);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x1700280C RID: 10252
		// (get) Token: 0x0600E3C0 RID: 58304 RVA: 0x0054AB4D File Offset: 0x00548D4D
		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				if (this.def.mapGenerator != null)
				{
					return this.def.mapGenerator;
				}
				if (base.Faction == Faction.OfPlayer)
				{
					return MapGeneratorDefOf.Base_Player;
				}
				return MapGeneratorDefOf.Base_Faction;
			}
		}

		// Token: 0x1700280D RID: 10253
		// (get) Token: 0x0600E3C1 RID: 58305 RVA: 0x0054AB80 File Offset: 0x00548D80
		public TraderKindDef TraderKind
		{
			get
			{
				Settlement_TraderTracker settlement_TraderTracker = this.trader;
				if (settlement_TraderTracker == null)
				{
					return null;
				}
				return settlement_TraderTracker.TraderKind;
			}
		}

		// Token: 0x1700280E RID: 10254
		// (get) Token: 0x0600E3C2 RID: 58306 RVA: 0x0054AB93 File Offset: 0x00548D93
		public IEnumerable<Thing> Goods
		{
			get
			{
				Settlement_TraderTracker settlement_TraderTracker = this.trader;
				if (settlement_TraderTracker == null)
				{
					return null;
				}
				return settlement_TraderTracker.StockListForReading;
			}
		}

		// Token: 0x1700280F RID: 10255
		// (get) Token: 0x0600E3C3 RID: 58307 RVA: 0x0054ABA6 File Offset: 0x00548DA6
		public int RandomPriceFactorSeed
		{
			get
			{
				Settlement_TraderTracker settlement_TraderTracker = this.trader;
				if (settlement_TraderTracker == null)
				{
					return 0;
				}
				return settlement_TraderTracker.RandomPriceFactorSeed;
			}
		}

		// Token: 0x17002810 RID: 10256
		// (get) Token: 0x0600E3C4 RID: 58308 RVA: 0x0054ABB9 File Offset: 0x00548DB9
		public string TraderName
		{
			get
			{
				Settlement_TraderTracker settlement_TraderTracker = this.trader;
				if (settlement_TraderTracker == null)
				{
					return null;
				}
				return settlement_TraderTracker.TraderName;
			}
		}

		// Token: 0x17002811 RID: 10257
		// (get) Token: 0x0600E3C5 RID: 58309 RVA: 0x0054ABCC File Offset: 0x00548DCC
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x17002812 RID: 10258
		// (get) Token: 0x0600E3C6 RID: 58310 RVA: 0x0054ABE3 File Offset: 0x00548DE3
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				Settlement_TraderTracker settlement_TraderTracker = this.trader;
				if (settlement_TraderTracker == null)
				{
					return 0f;
				}
				return settlement_TraderTracker.TradePriceImprovementOffsetForPlayer;
			}
		}

		// Token: 0x17002813 RID: 10259
		// (get) Token: 0x0600E3C7 RID: 58311 RVA: 0x0054ABFA File Offset: 0x00548DFA
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x0600E3C8 RID: 58312 RVA: 0x0054AC07 File Offset: 0x00548E07
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Settlement_TraderTracker settlement_TraderTracker = this.trader;
			if (settlement_TraderTracker == null)
			{
				return null;
			}
			return settlement_TraderTracker.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x0600E3C9 RID: 58313 RVA: 0x0054AC1B File Offset: 0x00548E1B
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x0600E3CA RID: 58314 RVA: 0x0054AC2B File Offset: 0x00548E2B
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x17002814 RID: 10260
		// (get) Token: 0x0600E3CB RID: 58315 RVA: 0x0054AC3B File Offset: 0x00548E3B
		public bool EverVisited
		{
			get
			{
				return this.trader.EverVisited;
			}
		}

		// Token: 0x17002815 RID: 10261
		// (get) Token: 0x0600E3CC RID: 58316 RVA: 0x0054AC48 File Offset: 0x00548E48
		public bool RestockedSinceLastVisit
		{
			get
			{
				return this.trader.RestockedSinceLastVisit;
			}
		}

		// Token: 0x17002816 RID: 10262
		// (get) Token: 0x0600E3CD RID: 58317 RVA: 0x0054AC55 File Offset: 0x00548E55
		public int NextRestockTick
		{
			get
			{
				return this.trader.NextRestockTick;
			}
		}

		// Token: 0x0600E3CE RID: 58318 RVA: 0x0054AC62 File Offset: 0x00548E62
		public Settlement()
		{
			this.trader = new Settlement_TraderTracker(this);
		}

		// Token: 0x0600E3CF RID: 58319 RVA: 0x0054AC81 File Offset: 0x00548E81
		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			Settlement.<IncidentTargetTags>d__57 <IncidentTargetTags>d__ = new Settlement.<IncidentTargetTags>d__57(-2);
			<IncidentTargetTags>d__.<>4__this = this;
			return <IncidentTargetTags>d__;
		}

		// Token: 0x0600E3D0 RID: 58320 RVA: 0x0054AC94 File Offset: 0x00548E94
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.previouslyGeneratedInhabitants, "previouslyGeneratedInhabitants", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<Settlement_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Values.Look<string>(ref this.nameInt, "nameInt", null, false);
			Scribe_Values.Look<bool>(ref this.namedByPlayer, "namedByPlayer", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.previouslyGeneratedInhabitants.RemoveAll((Pawn x) => x == null);
				if (base.Faction == null)
				{
					Log.Warning("Settlement '" + this.Name + "' had null faction on load - destroying.");
					this.Destroy();
				}
			}
		}

		// Token: 0x0600E3D1 RID: 58321 RVA: 0x0054AD58 File Offset: 0x00548F58
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			TimedDetectionRaids timedDetectionRaids;
			if (!base.Map.IsPlayerHome && base.TryGetComponent<TimedDetectionRaids>(out timedDetectionRaids))
			{
				timedDetectionRaids.StartDetectionCountdown(240000, -1);
			}
		}

		// Token: 0x0600E3D2 RID: 58322 RVA: 0x0054AD8E File Offset: 0x00548F8E
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.trader != null)
			{
				this.trader.TraderTrackerTick();
			}
			SettlementDefeatUtility.CheckDefeated(this);
		}

		// Token: 0x0600E3D3 RID: 58323 RVA: 0x0054ADB0 File Offset: 0x00548FB0
		public override void Notify_MyMapRemoved(Map map)
		{
			base.Notify_MyMapRemoved(map);
			for (int i = this.previouslyGeneratedInhabitants.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.previouslyGeneratedInhabitants[i];
				if (pawn.DestroyedOrNull() || !pawn.IsWorldPawn())
				{
					this.previouslyGeneratedInhabitants.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600E3D4 RID: 58324 RVA: 0x0054AE08 File Offset: 0x00549008
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return !base.Map.AnyBuildingBlockingMapRemoval && !base.Map.IsPlayerHome && !base.Map.mapPawns.AnyPawnBlockingMapRemoval && !TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map);
		}

		// Token: 0x0600E3D5 RID: 58325 RVA: 0x0054AE58 File Offset: 0x00549058
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.trader != null)
			{
				this.trader.TryDestroyStock();
			}
		}

		// Token: 0x0600E3D6 RID: 58326 RVA: 0x0054AE74 File Offset: 0x00549074
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Faction != Faction.OfPlayer)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += base.Faction.PlayerRelationKind.GetLabelCap();
				if (!base.Faction.Hidden)
				{
					text = text + " (" + base.Faction.PlayerGoodwill.ToStringWithSign() + ")";
				}
				TraderKindDef traderKind = this.TraderKind;
				RoyalTitleDef royalTitleDef = (traderKind != null) ? traderKind.TitleRequiredToTrade : null;
				if (royalTitleDef != null)
				{
					text += "\n" + "RequiresTradePermission".Translate(royalTitleDef.GetLabelCapForBothGenders());
				}
			}
			else
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "Settled".Translate() + ": " + GenDate.DateShortStringAt((long)GenDate.TickGameToAbs(this.creationGameTicks), Find.WorldGrid.LongLatOf(base.Tile));
				text += " (";
				text += "TimeAgo".Translate((Find.TickManager.TicksGame - this.creationGameTicks).ToStringTicksToPeriodVague(true, true));
				text += ")";
			}
			return text;
		}

		// Token: 0x0600E3D7 RID: 58327 RVA: 0x0054AFE0 File Offset: 0x005491E0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Settlement.<GetGizmos>d__65 <GetGizmos>d__ = new Settlement.<GetGizmos>d__65(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600E3D8 RID: 58328 RVA: 0x0054AFF0 File Offset: 0x005491F0
		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			Settlement.<GetCaravanGizmos>d__66 <GetCaravanGizmos>d__ = new Settlement.<GetCaravanGizmos>d__66(-2);
			<GetCaravanGizmos>d__.<>4__this = this;
			<GetCaravanGizmos>d__.<>3__caravan = caravan;
			return <GetCaravanGizmos>d__;
		}

		// Token: 0x0600E3D9 RID: 58329 RVA: 0x0054B007 File Offset: 0x00549207
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			Settlement.<GetFloatMenuOptions>d__67 <GetFloatMenuOptions>d__ = new Settlement.<GetFloatMenuOptions>d__67(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__caravan = caravan;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600E3DA RID: 58330 RVA: 0x0054B01E File Offset: 0x0054921E
		public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			Settlement.<GetTransportersFloatMenuOptions>d__68 <GetTransportersFloatMenuOptions>d__ = new Settlement.<GetTransportersFloatMenuOptions>d__68(-2);
			<GetTransportersFloatMenuOptions>d__.<>4__this = this;
			<GetTransportersFloatMenuOptions>d__.<>3__pods = pods;
			<GetTransportersFloatMenuOptions>d__.<>3__launchAction = launchAction;
			return <GetTransportersFloatMenuOptions>d__;
		}

		// Token: 0x0600E3DB RID: 58331 RVA: 0x0054B03C File Offset: 0x0054923C
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			Settlement.<GetShuttleFloatMenuOptions>d__69 <GetShuttleFloatMenuOptions>d__ = new Settlement.<GetShuttleFloatMenuOptions>d__69(-2);
			<GetShuttleFloatMenuOptions>d__.<>4__this = this;
			<GetShuttleFloatMenuOptions>d__.<>3__pods = pods;
			<GetShuttleFloatMenuOptions>d__.<>3__launchAction = launchAction;
			return <GetShuttleFloatMenuOptions>d__;
		}

		// Token: 0x0600E3DC RID: 58332 RVA: 0x0054B05A File Offset: 0x0054925A
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			if (this.trader != null)
			{
				outChildren.Add(this.trader);
			}
		}

		// Token: 0x0600E3DD RID: 58333 RVA: 0x0054B078 File Offset: 0x00549278
		public override void Abandon(bool wasGravshipLaunch)
		{
			base.Abandon(wasGravshipLaunch);
			if (!wasGravshipLaunch && base.Tile.LayerDef == PlanetLayerDefOf.Surface)
			{
				WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedSettlement);
				worldObject.Tile = base.Tile;
				worldObject.SetFaction(base.Faction);
				Find.WorldObjects.Add(worldObject);
			}
		}

		// Token: 0x04009116 RID: 37142
		public Settlement_TraderTracker trader;

		// Token: 0x04009117 RID: 37143
		public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

		// Token: 0x04009118 RID: 37144
		private string nameInt;

		// Token: 0x04009119 RID: 37145
		public bool namedByPlayer;

		// Token: 0x0400911A RID: 37146
		private Material cachedMat;

		// Token: 0x0400911B RID: 37147
		public static readonly Texture2D ShowSellableItemsCommand = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		// Token: 0x0400911C RID: 37148
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x0400911D RID: 37149
		public static readonly Texture2D AttackCommand = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement", true);
	}
}
