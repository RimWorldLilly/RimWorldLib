using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B65 RID: 7013
	public class ThingSetMaker_MarketValue : ThingSetMaker
	{
		// Token: 0x0600B401 RID: 46081 RVA: 0x003F7242 File Offset: 0x003F5442
		public ThingSetMaker_MarketValue()
		{
			this.nextSeed = Rand.Int;
		}

		// Token: 0x0600B402 RID: 46082 RVA: 0x003F7258 File Offset: 0x003F5458
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			List<ThingDef> list = this.AllowedThingDefs(parms).ToList<ThingDef>();
			float num;
			return list.Any<ThingDef>() && (parms.countRange == null || parms.countRange.Value.max > 0) && parms.totalMarketValueRange != null && parms.totalMarketValueRange.Value.max > 0f && (parms.maxTotalMass == null || parms.maxTotalMass.GetValueOrDefault() == float.MaxValue || ThingSetMakerUtility.PossibleToWeighNoMoreThan(list, parms.techLevel.GetValueOrDefault(), parms.maxTotalMass.Value, (parms.countRange != null) ? parms.countRange.Value.min : 1)) && this.GeneratePossibleDefs(parms, out num, this.nextSeed).Any<ThingStuffPairWithQuality>();
		}

		// Token: 0x0600B403 RID: 46083 RVA: 0x003F7348 File Offset: 0x003F5548
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			float valueOrDefault = parms.maxTotalMass.GetValueOrDefault(float.MaxValue);
			float totalValue;
			List<ThingStuffPairWithQuality> list = this.GeneratePossibleDefs(parms, out totalValue, this.nextSeed);
			for (int i = 0; i < list.Count; i++)
			{
				outThings.Add(list[i].MakeThing(false));
			}
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalValue, (Thing x) => x.MarketValue, valueOrDefault, true);
			this.nextSeed++;
		}

		// Token: 0x0600B404 RID: 46084 RVA: 0x003F73D5 File Offset: 0x003F55D5
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x0600B405 RID: 46085 RVA: 0x003F73DD File Offset: 0x003F55DD
		private float GetSingleThingValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue);
		}

		// Token: 0x0600B406 RID: 46086 RVA: 0x003F73EB File Offset: 0x003F55EB
		private float GetMinValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue) * (float)thingStuffPair.thing.minRewardCount;
		}

		// Token: 0x0600B407 RID: 46087 RVA: 0x003F7406 File Offset: 0x003F5606
		private float GetMaxValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue) * (float)thingStuffPair.thing.stackLimit;
		}

		// Token: 0x0600B408 RID: 46088 RVA: 0x003F7421 File Offset: 0x003F5621
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalMarketValue);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600B409 RID: 46089 RVA: 0x003F7438 File Offset: 0x003F5638
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (!enumerable.Any<ThingDef>())
			{
				totalMarketValue = 0f;
				return new List<ThingStuffPairWithQuality>();
			}
			TechLevel valueOrDefault = parms.techLevel.GetValueOrDefault();
			IntRange countRange = parms.countRange ?? new IntRange(1, int.MaxValue);
			FloatRange floatRange = parms.totalMarketValueRange ?? FloatRange.Zero;
			float valueOrDefault2 = parms.maxTotalMass.GetValueOrDefault(float.MaxValue);
			QualityGenerator valueOrDefault3 = parms.qualityGenerator.GetValueOrDefault();
			totalMarketValue = floatRange.RandomInRange;
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange, totalMarketValue, enumerable, valueOrDefault, valueOrDefault3, new Func<ThingStuffPairWithQuality, float>(this.GetMinValue), new Func<ThingStuffPairWithQuality, float>(this.GetMaxValue), new Func<ThingStuffPairWithQuality, float>(this.GetSingleThingValue), null, 100, valueOrDefault2, parms.allowNonStackableDuplicates.GetValueOrDefault(true), totalMarketValue * parms.minSingleItemMarketValuePct.GetValueOrDefault());
		}

		// Token: 0x0600B40A RID: 46090 RVA: 0x003F752C File Offset: 0x003F572C
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			ThingSetMaker_MarketValue.<AllGeneratableThingsDebugSub>d__10 <AllGeneratableThingsDebugSub>d__ = new ThingSetMaker_MarketValue.<AllGeneratableThingsDebugSub>d__10(-2);
			<AllGeneratableThingsDebugSub>d__.<>4__this = this;
			<AllGeneratableThingsDebugSub>d__.<>3__parms = parms;
			return <AllGeneratableThingsDebugSub>d__;
		}

		// Token: 0x040067C4 RID: 26564
		private int nextSeed;
	}
}
