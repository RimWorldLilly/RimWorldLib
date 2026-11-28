using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD7 RID: 7127
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		// Token: 0x170020FA RID: 8442
		// (get) Token: 0x0600B684 RID: 46724 RVA: 0x00404C38 File Offset: 0x00402E38
		private List<Thing> BadDispensers
		{
			get
			{
				this.badDispensersResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.FoodDispenser))
					{
						bool flag = false;
						foreach (IntVec3 c in ((Building_NutrientPasteDispenser)thing).AdjCellsCardinalInBounds)
						{
							Thing edifice = c.GetEdifice(thing.Map);
							if (edifice != null && edifice.IsHopper())
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.badDispensersResult.Add(thing);
						}
					}
				}
				return this.badDispensersResult;
			}
		}

		// Token: 0x0600B685 RID: 46725 RVA: 0x00404D34 File Offset: 0x00402F34
		public Alert_PasteDispenserNeedsHopper()
		{
			this.defaultLabel = "NeedFoodHopper".Translate();
			this.defaultExplanation = "NeedFoodHopperDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600B686 RID: 46726 RVA: 0x00404D83 File Offset: 0x00402F83
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadDispensers);
		}

		// Token: 0x040068D3 RID: 26835
		private List<Thing> badDispensersResult = new List<Thing>();
	}
}
