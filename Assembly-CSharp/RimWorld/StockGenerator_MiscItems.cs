using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B7F RID: 7039
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		// Token: 0x0600B4B1 RID: 46257 RVA: 0x003FB0D1 File Offset: 0x003F92D1
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_MiscItems.<GenerateThings>d__0 <GenerateThings>d__ = new StockGenerator_MiscItems.<GenerateThings>d__0(-2);
			<GenerateThings>d__.<>4__this = this;
			<GenerateThings>d__.<>3__faction = faction;
			return <GenerateThings>d__;
		}

		// Token: 0x0600B4B2 RID: 46258 RVA: 0x003FB0E8 File Offset: 0x003F92E8
		protected virtual Thing MakeThing(ThingDef def, Faction faction)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1, faction);
		}

		// Token: 0x0600B4B3 RID: 46259 RVA: 0x003FB0F2 File Offset: 0x003F92F2
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy;
		}

		// Token: 0x0600B4B4 RID: 46260 RVA: 0x003FB10F File Offset: 0x003F930F
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
