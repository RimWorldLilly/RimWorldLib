using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B96 RID: 7062
	public static class TradeSession
	{
		// Token: 0x170020BA RID: 8378
		// (get) Token: 0x0600B53A RID: 46394 RVA: 0x003FD60B File Offset: 0x003FB80B
		public static bool Active
		{
			get
			{
				return TradeSession.trader != null;
			}
		}

		// Token: 0x170020BB RID: 8379
		// (get) Token: 0x0600B53B RID: 46395 RVA: 0x003FD615 File Offset: 0x003FB815
		public static TradeCurrency TradeCurrency
		{
			get
			{
				return TradeSession.trader.TradeCurrency;
			}
		}

		// Token: 0x0600B53C RID: 46396 RVA: 0x003FD624 File Offset: 0x003FB824
		public static void SetupWith(ITrader newTrader, Pawn newPlayerNegotiator, bool giftMode)
		{
			if (!newTrader.CanTradeNow)
			{
				Log.Warning("Called SetupWith with a trader not willing to trade now.");
			}
			TradeSession.trader = newTrader;
			TradeSession.playerNegotiator = newPlayerNegotiator;
			TradeSession.giftMode = giftMode;
			TradeSession.deal = new TradeDeal();
			if (!giftMode && TradeSession.deal.cannotSellReasons.Count > 0)
			{
				Messages.Message("MessageCannotSellItemsReason".Translate() + TradeSession.deal.cannotSellReasons.ToCommaList(true, false).CapitalizeFirst(), MessageTypeDefOf.NegativeEvent, false);
			}
		}

		// Token: 0x0600B53D RID: 46397 RVA: 0x003FD6A9 File Offset: 0x003FB8A9
		public static void Close()
		{
			TradeSession.trader = null;
		}

		// Token: 0x04006835 RID: 26677
		public static ITrader trader;

		// Token: 0x04006836 RID: 26678
		public static Pawn playerNegotiator;

		// Token: 0x04006837 RID: 26679
		public static TradeDeal deal;

		// Token: 0x04006838 RID: 26680
		public static bool giftMode;
	}
}
