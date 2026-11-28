using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DF8 RID: 7672
	public class TransferableComparer_Category : TransferableComparer
	{
		// Token: 0x0600C780 RID: 51072 RVA: 0x004944A7 File Offset: 0x004926A7
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return TransferableComparer_Category.Compare(lhs.ThingDef, rhs.ThingDef);
		}

		// Token: 0x0600C781 RID: 51073 RVA: 0x004944BC File Offset: 0x004926BC
		public static int Compare(ThingDef lhsTh, ThingDef rhsTh)
		{
			if (lhsTh.category != rhsTh.category)
			{
				return lhsTh.category.CompareTo(rhsTh.category);
			}
			float num = TransferableUIUtility.DefaultListOrderPriority(lhsTh);
			float num2 = TransferableUIUtility.DefaultListOrderPriority(rhsTh);
			if (num != num2)
			{
				return num.CompareTo(num2);
			}
			int num3 = 0;
			if (!lhsTh.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				num3 = (int)lhsTh.thingCategories[0].index;
			}
			int value = 0;
			if (!rhsTh.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				value = (int)rhsTh.thingCategories[0].index;
			}
			return num3.CompareTo(value);
		}
	}
}
