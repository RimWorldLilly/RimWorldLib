using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE9 RID: 7145
	public class Alert_NeedMealSource : Alert
	{
		// Token: 0x0600B6D0 RID: 46800 RVA: 0x00406B0C File Offset: 0x00404D0C
		public Alert_NeedMealSource()
		{
			this.defaultLabel = "NeedMealSource".Translate();
			this.defaultExplanation = "NeedMealSourceDesc".Translate();
		}

		// Token: 0x0600B6D1 RID: 46801 RVA: 0x00406B40 File Offset: 0x00404D40
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 2)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedMealSource(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B6D2 RID: 46802 RVA: 0x00406B90 File Offset: 0x00404D90
		private bool NeedMealSource(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned)
			{
				return false;
			}
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				if (allBuildingsColonist[i].def.building.isMealSource)
				{
					return false;
				}
			}
			return true;
		}
	}
}
