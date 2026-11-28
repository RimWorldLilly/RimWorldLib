using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE6 RID: 6886
	public static class BlightUtility
	{
		// Token: 0x0600AFE2 RID: 45026 RVA: 0x003DFD18 File Offset: 0x003DDF18
		public static Plant GetFirstBlightableNowPlant(IntVec3 c, Map map)
		{
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.BlightableNow)
			{
				return plant;
			}
			return null;
		}

		// Token: 0x0600AFE3 RID: 45027 RVA: 0x003DFD3C File Offset: 0x003DDF3C
		public static Plant GetFirstBlightableEverPlant(IntVec3 c, Map map)
		{
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.Blightable)
			{
				return plant;
			}
			return null;
		}
	}
}
