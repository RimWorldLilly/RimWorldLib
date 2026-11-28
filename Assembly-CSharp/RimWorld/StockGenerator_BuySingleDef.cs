using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B7C RID: 7036
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		// Token: 0x0600B4A5 RID: 46245 RVA: 0x003FB01F File Offset: 0x003F921F
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x0600B4A6 RID: 46246 RVA: 0x003FB026 File Offset: 0x003F9226
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x0600B4A7 RID: 46247 RVA: 0x003FB031 File Offset: 0x003F9231
		public override Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (thingDef.tradeability == Tradeability.None || !this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			return Tradeability.Sellable;
		}

		// Token: 0x040067F4 RID: 26612
		public ThingDef thingDef;
	}
}
