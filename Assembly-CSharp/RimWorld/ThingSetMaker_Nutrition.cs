using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B66 RID: 7014
	public class ThingSetMaker_Nutrition : ThingSetMaker
	{
		// Token: 0x0600B40B RID: 46091 RVA: 0x003F7543 File Offset: 0x003F5743
		public ThingSetMaker_Nutrition()
		{
			this.nextSeed = Rand.Int;
		}

		// Token: 0x0600B40C RID: 46092 RVA: 0x003F7558 File Offset: 0x003F5758
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			float num;
			return this.AllowedThingDefs(parms).Any<ThingDef>() && (parms.countRange == null || parms.countRange.Value.max > 0) && parms.totalNutritionRange != null && parms.totalNutritionRange.Value.max > 0f && (parms.maxTotalMass == null || parms.maxTotalMass.GetValueOrDefault() == float.MaxValue || ThingSetMakerUtility.PossibleToWeighNoMoreThan(this.AllowedThingDefs(parms), parms.techLevel.GetValueOrDefault(), parms.maxTotalMass.Value, (parms.countRange != null) ? parms.countRange.Value.min : 1)) && this.GeneratePossibleDefs(parms, out num, this.nextSeed).Any<ThingStuffPairWithQuality>();
		}

		// Token: 0x0600B40D RID: 46093 RVA: 0x003F7648 File Offset: 0x003F5848
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			float valueOrDefault = parms.maxTotalMass.GetValueOrDefault(float.MaxValue);
			float totalValue;
			List<ThingStuffPairWithQuality> list = this.GeneratePossibleDefs(parms, out totalValue, this.nextSeed);
			for (int i = 0; i < list.Count; i++)
			{
				outThings.Add(list[i].MakeThing(false));
			}
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalValue, (Thing x) => x.GetStatValue(StatDefOf.Nutrition, true, -1), valueOrDefault, false);
			this.nextSeed++;
		}

		// Token: 0x0600B40E RID: 46094 RVA: 0x003F76D5 File Offset: 0x003F58D5
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x0600B40F RID: 46095 RVA: 0x003F76DD File Offset: 0x003F58DD
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalNutrition);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600B410 RID: 46096 RVA: 0x003F76F4 File Offset: 0x003F58F4
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition)
		{
			ThingSetMaker_Nutrition.<>c__DisplayClass6_0 CS$<>8__locals1 = new ThingSetMaker_Nutrition.<>c__DisplayClass6_0();
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (!enumerable.Any<ThingDef>())
			{
				totalNutrition = 0f;
				return new List<ThingStuffPairWithQuality>();
			}
			IntRange countRange = parms.countRange ?? new IntRange(1, int.MaxValue);
			FloatRange floatRange = parms.totalNutritionRange ?? FloatRange.Zero;
			TechLevel valueOrDefault = parms.techLevel.GetValueOrDefault();
			float valueOrDefault2 = parms.maxTotalMass.GetValueOrDefault(float.MaxValue);
			QualityGenerator valueOrDefault3 = parms.qualityGenerator.GetValueOrDefault();
			totalNutrition = floatRange.RandomInRange;
			CS$<>8__locals1.numMeats = enumerable.Count((ThingDef x) => x.IsMeat);
			CS$<>8__locals1.numLeathers = enumerable.Count((ThingDef x) => x.IsLeather);
			CS$<>8__locals1.numEggs = enumerable.Count((ThingDef x) => x.IsEgg);
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange, totalNutrition, enumerable, valueOrDefault, valueOrDefault3, (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition), (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition) * (float)x.thing.stackLimit, (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition), new Func<ThingDef, float>(CS$<>8__locals1.<GeneratePossibleDefs>g__WeightSelector|3), 100, valueOrDefault2, true, 0f);
		}

		// Token: 0x0600B411 RID: 46097 RVA: 0x003F789F File Offset: 0x003F5A9F
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			ThingSetMaker_Nutrition.<AllGeneratableThingsDebugSub>d__7 <AllGeneratableThingsDebugSub>d__ = new ThingSetMaker_Nutrition.<AllGeneratableThingsDebugSub>d__7(-2);
			<AllGeneratableThingsDebugSub>d__.<>4__this = this;
			<AllGeneratableThingsDebugSub>d__.<>3__parms = parms;
			return <AllGeneratableThingsDebugSub>d__;
		}

		// Token: 0x040067C5 RID: 26565
		private int nextSeed;
	}
}
