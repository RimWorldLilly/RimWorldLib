using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002249 RID: 8777
	public class QuestNode_TradeRequest_GetRequestedThing : QuestNode
	{
		// Token: 0x0600D917 RID: 55575 RVA: 0x00506820 File Offset: 0x00504A20
		private static int RandomRequestCount(ThingDef thingDef, Map map)
		{
			Rand.PushState(Find.TickManager.TicksGame ^ thingDef.GetHashCode() ^ 876093659);
			float num = (float)QuestNode_TradeRequest_GetRequestedThing.BaseValueWantedRange.RandomInRange;
			Rand.PopState();
			num *= QuestNode_TradeRequest_GetRequestedThing.ValueWantedFactorFromWealthCurve.Evaluate(map.wealthWatcher.WealthTotal);
			return ThingUtility.RoundedResourceStackCount(Mathf.Max(1, Mathf.RoundToInt(num / thingDef.BaseMarketValue)));
		}

		// Token: 0x0600D918 RID: 55576 RVA: 0x00506890 File Offset: 0x00504A90
		private static bool TryFindRandomRequestedThingDef(Map map, out ThingDef thingDef, out int count, List<ThingDef> dontRequest)
		{
			QuestNode_TradeRequest_GetRequestedThing.<>c__DisplayClass4_0 CS$<>8__locals1 = new QuestNode_TradeRequest_GetRequestedThing.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.dontRequest = dontRequest;
			QuestNode_TradeRequest_GetRequestedThing.requestCountDict.Clear();
			if (ThingSetMakerUtility.allGeneratableItems.Where(new Func<ThingDef, bool>(CS$<>8__locals1.<TryFindRandomRequestedThingDef>g__GlobalValidator|0)).TryRandomElement(out thingDef))
			{
				count = QuestNode_TradeRequest_GetRequestedThing.requestCountDict[thingDef];
				return true;
			}
			count = 0;
			return false;
		}

		// Token: 0x0600D919 RID: 55577 RVA: 0x005068F0 File Offset: 0x00504AF0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ThingDef thingDef;
			int num;
			if (QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef(slate.Get<Map>("map", null, false), out thingDef, out num, this.dontRequest.GetValue(slate)))
			{
				slate.Set<ThingDef>(this.storeThingAs.GetValue(slate), thingDef, false);
				slate.Set<int>(this.storeThingCountAs.GetValue(slate), num, false);
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)num, false);
				slate.Set<bool>(this.storeHasQualityAs.GetValue(slate), thingDef.HasComp(typeof(CompQuality)), false);
			}
		}

		// Token: 0x0600D91A RID: 55578 RVA: 0x00506994 File Offset: 0x00504B94
		protected override bool TestRunInt(Slate slate)
		{
			ThingDef thingDef;
			int num;
			if (QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef(slate.Get<Map>("map", null, false), out thingDef, out num, this.dontRequest.GetValue(slate)))
			{
				slate.Set<ThingDef>(this.storeThingAs.GetValue(slate), thingDef, false);
				slate.Set<int>(this.storeThingCountAs.GetValue(slate), num, false);
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)num, false);
				return true;
			}
			return false;
		}

		// Token: 0x04008ABE RID: 35518
		private static readonly IntRange BaseValueWantedRange = new IntRange(500, 2500);

		// Token: 0x04008ABF RID: 35519
		private static readonly SimpleCurve ValueWantedFactorFromWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.3f),
				true
			},
			{
				new CurvePoint(50000f, 1f),
				true
			},
			{
				new CurvePoint(300000f, 2f),
				true
			}
		};

		// Token: 0x04008AC0 RID: 35520
		private static Dictionary<ThingDef, int> requestCountDict = new Dictionary<ThingDef, int>();

		// Token: 0x04008AC1 RID: 35521
		[NoTranslate]
		public SlateRef<string> storeThingAs;

		// Token: 0x04008AC2 RID: 35522
		[NoTranslate]
		public SlateRef<string> storeThingCountAs;

		// Token: 0x04008AC3 RID: 35523
		[NoTranslate]
		public SlateRef<string> storeMarketValueAs;

		// Token: 0x04008AC4 RID: 35524
		[NoTranslate]
		public SlateRef<string> storeHasQualityAs;

		// Token: 0x04008AC5 RID: 35525
		public SlateRef<List<ThingDef>> dontRequest;
	}
}
