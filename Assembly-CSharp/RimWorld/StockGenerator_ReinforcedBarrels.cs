using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B81 RID: 7041
	public class StockGenerator_ReinforcedBarrels : StockGenerator
	{
		// Token: 0x0600B4BB RID: 46267 RVA: 0x003FB24F File Offset: 0x003F944F
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_ReinforcedBarrels.<GenerateThings>d__0 <GenerateThings>d__ = new StockGenerator_ReinforcedBarrels.<GenerateThings>d__0(-2);
			<GenerateThings>d__.<>4__this = this;
			<GenerateThings>d__.<>3__faction = faction;
			return <GenerateThings>d__;
		}

		// Token: 0x0600B4BC RID: 46268 RVA: 0x003FB266 File Offset: 0x003F9466
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == ThingDefOf.ReinforcedBarrel;
		}

		// Token: 0x0600B4BD RID: 46269 RVA: 0x003FB270 File Offset: 0x003F9470
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			return Enumerable.Empty<string>();
		}
	}
}
