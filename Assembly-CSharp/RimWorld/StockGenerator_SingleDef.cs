using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B86 RID: 7046
	public class StockGenerator_SingleDef : StockGenerator
	{
		// Token: 0x0600B4D6 RID: 46294 RVA: 0x003FB842 File Offset: 0x003F9A42
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_SingleDef.<GenerateThings>d__1 <GenerateThings>d__ = new StockGenerator_SingleDef.<GenerateThings>d__1(-2);
			<GenerateThings>d__.<>4__this = this;
			<GenerateThings>d__.<>3__faction = faction;
			return <GenerateThings>d__;
		}

		// Token: 0x0600B4D7 RID: 46295 RVA: 0x003FB859 File Offset: 0x003F9A59
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x0600B4D8 RID: 46296 RVA: 0x003FB864 File Offset: 0x003F9A64
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			StockGenerator_SingleDef.<ConfigErrors>d__3 <ConfigErrors>d__ = new StockGenerator_SingleDef.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04006808 RID: 26632
		private ThingDef thingDef;
	}
}
