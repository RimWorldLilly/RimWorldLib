using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B93 RID: 7059
	public class TradeDeal
	{
		// Token: 0x170020B7 RID: 8375
		// (get) Token: 0x0600B52B RID: 46379 RVA: 0x003FCE70 File Offset: 0x003FB070
		public int TradeableCount
		{
			get
			{
				return this.tradeables.Count;
			}
		}

		// Token: 0x170020B8 RID: 8376
		// (get) Token: 0x0600B52C RID: 46380 RVA: 0x003FCE80 File Offset: 0x003FB080
		public Tradeable CurrencyTradeable
		{
			get
			{
				for (int i = 0; i < this.tradeables.Count; i++)
				{
					if ((TradeSession.TradeCurrency == TradeCurrency.Favor) ? this.tradeables[i].IsFavor : (this.tradeables[i].ThingDef == ThingDefOf.Silver))
					{
						return this.tradeables[i];
					}
				}
				return null;
			}
		}

		// Token: 0x170020B9 RID: 8377
		// (get) Token: 0x0600B52D RID: 46381 RVA: 0x003FCEE6 File Offset: 0x003FB0E6
		public List<Tradeable> AllTradeables
		{
			get
			{
				return this.tradeables;
			}
		}

		// Token: 0x0600B52E RID: 46382 RVA: 0x003FCEEE File Offset: 0x003FB0EE
		public TradeDeal()
		{
			this.Reset();
		}

		// Token: 0x0600B52F RID: 46383 RVA: 0x003FCF12 File Offset: 0x003FB112
		public void Reset()
		{
			this.tradeables.Clear();
			this.cannotSellReasons.Clear();
			this.AddAllTradeables();
		}

		// Token: 0x0600B530 RID: 46384 RVA: 0x003FCF30 File Offset: 0x003FB130
		private void AddAllTradeables()
		{
			foreach (Thing t in TradeSession.trader.ColonyThingsWillingToBuy(TradeSession.playerNegotiator))
			{
				if (TradeUtility.PlayerSellableNow(t, TradeSession.trader))
				{
					string text;
					if (!TradeSession.playerNegotiator.IsWorldPawn() && !this.InSellablePosition(t, out text))
					{
						if (text != null && !this.cannotSellReasons.Contains(text))
						{
							this.cannotSellReasons.Add(text);
						}
					}
					else
					{
						this.AddToTradeables(t, Transactor.Colony);
					}
				}
			}
			if (!TradeSession.giftMode)
			{
				foreach (Thing t2 in TradeSession.trader.Goods)
				{
					this.AddToTradeables(t2, Transactor.Trader);
				}
			}
			if (!TradeSession.giftMode)
			{
				if (this.tradeables.Find((Tradeable x) => x.IsCurrency) == null)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
					thing.stackCount = 0;
					this.AddToTradeables(thing, Transactor.Trader);
				}
			}
			if (TradeSession.TradeCurrency == TradeCurrency.Favor)
			{
				this.tradeables.Add(new Tradeable_RoyalFavor());
			}
		}

		// Token: 0x0600B531 RID: 46385 RVA: 0x003FD07C File Offset: 0x003FB27C
		private bool InSellablePosition(Thing t, out string reason)
		{
			if (!t.Spawned && (!ModsConfig.BiotechActive || t.def != ThingDefOf.Genepack || !(t.ParentHolder is CompGenepackContainer)) && (!(t is Book) || !(t.ParentHolder is Building_Bookcase)) && !(t.ParentHolder is Building_OutfitStand))
			{
				reason = null;
				return false;
			}
			IntVec3 positionHeld = t.PositionHeld;
			Room room = t.SpawnedParentOrMe.GetRoom(RegionType.Set_All);
			if (positionHeld.Fogged(t.MapHeld))
			{
				reason = null;
				return false;
			}
			if (room != null)
			{
				int num = GenRadial.NumCellsInRadius(6.9f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = positionHeld + GenRadial.RadialPattern[i];
					if (intVec.InBounds(t.MapHeld) && intVec.GetRoom(t.MapHeld) == room)
					{
						List<Thing> thingList = intVec.GetThingList(t.MapHeld);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].PreventPlayerSellingThingsNearby(out reason))
							{
								return false;
							}
						}
					}
				}
			}
			reason = null;
			return true;
		}

		// Token: 0x0600B532 RID: 46386 RVA: 0x003FD18C File Offset: 0x003FB38C
		private void AddToTradeables(Thing t, Transactor trans)
		{
			Tradeable tradeable = TransferableUtility.TradeableMatching(t, this.tradeables);
			if (tradeable == null)
			{
				if (t is Pawn)
				{
					tradeable = new Tradeable_Pawn();
				}
				else
				{
					tradeable = new Tradeable();
				}
				this.tradeables.Add(tradeable);
			}
			tradeable.AddThing(t, trans);
		}

		// Token: 0x0600B533 RID: 46387 RVA: 0x003FD1D4 File Offset: 0x003FB3D4
		public void UpdateCurrencyCount()
		{
			if (this.CurrencyTradeable == null || TradeSession.giftMode)
			{
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.tradeables.Count; i++)
			{
				Tradeable tradeable = this.tradeables[i];
				if (!tradeable.IsCurrency)
				{
					num += tradeable.CurTotalCurrencyCostForSource;
				}
			}
			this.CurrencyTradeable.ForceToSource(-this.CurrencyTradeable.CostToInt(num));
		}

		// Token: 0x0600B534 RID: 46388 RVA: 0x003FD244 File Offset: 0x003FB444
		public bool TryExecute(out bool actuallyTraded)
		{
			if (TradeSession.giftMode)
			{
				this.UpdateCurrencyCount();
				this.LimitCurrencyCountToFunds();
				int goodwillChange = FactionGiftUtility.GetGoodwillChange(this.tradeables, TradeSession.trader.Faction);
				FactionGiftUtility.GiveGift(this.tradeables, TradeSession.trader.Faction, TradeSession.playerNegotiator);
				actuallyTraded = ((float)goodwillChange > 0f);
				return true;
			}
			if (this.CurrencyTradeable == null || this.CurrencyTradeable.CountPostDealFor(Transactor.Colony) < 0)
			{
				Find.WindowStack.WindowOfType<Dialog_Trade>().FlashSilver();
				Messages.Message("MessageColonyCannotAfford".Translate(), MessageTypeDefOf.RejectInput, false);
				actuallyTraded = false;
				return false;
			}
			if (ModsConfig.IdeologyActive)
			{
				if (this.tradeables.Any((Tradeable x) => x.ActionToDo != TradeAction.None && x.ThingDef != null && x.ThingDef.IsNaturalOrgan))
				{
					HistoryEvent historyEvent = new HistoryEvent(HistoryEventDefOf.TradedOrgan, TradeSession.playerNegotiator.Named(HistoryEventArgsNames.Doer));
					if (!historyEvent.Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"))
					{
						actuallyTraded = false;
						return false;
					}
					Find.HistoryEventsManager.RecordEvent(historyEvent, true);
				}
				if (this.tradeables.Any((Tradeable x) => x.ActionToDo == TradeAction.PlayerSells && x.ThingDef != null && x.ThingDef.IsNaturalOrgan))
				{
					HistoryEvent historyEvent2 = new HistoryEvent(HistoryEventDefOf.SoldOrgan, TradeSession.playerNegotiator.Named(HistoryEventArgsNames.Doer));
					if (!historyEvent2.Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"))
					{
						actuallyTraded = false;
						return false;
					}
					Find.HistoryEventsManager.RecordEvent(historyEvent2, true);
				}
			}
			this.UpdateCurrencyCount();
			this.LimitCurrencyCountToFunds();
			actuallyTraded = false;
			float num = 0f;
			foreach (Tradeable tradeable in this.tradeables)
			{
				if (tradeable.ActionToDo != TradeAction.None)
				{
					actuallyTraded = true;
				}
				if (tradeable.ActionToDo == TradeAction.PlayerSells)
				{
					num += tradeable.CurTotalCurrencyCostForDestination;
				}
				tradeable.ResolveTrade();
			}
			this.Reset();
			if (TradeSession.trader.Faction != null)
			{
				TradeSession.trader.Faction.Notify_PlayerTraded(num, TradeSession.playerNegotiator);
			}
			Pawn pawn = TradeSession.trader as Pawn;
			if (pawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.TradedWith, new object[]
				{
					TradeSession.playerNegotiator,
					pawn
				});
			}
			if (actuallyTraded)
			{
				TradeSession.playerNegotiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Trade);
			}
			return true;
		}

		// Token: 0x0600B535 RID: 46389 RVA: 0x003FD4B0 File Offset: 0x003FB6B0
		public bool DoesTraderHaveEnoughSilver()
		{
			return TradeSession.giftMode || (this.CurrencyTradeable != null && this.CurrencyTradeable.CountPostDealFor(Transactor.Trader) >= 0);
		}

		// Token: 0x0600B536 RID: 46390 RVA: 0x003FD4D8 File Offset: 0x003FB6D8
		private void LimitCurrencyCountToFunds()
		{
			if (this.CurrencyTradeable == null)
			{
				return;
			}
			if (this.CurrencyTradeable.CountToTransferToSource > this.CurrencyTradeable.CountHeldBy(Transactor.Trader))
			{
				this.CurrencyTradeable.ForceToSource(this.CurrencyTradeable.CountHeldBy(Transactor.Trader));
			}
			if (this.CurrencyTradeable.CountToTransferToDestination > this.CurrencyTradeable.CountHeldBy(Transactor.Colony))
			{
				this.CurrencyTradeable.ForceToDestination(this.CurrencyTradeable.CountHeldBy(Transactor.Colony));
			}
		}

		// Token: 0x0400682D RID: 26669
		private List<Tradeable> tradeables = new List<Tradeable>();

		// Token: 0x0400682E RID: 26670
		public List<string> cannotSellReasons = new List<string>();
	}
}
