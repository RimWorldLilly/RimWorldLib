using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B7E RID: 7038
	public class StockGenerator_BuySlaves : StockGenerator
	{
		// Token: 0x0600B4AD RID: 46253 RVA: 0x003FB091 File Offset: 0x003F9291
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x0600B4AE RID: 46254 RVA: 0x003FB098 File Offset: 0x003F9298
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability > Tradeability.None;
		}

		// Token: 0x0600B4AF RID: 46255 RVA: 0x003FB0BB File Offset: 0x003F92BB
		public override Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (!this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			return Tradeability.Sellable;
		}
	}
}
