using System;

namespace Verse
{
	// Token: 0x020002DB RID: 731
	public class ScattererValidator_TerrainDef : ScattererValidator
	{
		// Token: 0x06001638 RID: 5688 RVA: 0x0008544C File Offset: 0x0008364C
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c2 = new IntVec3(j, 0, i);
					if (!c2.InBounds(map))
					{
						return false;
					}
					if (c2.GetTerrain(map) != this.terrainDef)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001338 RID: 4920
		public int radius = 1;

		// Token: 0x04001339 RID: 4921
		public TerrainDef terrainDef;
	}
}
