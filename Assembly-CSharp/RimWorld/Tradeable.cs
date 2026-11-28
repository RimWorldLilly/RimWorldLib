using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8E RID: 7054
	public class Tradeable : Transferable
	{
		// Token: 0x17002097 RID: 8343
		// (get) Token: 0x0600B4ED RID: 46317 RVA: 0x003FBC90 File Offset: 0x003F9E90
		// (set) Token: 0x0600B4EE RID: 46318 RVA: 0x003FBC98 File Offset: 0x003F9E98
		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		// Token: 0x17002098 RID: 8344
		// (get) Token: 0x0600B4EF RID: 46319 RVA: 0x003FBCAD File Offset: 0x003F9EAD
		public Thing FirstThingColony
		{
			get
			{
				if (this.thingsColony.Count == 0)
				{
					return null;
				}
				return this.thingsColony[0];
			}
		}

		// Token: 0x17002099 RID: 8345
		// (get) Token: 0x0600B4F0 RID: 46320 RVA: 0x003FBCCA File Offset: 0x003F9ECA
		public Thing FirstThingTrader
		{
			get
			{
				if (this.thingsTrader.Count == 0)
				{
					return null;
				}
				return this.thingsTrader[0];
			}
		}

		// Token: 0x1700209A RID: 8346
		// (get) Token: 0x0600B4F1 RID: 46321 RVA: 0x003FBCE7 File Offset: 0x003F9EE7
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x1700209B RID: 8347
		// (get) Token: 0x0600B4F2 RID: 46322 RVA: 0x003FBCF4 File Offset: 0x003F9EF4
		public virtual float BaseMarketValue
		{
			get
			{
				return this.AnyThing.MarketValue;
			}
		}

		// Token: 0x1700209C RID: 8348
		// (get) Token: 0x0600B4F3 RID: 46323 RVA: 0x003FBD01 File Offset: 0x003F9F01
		public override bool Interactive
		{
			get
			{
				return !this.IsCurrency || (TradeSession.Active && TradeSession.giftMode);
			}
		}

		// Token: 0x1700209D RID: 8349
		// (get) Token: 0x0600B4F4 RID: 46324 RVA: 0x003FBD1B File Offset: 0x003F9F1B
		public virtual bool TraderWillTrade
		{
			get
			{
				return TradeSession.trader.TraderKind.WillTrade(this.ThingDef);
			}
		}

		// Token: 0x1700209E RID: 8350
		// (get) Token: 0x0600B4F5 RID: 46325 RVA: 0x003FBD32 File Offset: 0x003F9F32
		public override bool HasAnyThing
		{
			get
			{
				return this.AnyThing != null;
			}
		}

		// Token: 0x1700209F RID: 8351
		// (get) Token: 0x0600B4F6 RID: 46326 RVA: 0x003FBD40 File Offset: 0x003F9F40
		public override Thing AnyThing
		{
			get
			{
				if (this.FirstThingColony != null)
				{
					return this.FirstThingColony.GetInnerIfMinified();
				}
				if (this.FirstThingTrader != null)
				{
					return this.FirstThingTrader.GetInnerIfMinified();
				}
				Type type = base.GetType();
				Log.Error(((type != null) ? type.ToString() : null) + " lacks AnyThing.");
				return null;
			}
		}

		// Token: 0x170020A0 RID: 8352
		// (get) Token: 0x0600B4F7 RID: 46327 RVA: 0x003FBD97 File Offset: 0x003F9F97
		public override ThingDef ThingDef
		{
			get
			{
				Thing anyThing = this.AnyThing;
				if (anyThing == null)
				{
					return null;
				}
				return anyThing.def;
			}
		}

		// Token: 0x170020A1 RID: 8353
		// (get) Token: 0x0600B4F8 RID: 46328 RVA: 0x003FBDAA File Offset: 0x003F9FAA
		public ThingDef StuffDef
		{
			get
			{
				Thing anyThing = this.AnyThing;
				if (anyThing == null)
				{
					return null;
				}
				return anyThing.Stuff;
			}
		}

		// Token: 0x170020A2 RID: 8354
		// (get) Token: 0x0600B4F9 RID: 46329 RVA: 0x003FBDBD File Offset: 0x003F9FBD
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x170020A3 RID: 8355
		// (get) Token: 0x0600B4FA RID: 46330 RVA: 0x003FBDD8 File Offset: 0x003F9FD8
		public TradeAction ActionToDo
		{
			get
			{
				if (this.CountToTransfer == 0)
				{
					return TradeAction.None;
				}
				if (base.CountToTransferToDestination > 0)
				{
					return TradeAction.PlayerSells;
				}
				return TradeAction.PlayerBuys;
			}
		}

		// Token: 0x170020A4 RID: 8356
		// (get) Token: 0x0600B4FB RID: 46331 RVA: 0x003FBDF0 File Offset: 0x003F9FF0
		public virtual bool IsCurrency
		{
			get
			{
				return !this.Bugged && this.ThingDef == ThingDefOf.Silver;
			}
		}

		// Token: 0x170020A5 RID: 8357
		// (get) Token: 0x0600B4FC RID: 46332 RVA: 0x003FBE09 File Offset: 0x003FA009
		public virtual bool IsFavor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B4FD RID: 46333 RVA: 0x003FBE0C File Offset: 0x003FA00C
		public virtual int CostToInt(float cost)
		{
			return Mathf.RoundToInt(cost);
		}

		// Token: 0x170020A6 RID: 8358
		// (get) Token: 0x0600B4FE RID: 46334 RVA: 0x003FBE14 File Offset: 0x003FA014
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				if (TradeSession.Active && TradeSession.giftMode)
				{
					return TransferablePositiveCountDirection.Destination;
				}
				return TransferablePositiveCountDirection.Source;
			}
		}

		// Token: 0x170020A7 RID: 8359
		// (get) Token: 0x0600B4FF RID: 46335 RVA: 0x003FBE27 File Offset: 0x003FA027
		public float CurTotalCurrencyCostForSource
		{
			get
			{
				if (this.ActionToDo == TradeAction.None)
				{
					return 0f;
				}
				return (float)base.CountToTransferToSource * this.GetPriceFor(this.ActionToDo);
			}
		}

		// Token: 0x170020A8 RID: 8360
		// (get) Token: 0x0600B500 RID: 46336 RVA: 0x003FBE4B File Offset: 0x003FA04B
		public float CurTotalCurrencyCostForDestination
		{
			get
			{
				if (this.ActionToDo == TradeAction.None)
				{
					return 0f;
				}
				return (float)base.CountToTransferToDestination * this.GetPriceFor(this.ActionToDo);
			}
		}

		// Token: 0x170020A9 RID: 8361
		// (get) Token: 0x0600B501 RID: 46337 RVA: 0x003FBE6F File Offset: 0x003FA06F
		public virtual Window NewInfoDialog
		{
			get
			{
				return new Dialog_InfoCard(this.ThingDef, null);
			}
		}

		// Token: 0x170020AA RID: 8362
		// (get) Token: 0x0600B502 RID: 46338 RVA: 0x003FBE7D File Offset: 0x003FA07D
		private bool Bugged
		{
			get
			{
				if (!this.HasAnyThing)
				{
					Log.ErrorOnce(this.ToString() + " is bugged. There will be no more logs about this.", 162112);
					return true;
				}
				return false;
			}
		}

		// Token: 0x0600B503 RID: 46339 RVA: 0x003FBEA4 File Offset: 0x003FA0A4
		public Tradeable()
		{
		}

		// Token: 0x0600B504 RID: 46340 RVA: 0x003FBED8 File Offset: 0x003FA0D8
		public Tradeable(Thing thingColony, Thing thingTrader)
		{
			this.thingsColony.Add(thingColony);
			this.thingsTrader.Add(thingTrader);
		}

		// Token: 0x0600B505 RID: 46341 RVA: 0x003FBF2F File Offset: 0x003FA12F
		public void AddThing(Thing t, Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				this.thingsColony.Add(t);
			}
			if (trans == Transactor.Trader)
			{
				this.thingsTrader.Add(t);
			}
		}

		// Token: 0x0600B506 RID: 46342 RVA: 0x003FBF50 File Offset: 0x003FA150
		public PriceType PriceTypeFor(TradeAction action)
		{
			return TradeSession.trader.TraderKind.PriceTypeFor(this.ThingDef, action);
		}

		// Token: 0x0600B507 RID: 46343 RVA: 0x003FBF68 File Offset: 0x003FA168
		private void InitPriceDataIfNeeded()
		{
			if (this.pricePlayerBuy > 0f)
			{
				return;
			}
			if (this.IsCurrency)
			{
				this.pricePlayerBuy = this.BaseMarketValue;
				this.pricePlayerSell = this.BaseMarketValue;
				return;
			}
			this.priceFactorBuy_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerBuys).PriceMultiplier();
			this.priceFactorSell_TraderPriceType = this.PriceTypeFor(TradeAction.PlayerSells).PriceMultiplier();
			this.priceGain_PlayerNegotiator = TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true, -1);
			this.priceGain_Leader = ((TradeSession.playerNegotiator == Faction.OfPlayer.leader) ? 0.02f : 0f);
			this.priceGain_Settlement = TradeSession.trader.TradePriceImprovementOffsetForPlayer;
			this.priceFactorSell_ItemSellPriceFactor = this.AnyThing.GetStatValue(StatDefOf.SellPriceFactor, true, -1);
			this.priceFactorBuy_JoinAs = 1f;
			this.priceFactorSell_HumanPawn = 1f;
			if (this.ThingDef.IsNonMedicalDrug)
			{
				this.priceGain_DrugBonus = TradeSession.playerNegotiator.GetStatValue(StatDefOf.DrugSellPriceImprovement, true, -1);
			}
			if (ModsConfig.IdeologyActive)
			{
				if (this.ThingDef.IsLeather || this.ThingDef.IsMeat || this.ThingDef.IsWool)
				{
					this.priceGain_AnimalProduceBonus = TradeSession.playerNegotiator.GetStatValue(StatDefOf.AnimalProductsSellImprovement, true, -1);
				}
				Pawn pawn = this.AnyThing as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike && pawn.guest != null)
				{
					if (pawn.guest.joinStatus == JoinStatus.JoinAsSlave)
					{
						this.priceFactorBuy_JoinAs = 0.6f;
					}
					this.priceFactorSell_HumanPawn = 0.6f;
				}
			}
			this.pricePlayerBuy = TradeUtility.GetPricePlayerBuy(this.AnyThing, this.priceFactorBuy_TraderPriceType, this.priceFactorBuy_JoinAs, this.priceGain_PlayerNegotiator, this.priceGain_Settlement);
			this.pricePlayerSell = TradeUtility.GetPricePlayerSell(this.AnyThing, this.priceFactorSell_TraderPriceType, this.priceFactorSell_HumanPawn, this.priceGain_PlayerNegotiator, this.priceGain_Settlement, this.priceGain_DrugBonus, this.priceGain_AnimalProduceBonus, TradeSession.TradeCurrency);
			if (this.pricePlayerSell >= this.pricePlayerBuy)
			{
				this.pricePlayerSell = this.pricePlayerBuy;
			}
		}

		// Token: 0x0600B508 RID: 46344 RVA: 0x003FC174 File Offset: 0x003FA374
		public string GetPriceTooltip(TradeAction action)
		{
			if (!this.HasAnyThing)
			{
				return "";
			}
			this.InitPriceDataIfNeeded();
			string text = (action == TradeAction.PlayerBuys) ? "BuyPriceDesc".Translate() : "SellPriceDesc".Translate();
			if (TradeSession.TradeCurrency != TradeCurrency.Silver)
			{
				return text;
			}
			text += "\n\n";
			text += StatDefOf.MarketValue.LabelCap + ": " + this.BaseMarketValue.ToStringMoney(null);
			if (action == TradeAction.PlayerBuys)
			{
				text += "\n  x " + 1.4f.ToString("F2") + " (" + "Buying".Translate() + ")";
				if (this.priceFactorBuy_TraderPriceType != 1f)
				{
					text += "\n  x " + this.priceFactorBuy_TraderPriceType.ToString("F2") + " (" + "TraderTypePrice".Translate() + ")";
				}
				if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0f)
				{
					text += "\n  x " + (1f + Find.Storyteller.difficulty.tradePriceFactorLoss).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				text += "\n";
				text += "\n" + "YourNegotiatorBonus".Translate() + ": -" + (this.priceGain_PlayerNegotiator - this.priceGain_Leader).ToStringPercent();
				if (ModsConfig.IdeologyActive)
				{
					if (this.priceGain_Leader != 0f)
					{
						text += "\n" + "YourLeaderTradeBonus".Translate() + ": -" + this.priceGain_Leader.ToStringPercent();
					}
					if (this.priceFactorBuy_JoinAs != 1f)
					{
						text += "\n" + "Slave".Translate().CapitalizeFirst() + ": x" + this.priceFactorBuy_JoinAs.ToStringPercent();
					}
				}
				if (this.priceGain_Settlement != 0f)
				{
					text += "\n" + "TradeWithFactionBaseBonus".Translate() + ": -" + this.priceGain_Settlement.ToStringPercent();
				}
			}
			else
			{
				text += "\n  x " + 0.6f.ToString("F2") + " (" + "Selling".Translate() + ")";
				if (this.priceFactorSell_TraderPriceType != 1f)
				{
					text += "\n  x " + this.priceFactorSell_TraderPriceType.ToString("F2") + " (" + "TraderTypePrice".Translate() + ")";
				}
				if (this.priceFactorSell_ItemSellPriceFactor != 1f)
				{
					text += "\n  x " + this.priceFactorSell_ItemSellPriceFactor.ToString("F2") + " (" + "ItemSellPriceFactor".Translate() + ")";
				}
				if (this.priceFactorSell_HumanPawn != 1f)
				{
					text += "\n  x " + this.priceFactorSell_HumanPawn.ToString("F2") + " (" + "Slave".Translate() + ")";
				}
				if (Find.Storyteller.difficulty.tradePriceFactorLoss != 0f)
				{
					text += "\n  x " + (1f - Find.Storyteller.difficulty.tradePriceFactorLoss).ToString("F2") + " (" + "DifficultyLevel".Translate() + ")";
				}
				text += "\n";
				text += "\n" + "YourNegotiatorBonus".Translate() + ": " + (this.priceGain_PlayerNegotiator - this.priceGain_Leader).ToStringPercent();
				if (ModsConfig.IdeologyActive && this.priceGain_Leader != 0f)
				{
					text += "\n" + "YourLeaderTradeBonus".Translate() + ": " + this.priceGain_Leader.ToStringPercent();
				}
				if (this.priceGain_Settlement != 0f)
				{
					text += "\n" + "TradeWithFactionBaseBonus".Translate() + ": " + this.priceGain_Settlement.ToStringPercent();
				}
				if (this.priceGain_DrugBonus != 0f)
				{
					text += "\n" + "TradingDrugsBonus".Translate() + ": " + this.priceGain_DrugBonus.ToStringPercent();
				}
				if (this.priceGain_AnimalProduceBonus != 0f)
				{
					text += "\n" + "TradingAnimalProduceBonus".Translate() + ": " + this.priceGain_AnimalProduceBonus.ToStringPercent();
				}
			}
			text += "\n\n";
			float priceFor = this.GetPriceFor(action);
			text += "FinalPrice".Translate() + ": " + priceFor.ToStringMoney(null);
			if ((action == TradeAction.PlayerBuys && priceFor <= 0.5f) || (action == TradeAction.PlayerBuys && priceFor <= 0.01f))
			{
				text += " (" + "minimum".Translate() + ")";
			}
			return text;
		}

		// Token: 0x0600B509 RID: 46345 RVA: 0x003FC7D9 File Offset: 0x003FA9D9
		public virtual float GetPriceFor(TradeAction action)
		{
			this.InitPriceDataIfNeeded();
			if (action == TradeAction.PlayerBuys)
			{
				return this.pricePlayerBuy;
			}
			return this.pricePlayerSell;
		}

		// Token: 0x0600B50A RID: 46346 RVA: 0x003FC7F2 File Offset: 0x003FA9F2
		public override int GetMinimumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return -this.CountHeldBy(Transactor.Trader);
			}
			return -this.CountHeldBy(Transactor.Colony);
		}

		// Token: 0x0600B50B RID: 46347 RVA: 0x003FC80E File Offset: 0x003FAA0E
		public override int GetMaximumToTransfer()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return this.CountHeldBy(Transactor.Colony);
			}
			return this.CountHeldBy(Transactor.Trader);
		}

		// Token: 0x0600B50C RID: 46348 RVA: 0x003FC828 File Offset: 0x003FAA28
		public override AcceptanceReport UnderflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("TraderHasNoMore".Translate());
			}
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x0600B50D RID: 46349 RVA: 0x003FC85C File Offset: 0x003FAA5C
		public override AcceptanceReport OverflowReport()
		{
			if (this.PositiveCountDirection == TransferablePositiveCountDirection.Destination)
			{
				return new AcceptanceReport("ColonyHasNoMore".Translate());
			}
			return new AcceptanceReport("TraderHasNoMore".Translate());
		}

		// Token: 0x0600B50E RID: 46350 RVA: 0x003FC890 File Offset: 0x003FAA90
		private List<Thing> TransactorThings(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.thingsColony;
			}
			return this.thingsTrader;
		}

		// Token: 0x0600B50F RID: 46351 RVA: 0x003FC8A4 File Offset: 0x003FAAA4
		public virtual int CountHeldBy(Transactor trans)
		{
			List<Thing> list = this.TransactorThings(trans);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].stackCount;
			}
			return num;
		}

		// Token: 0x0600B510 RID: 46352 RVA: 0x003FC8DC File Offset: 0x003FAADC
		public int CountPostDealFor(Transactor trans)
		{
			if (trans == Transactor.Colony)
			{
				return this.CountHeldBy(trans) + base.CountToTransferToSource;
			}
			return this.CountHeldBy(trans) + base.CountToTransferToDestination;
		}

		// Token: 0x0600B511 RID: 46353 RVA: 0x003FC900 File Offset: 0x003FAB00
		public virtual void ResolveTrade()
		{
			if (this.ActionToDo == TradeAction.PlayerSells)
			{
				TransferableUtility.TransferNoSplit(this.thingsColony, base.CountToTransferToDestination, delegate(Thing thing, int countToTransfer)
				{
					TradeSession.trader.GiveSoldThingToTrader(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
				return;
			}
			if (this.ActionToDo == TradeAction.PlayerBuys)
			{
				TransferableUtility.TransferNoSplit(this.thingsTrader, base.CountToTransferToSource, delegate(Thing thing, int countToTransfer)
				{
					this.CheckTeachOpportunity(thing, countToTransfer);
					TradeSession.trader.GiveSoldThingToPlayer(thing, countToTransfer, TradeSession.playerNegotiator);
				}, true, true);
			}
		}

		// Token: 0x0600B512 RID: 46354 RVA: 0x003FC974 File Offset: 0x003FAB74
		private void CheckTeachOpportunity(Thing boughtThing, int boughtCount)
		{
			Building building = boughtThing as Building;
			if (building == null)
			{
				MinifiedThing minifiedThing = boughtThing as MinifiedThing;
				if (minifiedThing != null)
				{
					building = (minifiedThing.InnerThing as Building);
				}
			}
			bool flag;
			if (building == null)
			{
				flag = (null != null);
			}
			else
			{
				BuildingProperties building2 = building.def.building;
				flag = (((building2 != null) ? building2.boughtConceptLearnOpportunity : null) != null);
			}
			if (flag)
			{
				LessonAutoActivator.TeachOpportunity(building.def.building.boughtConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600B513 RID: 46355 RVA: 0x003FC9D8 File Offset: 0x003FABD8
		public override string ToString()
		{
			string[] array = new string[6];
			int num = 0;
			Type type = base.GetType();
			array[num] = ((type != null) ? type.ToString() : null);
			array[1] = "(";
			int num2 = 2;
			ThingDef thingDef = this.ThingDef;
			array[num2] = ((thingDef != null) ? thingDef.ToString() : null);
			array[3] = ", countToTransfer=";
			array[4] = this.CountToTransfer.ToString();
			array[5] = ")";
			return string.Concat(array);
		}

		// Token: 0x0600B514 RID: 46356 RVA: 0x003FCA43 File Offset: 0x003FAC43
		public override int GetHashCode()
		{
			return this.AnyThing.GetHashCode();
		}

		// Token: 0x0600B515 RID: 46357 RVA: 0x003FCA50 File Offset: 0x003FAC50
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.thingsColony.RemoveAll((Thing x) => x.Destroyed);
				this.thingsTrader.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.thingsColony, "thingsColony", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsTrader, "thingsTrader", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingsColony.RemoveAll((Thing x) => x == null) == 0)
				{
					if (this.thingsTrader.RemoveAll((Thing x) => x == null) == 0)
					{
						return;
					}
				}
				Log.Warning("Some of the things were null after loading.");
			}
		}

		// Token: 0x04006816 RID: 26646
		public List<Thing> thingsColony = new List<Thing>();

		// Token: 0x04006817 RID: 26647
		public List<Thing> thingsTrader = new List<Thing>();

		// Token: 0x04006818 RID: 26648
		private int countToTransfer;

		// Token: 0x04006819 RID: 26649
		public const float NegotiatorLeaderOffset = 0.02f;

		// Token: 0x0400681A RID: 26650
		private const float PriceFactor_JoinAsSlaveOrColonyPawn = 0.6f;

		// Token: 0x0400681B RID: 26651
		private float pricePlayerBuy = -1f;

		// Token: 0x0400681C RID: 26652
		private float pricePlayerSell = -1f;

		// Token: 0x0400681D RID: 26653
		private float priceFactorBuy_TraderPriceType;

		// Token: 0x0400681E RID: 26654
		private float priceFactorBuy_JoinAs;

		// Token: 0x0400681F RID: 26655
		private float priceFactorSell_HumanPawn;

		// Token: 0x04006820 RID: 26656
		private float priceFactorSell_TraderPriceType;

		// Token: 0x04006821 RID: 26657
		private float priceFactorSell_ItemSellPriceFactor;

		// Token: 0x04006822 RID: 26658
		private float priceGain_PlayerNegotiator;

		// Token: 0x04006823 RID: 26659
		private float priceGain_Leader;

		// Token: 0x04006824 RID: 26660
		private float priceGain_Settlement;

		// Token: 0x04006825 RID: 26661
		private float priceGain_DrugBonus;

		// Token: 0x04006826 RID: 26662
		private float priceGain_AnimalProduceBonus;
	}
}
