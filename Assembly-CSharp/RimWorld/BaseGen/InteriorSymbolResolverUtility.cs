using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002140 RID: 8512
	public static class InteriorSymbolResolverUtility
	{
		// Token: 0x0600D50B RID: 54539 RVA: 0x004EF8A0 File Offset: 0x004EDAA0
		public static void PushBedroomHeatersCoolersAndLightSourcesSymbols(ResolveParams rp, bool hasToSpawnLightSource = true)
		{
			Map map = BaseGen.globalSettings.map;
			if (map.mapTemperature.OutdoorTemp > 22f)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
			bool flag = false;
			if (map.mapTemperature.OutdoorTemp < 3f)
			{
				ThingDef singleThingDef;
				if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
				{
					singleThingDef = ThingDefOf.Heater;
				}
				else
				{
					singleThingDef = ((map.mapTemperature.OutdoorTemp < -20f) ? ThingDefOf.Campfire : ThingDefOf.TorchLamp);
					flag = true;
				}
				int num = (map.mapTemperature.OutdoorTemp < -45f) ? 2 : 1;
				for (int i = 0; i < num; i++)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.singleThingDef = singleThingDef;
					BaseGen.symbolStack.Push("edgeThing", resolveParams2, null);
				}
			}
			if (!flag && hasToSpawnLightSource)
			{
				BaseGen.symbolStack.Push("indoorLighting", rp, null);
			}
		}

		// Token: 0x04008716 RID: 34582
		private const float SpawnHeaterIfTemperatureBelow = 3f;

		// Token: 0x04008717 RID: 34583
		private const float SpawnSecondHeaterIfTemperatureBelow = -45f;

		// Token: 0x04008718 RID: 34584
		private const float NonIndustrial_SpawnCampfireIfTemperatureBelow = -20f;

		// Token: 0x04008719 RID: 34585
		private const float SpawnPassiveCoolerIfTemperatureAbove = 22f;
	}
}
