using System;

namespace RimWorld
{
	// Token: 0x02001DFA RID: 7674
	public class TransferableComparer_MarketValue : TransferableComparer
	{
		// Token: 0x0600C786 RID: 51078 RVA: 0x004945F0 File Offset: 0x004927F0
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.MarketValue.CompareTo(rhs.AnyThing.MarketValue);
		}
	}
}
