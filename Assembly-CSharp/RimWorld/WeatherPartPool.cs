using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013BE RID: 5054
	public static class WeatherPartPool
	{
		// Token: 0x06007D2A RID: 32042 RVA: 0x002CB514 File Offset: 0x002C9714
		public static SkyOverlay GetInstanceOf<T>() where T : SkyOverlay
		{
			for (int i = 0; i < WeatherPartPool.instances.Count; i++)
			{
				T t = WeatherPartPool.instances[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			SkyOverlay skyOverlay = Activator.CreateInstance<T>();
			WeatherPartPool.instances.Add(skyOverlay);
			return skyOverlay;
		}

		// Token: 0x04004ED0 RID: 20176
		private static List<SkyOverlay> instances = new List<SkyOverlay>();
	}
}
