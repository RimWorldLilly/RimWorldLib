using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103E RID: 4158
	public class HistoryAutoRecorderWorker_WealthTotal : HistoryAutoRecorderWorker
	{
		// Token: 0x0600689D RID: 26781 RVA: 0x0024F73C File Offset: 0x0024D93C
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthTotal;
				}
			}
			return num;
		}
	}
}
