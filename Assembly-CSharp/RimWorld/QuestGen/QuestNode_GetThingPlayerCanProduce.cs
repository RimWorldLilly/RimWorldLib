using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021E2 RID: 8674
	public class QuestNode_GetThingPlayerCanProduce : QuestNode
	{
		// Token: 0x0600D7BA RID: 55226 RVA: 0x00500CFC File Offset: 0x004FEEFC
		public static void ResetStaticData()
		{
			QuestNode_GetThingPlayerCanProduce.allWorkTables.Clear();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].IsWorkTable)
				{
					QuestNode_GetThingPlayerCanProduce.allWorkTables.Add(allDefsListForReading[i]);
				}
			}
		}

		// Token: 0x0600D7BB RID: 55227 RVA: 0x00500D49 File Offset: 0x004FEF49
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x0600D7BC RID: 55228 RVA: 0x00500D52 File Offset: 0x004FEF52
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D7BD RID: 55229 RVA: 0x00500D60 File Offset: 0x004FEF60
		private bool DoWork(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			float x2 = slate.Get<float>("points", 0f, false);
			SimpleCurve value = this.pointsToMaxItemMarketValueCurve.GetValue(slate);
			float valueOrDefault = this.maxMarketValueFactor.GetValue(slate).GetValueOrDefault(1f);
			float maxMarketValue = value.Evaluate(x2) * valueOrDefault;
			SimpleCurve value2 = this.pointsToRequiredWorkCurve.GetValue(slate);
			float randomInRange = (this.workAmountRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			float num = value2.Evaluate(x2) * randomInRange;
			QuestNode_GetThingPlayerCanProduce.tmpCandidates.Clear();
			for (int i = 0; i < QuestNode_GetThingPlayerCanProduce.allWorkTables.Count; i++)
			{
				if (BuildCopyCommandUtility.FindAllowedDesignator(QuestNode_GetThingPlayerCanProduce.allWorkTables[i], true) != null)
				{
					List<RecipeDef> recipes = QuestNode_GetThingPlayerCanProduce.allWorkTables[i].AllRecipes;
					int j2;
					int j;
					for (j = 0; j < recipes.Count; j = j2 + 1)
					{
						if (recipes[j].AvailableNow && recipes[j].products.Any<ThingDefCountClass>() && !recipes[j].PotentiallyMissingIngredients(null, map).Any<ThingDef>())
						{
							using (IEnumerator<ThingDef> enumerator = (recipes[j].products[0].thingDef.MadeFromStuff ? GenStuff.AllowedStuffsFor(recipes[j].products[0].thingDef, TechLevel.Undefined, false) : Gen.YieldSingle<ThingDef>(null)).GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									ThingDef stuff = enumerator.Current;
									if (stuff == null || (map.listerThings.ThingsOfDef(stuff).Any<Thing>() && stuff.stuffProps.commonality >= this.minStuffCommonality.GetValue(slate)))
									{
										int num2 = 0;
										if (stuff != null)
										{
											List<Thing> list = map.listerThings.ThingsOfDef(stuff);
											for (int k = 0; k < list.Count; k++)
											{
												num2 += list[k].stackCount;
											}
										}
										float num3 = recipes[j].WorkAmountForStuff(stuff);
										if (num3 > 0f)
										{
											int num4 = Mathf.FloorToInt(num / num3);
											if (stuff != null)
											{
												IngredientCount ingredientCount = (from x in recipes[j].ingredients
												where x.filter.Allows(stuff)
												select x).MaxByWithFallback((IngredientCount x) => x.CountRequiredOfFor(stuff, recipes[j], null), null);
												num4 = Mathf.Min(num4, Mathf.FloorToInt((float)num2 / (float)ingredientCount.CountRequiredOfFor(stuff, recipes[j], null)));
											}
											if (num4 > 0)
											{
												QuestNode_GetThingPlayerCanProduce.tmpCandidates.Add(new Pair<ThingStuffPair, int>(new ThingStuffPair(recipes[j].products[0].thingDef, stuff, 1f), recipes[j].products[0].count * num4));
											}
										}
									}
								}
							}
						}
						j2 = j;
					}
				}
			}
			QuestNode_GetThingPlayerCanProduce.tmpCandidates.RemoveAll((Pair<ThingStuffPair, int> x) => x.Second <= 0);
			QuestNode_GetThingPlayerCanProduce.tmpCandidates.RemoveAll((Pair<ThingStuffPair, int> x) => StatDefOf.MarketValue.Worker.GetValueAbstract(x.First.thing, x.First.stuff) > maxMarketValue);
			if (!QuestNode_GetThingPlayerCanProduce.tmpCandidates.Any<Pair<ThingStuffPair, int>>())
			{
				return false;
			}
			Pair<ThingStuffPair, int> pair = QuestNode_GetThingPlayerCanProduce.tmpCandidates.RandomElement<Pair<ThingStuffPair, int>>();
			int num5 = Mathf.Min(Mathf.RoundToInt(maxMarketValue / StatDefOf.MarketValue.Worker.GetValueAbstract(pair.First.thing, pair.First.stuff)), pair.Second);
			float randomInRange2 = (this.productionItemCountRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			num5 = Mathf.RoundToInt((float)num5 * randomInRange2);
			num5 = Mathf.Max(num5, 1);
			slate.Set<ThingDef>(this.storeProductionItemDefAs.GetValue(slate), pair.First.thing, false);
			slate.Set<ThingDef>(this.storeProductionItemStuffAs.GetValue(slate), pair.First.stuff, false);
			slate.Set<int>(this.storeProductionItemCountAs.GetValue(slate), num5, false);
			string value3 = this.storeProductionItemLabelAs.GetValue(slate);
			if (!string.IsNullOrEmpty(value3))
			{
				slate.Set<string>(value3, GenLabel.ThingLabel(pair.First.thing, pair.First.stuff, num5), false);
			}
			return true;
		}

		// Token: 0x0400891A RID: 35098
		[NoTranslate]
		public SlateRef<string> storeProductionItemDefAs;

		// Token: 0x0400891B RID: 35099
		[NoTranslate]
		public SlateRef<string> storeProductionItemStuffAs;

		// Token: 0x0400891C RID: 35100
		[NoTranslate]
		public SlateRef<string> storeProductionItemCountAs;

		// Token: 0x0400891D RID: 35101
		[NoTranslate]
		public SlateRef<string> storeProductionItemLabelAs;

		// Token: 0x0400891E RID: 35102
		public SlateRef<SimpleCurve> pointsToRequiredWorkCurve;

		// Token: 0x0400891F RID: 35103
		public SlateRef<SimpleCurve> pointsToMaxItemMarketValueCurve;

		// Token: 0x04008920 RID: 35104
		public SlateRef<float?> maxMarketValueFactor;

		// Token: 0x04008921 RID: 35105
		public SlateRef<float> minStuffCommonality;

		// Token: 0x04008922 RID: 35106
		public SlateRef<FloatRange?> workAmountRandomFactorRange;

		// Token: 0x04008923 RID: 35107
		public SlateRef<FloatRange?> productionItemCountRandomFactorRange;

		// Token: 0x04008924 RID: 35108
		private static List<ThingDef> allWorkTables = new List<ThingDef>();

		// Token: 0x04008925 RID: 35109
		private static List<Pair<ThingStuffPair, int>> tmpCandidates = new List<Pair<ThingStuffPair, int>>();
	}
}
