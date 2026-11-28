using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B88 RID: 7048
	public class StockGenerator_Tag : StockGenerator
	{
		// Token: 0x0600B4DE RID: 46302 RVA: 0x003FB8D5 File Offset: 0x003F9AD5
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_Tag.<GenerateThings>d__3 <GenerateThings>d__ = new StockGenerator_Tag.<GenerateThings>d__3(-2);
			<GenerateThings>d__.<>4__this = this;
			<GenerateThings>d__.<>3__faction = faction;
			return <GenerateThings>d__;
		}

		// Token: 0x0600B4DF RID: 46303 RVA: 0x003FB8EC File Offset: 0x003F9AEC
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}

		// Token: 0x0600B4E0 RID: 46304 RVA: 0x003FB91F File Offset: 0x003F9B1F
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}

		// Token: 0x0400680B RID: 26635
		[NoTranslate]
		public string tradeTag;

		// Token: 0x0400680C RID: 26636
		private IntRange thingDefCountRange = IntRange.One;

		// Token: 0x0400680D RID: 26637
		private List<ThingDef> excludedThingDefs;
	}
}
