using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF2 RID: 7154
	public class Alert_BilliardsTableOnWall : Alert
	{
		// Token: 0x17002110 RID: 8464
		// (get) Token: 0x0600B6F3 RID: 46835 RVA: 0x0040787C File Offset: 0x00405A7C
		private List<Thing> BadTables
		{
			get
			{
				this.badTablesResult.Clear();
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.BilliardsTable);
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].Faction == ofPlayer && !JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(list[j]))
						{
							this.badTablesResult.Add(list[j]);
						}
					}
				}
				return this.badTablesResult;
			}
		}

		// Token: 0x0600B6F4 RID: 46836 RVA: 0x00407915 File Offset: 0x00405B15
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		// Token: 0x0600B6F5 RID: 46837 RVA: 0x00407952 File Offset: 0x00405B52
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}

		// Token: 0x040068FB RID: 26875
		private List<Thing> badTablesResult = new List<Thing>();
	}
}
