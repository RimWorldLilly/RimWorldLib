using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Unity.Collections;

namespace Verse
{
	// Token: 0x0200036E RID: 878
	public class BuildingSource : IPathFinderDataSource, IDisposable
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001AEC RID: 6892 RVA: 0x000A593C File Offset: 0x000A3B3C
		public NativeBitArray.ReadOnly Buildings
		{
			get
			{
				return this.buildings.AsReadOnly();
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x000A5949 File Offset: 0x000A3B49
		public NativeBitArray.ReadOnly Destroyable
		{
			get
			{
				return this.destroyable.AsReadOnly();
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001AEE RID: 6894 RVA: 0x000A5956 File Offset: 0x000A3B56
		public NativeBitArray.ReadOnly Player
		{
			get
			{
				return this.player.AsReadOnly();
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001AEF RID: 6895 RVA: 0x000A5963 File Offset: 0x000A3B63
		public NativeArray<ushort>.ReadOnly Hitpoints
		{
			get
			{
				return this.hitpoints.AsReadOnly();
			}
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x000A5970 File Offset: 0x000A3B70
		public BuildingSource(Map map)
		{
			this.map = map;
			this.cellCount = map.cellIndices.NumGridCells;
			this.buildings = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.destroyable = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.player = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.hitpoints = new NativeArray<ushort>(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x000A59F6 File Offset: 0x000A3BF6
		public void Dispose()
		{
			this.buildings.Dispose();
			this.destroyable.Dispose();
			this.player.Dispose();
			this.hitpoints.Dispose();
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000A5A24 File Offset: 0x000A3C24
		public void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.buildings.Clear();
			this.destroyable.Clear();
			this.player.Clear();
			this.hitpoints.Clear<ushort>();
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			for (int i = 0; i < this.cellCount; i++)
			{
				if (innerArray[i] != null)
				{
					this.SetBuildingData(i);
				}
			}
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000A5A8C File Offset: 0x000A3C8C
		public bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			foreach (IntVec3 c in cellDeltas)
			{
				int num = cellIndices.CellToIndex(c);
				if (innerArray[num] != null)
				{
					this.SetBuildingData(num);
				}
				else
				{
					this.buildings.Set(num, false);
					this.destroyable.Set(num, false);
					this.player.Set(num, false);
					this.hitpoints[num] = 0;
				}
			}
			return false;
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x000A5B44 File Offset: 0x000A3D44
		private void SetBuildingData(int index)
		{
			Building building = this.map.edificeGrid[index];
			if (building == null)
			{
				return;
			}
			if (building.def.Fillage == FillCategory.Full)
			{
				this.buildings.Set(index, true);
				this.destroyable.Set(index, building.def.destroyable);
				this.hitpoints[index] = (ushort)Math.Clamp(building.HitPoints, 0, 65535);
			}
			else
			{
				this.buildings.Set(index, false);
				this.destroyable.Set(index, false);
				this.hitpoints[index] = 0;
			}
			Faction faction = building.Faction;
			this.player.Set(index, faction != null && faction.IsPlayer);
		}

		// Token: 0x04001626 RID: 5670
		private readonly Map map;

		// Token: 0x04001627 RID: 5671
		private readonly int cellCount;

		// Token: 0x04001628 RID: 5672
		private NativeBitArray buildings;

		// Token: 0x04001629 RID: 5673
		private NativeBitArray destroyable;

		// Token: 0x0400162A RID: 5674
		private NativeBitArray player;

		// Token: 0x0400162B RID: 5675
		private NativeArray<ushort> hitpoints;
	}
}
