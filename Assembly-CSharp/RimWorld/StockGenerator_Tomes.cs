using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B82 RID: 7042
	public class StockGenerator_Tomes : StockGenerator_SingleDef
	{
		// Token: 0x0600B4BF RID: 46271 RVA: 0x003FB27F File Offset: 0x003F947F
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_Tomes.<GenerateThings>d__0 <GenerateThings>d__ = new StockGenerator_Tomes.<GenerateThings>d__0(-2);
			<GenerateThings>d__.<>4__this = this;
			<GenerateThings>d__.<>3__faction = faction;
			return <GenerateThings>d__;
		}

		// Token: 0x0600B4C0 RID: 46272 RVA: 0x003FB296 File Offset: 0x003F9496
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return ModsConfig.AnomalyActive && Find.Storyteller.difficulty.AnomalyPlaystyleDef.enableAnomalyContent && thingDef == ThingDefOf.Tome;
		}

		// Token: 0x0600B4C1 RID: 46273 RVA: 0x003FB2C1 File Offset: 0x003F94C1
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			return new StockGenerator_Tomes.<ConfigErrors>d__2(-2);
		}
	}
}
