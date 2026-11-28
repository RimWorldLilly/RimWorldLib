using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B7A RID: 7034
	public class StockGeneratorUtility
	{
		// Token: 0x0600B49E RID: 46238 RVA: 0x003FAE9A File Offset: 0x003F909A
		public static IEnumerable<Thing> TryMakeForStock(ThingDef thingDef, int count, Faction faction)
		{
			StockGeneratorUtility.<TryMakeForStock>d__0 <TryMakeForStock>d__ = new StockGeneratorUtility.<TryMakeForStock>d__0(-2);
			<TryMakeForStock>d__.<>3__thingDef = thingDef;
			<TryMakeForStock>d__.<>3__count = count;
			<TryMakeForStock>d__.<>3__faction = faction;
			return <TryMakeForStock>d__;
		}

		// Token: 0x0600B49F RID: 46239 RVA: 0x003FAEB8 File Offset: 0x003F90B8
		public static Thing TryMakeForStockSingle(ThingDef thingDef, int stackCount, Faction faction)
		{
			if (stackCount <= 0)
			{
				return null;
			}
			if (!thingDef.tradeability.TraderCanSell())
			{
				string str = "Tried to make non-trader-sellable thing for trader stock: ";
				ThingDef thingDef2 = thingDef;
				Log.Error(str + ((thingDef2 != null) ? thingDef2.ToString() : null));
				return null;
			}
			ThingDef stuff = null;
			if (thingDef.MadeFromStuff)
			{
				if (!(from x in GenStuff.AllowedStuffsFor(thingDef, TechLevel.Undefined, true)
				where !PawnWeaponGenerator.IsDerpWeapon(thingDef, x)
				select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff))
				{
					stuff = GenStuff.RandomStuffByCommonalityFor(thingDef, TechLevel.Undefined);
				}
			}
			Thing thing = ThingMaker.MakeThing(thingDef, stuff);
			thing.stackCount = stackCount;
			return thing;
		}
	}
}
