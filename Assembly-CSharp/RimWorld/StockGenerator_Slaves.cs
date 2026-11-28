using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B87 RID: 7047
	public class StockGenerator_Slaves : StockGenerator
	{
		// Token: 0x0600B4DB RID: 46299 RVA: 0x003FB88C File Offset: 0x003F9A8C
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_Slaves.<GenerateThings>d__2 <GenerateThings>d__ = new StockGenerator_Slaves.<GenerateThings>d__2(-2);
			<GenerateThings>d__.<>4__this = this;
			<GenerateThings>d__.<>3__forTile = forTile;
			<GenerateThings>d__.<>3__faction = faction;
			return <GenerateThings>d__;
		}

		// Token: 0x0600B4DC RID: 46300 RVA: 0x003FB8AA File Offset: 0x003F9AAA
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability > Tradeability.None;
		}

		// Token: 0x04006809 RID: 26633
		private bool respectPopulationIntent;

		// Token: 0x0400680A RID: 26634
		public PawnKindDef slaveKindDef;
	}
}
