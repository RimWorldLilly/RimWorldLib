using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BFE RID: 7166
	public class Alert_AnimalPenNotEnclosed : Alert
	{
		// Token: 0x0600B71D RID: 46877 RVA: 0x00408815 File Offset: 0x00406A15
		public Alert_AnimalPenNotEnclosed()
		{
			this.defaultLabel = "AlertAnimalPenNotEnclosed".Translate();
			this.defaultExplanation = "AlertAnimalPenNotEnclosedDesc".Translate();
		}

		// Token: 0x0600B71E RID: 46878 RVA: 0x00408854 File Offset: 0x00406A54
		private void CalculateTargets()
		{
			this.targets.Clear();
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					foreach (Building building in map.listerBuildings.allBuildingsAnimalPenMarkers)
					{
						CompAnimalPenMarker compAnimalPenMarker = building.TryGetComp<CompAnimalPenMarker>();
						if (!building.IsForbidden(Faction.OfPlayer) && compAnimalPenMarker.PenState.Unenclosed)
						{
							this.targets.Add(building);
						}
					}
				}
			}
		}

		// Token: 0x0600B71F RID: 46879 RVA: 0x0040892C File Offset: 0x00406B2C
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400690E RID: 26894
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
