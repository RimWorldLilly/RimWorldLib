using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B7D RID: 7037
	public class StockGenerator_BuyTradeTag : StockGenerator
	{
		// Token: 0x0600B4A9 RID: 46249 RVA: 0x003FB04F File Offset: 0x003F924F
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x0600B4AA RID: 46250 RVA: 0x003FB056 File Offset: 0x003F9256
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeTags.Contains(this.tag);
		}

		// Token: 0x0600B4AB RID: 46251 RVA: 0x003FB073 File Offset: 0x003F9273
		public override Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (thingDef.tradeability == Tradeability.None || !this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			return Tradeability.Sellable;
		}

		// Token: 0x040067F5 RID: 26613
		public string tag;
	}
}
