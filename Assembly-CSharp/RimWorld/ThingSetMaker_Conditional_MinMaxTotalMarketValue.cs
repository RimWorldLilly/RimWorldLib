using System;

namespace RimWorld
{
	// Token: 0x02001B5F RID: 7007
	public class ThingSetMaker_Conditional_MinMaxTotalMarketValue : ThingSetMaker_Conditional
	{
		// Token: 0x0600B3E7 RID: 46055 RVA: 0x003F6949 File Offset: 0x003F4B49
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return parms.totalMarketValueRange != null && parms.totalMarketValueRange.Value.max >= this.minMaxTotalMarketValue;
		}

		// Token: 0x040067BD RID: 26557
		public float minMaxTotalMarketValue;
	}
}
