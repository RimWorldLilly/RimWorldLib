using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B97 RID: 7063
	public class TradeShip : PassingShip, ITrader, IThingHolder
	{
		// Token: 0x170020BC RID: 8380
		// (get) Token: 0x0600B53E RID: 46398 RVA: 0x003FD6B1 File Offset: 0x003FB8B1
		public override string FullTitle
		{
			get
			{
				return this.name + " (" + this.def.label + ")";
			}
		}

		// Token: 0x170020BD RID: 8381
		// (get) Token: 0x0600B53F RID: 46399 RVA: 0x003FD6D3 File Offset: 0x003FB8D3
		public int Silver
		{
			get
			{
				return this.CountHeldOf(ThingDefOf.Silver, null);
			}
		}

		// Token: 0x170020BE RID: 8382
		// (get) Token: 0x0600B540 RID: 46400 RVA: 0x003FD6E1 File Offset: 0x003FB8E1
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.def.tradeCurrency;
			}
		}

		// Token: 0x170020BF RID: 8383
		// (get) Token: 0x0600B541 RID: 46401 RVA: 0x003FD6EE File Offset: 0x003FB8EE
		public IThingHolder ParentHolder
		{
			get
			{
				return base.Map;
			}
		}

		// Token: 0x170020C0 RID: 8384
		// (get) Token: 0x0600B542 RID: 46402 RVA: 0x003FD6F6 File Offset: 0x003FB8F6
		public TraderKindDef TraderKind
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x170020C1 RID: 8385
		// (get) Token: 0x0600B543 RID: 46403 RVA: 0x003FD6FE File Offset: 0x003FB8FE
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.randomPriceFactorSeed;
			}
		}

		// Token: 0x170020C2 RID: 8386
		// (get) Token: 0x0600B544 RID: 46404 RVA: 0x003FD706 File Offset: 0x003FB906
		public string TraderName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170020C3 RID: 8387
		// (get) Token: 0x0600B545 RID: 46405 RVA: 0x003FD70E File Offset: 0x003FB90E
		public bool CanTradeNow
		{
			get
			{
				return !base.Departed;
			}
		}

		// Token: 0x170020C4 RID: 8388
		// (get) Token: 0x0600B546 RID: 46406 RVA: 0x003FD719 File Offset: 0x003FB919
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170020C5 RID: 8389
		// (get) Token: 0x0600B547 RID: 46407 RVA: 0x003FD720 File Offset: 0x003FB920
		// (set) Token: 0x0600B548 RID: 46408 RVA: 0x003FD728 File Offset: 0x003FB928
		public bool WasAnnounced
		{
			get
			{
				return this.wasAnnounced;
			}
			set
			{
				this.wasAnnounced = value;
			}
		}

		// Token: 0x170020C6 RID: 8390
		// (get) Token: 0x0600B549 RID: 46409 RVA: 0x003FD731 File Offset: 0x003FB931
		public IEnumerable<Thing> Goods
		{
			get
			{
				TradeShip.<get_Goods>d__27 <get_Goods>d__ = new TradeShip.<get_Goods>d__27(-2);
				<get_Goods>d__.<>4__this = this;
				return <get_Goods>d__;
			}
		}

		// Token: 0x0600B54A RID: 46410 RVA: 0x003FD741 File Offset: 0x003FB941
		public TradeShip()
		{
		}

		// Token: 0x0600B54B RID: 46411 RVA: 0x003FD75C File Offset: 0x003FB95C
		public TradeShip(TraderKindDef def, Faction faction = null) : base(faction)
		{
			this.def = def;
			this.things = new ThingOwner<Thing>(this);
			TradeShip.tmpExtantNames.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				TradeShip.tmpExtantNames.AddRange(from x in maps[i].passingShipManager.passingShips
				select x.name);
			}
			this.name = NameGenerator.GenerateName(RulePackDefOf.NamerTraderGeneral, TradeShip.tmpExtantNames, false, null);
			if (faction != null)
			{
				this.name = "GuildTradeShipName".Translate(this.name, faction.Name);
			}
			this.randomPriceFactorSeed = Rand.RangeInclusive(1, 10000000);
			this.loadID = Find.UniqueIDsManager.GetNextPassingShipID();
		}

		// Token: 0x0600B54C RID: 46412 RVA: 0x003FD85B File Offset: 0x003FBA5B
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			TradeShip.<ColonyThingsWillingToBuy>d__31 <ColonyThingsWillingToBuy>d__ = new TradeShip.<ColonyThingsWillingToBuy>d__31(-2);
			<ColonyThingsWillingToBuy>d__.<>4__this = this;
			return <ColonyThingsWillingToBuy>d__;
		}

		// Token: 0x0600B54D RID: 46413 RVA: 0x003FD86C File Offset: 0x003FBA6C
		public void GenerateThings()
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = this.def;
			parms.tile = new PlanetTile?(base.Map.Tile);
			this.things.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms), true, false);
		}

		// Token: 0x0600B54E RID: 46414 RVA: 0x003FD8C4 File Offset: 0x003FBAC4
		public override void PassingShipTick()
		{
			base.PassingShipTick();
			for (int i = this.things.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.things[i] as Pawn;
				if (pawn != null)
				{
					pawn.DoTick();
					if (pawn.Dead)
					{
						this.things.Remove(pawn);
					}
				}
			}
		}

		// Token: 0x0600B54F RID: 46415 RVA: 0x003FD920 File Offset: 0x003FBB20
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TraderKindDef>(ref this.def, "def");
			Scribe_Deep.Look<ThingOwner>(ref this.things, "things", new object[]
			{
				this
			});
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.randomPriceFactorSeed, "randomPriceFactorSeed", 0, false);
			Scribe_Values.Look<bool>(ref this.wasAnnounced, "wasAnnounced", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600B550 RID: 46416 RVA: 0x003FD9CC File Offset: 0x003FBBCC
		public override void TryOpenComms(Pawn negotiator)
		{
			if (!this.CanTradeNow)
			{
				return;
			}
			Find.WindowStack.Add(new Dialog_Trade(negotiator, this, false));
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.Critical);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(this.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradeShip".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, false, true);
			TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.TradeGoodsMustBeNearBeacon, Array.Empty<string>());
		}

		// Token: 0x0600B551 RID: 46417 RVA: 0x003FDA48 File Offset: 0x003FBC48
		public override void Depart()
		{
			base.Depart();
			this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			this.soldPrisoners.Clear();
		}

		// Token: 0x0600B552 RID: 46418 RVA: 0x003FDA67 File Offset: 0x003FBC67
		public override string GetCallLabel()
		{
			return this.name + " (" + this.def.label + ")";
		}

		// Token: 0x0600B553 RID: 46419 RVA: 0x003FDA8C File Offset: 0x003FBC8C
		protected override AcceptanceReport CanCommunicateWith(Pawn negotiator)
		{
			AcceptanceReport result = base.CanCommunicateWith(negotiator);
			if (!result.Accepted)
			{
				return result;
			}
			return negotiator.CanTradeWith(base.Faction, this.TraderKind);
		}

		// Token: 0x0600B554 RID: 46420 RVA: 0x003FDAC0 File Offset: 0x003FBCC0
		public int CountHeldOf(ThingDef thingDef, ThingDef stuffDef = null)
		{
			Thing thing = this.HeldThingMatching(thingDef, stuffDef);
			if (thing != null)
			{
				return thing.stackCount;
			}
			return 0;
		}

		// Token: 0x0600B555 RID: 46421 RVA: 0x003FDAE4 File Offset: 0x003FBCE4
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this);
			Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this, thing);
			if (thing2 != null)
			{
				if (!thing2.TryAbsorbStack(thing, false))
				{
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike)
				{
					this.soldPrisoners.Add(pawn);
				}
				this.things.TryAdd(thing, false);
			}
		}

		// Token: 0x0600B556 RID: 46422 RVA: 0x003FDB50 File Offset: 0x003FBD50
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				this.soldPrisoners.Remove(pawn);
			}
			TradeUtility.SpawnDropPod(DropCellFinder.TradeDropSpot(base.Map), base.Map, thing);
		}

		// Token: 0x0600B557 RID: 46423 RVA: 0x003FDB9C File Offset: 0x003FBD9C
		private Thing HeldThingMatching(ThingDef thingDef, ThingDef stuffDef)
		{
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].def == thingDef && this.things[i].Stuff == stuffDef)
				{
					return this.things[i];
				}
			}
			return null;
		}

		// Token: 0x0600B558 RID: 46424 RVA: 0x003FDBF5 File Offset: 0x003FBDF5
		public void ChangeCountHeldOf(ThingDef thingDef, ThingDef stuffDef, int count)
		{
			Thing thing = this.HeldThingMatching(thingDef, stuffDef);
			if (thing == null)
			{
				Log.Error("Changing count of thing trader doesn't have: " + ((thingDef != null) ? thingDef.ToString() : null));
			}
			thing.stackCount += count;
		}

		// Token: 0x0600B559 RID: 46425 RVA: 0x003FDC2B File Offset: 0x003FBE2B
		public override string ToString()
		{
			return this.FullTitle;
		}

		// Token: 0x0600B55A RID: 46426 RVA: 0x003FDC33 File Offset: 0x003FBE33
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		// Token: 0x0600B55B RID: 46427 RVA: 0x003FDC3B File Offset: 0x003FBE3B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04006839 RID: 26681
		public TraderKindDef def;

		// Token: 0x0400683A RID: 26682
		private ThingOwner things;

		// Token: 0x0400683B RID: 26683
		private List<Pawn> soldPrisoners = new List<Pawn>();

		// Token: 0x0400683C RID: 26684
		private int randomPriceFactorSeed = -1;

		// Token: 0x0400683D RID: 26685
		private bool wasAnnounced;

		// Token: 0x0400683E RID: 26686
		private static List<string> tmpExtantNames = new List<string>();
	}
}
