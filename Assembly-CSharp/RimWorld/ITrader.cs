using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B75 RID: 7029
	public interface ITrader
	{
		// Token: 0x17002087 RID: 8327
		// (get) Token: 0x0600B470 RID: 46192
		TraderKindDef TraderKind { get; }

		// Token: 0x17002088 RID: 8328
		// (get) Token: 0x0600B471 RID: 46193
		IEnumerable<Thing> Goods { get; }

		// Token: 0x17002089 RID: 8329
		// (get) Token: 0x0600B472 RID: 46194
		int RandomPriceFactorSeed { get; }

		// Token: 0x1700208A RID: 8330
		// (get) Token: 0x0600B473 RID: 46195
		string TraderName { get; }

		// Token: 0x1700208B RID: 8331
		// (get) Token: 0x0600B474 RID: 46196
		bool CanTradeNow { get; }

		// Token: 0x1700208C RID: 8332
		// (get) Token: 0x0600B475 RID: 46197
		float TradePriceImprovementOffsetForPlayer { get; }

		// Token: 0x1700208D RID: 8333
		// (get) Token: 0x0600B476 RID: 46198
		Faction Faction { get; }

		// Token: 0x1700208E RID: 8334
		// (get) Token: 0x0600B477 RID: 46199
		TradeCurrency TradeCurrency { get; }

		// Token: 0x0600B478 RID: 46200
		IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator);

		// Token: 0x0600B479 RID: 46201
		void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator);

		// Token: 0x0600B47A RID: 46202
		void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator);
	}
}
