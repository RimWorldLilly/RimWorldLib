using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8B RID: 7051
	public class StockGenerator_Techprints : StockGenerator
	{
		// Token: 0x0600B4E3 RID: 46307 RVA: 0x003FB9AA File Offset: 0x003F9BAA
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_Techprints.<GenerateThings>d__2 <GenerateThings>d__ = new StockGenerator_Techprints.<GenerateThings>d__2(-2);
			<GenerateThings>d__.<>4__this = this;
			<GenerateThings>d__.<>3__faction = faction;
			return <GenerateThings>d__;
		}

		// Token: 0x0600B4E4 RID: 46308 RVA: 0x003FB9C1 File Offset: 0x003F9BC1
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains("Techprint");
		}

		// Token: 0x04006810 RID: 26640
		private List<CountChance> countChances;

		// Token: 0x04006811 RID: 26641
		private List<ThingDef> tmpGenerated = new List<ThingDef>();
	}
}
