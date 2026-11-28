using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B7B RID: 7035
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		// Token: 0x0600B4A1 RID: 46241 RVA: 0x003FAF8A File Offset: 0x003F918A
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x0600B4A2 RID: 46242 RVA: 0x003FAF94 File Offset: 0x003F9194
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && thingDef.genericMarketSellable && (thingDef == ThingDefOf.InsectJelly || thingDef.BaseMarketValue / thingDef.VolumePerUnit >= this.minValuePerUnit);
		}

		// Token: 0x0600B4A3 RID: 46243 RVA: 0x003FAFF6 File Offset: 0x003F91F6
		public override Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (thingDef.tradeability == Tradeability.None || !this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			return Tradeability.Sellable;
		}

		// Token: 0x040067F3 RID: 26611
		public float minValuePerUnit = 15f;
	}
}
