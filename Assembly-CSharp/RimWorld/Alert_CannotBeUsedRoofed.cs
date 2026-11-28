using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF6 RID: 7158
	public class Alert_CannotBeUsedRoofed : Alert
	{
		// Token: 0x17002115 RID: 8469
		// (get) Token: 0x0600B6FF RID: 46847 RVA: 0x00407B38 File Offset: 0x00405D38
		private List<Thing> UnusableBuildings
		{
			get
			{
				this.unusableBuildingsResult.Clear();
				if (this.thingDefsToCheck == null)
				{
					this.thingDefsToCheck = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (!thingDef.canBeUsedUnderRoof)
						{
							this.thingDefsToCheck.Add(thingDef);
						}
					}
				}
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < this.thingDefsToCheck.Count; i++)
				{
					for (int j = 0; j < maps.Count; j++)
					{
						List<Thing> list = maps[j].listerThings.ThingsOfDef(this.thingDefsToCheck[i]);
						for (int k = 0; k < list.Count; k++)
						{
							if (list[k].Faction == ofPlayer && RoofUtility.IsAnyCellUnderRoof(list[k]))
							{
								this.unusableBuildingsResult.Add(list[k]);
							}
						}
					}
				}
				return this.unusableBuildingsResult;
			}
		}

		// Token: 0x0600B700 RID: 46848 RVA: 0x00407C68 File Offset: 0x00405E68
		public Alert_CannotBeUsedRoofed()
		{
			this.defaultLabel = "BuildingCantBeUsedRoofed".Translate();
			this.defaultExplanation = "BuildingCantBeUsedRoofedDesc".Translate();
		}

		// Token: 0x0600B701 RID: 46849 RVA: 0x00407CA5 File Offset: 0x00405EA5
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnusableBuildings);
		}

		// Token: 0x040068FD RID: 26877
		private List<ThingDef> thingDefsToCheck;

		// Token: 0x040068FE RID: 26878
		private List<Thing> unusableBuildingsResult = new List<Thing>();
	}
}
