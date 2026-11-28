using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FE0 RID: 8160
	public class WealthUtility
	{
		// Token: 0x17002550 RID: 9552
		// (get) Token: 0x0600CEFE RID: 52990 RVA: 0x004C4164 File Offset: 0x004C2364
		public static float PlayerWealth
		{
			get
			{
				List<Map> maps = Find.Maps;
				float num = 0f;
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
}
