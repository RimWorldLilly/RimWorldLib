using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021D1 RID: 8657
	public class QuestNode_GetPlantPlayerCanHarvest : QuestNode
	{
		// Token: 0x0600D776 RID: 55158 RVA: 0x004FFAB7 File Offset: 0x004FDCB7
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x0600D777 RID: 55159 RVA: 0x004FFAC0 File Offset: 0x004FDCC0
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D778 RID: 55160 RVA: 0x004FFAD0 File Offset: 0x004FDCD0
		private bool DoWork(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			float x = slate.Get<float>("points", 0f, false);
			float seasonalTemp = Find.World.tileTemperatures.GetSeasonalTemp(map.Tile);
			float lowestTemp = seasonalTemp;
			float highestTemp = seasonalTemp;
			int ticksAbs = GenTicks.TicksAbs;
			for (int i = 0; i < 15; i++)
			{
				int absTick = ticksAbs + 60000 * i;
				float num = seasonalTemp + Find.World.tileTemperatures.OffsetFromDailyRandomVariation(map.Tile, absTick);
				if (num < lowestTemp)
				{
					lowestTemp = num;
				}
				if (num > highestTemp)
				{
					highestTemp = num;
				}
			}
			ThingDef thingDef;
			if (!(from def in DefDatabase<ThingDef>.AllDefs
			where def.category == ThingCategory.Plant && !def.plant.cavePlant && def.plant.Sowable && def.plant.harvestedThingDef != null && def.plant.growDays <= (float)this.maxPlantGrowDays.GetValue(slate) && def.plant.minGrowthTemperature <= lowestTemp + 5f && def.plant.maxGrowthTemperature >= highestTemp - 5f && Command_SetPlantToGrow.IsPlantAvailable(def, map)
			select def).TryRandomElement(out thingDef))
			{
				return false;
			}
			SimpleCurve value = this.pointsToRequiredWorkCurve.GetValue(slate);
			float randomInRange = (this.workRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			float num2 = value.Evaluate(x) * randomInRange;
			float num3 = (thingDef.plant.sowWork + thingDef.plant.harvestWork) / thingDef.plant.harvestYield;
			int num4 = GenMath.RoundRandom(num2 / num3);
			num4 = Mathf.Max(num4, 1);
			slate.Set<ThingDef>(this.storeHarvestItemDefAs.GetValue(slate), thingDef.plant.harvestedThingDef, false);
			slate.Set<int>(this.storeHarvestItemCountAs.GetValue(slate), num4, false);
			if (this.storeGrowDaysAs.GetValue(slate) != null)
			{
				slate.Set<float>(this.storeGrowDaysAs.GetValue(slate), thingDef.plant.growDays, false);
			}
			return true;
		}

		// Token: 0x040088DD RID: 35037
		[NoTranslate]
		public SlateRef<string> storeHarvestItemDefAs;

		// Token: 0x040088DE RID: 35038
		[NoTranslate]
		public SlateRef<string> storeHarvestItemCountAs;

		// Token: 0x040088DF RID: 35039
		[NoTranslate]
		public SlateRef<string> storeGrowDaysAs;

		// Token: 0x040088E0 RID: 35040
		public SlateRef<int> maxPlantGrowDays;

		// Token: 0x040088E1 RID: 35041
		public SlateRef<SimpleCurve> pointsToRequiredWorkCurve;

		// Token: 0x040088E2 RID: 35042
		public SlateRef<FloatRange?> workRandomFactorRange;

		// Token: 0x040088E3 RID: 35043
		private const float TemperatureBuffer = 5f;

		// Token: 0x040088E4 RID: 35044
		private const int TemperatureCheckDays = 15;
	}
}
