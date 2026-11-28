using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6F RID: 7023
	public class ThingSetMaker_ResourcePod : ThingSetMaker
	{
		// Token: 0x0600B444 RID: 46148 RVA: 0x003F8A68 File Offset: 0x003F6C68
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingDef thingDef = ThingSetMaker_ResourcePod.RandomPodContentsDef(false);
			float num = Rand.Range(150f, 600f);
			while (num > thingDef.BaseMarketValue)
			{
				ThingDef stuff = GenStuff.RandomStuffByCommonalityFor(thingDef, TechLevel.Undefined);
				Thing thing = ThingMaker.MakeThing(thingDef, stuff);
				int num2 = Rand.Range(20, 40);
				if (num2 > thing.def.stackLimit)
				{
					num2 = thing.def.stackLimit;
				}
				float statValue = thing.GetStatValue(StatDefOf.MarketValue, true, -1);
				if ((float)num2 * statValue > num)
				{
					num2 = Mathf.FloorToInt(num / statValue);
				}
				if (num2 == 0)
				{
					num2 = 1;
				}
				thing.stackCount = num2;
				outThings.Add(thing);
				num -= (float)num2 * statValue;
				if (outThings.Count >= 7 || num <= statValue)
				{
					break;
				}
			}
		}

		// Token: 0x0600B445 RID: 46149 RVA: 0x003F8B22 File Offset: 0x003F6D22
		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability.TraderCanSell() && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 40f && !d.preventSpawningInResourcePod && !d.HasComp(typeof(CompHatcher))
			select d;
		}

		// Token: 0x0600B446 RID: 46150 RVA: 0x003F8B50 File Offset: 0x003F6D50
		public static ThingDef RandomPodContentsDef(bool mustBeResource = false)
		{
			IEnumerable<ThingDef> source = ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
			if (mustBeResource)
			{
				source = from x in source
				where x.stackLimit > 1
				select x;
			}
			int numMeats = source.Count((ThingDef x) => x.IsMeat);
			int numLeathers = source.Count((ThingDef x) => x.IsLeather);
			int numEggs = source.Count((ThingDef x) => x.IsEgg);
			return source.RandomElementByWeight((ThingDef d) => ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(d, numMeats, numLeathers, numEggs));
		}

		// Token: 0x0600B447 RID: 46151 RVA: 0x003F8C28 File Offset: 0x003F6E28
		[DebugOutput("Incidents", false)]
		private static void PodContentsPossibleDefs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ThingDefs that can go in the resource pod crash incident.");
			foreach (ThingDef thingDef in ThingSetMaker_ResourcePod.PossiblePodContentsDefs())
			{
				stringBuilder.AppendLine(thingDef.defName);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600B448 RID: 46152 RVA: 0x003F8C98 File Offset: 0x003F6E98
		[DebugOutput("Incidents", false)]
		private static void PodContentsTest()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				stringBuilder.AppendLine(ThingSetMaker_ResourcePod.RandomPodContentsDef(false).LabelCap);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600B449 RID: 46153 RVA: 0x003F8CDA File Offset: 0x003F6EDA
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
		}

		// Token: 0x040067D6 RID: 26582
		private const int MaxStacks = 7;

		// Token: 0x040067D7 RID: 26583
		private const float MaxMarketValue = 40f;

		// Token: 0x040067D8 RID: 26584
		private const float MinMoney = 150f;

		// Token: 0x040067D9 RID: 26585
		private const float MaxMoney = 600f;
	}
}
