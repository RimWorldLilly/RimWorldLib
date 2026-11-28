using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103C RID: 4156
	public class HistoryAutoRecorderWorker_WealthItems : HistoryAutoRecorderWorker
	{
		// Token: 0x06006899 RID: 26777 RVA: 0x0024F68C File Offset: 0x0024D88C
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthItems;
				}
			}
			return num;
		}
	}
}
