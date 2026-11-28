using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103D RID: 4157
	public class HistoryAutoRecorderWorker_WealthPawns : HistoryAutoRecorderWorker
	{
		// Token: 0x0600689B RID: 26779 RVA: 0x0024F6E4 File Offset: 0x0024D8E4
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthPawns;
				}
			}
			return num;
		}
	}
}
