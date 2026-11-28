using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002CE RID: 718
	public class GenStep_TerrainPatches : GenStep
	{
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060015F5 RID: 5621 RVA: 0x00082503 File Offset: 0x00080703
		public override int SeedPart
		{
			get
			{
				return 1370184742;
			}
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0008250C File Offset: 0x0008070C
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = Mathf.RoundToInt((float)map.Area / 10000f * this.patchesPer10kCellsRange.RandomInRange);
			using (map.pathing.DisableIncrementalScope())
			{
				for (int i = 0; i < num; i++)
				{
					float randomInRange = this.patchSizeRange.RandomInRange;
					IntVec3 a = CellFinder.RandomCell(map);
					foreach (IntVec3 b in GenRadial.RadialPatternInRadius(randomInRange / 2f))
					{
						IntVec3 c = a + b;
						if (c.InBounds(map))
						{
							map.terrainGrid.SetTerrain(c, this.terrainDef);
						}
					}
				}
			}
		}

		// Token: 0x040012ED RID: 4845
		public TerrainDef terrainDef;

		// Token: 0x040012EE RID: 4846
		public FloatRange patchesPer10kCellsRange;

		// Token: 0x040012EF RID: 4847
		public FloatRange patchSizeRange;
	}
}
