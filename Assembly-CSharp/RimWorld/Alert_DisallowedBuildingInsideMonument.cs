using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA7 RID: 7079
	public class Alert_DisallowedBuildingInsideMonument : Alert_Critical
	{
		// Token: 0x170020DB RID: 8411
		// (get) Token: 0x0600B5B5 RID: 46517 RVA: 0x003FFFC8 File Offset: 0x003FE1C8
		private List<Thing> DisallowedBuildings
		{
			get
			{
				this.disallowedBuildingsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (monumentMarker.AllDone)
						{
							Thing firstDisallowedBuilding = monumentMarker.FirstDisallowedBuilding;
							if (firstDisallowedBuilding != null)
							{
								this.disallowedBuildingsResult.Add(firstDisallowedBuilding);
							}
						}
					}
				}
				return this.disallowedBuildingsResult;
			}
		}

		// Token: 0x170020DC RID: 8412
		// (get) Token: 0x0600B5B6 RID: 46518 RVA: 0x00400058 File Offset: 0x003FE258
		private int MinTicksLeft
		{
			get
			{
				int num = int.MaxValue;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (monumentMarker.AllDone && monumentMarker.AnyDisallowedBuilding)
						{
							num = Mathf.Min(num, 60000 - monumentMarker.ticksSinceDisallowedBuilding);
						}
					}
				}
				return num;
			}
		}

		// Token: 0x0600B5B7 RID: 46519 RVA: 0x004000E4 File Offset: 0x003FE2E4
		public Alert_DisallowedBuildingInsideMonument()
		{
			this.defaultLabel = "DisallowedBuildingInsideMonument".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0600B5B8 RID: 46520 RVA: 0x00400113 File Offset: 0x003FE313
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.DisallowedBuildings);
		}

		// Token: 0x0600B5B9 RID: 46521 RVA: 0x00400120 File Offset: 0x003FE320
		public override TaggedString GetExplanation()
		{
			return "DisallowedBuildingInsideMonumentDesc".Translate(this.MinTicksLeft.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x04006886 RID: 26758
		private List<Thing> disallowedBuildingsResult = new List<Thing>();
	}
}
