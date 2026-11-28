using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F36 RID: 3894
	public class TerrainThreshold
	{
		// Token: 0x06006495 RID: 25749 RVA: 0x0023EC64 File Offset: 0x0023CE64
		public static TerrainDef TerrainAtValue(List<TerrainThreshold> threshes, float val)
		{
			for (int i = 0; i < threshes.Count; i++)
			{
				if (threshes[i].min <= val && threshes[i].max >= val)
				{
					return threshes[i].terrain;
				}
			}
			return null;
		}

		// Token: 0x040039D9 RID: 14809
		public TerrainDef terrain;

		// Token: 0x040039DA RID: 14810
		public float min = -1000f;

		// Token: 0x040039DB RID: 14811
		public float max = 1000f;
	}
}
