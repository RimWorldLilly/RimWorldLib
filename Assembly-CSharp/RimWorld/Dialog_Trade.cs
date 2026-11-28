using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DF3 RID: 7667
	[StaticConstructorOnStartup]
	public class Dialog_Trade : Window
	{
		// Token: 0x170024CE RID: 9422
		// (get) Token: 0x0600C75C RID: 51036 RVA: 0x00492B67 File Offset: 0x00490D67
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x170024CF RID: 9423
		// (get) Token: 0x0600C75D RID: 51037 RVA: 0x00492B79 File Offset: 0x00490D79
		private PlanetTile Tile
		{
			get
			{
				return TradeSession.playerNegotiator.Tile;
			}
		}

		// Token: 0x170024D0 RID: 9424
		// (get) Token: 0x0600C75E RID: 51038 RVA: 0x00492B85 File Offset: 0x00490D85
		private BiomeDef Biome
		{
			get
			{
				return Find.WorldGrid[this.Tile].PrimaryBiome;
			}
		}

		// Token: 0x170024D1 RID: 9425
		// (get) Token: 0x0600C75F RID: 51039 RVA: 0x00492B9C File Offset: 0x00490D9C
		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.Add(this.cachedCurrencyTradeable);
					}
					Building_PassengerShuttle shuttle = TradeSession.playerNegotiator.GetCaravan().Shuttle;
					if (shuttle != null)
					{
						this.cachedMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, IgnorePawnsInventoryMode.Ignore, true, false);
						this.cachedMassUsage -= shuttle.GetStatValue(StatDefOf.Mass, true, -1);
					}
					else
					{
						this.cachedMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, IgnorePawnsInventoryMode.Ignore, false, false);
					}
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast<Tradeable>();
					}
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x170024D2 RID: 9426
		// (get) Token: 0x0600C760 RID: 51040 RVA: 0x00492C5C File Offset: 0x00490E5C
		private float MassCapacity
		{
			get
			{
				if (this.massCapacityDirty)
				{
					this.massCapacityDirty = false;
					Building_PassengerShuttle shuttle = TradeSession.playerNegotiator.GetCaravan().Shuttle;
					if (shuttle != null)
					{
						this.cachedMassCapacity = shuttle.TransporterComp.MassCapacity;
						return this.cachedMassCapacity;
					}
					TradeSession.deal.UpdateCurrencyCount();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.Add(this.cachedCurrencyTradeable);
					}
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, stringBuilder);
					this.cachedMassCapacityExplanation = stringBuilder.ToString();
					if (this.cachedCurrencyTradeable != null)
					{
						this.cachedTradeables.RemoveLast<Tradeable>();
					}
				}
				return this.cachedMassCapacity;
			}
		}

		// Token: 0x170024D3 RID: 9427
		// (get) Token: 0x0600C761 RID: 51041 RVA: 0x00492D0C File Offset: 0x00490F0C
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
					if (caravan.Shuttle != null)
					{
						this.cachedTilesPerDayExplanation = "CaravanMovementSpeedShuttle".Translate();
						return 0f;
					}
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDayLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.MassUsage, this.MassCapacity, this.Tile, caravan.pather.Moving ? caravan.pather.nextTile : PlanetTile.Invalid, TradeSession.playerNegotiator.GetCaravan().Shuttle != null, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x170024D4 RID: 9428
		// (get) Token: 0x0600C762 RID: 51042 RVA: 0x00492DD8 File Offset: 0x00490FD8
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> DaysWorthOfFood
		{
			[return: TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					float item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore, Faction.OfPlayer);
					this.cachedDaysWorthOfFood = new ValueTuple<float, float>(item, DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Tile, IgnorePawnsInventoryMode.Ignore));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x170024D5 RID: 9429
		// (get) Token: 0x0600C763 RID: 51043 RVA: 0x00492E48 File Offset: 0x00491048
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> ForagedFoodPerDay
		{
			[return: TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			get
			{
				if (this.foragedFoodPerDayDirty)
				{
					this.foragedFoodPerDayDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDayLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedForagedFoodPerDay;
			}
		}

		// Token: 0x170024D6 RID: 9430
		// (get) Token: 0x0600C764 RID: 51044 RVA: 0x00492EAC File Offset: 0x004910AC
		private float Visibility
		{
			get
			{
				if (this.visibilityDirty)
				{
					this.visibilityDirty = false;
					TradeSession.deal.UpdateCurrencyCount();
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedVisibility = CaravanVisibilityCalculator.VisibilityLeftAfterTradeableTransfer(this.playerCaravanAllPawnsAndItems, this.cachedTradeables, stringBuilder);
					this.cachedVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedVisibility;
			}
		}

		// Token: 0x170024D7 RID: 9431
		// (get) Token: 0x0600C765 RID: 51045 RVA: 0x00492F02 File Offset: 0x00491102
		public override QuickSearchWidget CommonSearchWidget
		{
			get
			{
				return this.quickSearchWidget;
			}
		}

		// Token: 0x0600C766 RID: 51046 RVA: 0x00492F0C File Offset: 0x0049110C
		public Dialog_Trade(Pawn playerNegotiator, ITrader trader, bool giftsOnly = false) : base(null)
		{
			this.giftsOnly = giftsOnly;
			TradeSession.SetupWith(trader, playerNegotiator, giftsOnly);
			this.SetupPlayerCaravanVariables();
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (trader is PassingShip)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
			this.commonSearchWidgetOffset.x = this.commonSearchWidgetOffset.x + 18f;
			this.commonSearchWidgetOffset.y = this.commonSearchWidgetOffset.y - 18f;
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		// Token: 0x0600C767 RID: 51047 RVA: 0x00492FEA File Offset: 0x004911EA
		public override void PreOpen()
		{
			base.PreOpen();
			this.quickSearchWidget.Reset();
		}

		// Token: 0x0600C768 RID: 51048 RVA: 0x00493000 File Offset: 0x00491200
		public override void PostOpen()
		{
			base.PostOpen();
			if (!this.giftsOnly && !this.playerIsCaravan)
			{
				Pawn playerNegotiator = TradeSession.playerNegotiator;
				float level = playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Talking);
				float level2 = playerNegotiator.health.capacities.GetLevel(PawnCapacityDefOf.Hearing);
				if (level < 0.95f || level2 < 0.95f)
				{
					TaggedString taggedString;
					if (level < 0.95f)
					{
						taggedString = "NegotiatorTalkingImpaired".Translate(playerNegotiator.LabelShort, playerNegotiator);
					}
					else
					{
						taggedString = "NegotiatorHearingImpaired".Translate(playerNegotiator.LabelShort, playerNegotiator);
					}
					taggedString += "\n\n" + "NegotiatorCapacityImpaired".Translate();
					Find.WindowStack.Add(new Dialog_MessageBox(taggedString, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				}
			}
			this.CacheTradeables();
		}

		// Token: 0x0600C769 RID: 51049 RVA: 0x004930E8 File Offset: 0x004912E8
		private void CacheTradeables()
		{
			this.cachedCurrencyTradeable = TradeSession.deal.AllTradeables.FirstOrDefault((Tradeable x) => x.IsCurrency && (TradeSession.TradeCurrency != TradeCurrency.Favor || x.IsFavor));
			this.cachedTradeables = (from tr in TradeSession.deal.AllTradeables
			where !tr.IsCurrency && (tr.TraderWillTrade || !TradeSession.trader.TraderKind.hideThingsNotWillingToTrade)
			where this.quickSearchWidget.filter.Matches(tr.Label)
			select tr).OrderByDescending(delegate(Tradeable tr)
			{
				if (!tr.TraderWillTrade)
				{
					return -1;
				}
				return 0;
			}).ThenBy((Tradeable tr) => tr, this.sorter1.Comparer).ThenBy((Tradeable tr) => tr, this.sorter2.Comparer).ThenBy((Tradeable tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ThenBy((Tradeable tr) => tr.ThingDef.label).ThenBy(delegate(Tradeable tr)
			{
				QualityCategory result;
				if (tr.AnyThing.TryGetQuality(out result))
				{
					return (int)result;
				}
				return -1;
			}).ThenBy((Tradeable tr) => tr.AnyThing.HitPoints).ToList<Tradeable>();
			this.quickSearchWidget.noResultsMatched = !this.cachedTradeables.Any<Tradeable>();
		}

		// Token: 0x0600C76A RID: 51050 RVA: 0x004932A0 File Offset: 0x004914A0
		public override void DoWindowContents(Rect inRect)
		{
			if (this.playerIsCaravan)
			{
				CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, this.cachedMassCapacityExplanation, this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, -1f, -1f, null), null, this.Tile, null, -9999f, new Rect(12f, 0f, inRect.width - 24f, 40f), true, null, false, true);
				inRect.yMin += 52f;
			}
			TradeSession.deal.UpdateCurrencyCount();
			Widgets.BeginGroup(inRect);
			inRect = inRect.AtZero();
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTradeables();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTradeables();
			});
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(new Rect(0f, 27f, inRect.width / 2f, inRect.height / 2f), "NegotiatorTradeDialogInfo".Translate(TradeSession.playerNegotiator.Name.ToStringFull, TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true, -1).ToStringPercent()));
			float num = inRect.width - 590f;
			Rect rect = new Rect(num, 0f, inRect.width - num, 58f);
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Medium;
			Rect rect2 = new Rect(0f, 0f, rect.width / 2f, rect.height);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect2, Faction.OfPlayer.Name.Truncate(rect2.width, null));
			Rect rect3 = new Rect(rect.width / 2f, 0f, rect.width / 2f, rect.height);
			Text.Anchor = TextAnchor.UpperRight;
			string text = TradeSession.trader.TraderName;
			if (Text.CalcSize(text).x > rect3.width)
			{
				Text.Font = GameFont.Small;
				text = text.Truncate(rect3.width, null);
			}
			Widgets.Label(rect3, text);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(new Rect(rect.width / 2f, 27f, rect.width / 2f, rect.height / 2f), TradeSession.trader.TraderKind.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (!TradeSession.giftMode)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.6f);
				Text.Font = GameFont.Tiny;
				Rect rect4 = new Rect(rect.width / 2f - 100f - 30f, 0f, 200f, rect.height);
				Text.Anchor = TextAnchor.LowerCenter;
				Widgets.Label(rect4, "PositiveBuysNegativeSells".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			Widgets.EndGroup();
			float num2 = 0f;
			if (this.cachedCurrencyTradeable != null)
			{
				float num3 = inRect.width - 16f;
				Rect rect5 = new Rect(0f, 58f, num3, 30f);
				int countToTransfer = this.cachedCurrencyTradeable.CountToTransfer;
				TradeUI.DrawTradeableRow(rect5, this.cachedCurrencyTradeable, 1);
				if (countToTransfer != this.cachedCurrencyTradeable.CountToTransfer)
				{
					this.CountToTransferChanged();
				}
				GUI.color = Color.gray;
				Widgets.DrawLineHorizontal(0f, 87f, num3);
				GUI.color = Color.white;
				num2 = 30f;
			}
			Rect mainRect = new Rect(0f, 58f + num2, inRect.width, inRect.height - 58f - 38f - num2 - 20f);
			this.FillMainRect(mainRect);
			Text.Font = GameFont.Small;
			Rect rect6 = new Rect(inRect.width / 2f - Dialog_Trade.AcceptButtonSize.x / 2f, inRect.height - 55f, Dialog_Trade.AcceptButtonSize.x, Dialog_Trade.AcceptButtonSize.y);
			if (Widgets.ButtonText(rect6, TradeSession.giftMode ? ("OfferGifts".Translate() + " (" + FactionGiftUtility.GetGoodwillChange(TradeSession.deal.AllTradeables, TradeSession.trader.Faction).ToStringWithSign() + ")") : "AcceptButton".Translate(), true, true, true, null))
			{
				Action action = delegate()
				{
					bool flag;
					if (TradeSession.deal.TryExecute(out flag))
					{
						if (flag)
						{
							SoundDefOf.ExecuteTrade.PlayOneShotOnCamera(null);
							Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
							if (caravan != null)
							{
								caravan.RecacheInventory();
							}
							this.Close(false);
							return;
						}
						this.Close(true);
					}
				};
				if (TradeSession.deal.DoesTraderHaveEnoughSilver())
				{
					action();
				}
				else
				{
					this.FlashSilver();
					SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmTraderShortFunds".Translate(), action, false, null, WindowLayer.Dialog));
				}
				Event.current.Use();
			}
			if (Widgets.ButtonText(new Rect(rect6.x - 10f - Dialog_Trade.OtherBottomButtonSize.x, rect6.y, Dialog_Trade.OtherBottomButtonSize.x, Dialog_Trade.OtherBottomButtonSize.y), "ResetButton".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				TradeSession.deal.Reset();
				this.CacheTradeables();
				this.CountToTransferChanged();
			}
			if (Widgets.ButtonText(new Rect(rect6.xMax + 10f, rect6.y, Dialog_Trade.OtherBottomButtonSize.x, Dialog_Trade.OtherBottomButtonSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
				Event.current.Use();
			}
			float y = Dialog_Trade.OtherBottomButtonSize.y;
			Rect rect7 = new Rect(inRect.width - y, rect6.y, y, y);
			if (Widgets.ButtonImageWithBG(rect7, Dialog_Trade.ShowSellableItemsIcon, new Vector2?(new Vector2(32f, 32f))))
			{
				Find.WindowStack.Add(new Dialog_SellableItems(TradeSession.trader));
			}
			TooltipHandler.TipRegionByKey(rect7, "CommandShowSellableItemsDesc");
			Faction faction = TradeSession.trader.Faction;
			if (faction != null && !this.giftsOnly && !faction.def.permanentEnemy && TradeSession.trader.TradeCurrency != TradeCurrency.Favor)
			{
				Rect rect8 = new Rect(rect7.x - y - 4f, rect6.y, y, y);
				if (TradeSession.giftMode)
				{
					if (Widgets.ButtonImageWithBG(rect8, Dialog_Trade.TradeModeIcon, new Vector2?(new Vector2(32f, 32f))))
					{
						TradeSession.giftMode = false;
						TradeSession.deal.Reset();
						this.CacheTradeables();
						this.CountToTransferChanged();
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					TooltipHandler.TipRegionByKey(rect8, "TradeModeTip");
				}
				else
				{
					if (Widgets.ButtonImageWithBG(rect8, Dialog_Trade.GiftModeIcon, new Vector2?(new Vector2(32f, 32f))))
					{
						TradeSession.giftMode = true;
						TradeSession.deal.Reset();
						this.CacheTradeables();
						this.CountToTransferChanged();
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					TooltipHandler.TipRegionByKey(rect8, "GiftModeTip", faction.Name);
				}
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600C76B RID: 51051 RVA: 0x00493A30 File Offset: 0x00491C30
		public override void Close(bool doCloseSound = true)
		{
			DragSliderManager.ForceStop();
			base.Close(doCloseSound);
			Pawn pawn = TradeSession.trader as Pawn;
			if (pawn != null && pawn.mindState.hasQuest)
			{
				TradeUtility.ReceiveQuestFromTrader(pawn, TradeSession.playerNegotiator);
			}
		}

		// Token: 0x0600C76C RID: 51052 RVA: 0x00493A70 File Offset: 0x00491C70
		private void FillMainRect(Rect mainRect)
		{
			Text.Font = GameFont.Small;
			float height = 6f + (float)this.cachedTradeables.Count * 30f;
			Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, height);
			Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
			float num = 6f;
			float num2 = this.scrollPosition.y - 30f;
			float num3 = this.scrollPosition.y + mainRect.height;
			int num4 = 0;
			for (int i = 0; i < this.cachedTradeables.Count; i++)
			{
				if (num > num2 && num < num3)
				{
					Rect rect = new Rect(0f, num, viewRect.width, 30f);
					int countToTransfer = this.cachedTradeables[i].CountToTransfer;
					TradeUI.DrawTradeableRow(rect, this.cachedTradeables[i], num4);
					if (countToTransfer != this.cachedTradeables[i].CountToTransfer)
					{
						this.CountToTransferChanged();
					}
				}
				num += 30f;
				num4++;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600C76D RID: 51053 RVA: 0x00493B8D File Offset: 0x00491D8D
		public void FlashSilver()
		{
			Dialog_Trade.lastCurrencyFlashTime = Time.time;
		}

		// Token: 0x0600C76E RID: 51054 RVA: 0x00493B99 File Offset: 0x00491D99
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x0600C76F RID: 51055 RVA: 0x00493B9C File Offset: 0x00491D9C
		private void SetupPlayerCaravanVariables()
		{
			Caravan caravan = TradeSession.playerNegotiator.GetCaravan();
			if (caravan != null)
			{
				this.playerIsCaravan = true;
				this.playerCaravanAllPawnsAndItems = new List<Thing>();
				List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					this.playerCaravanAllPawnsAndItems.Add(pawnsListForReading[i]);
				}
				this.playerCaravanAllPawnsAndItems.AddRange(CaravanInventoryUtility.AllInventoryItems(caravan));
				caravan.Notify_StartedTrading();
				return;
			}
			this.playerIsCaravan = false;
		}

		// Token: 0x0600C770 RID: 51056 RVA: 0x00493C12 File Offset: 0x00491E12
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
		}

		// Token: 0x0600C771 RID: 51057 RVA: 0x00493C3E File Offset: 0x00491E3E
		public override void Notify_CommonSearchChanged()
		{
			this.CacheTradeables();
		}

		// Token: 0x04007196 RID: 29078
		private bool giftsOnly;

		// Token: 0x04007197 RID: 29079
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04007198 RID: 29080
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x04007199 RID: 29081
		public static float lastCurrencyFlashTime = -100f;

		// Token: 0x0400719A RID: 29082
		private List<Tradeable> cachedTradeables;

		// Token: 0x0400719B RID: 29083
		private Tradeable cachedCurrencyTradeable;

		// Token: 0x0400719C RID: 29084
		private TransferableSorterDef sorter1;

		// Token: 0x0400719D RID: 29085
		private TransferableSorterDef sorter2;

		// Token: 0x0400719E RID: 29086
		private bool playerIsCaravan;

		// Token: 0x0400719F RID: 29087
		private bool isShuttleCaravan;

		// Token: 0x040071A0 RID: 29088
		private List<Thing> playerCaravanAllPawnsAndItems;

		// Token: 0x040071A1 RID: 29089
		private bool massUsageDirty = true;

		// Token: 0x040071A2 RID: 29090
		private float cachedMassUsage;

		// Token: 0x040071A3 RID: 29091
		private bool massCapacityDirty = true;

		// Token: 0x040071A4 RID: 29092
		private float cachedMassCapacity;

		// Token: 0x040071A5 RID: 29093
		private string cachedMassCapacityExplanation;

		// Token: 0x040071A6 RID: 29094
		private bool tilesPerDayDirty = true;

		// Token: 0x040071A7 RID: 29095
		private float cachedTilesPerDay;

		// Token: 0x040071A8 RID: 29096
		private string cachedTilesPerDayExplanation;

		// Token: 0x040071A9 RID: 29097
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x040071AA RID: 29098
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedDaysWorthOfFood;

		// Token: 0x040071AB RID: 29099
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x040071AC RID: 29100
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x040071AD RID: 29101
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x040071AE RID: 29102
		private bool visibilityDirty = true;

		// Token: 0x040071AF RID: 29103
		private float cachedVisibility;

		// Token: 0x040071B0 RID: 29104
		private string cachedVisibilityExplanation;

		// Token: 0x040071B1 RID: 29105
		private const float TitleAreaHeight = 45f;

		// Token: 0x040071B2 RID: 29106
		private const float TopAreaHeight = 58f;

		// Token: 0x040071B3 RID: 29107
		private const float ColumnWidth = 120f;

		// Token: 0x040071B4 RID: 29108
		private const float FirstCommodityY = 6f;

		// Token: 0x040071B5 RID: 29109
		private const float RowInterval = 30f;

		// Token: 0x040071B6 RID: 29110
		private const float SpaceBetweenTraderNameAndTraderKind = 27f;

		// Token: 0x040071B7 RID: 29111
		private const float ShowSellableItemsIconSize = 32f;

		// Token: 0x040071B8 RID: 29112
		private const float GiftModeIconSize = 32f;

		// Token: 0x040071B9 RID: 29113
		private const float TradeModeIconSize = 32f;

		// Token: 0x040071BA RID: 29114
		protected static readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		// Token: 0x040071BB RID: 29115
		protected static readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x040071BC RID: 29116
		private static readonly Texture2D ShowSellableItemsIcon = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		// Token: 0x040071BD RID: 29117
		private static readonly Texture2D GiftModeIcon = ContentFinder<Texture2D>.Get("UI/Buttons/GiftMode", true);

		// Token: 0x040071BE RID: 29118
		private static readonly Texture2D TradeModeIcon = ContentFinder<Texture2D>.Get("UI/Buttons/TradeMode", true);
	}
}
