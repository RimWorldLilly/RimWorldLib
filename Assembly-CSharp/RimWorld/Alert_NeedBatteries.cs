using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BEC RID: 7148
	public class Alert_NeedBatteries : Alert
	{
		// Token: 0x0600B6DC RID: 46812 RVA: 0x00406FAC File Offset: 0x004051AC
		public Alert_NeedBatteries()
		{
			this.defaultLabel = "NeedBatteries".Translate();
			this.defaultExplanation = "NeedBatteriesDesc".Translate();
		}

		// Token: 0x0600B6DD RID: 46813 RVA: 0x00406FE0 File Offset: 0x004051E0
		public override TaggedString GetExplanation()
		{
			string text = this.defaultExplanation + "\n\n";
			if (ResearchProjectDefOf.Batteries.IsFinished)
			{
				text += "NeedBatteriesResearchedDesc".Translate();
			}
			else
			{
				text += "NeedBatteriesNotResearchedDesc".Translate();
			}
			return text;
		}

		// Token: 0x0600B6DE RID: 46814 RVA: 0x00407040 File Offset: 0x00405240
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedBatteries(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B6DF RID: 46815 RVA: 0x00407080 File Offset: 0x00405280
		private bool NeedBatteries(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			return !map.listerBuildings.ColonistsHaveBuilding((Thing building) => building is Building_Battery) && (map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.SolarGenerator) || map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.WindTurbine)) && !map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.GeothermalGenerator) && !map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.WatermillGenerator);
		}
	}
}
