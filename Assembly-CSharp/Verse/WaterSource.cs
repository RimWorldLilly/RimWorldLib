using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000377 RID: 887
	public class WaterSource : SimpleBoolPathFinderDataSource
	{
		// Token: 0x06001B1A RID: 6938 RVA: 0x000A6D9C File Offset: 0x000A4F9C
		public WaterSource(Map map) : base(map)
		{
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x000A6DA8 File Offset: 0x000A4FA8
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.Clear();
			TerrainGrid terrainGrid = this.map.terrainGrid;
			for (int i = 0; i < this.cellCount; i++)
			{
				TerrainDef terrainDef = terrainGrid.TerrainAt(i);
				if (terrainDef != null && terrainDef.IsWater)
				{
					this.data.Set(i, true);
				}
			}
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x000A6E00 File Offset: 0x000A5000
		public override bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			TerrainGrid terrainGrid = this.map.terrainGrid;
			foreach (IntVec3 c in cellDeltas)
			{
				int num = cellIndices.CellToIndex(c);
				int pos = num;
				TerrainDef terrainDef = terrainGrid.TerrainAt(num);
				this.data.Set(pos, terrainDef != null && terrainDef.IsWater);
			}
			return false;
		}
	}
}
