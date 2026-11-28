using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC6 RID: 7110
	public class Alert_LowFood : Alert
	{
		// Token: 0x0600B63E RID: 46654 RVA: 0x00402934 File Offset: 0x00400B34
		public Alert_LowFood()
		{
			this.defaultLabel = "LowFood".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600B63F RID: 46655 RVA: 0x00402958 File Offset: 0x00400B58
		public override TaggedString GetExplanation()
		{
			Map map = this.MapWithLowFood();
			if (map == null)
			{
				return string.Empty;
			}
			float totalHumanEdibleNutrition = map.resourceCounter.TotalHumanEdibleNutrition;
			int num = map.mapPawns.FreeColonistsSpawnedCount + map.mapPawns.PrisonersOfColonyCount;
			int num2 = Mathf.FloorToInt(totalHumanEdibleNutrition / (float)num);
			return "LowFoodDesc".Translate(totalHumanEdibleNutrition.ToString("F0"), num.ToStringCached(), num2.ToStringCached());
		}

		// Token: 0x0600B640 RID: 46656 RVA: 0x004029D9 File Offset: 0x00400BD9
		public override AlertReport GetReport()
		{
			if ((float)Find.TickManager.TicksGame < 150000f)
			{
				return false;
			}
			return this.MapWithLowFood() != null;
		}

		// Token: 0x0600B641 RID: 46657 RVA: 0x00402A04 File Offset: 0x00400C04
		private Map MapWithLowFood()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.AnyColonistSpawned)
				{
					int freeColonistsSpawnedCount = map.mapPawns.FreeColonistsSpawnedCount;
					if (map.resourceCounter.TotalHumanEdibleNutrition < 4f * (float)freeColonistsSpawnedCount)
					{
						return map;
					}
				}
			}
			return null;
		}

		// Token: 0x040068AF RID: 26799
		private const float NutritionThresholdPerColonist = 4f;
	}
}
