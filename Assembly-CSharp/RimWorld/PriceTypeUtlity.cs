using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001DF5 RID: 7669
	public static class PriceTypeUtlity
	{
		// Token: 0x0600C777 RID: 51063 RVA: 0x00493D3C File Offset: 0x00491F3C
		public static float PriceMultiplier(this PriceType pType)
		{
			switch (pType)
			{
			case PriceType.VeryCheap:
				return 0.4f;
			case PriceType.Cheap:
				return 0.7f;
			case PriceType.Normal:
				return 1f;
			case PriceType.Expensive:
				return 2f;
			case PriceType.Exorbitant:
				return 5f;
			default:
				return -1f;
			}
		}

		// Token: 0x0600C778 RID: 51064 RVA: 0x00493D8C File Offset: 0x00491F8C
		public static PriceType ClosestPriceType(float priceFactor)
		{
			float num = 99999f;
			PriceType priceType = PriceType.Undefined;
			foreach (object obj in Enum.GetValues(typeof(PriceType)))
			{
				PriceType priceType2 = (PriceType)obj;
				float num2 = Mathf.Abs(priceFactor - priceType2.PriceMultiplier());
				if (num2 < num)
				{
					num = num2;
					priceType = priceType2;
				}
			}
			if (priceType == PriceType.Undefined)
			{
				priceType = PriceType.VeryCheap;
			}
			return priceType;
		}
	}
}
