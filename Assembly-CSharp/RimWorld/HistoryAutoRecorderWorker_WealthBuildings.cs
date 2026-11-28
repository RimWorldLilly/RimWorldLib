using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103B RID: 4155
	public class HistoryAutoRecorderWorker_WealthBuildings : HistoryAutoRecorderWorker
	{
		// Token: 0x06006897 RID: 26775 RVA: 0x0024F634 File Offset: 0x0024D834
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthBuildings;
				}
			}
			return num;
		}
	}
}
