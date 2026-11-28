using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC5 RID: 7109
	public class Alert_NeedWarden : Alert
	{
		// Token: 0x0600B63C RID: 46652 RVA: 0x004027FC File Offset: 0x004009FC
		public Alert_NeedWarden()
		{
			this.defaultLabel = "NeedWarden".Translate();
			this.defaultExplanation = "NeedWardenDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600B63D RID: 46653 RVA: 0x00402838 File Offset: 0x00400A38
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.PrisonersOfColonySpawned.Any<Pawn>())
				{
					bool flag = false;
					foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
					{
						if ((pawn.Spawned || pawn.BrieflyDespawned()) && !pawn.Downed && pawn.workSettings != null && pawn.workSettings.GetPriority(WorkTypeDefOf.Warden) > 0)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return AlertReport.CulpritIs(map.mapPawns.PrisonersOfColonySpawned[0]);
					}
				}
			}
			return false;
		}
	}
}
