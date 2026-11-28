using System;

namespace RimWorld
{
	// Token: 0x02001B99 RID: 7065
	public static class TradeabilityUtility
	{
		// Token: 0x0600B55D RID: 46429 RVA: 0x003FDC55 File Offset: 0x003FBE55
		public static bool PlayerCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Sellable;
		}

		// Token: 0x0600B55E RID: 46430 RVA: 0x003FDC61 File Offset: 0x003FBE61
		public static bool TraderCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Buyable;
		}
	}
}
