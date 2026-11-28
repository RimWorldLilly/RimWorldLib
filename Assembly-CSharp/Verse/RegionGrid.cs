using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002F9 RID: 761
	public sealed class RegionGrid : IDisposable
	{
		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x0008C010 File Offset: 0x0008A210
		public IReadOnlyList<Room> AllRooms
		{
			get
			{
				return this.allRooms;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x0008C018 File Offset: 0x0008A218
		public IReadOnlyDictionary<int, Room> RoomLookup
		{
			get
			{
				return this.roomLookup;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x0008C020 File Offset: 0x0008A220
		public Region[] DirectGrid
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get the region grid but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				return this.regionGrid;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x0008C071 File Offset: 0x0008A271
		public IEnumerable<Region> AllRegions_NoRebuild_InvalidAllowed
		{
			get
			{
				RegionGrid.<get_AllRegions_NoRebuild_InvalidAllowed>d__15 <get_AllRegions_NoRebuild_InvalidAllowed>d__ = new RegionGrid.<get_AllRegions_NoRebuild_InvalidAllowed>d__15(-2);
				<get_AllRegions_NoRebuild_InvalidAllowed>d__.<>4__this = this;
				return <get_AllRegions_NoRebuild_InvalidAllowed>d__;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x0008C081 File Offset: 0x0008A281
		public IEnumerable<Region> AllRegions
		{
			get
			{
				RegionGrid.<get_AllRegions>d__17 <get_AllRegions>d__ = new RegionGrid.<get_AllRegions>d__17(-2);
				<get_AllRegions>d__.<>4__this = this;
				return <get_AllRegions>d__;
			}
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x0008C094 File Offset: 0x0008A294
		public RegionGrid(Map map)
		{
			this.map = map;
			this.regionGrid = new Region[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x0008C0F0 File Offset: 0x0008A2F0
		public Region GetValidRegionAt(IntVec3 c)
		{
			if (this.disposed)
			{
				return null;
			}
			if (!c.InBounds(this.map))
			{
				string str = "Tried to get valid region out of bounds at ";
				IntVec3 intVec = c;
				Log.Error(str + intVec.ToString());
				return null;
			}
			if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
			{
				string str2 = "Trying to get valid region at ";
				IntVec3 intVec = c;
				Log.Warning(str2 + intVec.ToString() + " but RegionAndRoomUpdater is disabled. The result may be incorrect.");
			}
			this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x0008C1B4 File Offset: 0x0008A3B4
		public Region GetValidRegionAt_NoRebuild(IntVec3 c)
		{
			if (this.disposed)
			{
				return null;
			}
			if (!c.InBounds(this.map))
			{
				string str = "Tried to get valid region out of bounds at ";
				IntVec3 intVec = c;
				Log.Error(str + intVec.ToString());
				return null;
			}
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x0008C220 File Offset: 0x0008A420
		public Region GetRegionAt_NoRebuild_InvalidAllowed(IntVec3 c)
		{
			if (this.disposed)
			{
				return null;
			}
			return this.regionGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x0008C244 File Offset: 0x0008A444
		public void SetRegionAt(IntVec3 c, Region reg)
		{
			this.regionGrid[this.map.cellIndices.CellToIndex(c)] = reg;
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x0008C25F File Offset: 0x0008A45F
		public void RoomAdded(Room room)
		{
			this.allRooms.Add(room);
			this.roomLookup[room.ID] = room;
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x0008C27F File Offset: 0x0008A47F
		public void RoomRemoved(Room room)
		{
			this.allRooms.Remove(room);
			this.roomLookup.Remove(room.ID);
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x0008C2A0 File Offset: 0x0008A4A0
		public void UpdateClean()
		{
			if (this.disposed)
			{
				return;
			}
			for (int i = 0; i < 16; i++)
			{
				if (this.curCleanIndex >= this.regionGrid.Length)
				{
					this.curCleanIndex = 0;
				}
				Region region = this.regionGrid[this.curCleanIndex];
				if (region != null && !region.valid)
				{
					this.regionGrid[this.curCleanIndex] = null;
				}
				this.curCleanIndex++;
			}
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x0008C310 File Offset: 0x0008A510
		public void DebugDraw()
		{
			if (this.map != Find.CurrentMap || this.disposed)
			{
				return;
			}
			if (DebugViewSettings.drawRegionTraversal)
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				currentViewRect.ClipInsideMap(this.map);
				foreach (IntVec3 c in currentViewRect)
				{
					Region validRegionAt = this.GetValidRegionAt(c);
					if (validRegionAt != null && !this.drawnRegions.Contains(validRegionAt))
					{
						validRegionAt.DebugDraw();
						this.drawnRegions.Add(validRegionAt);
					}
				}
				this.drawnRegions.Clear();
			}
			IntVec3 intVec = UI.MouseCell();
			if (intVec.InBounds(this.map))
			{
				if (DebugViewSettings.drawDistricts)
				{
					District district = intVec.GetDistrict(this.map, RegionType.Set_Passable);
					if (district != null)
					{
						district.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRooms)
				{
					Room room = intVec.GetRoom(this.map);
					if (room != null)
					{
						room.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRegions || DebugViewSettings.drawRegionLinks || DebugViewSettings.drawRegionThings)
				{
					Region regionAt_NoRebuild_InvalidAllowed = this.GetRegionAt_NoRebuild_InvalidAllowed(intVec);
					if (regionAt_NoRebuild_InvalidAllowed != null)
					{
						regionAt_NoRebuild_InvalidAllowed.DebugDrawMouseover();
					}
				}
			}
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x0008C44C File Offset: 0x0008A64C
		public void Dispose()
		{
			this.regionGrid = null;
			this.allDistricts.Clear();
			this.allRooms.Clear();
			this.roomLookup.Clear();
			RegionGrid.allRegionsYielded.Clear();
			this.disposed = true;
		}

		// Token: 0x040013CB RID: 5067
		private readonly Map map;

		// Token: 0x040013CC RID: 5068
		private Region[] regionGrid;

		// Token: 0x040013CD RID: 5069
		private int curCleanIndex;

		// Token: 0x040013CE RID: 5070
		private bool disposed;

		// Token: 0x040013CF RID: 5071
		public List<District> allDistricts = new List<District>();

		// Token: 0x040013D0 RID: 5072
		private List<Room> allRooms = new List<Room>();

		// Token: 0x040013D1 RID: 5073
		private Dictionary<int, Room> roomLookup = new Dictionary<int, Room>();

		// Token: 0x040013D2 RID: 5074
		private static HashSet<Region> allRegionsYielded = new HashSet<Region>();

		// Token: 0x040013D3 RID: 5075
		private const int CleanSquaresPerFrame = 16;

		// Token: 0x040013D4 RID: 5076
		private HashSet<Region> drawnRegions = new HashSet<Region>();
	}
}
