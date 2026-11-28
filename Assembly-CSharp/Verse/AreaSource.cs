using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Verse
{
	// Token: 0x0200036D RID: 877
	public class AreaSource : IPathFinderDataSource, IDisposable
	{
		// Token: 0x06001AE5 RID: 6885 RVA: 0x000A5560 File Offset: 0x000A3760
		public AreaSource(Map map)
		{
			this.map = map;
			this.cellCount = map.cellIndices.NumGridCells;
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x000A55AC File Offset: 0x000A37AC
		public void Dispose()
		{
			foreach (NativeBitArray nativeBitArray in this.areaToBits.Values)
			{
				nativeBitArray.Dispose();
			}
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x000A5604 File Offset: 0x000A3804
		public NativeBitArray DataForArea(Area area)
		{
			return this.areaToBits[area];
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x000A5614 File Offset: 0x000A3814
		public void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.Prune();
			foreach (Area area in this.map.areaManager.AllAreas)
			{
				NativeBitArray value;
				if (!this.areaToBits.TryGetValue(area, out value))
				{
					value = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
					this.areaToBits[area] = value;
				}
				BoolGrid innerGrid = area.InnerGrid;
				for (int i = 0; i < this.cellCount; i++)
				{
					value.Set(i, innerGrid[i]);
				}
			}
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x000A56D0 File Offset: 0x000A38D0
		public bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> __)
		{
			this.Prune();
			bool result = false;
			CellIndices cellIndices = this.map.cellIndices;
			foreach (Area area in this.map.areaManager.AllAreas)
			{
				NativeBitArray value;
				if (!this.areaToBits.TryGetValue(area, out value))
				{
					value = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
					this.areaToBits[area] = value;
					BoolGrid innerGrid = area.InnerGrid;
					for (int i = 0; i < this.cellCount; i++)
					{
						value.Set(i, innerGrid[i]);
					}
					result = true;
				}
				List<IntVec3> list;
				if (this.areaToDeltas.TryGetValue(area, out list))
				{
					BoolGrid innerGrid2 = area.InnerGrid;
					foreach (IntVec3 c in list)
					{
						int num = cellIndices.CellToIndex(c);
						value.Set(num, innerGrid2[num]);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x000A5814 File Offset: 0x000A3A14
		public void Notify_AreaDelta(Area area, IntVec3 cell)
		{
			List<IntVec3> list;
			if (!this.areaToDeltas.TryGetValue(area, out list))
			{
				list = new List<IntVec3>();
				this.areaToDeltas[area] = list;
			}
			list.Add(cell);
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x000A584C File Offset: 0x000A3A4C
		private void Prune()
		{
			List<Area> allAreas = this.map.areaManager.AllAreas;
			foreach (Area item in this.areaToBits.Keys)
			{
				if (!allAreas.Contains(item))
				{
					this.tmpToPrune.Add(item);
				}
			}
			foreach (Area key in this.tmpToPrune)
			{
				this.areaToBits[key].Dispose();
				this.areaToBits.Remove(key);
				this.areaToDeltas.Remove(key);
			}
			this.tmpToPrune.Clear();
		}

		// Token: 0x04001621 RID: 5665
		private readonly Map map;

		// Token: 0x04001622 RID: 5666
		private readonly int cellCount;

		// Token: 0x04001623 RID: 5667
		private readonly Dictionary<Area, NativeBitArray> areaToBits = new Dictionary<Area, NativeBitArray>();

		// Token: 0x04001624 RID: 5668
		private readonly Dictionary<Area, List<IntVec3>> areaToDeltas = new Dictionary<Area, List<IntVec3>>();

		// Token: 0x04001625 RID: 5669
		private readonly List<Area> tmpToPrune = new List<Area>();
	}
}
