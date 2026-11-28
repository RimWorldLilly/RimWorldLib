using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B80 RID: 7040
	public class StockGenerator_MarketValue : StockGenerator_MiscItems
	{
		// Token: 0x0600B4B7 RID: 46263 RVA: 0x003FB14C File Offset: 0x003F934C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return base.HandlesThingDef(thingDef) && thingDef.tradeTags.NotNullAndContains(this.tradeTag) && (this.weaponTag.NullOrEmpty() || thingDef.weaponTags.NotNullAndContains(this.weaponTag)) && (this.apparelTag.NullOrEmpty() || (thingDef.apparel != null && thingDef.apparel.tags.NotNullAndContains(this.apparelTag)));
		}

		// Token: 0x0600B4B8 RID: 46264 RVA: 0x003FB1C6 File Offset: 0x003F93C6
		protected override float SelectionWeight(ThingDef thingDef)
		{
			return StockGenerator_MarketValue.SelectionWeightMarketValueCurve.Evaluate(thingDef.BaseMarketValue);
		}

		// Token: 0x040067F6 RID: 26614
		private static readonly SimpleCurve SelectionWeightMarketValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(500f, 0.5f),
				true
			},
			{
				new CurvePoint(1500f, 0.2f),
				true
			},
			{
				new CurvePoint(5000f, 0.1f),
				true
			}
		};

		// Token: 0x040067F7 RID: 26615
		[NoTranslate]
		public string tradeTag;

		// Token: 0x040067F8 RID: 26616
		[NoTranslate]
		public string weaponTag;

		// Token: 0x040067F9 RID: 26617
		[NoTranslate]
		public string apparelTag;
	}
}
