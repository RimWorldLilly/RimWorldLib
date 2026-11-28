using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C17 RID: 7191
	public class Alert_GenebankUnpowered : Alert
	{
		// Token: 0x0600B78E RID: 46990 RVA: 0x0040B37C File Offset: 0x0040957C
		public Alert_GenebankUnpowered()
		{
			this.defaultLabel = "AlertGenebankUnpowered".Translate();
			this.defaultExplanation = "AlertGenebankUnpoweredDesc".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x0600B78F RID: 46991 RVA: 0x0040B3CB File Offset: 0x004095CB
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600B790 RID: 46992 RVA: 0x0040B3E0 File Offset: 0x004095E0
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.GeneBank))
				{
					CompGenepackContainer compGenepackContainer = building.TryGetComp<CompGenepackContainer>();
					if (compGenepackContainer != null && compGenepackContainer.innerContainer.Any<Thing>() && !compGenepackContainer.PowerOn)
					{
						this.targets.Add(building);
					}
				}
			}
		}

		// Token: 0x04006940 RID: 26944
		public List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
