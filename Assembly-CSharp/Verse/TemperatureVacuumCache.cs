using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000319 RID: 793
	public sealed class TemperatureVacuumCache : IExposable
	{
		// Token: 0x06001867 RID: 6247 RVA: 0x00092998 File Offset: 0x00090B98
		public TemperatureVacuumCache(Map map)
		{
			this.map = map;
			this.tempCache = new CachedTempInfo[map.cellIndices.NumGridCells];
			this.TemperatureVacuumSaveLoad = new TemperatureVacuumSaveLoad(map);
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x000929EC File Offset: 0x00090BEC
		public void ResetTemperatureCache()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.tempCache[i].Reset();
			}
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x00092A27 File Offset: 0x00090C27
		public void ExposeData()
		{
			this.TemperatureVacuumSaveLoad.DoExposeWork();
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00092A34 File Offset: 0x00090C34
		public void ResetCachedCellInfo(IntVec3 c)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)].Reset();
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00092A57 File Offset: 0x00090C57
		private void SetCachedCellInfo(IntVec3 c, CachedTempInfo info)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)] = info;
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x00092A78 File Offset: 0x00090C78
		public void TryCacheRegionTempInfo(IntVec3 c, Region reg)
		{
			Room room = reg.Room;
			if (room != null)
			{
				this.SetCachedCellInfo(c, new CachedTempInfo(room.ID, room.CellCount, room.Temperature, room.Vacuum));
			}
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x00092AB4 File Offset: 0x00090CB4
		public bool TryGetAverageCachedRoomTempVacuum(Room r, out float temperature, out float vacuum)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in r.Cells)
			{
				CachedTempInfo cachedTempInfo = this.map.TemperatureVacuumCache.tempCache[cellIndices.CellToIndex(c)];
				if (cachedTempInfo.numCells > 0 && !this.processedRoomIDs.Contains(cachedTempInfo.roomID))
				{
					this.relevantTempInfoList.Add(cachedTempInfo);
					this.processedRoomIDs.Add(cachedTempInfo.roomID);
				}
			}
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			foreach (CachedTempInfo cachedTempInfo2 in this.relevantTempInfoList)
			{
				num += cachedTempInfo2.numCells;
				num2 += cachedTempInfo2.temperature * (float)cachedTempInfo2.numCells;
				num3 += cachedTempInfo2.vacuum * (float)cachedTempInfo2.numCells;
			}
			temperature = num2 / (float)num;
			vacuum = num3 / (float)num;
			bool result = !this.relevantTempInfoList.NullOrEmpty<CachedTempInfo>();
			this.processedRoomIDs.Clear();
			this.relevantTempInfoList.Clear();
			return result;
		}

		// Token: 0x0400145B RID: 5211
		private readonly Map map;

		// Token: 0x0400145C RID: 5212
		internal TemperatureVacuumSaveLoad TemperatureVacuumSaveLoad;

		// Token: 0x0400145D RID: 5213
		private readonly CachedTempInfo[] tempCache;

		// Token: 0x0400145E RID: 5214
		private readonly HashSet<int> processedRoomIDs = new HashSet<int>();

		// Token: 0x0400145F RID: 5215
		private readonly List<CachedTempInfo> relevantTempInfoList = new List<CachedTempInfo>();
	}
}
