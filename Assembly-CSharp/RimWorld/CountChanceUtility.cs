using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8A RID: 7050
	public static class CountChanceUtility
	{
		// Token: 0x0600B4E2 RID: 46306 RVA: 0x003FB93C File Offset: 0x003F9B3C
		public static int RandomCount(List<CountChance> chances)
		{
			float value = Rand.Value;
			float num = 0f;
			for (int i = 0; i < chances.Count; i++)
			{
				num += chances[i].chance;
				if (value < num)
				{
					if (num > 1f)
					{
						Log.Error("CountChances error: Total chance is " + num.ToString() + " but it should not be above 1.");
					}
					return chances[i].count;
				}
			}
			return 0;
		}
	}
}
