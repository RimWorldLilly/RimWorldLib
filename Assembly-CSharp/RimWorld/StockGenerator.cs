using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B79 RID: 7033
	public abstract class StockGenerator
	{
		// Token: 0x0600B496 RID: 46230 RVA: 0x003FAD34 File Offset: 0x003F8F34
		public virtual void ResolveReferences(TraderKindDef trader)
		{
			this.trader = trader;
		}

		// Token: 0x0600B497 RID: 46231 RVA: 0x003FAD3D File Offset: 0x003F8F3D
		public virtual IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x0600B498 RID: 46232
		public abstract IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null);

		// Token: 0x0600B499 RID: 46233
		public abstract bool HandlesThingDef(ThingDef thingDef);

		// Token: 0x0600B49A RID: 46234 RVA: 0x003FAD44 File Offset: 0x003F8F44
		public virtual Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (!this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			return thingDef.tradeability;
		}

		// Token: 0x0600B49B RID: 46235 RVA: 0x003FAD57 File Offset: 0x003F8F57
		public bool TryGetPriceType(ThingDef thingDef, TradeAction action, out PriceType priceType)
		{
			if (!this.HandlesThingDef(thingDef))
			{
				priceType = PriceType.Undefined;
				return false;
			}
			priceType = this.price;
			return true;
		}

		// Token: 0x0600B49C RID: 46236 RVA: 0x003FAD70 File Offset: 0x003F8F70
		protected int RandomCountOf(ThingDef def)
		{
			IntRange intRange = this.countRange;
			if (this.customCountRanges != null)
			{
				for (int i = 0; i < this.customCountRanges.Count; i++)
				{
					if (this.customCountRanges[i].thingDef == def)
					{
						intRange = this.customCountRanges[i].countRange;
						break;
					}
				}
			}
			if (intRange.max <= 0 && this.totalPriceRange.max <= 0f)
			{
				return 0;
			}
			if (intRange.max > 0 && this.totalPriceRange.max <= 0f)
			{
				return intRange.RandomInRange;
			}
			if (intRange.max <= 0 && this.totalPriceRange.max > 0f)
			{
				return Mathf.RoundToInt(this.totalPriceRange.RandomInRange / def.BaseMarketValue);
			}
			int num = 0;
			int randomInRange;
			do
			{
				randomInRange = intRange.RandomInRange;
				num++;
			}
			while (num <= 100 && !this.totalPriceRange.Includes((float)randomInRange * def.BaseMarketValue));
			return randomInRange;
		}

		// Token: 0x040067EC RID: 26604
		[Unsaved(false)]
		public TraderKindDef trader;

		// Token: 0x040067ED RID: 26605
		public IntRange countRange = IntRange.Zero;

		// Token: 0x040067EE RID: 26606
		public List<ThingDefCountRangeClass> customCountRanges;

		// Token: 0x040067EF RID: 26607
		public FloatRange totalPriceRange = FloatRange.Zero;

		// Token: 0x040067F0 RID: 26608
		public TechLevel maxTechLevelGenerate = TechLevel.Archotech;

		// Token: 0x040067F1 RID: 26609
		public TechLevel maxTechLevelBuy = TechLevel.Archotech;

		// Token: 0x040067F2 RID: 26610
		public PriceType price = PriceType.Normal;
	}
}
