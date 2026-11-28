using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDB RID: 7131
	public class Alert_Biostarvation : Alert
	{
		// Token: 0x170020FE RID: 8446
		// (get) Token: 0x0600B695 RID: 46741 RVA: 0x00405188 File Offset: 0x00403388
		public List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Building_GrowthVat building_GrowthVat in maps[i].listerBuildings.AllBuildingsColonistOfClass<Building_GrowthVat>())
					{
						if (building_GrowthVat.Working && building_GrowthVat.BiostarvationDailyOffset > 0f)
						{
							this.targets.Add(building_GrowthVat);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0600B696 RID: 46742 RVA: 0x00405228 File Offset: 0x00403428
		public Alert_Biostarvation()
		{
			this.defaultLabel = "Biostarvation".Translate().CapitalizeFirst();
			this.defaultExplanation = "BiostarvationExplanation".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x0600B697 RID: 46743 RVA: 0x00405286 File Offset: 0x00403486
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x040068D8 RID: 26840
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
