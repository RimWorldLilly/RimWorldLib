using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006AF RID: 1711
	public static class ThingCountUtility
	{
		// Token: 0x06003656 RID: 13910 RVA: 0x0015DF54 File Offset: 0x0015C154
		public static int CountOf(List<ThingCount> list, Thing thing)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Thing == thing)
				{
					num += list[i].Count;
				}
			}
			return num;
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x0015DF9C File Offset: 0x0015C19C
		public static void AddToList(List<ThingCount> list, Thing thing, int countToAdd)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Thing == thing)
				{
					list[i] = list[i].WithCount(list[i].Count + countToAdd);
					return;
				}
			}
			list.Add(new ThingCount(thing, countToAdd, false));
		}
	}
}
