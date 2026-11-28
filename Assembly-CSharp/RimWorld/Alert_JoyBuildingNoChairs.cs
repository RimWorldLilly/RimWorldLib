using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF3 RID: 7155
	public abstract class Alert_JoyBuildingNoChairs : Alert
	{
		// Token: 0x17002111 RID: 8465
		// (get) Token: 0x0600B6F6 RID: 46838
		protected abstract JoyGiverDef JoyGiver { get; }

		// Token: 0x17002112 RID: 8466
		// (get) Token: 0x0600B6F7 RID: 46839 RVA: 0x00407960 File Offset: 0x00405B60
		private List<Thing> BadBuildings
		{
			get
			{
				this.badBuildingsResult.Clear();
				List<Map> maps = Find.Maps;
				foreach (ThingDef def in this.JoyGiver.thingDefs)
				{
					for (int i = 0; i < maps.Count; i++)
					{
						foreach (Thing thing in maps[i].listerThings.ThingsOfDef(def))
						{
							if (thing.Faction == Faction.OfPlayer && !this.JoyBuildingUsable(thing))
							{
								this.badBuildingsResult.Add(thing);
							}
						}
					}
				}
				return this.badBuildingsResult;
			}
		}

		// Token: 0x0600B6F8 RID: 46840 RVA: 0x00407A4C File Offset: 0x00405C4C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadBuildings);
		}

		// Token: 0x0600B6F9 RID: 46841 RVA: 0x00407A5C File Offset: 0x00405C5C
		private bool JoyBuildingUsable(Thing t)
		{
			List<IntVec3> list = GenAdjFast.AdjacentCellsCardinal(t);
			for (int i = 0; i < list.Count; i++)
			{
				Building edifice = list[i].GetEdifice(t.Map);
				if (edifice != null && edifice.def.building.isSittable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040068FC RID: 26876
		private List<Thing> badBuildingsResult = new List<Thing>();
	}
}
