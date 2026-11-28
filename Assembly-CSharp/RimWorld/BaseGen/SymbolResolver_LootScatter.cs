using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002106 RID: 8454
	public class SymbolResolver_LootScatter : SymbolResolver
	{
		// Token: 0x0600D453 RID: 54355 RVA: 0x004E7B4C File Offset: 0x004E5D4C
		public override void Resolve(ResolveParams rp)
		{
			float? lootMarketValue = rp.lootMarketValue;
			if (lootMarketValue != null && lootMarketValue.GetValueOrDefault() <= 0f)
			{
				return;
			}
			Map map = BaseGen.globalSettings.map;
			IList<Thing> list = rp.lootConcereteContents;
			if (list == null)
			{
				ThingSetMakerParams parms;
				if (rp.thingSetMakerParams != null)
				{
					parms = rp.thingSetMakerParams.Value;
				}
				else
				{
					parms = new ThingSetMakerParams
					{
						countRange = new IntRange?(SymbolResolver_LootScatter.DefaultLootCountRange),
						techLevel = new TechLevel?((rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Undefined)
					};
				}
				parms.makingFaction = rp.faction;
				parms.totalMarketValueRange = new FloatRange?(new FloatRange(rp.lootMarketValue.Value, rp.lootMarketValue.Value));
				list = rp.thingSetMakerDef.root.Generate(parms);
			}
			MapGenUtility.GenerateLoot(map, rp.rect, list, false);
		}

		// Token: 0x040086CF RID: 34511
		private static readonly IntRange DefaultLootCountRange = new IntRange(3, 10);
	}
}
