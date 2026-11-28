using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C1B RID: 7195
	public class Alert_InhibitorBlocked : Alert
	{
		// Token: 0x0600B79D RID: 47005 RVA: 0x0040BACC File Offset: 0x00409CCC
		public Alert_InhibitorBlocked()
		{
			this.defaultLabel = "AlertInhibitorBlocked".Translate();
			this.defaultExplanation = "AlertInhibitorBlockedDesc".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0600B79E RID: 47006 RVA: 0x0040BB1B File Offset: 0x00409D1B
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600B79F RID: 47007 RVA: 0x0040BB30 File Offset: 0x00409D30
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.ElectricInhibitor))
				{
					if (building.HasComp<CompFacility>() && ContainmentUtility.IsLinearBuildingBlocked(building.def, building.Position, building.Rotation, building.Map, null))
					{
						this.targets.Add(building);
					}
				}
			}
		}

		// Token: 0x04006945 RID: 26949
		public readonly List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
