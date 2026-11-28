using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020002F7 RID: 759
	public class RegionAndRoomUpdater
	{
		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001723 RID: 5923 RVA: 0x0008B0CE File Offset: 0x000892CE
		// (set) Token: 0x06001724 RID: 5924 RVA: 0x0008B0D6 File Offset: 0x000892D6
		public bool Enabled
		{
			get
			{
				return this.enabledInt;
			}
			set
			{
				this.enabledInt = value;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001725 RID: 5925 RVA: 0x0008B0DF File Offset: 0x000892DF
		public bool AnythingToRebuild
		{
			get
			{
				return this.map.regionDirtyer.AnyDirty || !this.initialized;
			}
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0008B100 File Offset: 0x00089300
		public RegionAndRoomUpdater(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x0008B18F File Offset: 0x0008938F
		public void RebuildAllRegionsAndRooms()
		{
			if (!this.Enabled)
			{
				Log.Warning("Called RebuildAllRegionsAndRooms() but RegionAndRoomUpdater is disabled. Regions won't be rebuilt.");
			}
			this.map.TemperatureVacuumCache.ResetTemperatureCache();
			this.map.regionDirtyer.SetAllDirty();
			this.TryRebuildDirtyRegionsAndRooms();
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x0008B1CC File Offset: 0x000893CC
		public void TryRebuildDirtyRegionsAndRooms()
		{
			if (this.working || !this.Enabled)
			{
				return;
			}
			this.working = true;
			if (!this.initialized)
			{
				this.RebuildAllRegionsAndRooms();
			}
			if (!this.map.regionDirtyer.AnyDirty)
			{
				this.working = false;
				return;
			}
			try
			{
				this.RegenerateNewRegionsFromDirtyCells();
				this.CreateOrUpdateRooms();
			}
			catch (Exception ex)
			{
				string str = "Exception while rebuilding dirty regions: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			this.newRegions.Clear();
			this.map.regionDirtyer.SetAllClean();
			this.initialized = true;
			this.working = false;
			if (DebugSettings.detectRegionListersBugs)
			{
				Autotests_RegionListers.CheckBugs(this.map);
			}
			this.map.events.Notify_RegionsRoomsChanged();
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x0008B2A4 File Offset: 0x000894A4
		private void RegenerateNewRegionsFromDirtyCells()
		{
			this.newRegions.Clear();
			foreach (IntVec3 intVec in this.map.regionDirtyer.DirtyCells)
			{
				if (intVec.GetRegion(this.map, RegionType.Set_All) == null)
				{
					Region region = this.map.regionMaker.TryGenerateRegionFrom(intVec);
					if (region != null)
					{
						this.newRegions.Add(region);
					}
				}
			}
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x0008B338 File Offset: 0x00089538
		private void CreateOrUpdateRooms()
		{
			this.newDistricts.Clear();
			this.reusedOldDistricts.Clear();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			int numRegionGroups = this.CombineNewRegionsIntoContiguousGroups();
			this.CreateOrAttachToExistingDistricts(numRegionGroups);
			int numRooms = this.CombineNewAndReusedDistrictsIntoContiguousRooms();
			this.CreateOrAttachToExistingRooms(numRooms);
			this.NotifyAffectedDistrictsAndRoomsAndUpdateTemperatureVacuum();
			this.newDistricts.Clear();
			this.reusedOldDistricts.Clear();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x0008B3C0 File Offset: 0x000895C0
		private int CombineNewRegionsIntoContiguousGroups()
		{
			int num = 0;
			for (int i = 0; i < this.newRegions.Count; i++)
			{
				if (this.newRegions[i].newRegionGroupIndex < 0)
				{
					RegionTraverser.FloodAndSetNewRegionIndex(this.newRegions[i], num);
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x0008B410 File Offset: 0x00089610
		private void CreateOrAttachToExistingDistricts(int numRegionGroups)
		{
			for (int i = 0; i < numRegionGroups; i++)
			{
				this.currentRegionGroup.Clear();
				for (int j = 0; j < this.newRegions.Count; j++)
				{
					if (this.newRegions[j].newRegionGroupIndex == i)
					{
						this.currentRegionGroup.Add(this.newRegions[j]);
					}
				}
				if (!this.currentRegionGroup[0].type.AllowsMultipleRegionsPerDistrict())
				{
					if (this.currentRegionGroup.Count != 1)
					{
						Log.Error("Region type doesn't allow multiple regions per room but there are >1 regions in this group.");
					}
					District district = District.MakeNew(this.map);
					this.currentRegionGroup[0].District = district;
					this.newDistricts.Add(district);
				}
				else
				{
					bool flag;
					District district2 = this.FindCurrentRegionGroupNeighborWithMostRegions(out flag);
					if (district2 == null)
					{
						District item = RegionTraverser.FloodAndSetDistricts(this.currentRegionGroup[0], this.map, null);
						this.newDistricts.Add(item);
					}
					else if (!flag)
					{
						for (int k = 0; k < this.currentRegionGroup.Count; k++)
						{
							this.currentRegionGroup[k].District = district2;
						}
						this.reusedOldDistricts.Add(district2);
					}
					else
					{
						RegionTraverser.FloodAndSetDistricts(this.currentRegionGroup[0], this.map, district2);
						this.reusedOldDistricts.Add(district2);
					}
				}
			}
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x0008B578 File Offset: 0x00089778
		private int CombineNewAndReusedDistrictsIntoContiguousRooms()
		{
			int num = 0;
			foreach (District district in this.reusedOldDistricts)
			{
				district.newOrReusedRoomIndex = -1;
			}
			foreach (District district2 in this.reusedOldDistricts.Concat(this.newDistricts))
			{
				if (district2.newOrReusedRoomIndex < 0)
				{
					this.tmpDistrictStack.Clear();
					this.tmpDistrictStack.Push(district2);
					district2.newOrReusedRoomIndex = num;
					while (this.tmpDistrictStack.Count != 0)
					{
						District district3 = this.tmpDistrictStack.Pop();
						foreach (District district4 in district3.Neighbors)
						{
							if (district4.newOrReusedRoomIndex < 0 && this.ShouldBeInTheSameRoom(district3, district4))
							{
								district4.newOrReusedRoomIndex = num;
								this.tmpDistrictStack.Push(district4);
							}
						}
					}
					this.tmpDistrictStack.Clear();
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x0008B6D0 File Offset: 0x000898D0
		private void CreateOrAttachToExistingRooms(int numRooms)
		{
			for (int i = 0; i < numRooms; i++)
			{
				this.currentDistrictGroup.Clear();
				foreach (District district in this.reusedOldDistricts)
				{
					if (district.newOrReusedRoomIndex == i)
					{
						this.currentDistrictGroup.Add(district);
					}
				}
				for (int j = 0; j < this.newDistricts.Count; j++)
				{
					if (this.newDistricts[j].newOrReusedRoomIndex == i)
					{
						this.currentDistrictGroup.Add(this.newDistricts[j]);
					}
				}
				bool flag;
				Room room = this.FindCurrentRoomNeighborWithMostRegions(out flag);
				if (room == null)
				{
					Room room2 = Room.MakeNew(this.map);
					this.roomLookup[room2.ID] = room2;
					this.FloodAndSetRooms(this.currentDistrictGroup[0], room2);
					this.newRooms.Add(room2);
				}
				else if (!flag)
				{
					for (int k = 0; k < this.currentDistrictGroup.Count; k++)
					{
						this.currentDistrictGroup[k].Room = room;
					}
					this.reusedOldRooms.Add(room);
				}
				else
				{
					this.FloodAndSetRooms(this.currentDistrictGroup[0], room);
					this.reusedOldRooms.Add(room);
				}
			}
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x0008B848 File Offset: 0x00089A48
		private void FloodAndSetRooms(District start, Room room)
		{
			this.tmpDistrictStack.Clear();
			this.tmpDistrictStack.Push(start);
			this.tmpVisitedDistricts.Clear();
			this.tmpVisitedDistricts.Add(start);
			while (this.tmpDistrictStack.Count != 0)
			{
				District district = this.tmpDistrictStack.Pop();
				district.Room = room;
				foreach (District district2 in district.Neighbors)
				{
					if (!this.tmpVisitedDistricts.Contains(district2) && this.ShouldBeInTheSameRoom(district, district2))
					{
						this.tmpDistrictStack.Push(district2);
						this.tmpVisitedDistricts.Add(district2);
					}
				}
			}
			this.tmpVisitedDistricts.Clear();
			this.tmpDistrictStack.Clear();
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x0008B92C File Offset: 0x00089B2C
		private void NotifyAffectedDistrictsAndRoomsAndUpdateTemperatureVacuum()
		{
			foreach (District district in this.reusedOldDistricts)
			{
				district.Notify_RoomShapeOrContainedBedsChanged();
			}
			for (int i = 0; i < this.newDistricts.Count; i++)
			{
				this.newDistricts[i].Notify_RoomShapeOrContainedBedsChanged();
			}
			foreach (Room room in this.reusedOldRooms)
			{
				room.Notify_RoomShapeChanged();
			}
			for (int j = 0; j < this.newRooms.Count; j++)
			{
				Room room2 = this.newRooms[j];
				room2.Notify_RoomShapeChanged();
				float temperature;
				float vacuum;
				if (this.map.TemperatureVacuumCache.TryGetAverageCachedRoomTempVacuum(room2, out temperature, out vacuum))
				{
					room2.Temperature = temperature;
					room2.Vacuum = vacuum;
				}
				else if (this.map.Biome.inVacuum)
				{
					room2.Vacuum = 1f;
				}
			}
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x0008BA58 File Offset: 0x00089C58
		private District FindCurrentRegionGroupNeighborWithMostRegions(out bool multipleOldNeighborDistricts)
		{
			multipleOldNeighborDistricts = false;
			District district = null;
			for (int i = 0; i < this.currentRegionGroup.Count; i++)
			{
				foreach (Region region in this.currentRegionGroup[i].NeighborsOfSameType)
				{
					if (region.District != null && !this.reusedOldDistricts.Contains(region.District))
					{
						if (district == null)
						{
							district = region.District;
						}
						else if (region.District != district)
						{
							multipleOldNeighborDistricts = true;
							if (region.District.RegionCount > district.RegionCount)
							{
								district = region.District;
							}
						}
					}
				}
			}
			return district;
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x0008BB18 File Offset: 0x00089D18
		private Room FindCurrentRoomNeighborWithMostRegions(out bool multipleOldNeighborRooms)
		{
			multipleOldNeighborRooms = false;
			Room room = null;
			for (int i = 0; i < this.currentDistrictGroup.Count; i++)
			{
				foreach (District district in this.currentDistrictGroup[i].Neighbors)
				{
					if (district.Room != null && this.ShouldBeInTheSameRoom(this.currentDistrictGroup[i], district) && !this.reusedOldRooms.Contains(district.Room))
					{
						if (room == null)
						{
							room = district.Room;
						}
						else if (district.Room != room)
						{
							multipleOldNeighborRooms = true;
							if (district.Room.RegionCount > room.RegionCount)
							{
								room = district.Room;
							}
						}
					}
				}
			}
			return room;
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x0008BBF4 File Offset: 0x00089DF4
		private bool ShouldBeInTheSameRoom(District a, District b)
		{
			RegionType regionType = a.RegionType;
			RegionType regionType2 = b.RegionType;
			return (regionType == RegionType.Normal || regionType == RegionType.ImpassableFreeAirExchange || regionType == RegionType.Fence) && (regionType2 == RegionType.Normal || regionType2 == RegionType.ImpassableFreeAirExchange || regionType2 == RegionType.Fence);
		}

		// Token: 0x040013BA RID: 5050
		private Map map;

		// Token: 0x040013BB RID: 5051
		private List<Region> newRegions = new List<Region>();

		// Token: 0x040013BC RID: 5052
		private List<District> newDistricts = new List<District>();

		// Token: 0x040013BD RID: 5053
		private HashSet<District> reusedOldDistricts = new HashSet<District>();

		// Token: 0x040013BE RID: 5054
		private List<Room> newRooms = new List<Room>();

		// Token: 0x040013BF RID: 5055
		private HashSet<Room> reusedOldRooms = new HashSet<Room>();

		// Token: 0x040013C0 RID: 5056
		private List<Region> currentRegionGroup = new List<Region>();

		// Token: 0x040013C1 RID: 5057
		private List<District> currentDistrictGroup = new List<District>();

		// Token: 0x040013C2 RID: 5058
		private Stack<District> tmpDistrictStack = new Stack<District>();

		// Token: 0x040013C3 RID: 5059
		private HashSet<District> tmpVisitedDistricts = new HashSet<District>();

		// Token: 0x040013C4 RID: 5060
		private bool initialized;

		// Token: 0x040013C5 RID: 5061
		private bool working;

		// Token: 0x040013C6 RID: 5062
		private bool enabledInt = true;

		// Token: 0x040013C7 RID: 5063
		public Dictionary<int, Room> roomLookup = new Dictionary<int, Room>();
	}
}
