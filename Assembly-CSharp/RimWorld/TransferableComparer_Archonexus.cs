using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DF7 RID: 7671
	public class TransferableComparer_Archonexus : TransferableComparer
	{
		// Token: 0x0600C77D RID: 51069 RVA: 0x0049445A File Offset: 0x0049265A
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return TransferableComparer_Archonexus.Compare(lhs.ThingDef, rhs.ThingDef);
		}

		// Token: 0x0600C77E RID: 51070 RVA: 0x00494470 File Offset: 0x00492670
		public static int Compare(ThingDef lhsTh, ThingDef rhsTh)
		{
			float num = TransferableUIUtility.DefaultArchonexusItemListOrderPriority(lhsTh);
			float num2 = TransferableUIUtility.DefaultArchonexusItemListOrderPriority(rhsTh);
			if (num != num2)
			{
				return num.CompareTo(num2);
			}
			return TransferableComparer_Category.Compare(lhsTh, rhsTh);
		}
	}
}
