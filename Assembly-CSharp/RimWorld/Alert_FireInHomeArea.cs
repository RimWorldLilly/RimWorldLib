using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA5 RID: 7077
	public class Alert_FireInHomeArea : Alert_Critical
	{
		// Token: 0x0600B5AF RID: 46511 RVA: 0x003FFE20 File Offset: 0x003FE020
		public Alert_FireInHomeArea()
		{
			this.defaultLabel = "FireInHomeArea".Translate();
			this.defaultExplanation = "FireInHomeAreaDesc".Translate();
		}

		// Token: 0x170020D9 RID: 8409
		// (get) Token: 0x0600B5B0 RID: 46512 RVA: 0x003FFE54 File Offset: 0x003FE054
		private Fire FireInHomeArea
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.Fire);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (maps[i].areaManager.Home[thing.Position] && !thing.Position.Fogged(thing.Map))
						{
							return (Fire)thing;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x0600B5B1 RID: 46513 RVA: 0x003FFEE6 File Offset: 0x003FE0E6
		public override AlertReport GetReport()
		{
			return this.FireInHomeArea;
		}
	}
}
