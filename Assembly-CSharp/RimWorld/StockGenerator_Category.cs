using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B84 RID: 7044
	public class StockGenerator_Category : StockGenerator
	{
		// Token: 0x0600B4CE RID: 46286 RVA: 0x003FB72D File Offset: 0x003F992D
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_Category.<GenerateThings>d__4 <GenerateThings>d__ = new StockGenerator_Category.<GenerateThings>d__4(-2);
			<GenerateThings>d__.<>4__this = this;
			<GenerateThings>d__.<>3__faction = faction;
			return <GenerateThings>d__;
		}

		// Token: 0x0600B4CF RID: 46287 RVA: 0x003FB744 File Offset: 0x003F9944
		public override bool HandlesThingDef(ThingDef t)
		{
			return this.categoryDef.DescendantThingDefs.Contains(t) && t.tradeability != Tradeability.None && t.techLevel <= this.maxTechLevelBuy && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
		}

		// Token: 0x04006803 RID: 26627
		private ThingCategoryDef categoryDef;

		// Token: 0x04006804 RID: 26628
		private IntRange thingDefCountRange = IntRange.One;

		// Token: 0x04006805 RID: 26629
		private List<ThingDef> excludedThingDefs;

		// Token: 0x04006806 RID: 26630
		private List<ThingCategoryDef> excludedCategories;
	}
}
