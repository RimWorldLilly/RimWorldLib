using System;
using System.Collections.Generic;
using LudeonTK;
using Unity.Collections;

namespace Verse
{
	// Token: 0x02000375 RID: 885
	public class PerceptualSource : IPathFinderDataSource, IDisposable
	{
		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x000A6A28 File Offset: 0x000A4C28
		public NativeArray<ushort>.ReadOnly CostDrafted
		{
			get
			{
				return this.costDrafted.AsReadOnly();
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x000A6A35 File Offset: 0x000A4C35
		public NativeArray<ushort>.ReadOnly CostUndrafted
		{
			get
			{
				return this.costUndrafted.AsReadOnly();
			}
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000A6A44 File Offset: 0x000A4C44
		public PerceptualSource(Map map)
		{
			this.map = map;
			int numGridCells = map.cellIndices.NumGridCells;
			this.costDrafted = new NativeArray<ushort>(numGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.costUndrafted = new NativeArray<ushort>(numGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x000A6A86 File Offset: 0x000A4C86
		public void Dispose()
		{
			this.costDrafted.Dispose();
			this.costUndrafted.Dispose();
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x000A6AA0 File Offset: 0x000A4CA0
		public void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.costDrafted.Clear<ushort>();
			this.costUndrafted.Clear<ushort>();
			TerrainGrid terrainGrid = this.map.terrainGrid;
			for (int i = 0; i < this.map.cellIndices.NumGridCells; i++)
			{
				TerrainDef terrainDef = terrainGrid.TerrainAt(i);
				if (terrainDef != null)
				{
					this.costUndrafted[i] = (ushort)Math.Clamp(terrainDef.extraNonDraftedPerceivedPathCost, 0, 65535);
					this.costDrafted[i] = (ushort)Math.Clamp(terrainDef.extraDraftedPerceivedPathCost, 0, 65535);
				}
			}
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000A6B34 File Offset: 0x000A4D34
		public bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			TerrainGrid terrainGrid = this.map.terrainGrid;
			foreach (IntVec3 c in cellDeltas)
			{
				int num = cellIndices.CellToIndex(c);
				TerrainDef terrainDef = terrainGrid.TerrainAt(num);
				if (terrainDef != null)
				{
					this.costUndrafted[num] = (ushort)Math.Clamp(terrainDef.extraNonDraftedPerceivedPathCost, 0, 65535);
					this.costDrafted[num] = (ushort)Math.Clamp(terrainDef.extraDraftedPerceivedPathCost, 0, 65535);
				}
				else
				{
					this.costUndrafted[num] = 0;
					this.costDrafted[num] = 0;
				}
			}
			return false;
		}

		// Token: 0x04001636 RID: 5686
		private readonly Map map;

		// Token: 0x04001637 RID: 5687
		private NativeArray<ushort> costDrafted;

		// Token: 0x04001638 RID: 5688
		private NativeArray<ushort> costUndrafted;
	}
}
