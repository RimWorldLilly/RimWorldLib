using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C10 RID: 7184
	public class Alert_MechChargerFull : Alert
	{
		// Token: 0x0600B76F RID: 46959 RVA: 0x0040A8EC File Offset: 0x00408AEC
		public Alert_MechChargerFull()
		{
			this.defaultLabel = "AlertRechargerFull".Translate();
			this.defaultExplanation = "AlertRechargerFullDesc".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x0600B770 RID: 46960 RVA: 0x0040A93B File Offset: 0x00408B3B
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600B771 RID: 46961 RVA: 0x0040A950 File Offset: 0x00408B50
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.MechCharger);
				for (int j = 0; j < list.Count; j++)
				{
					Building_MechCharger building_MechCharger = (Building_MechCharger)list[j];
					if (building_MechCharger.Faction == Faction.OfPlayer && building_MechCharger.IsFullOfWaste)
					{
						this.targets.Add(building_MechCharger);
					}
				}
			}
		}

		// Token: 0x04006935 RID: 26933
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
