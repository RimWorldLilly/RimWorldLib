using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B85 RID: 7045
	public class StockGenerator_MultiDef : StockGenerator
	{
		// Token: 0x0600B4D1 RID: 46289 RVA: 0x003FB7EA File Offset: 0x003F99EA
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_MultiDef.<GenerateThings>d__1 <GenerateThings>d__ = new StockGenerator_MultiDef.<GenerateThings>d__1(-2);
			<GenerateThings>d__.<>4__this = this;
			<GenerateThings>d__.<>3__faction = faction;
			return <GenerateThings>d__;
		}

		// Token: 0x0600B4D2 RID: 46290 RVA: 0x003FB801 File Offset: 0x003F9A01
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		// Token: 0x0600B4D3 RID: 46291 RVA: 0x003FB80F File Offset: 0x003F9A0F
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			StockGenerator_MultiDef.<ConfigErrors>d__3 <ConfigErrors>d__ = new StockGenerator_MultiDef.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04006807 RID: 26631
		private List<ThingDef> thingDefs = new List<ThingDef>();
	}
}
