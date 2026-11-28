using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020002EB RID: 747
	public sealed class District
	{
		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060016AE RID: 5806 RVA: 0x00088038 File Offset: 0x00086238
		public Map Map
		{
			get
			{
				if (this.mapIndex >= 0)
				{
					return Find.Maps[(int)this.mapIndex];
				}
				return null;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060016AF RID: 5807 RVA: 0x00088055 File Offset: 0x00086255
		public RegionType RegionType
		{
			get
			{
				if (!this.regions.Any<Region>())
				{
					return RegionType.None;
				}
				return this.regions[0].type;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060016B0 RID: 5808 RVA: 0x00088077 File Offset: 0x00086277
		public List<Region> Regions
		{
			get
			{
				return this.regions;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060016B1 RID: 5809 RVA: 0x0008807F File Offset: 0x0008627F
		public int RegionCount
		{
			get
			{
				return this.regions.Count;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060016B2 RID: 5810 RVA: 0x0008808C File Offset: 0x0008628C
		public bool TouchesMapEdge
		{
			get
			{
				return this.numRegionsTouchingMapEdge > 0;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060016B3 RID: 5811 RVA: 0x00088097 File Offset: 0x00086297
		public bool Passable
		{
			get
			{
				return this.RegionType.Passable();
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060016B4 RID: 5812 RVA: 0x000880A4 File Offset: 0x000862A4
		public bool IsDoorway
		{
			get
			{
				return this.regions.Count == 1 && this.regions[0].IsDoorway;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060016B5 RID: 5813 RVA: 0x000880C7 File Offset: 0x000862C7
		// (set) Token: 0x060016B6 RID: 5814 RVA: 0x000880CF File Offset: 0x000862CF
		public Room Room
		{
			get
			{
				return this.roomInt;
			}
			set
			{
				if (value == this.roomInt)
				{
					return;
				}
				if (this.roomInt != null)
				{
					this.roomInt.RemoveDistrict(this);
				}
				this.roomInt = value;
				if (this.roomInt != null)
				{
					this.roomInt.AddDistrict(this);
				}
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x0008810C File Offset: 0x0008630C
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.regions.Count; i++)
					{
						this.cachedCellCount += this.regions[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060016B8 RID: 5816 RVA: 0x00088164 File Offset: 0x00086364
		public List<District> Neighbors
		{
			get
			{
				this.uniqueNeighborsSet.Clear();
				this.uniqueNeighbors.Clear();
				for (int i = 0; i < this.regions.Count; i++)
				{
					foreach (Region region in this.regions[i].Neighbors)
					{
						if (this.uniqueNeighborsSet.Add(region.District) && region.District != this)
						{
							this.uniqueNeighbors.Add(region.District);
						}
					}
				}
				this.uniqueNeighborsSet.Clear();
				return this.uniqueNeighbors;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060016B9 RID: 5817 RVA: 0x00088220 File Offset: 0x00086420
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				District.<get_Cells>d__37 <get_Cells>d__ = new District.<get_Cells>d__37(-2);
				<get_Cells>d__.<>4__this = this;
				return <get_Cells>d__;
			}
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x00088230 File Offset: 0x00086430
		public static District MakeNew(Map map)
		{
			District district = new District();
			district.mapIndex = (sbyte)map.Index;
			district.ID = District.nextDistrictID;
			District.nextDistrictID++;
			return district;
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0008825C File Offset: 0x0008645C
		public void AddRegion(Region r)
		{
			if (this.regions.Contains(r))
			{
				Log.Error("Tried to add the same region twice to District. region=" + ((r != null) ? r.ToString() : null) + ", district=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.regions.Add(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge++;
			}
			if (this.regions.Count == 1)
			{
				this.Map.regionGrid.allDistricts.Add(this);
			}
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x000882EC File Offset: 0x000864EC
		public void RemoveRegion(Region r)
		{
			if (!this.regions.Contains(r))
			{
				Log.Error("Tried to remove region from District but this region is not here. region=" + ((r != null) ? r.ToString() : null) + ", district=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.regions.Remove(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge--;
			}
			if (this.regions.Count == 0)
			{
				this.Room = null;
				this.cachedOpenRoofCount = -1;
				this.cachedExposedCount = -1;
				IEnumerator<IntVec3> enumerator = this.cachedOpenRoofState;
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
				this.cachedOpenRoofState = null;
				IEnumerator<IntVec3> enumerator2 = this.cachedExposedState;
				if (enumerator2 != null)
				{
					enumerator2.Dispose();
				}
				this.cachedExposedState = null;
				Map map = this.Map;
				if (map == null)
				{
					return;
				}
				map.regionGrid.allDistricts.Remove(this);
			}
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x000883C7 File Offset: 0x000865C7
		public void Notify_MyMapRemoved()
		{
			this.mapIndex = -1;
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000883D0 File Offset: 0x000865D0
		public void Notify_TerrainChanged()
		{
			this.cachedExposedCount = -1;
			IEnumerator<IntVec3> enumerator = this.cachedExposedState;
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
			this.cachedExposedState = null;
			this.Room.Notify_TerrainChanged();
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x000883FC File Offset: 0x000865FC
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedExposedCount = -1;
			IEnumerator<IntVec3> enumerator = this.cachedOpenRoofState;
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
			this.cachedOpenRoofState = null;
			IEnumerator<IntVec3> enumerator2 = this.cachedExposedState;
			if (enumerator2 != null)
			{
				enumerator2.Dispose();
			}
			this.cachedExposedState = null;
			this.Room.Notify_RoofChanged();
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x00088454 File Offset: 0x00086654
		public void Notify_RoomShapeOrContainedBedsChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.cachedExposedCount = -1;
			IEnumerator<IntVec3> enumerator = this.cachedOpenRoofState;
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
			this.cachedOpenRoofState = null;
			IEnumerator<IntVec3> enumerator2 = this.cachedExposedState;
			if (enumerator2 != null)
			{
				enumerator2.Dispose();
			}
			this.cachedExposedState = null;
			AnimalPenConnectedDistrictsCalculator.InvalidateDistrictCache(this);
			this.lastChangeTick = Find.TickManager.TicksGame;
			FacilitiesUtility.NotifyFacilitiesAboutChangedLOSBlockers(this.regions);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x000884C8 File Offset: 0x000866C8
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning("Tried to decrement map index for district " + this.ID.ToString() + ", but mapIndex=" + this.mapIndex.ToString());
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00088518 File Offset: 0x00086718
		public int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount == -1 && this.cachedOpenRoofState == null)
			{
				this.cachedOpenRoofCount = 0;
				this.cachedOpenRoofState = this.Cells.GetEnumerator();
			}
			if (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState != null)
			{
				RoofGrid roofGrid = this.Map.roofGrid;
				while (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState.MoveNext())
				{
					if (!roofGrid.Roofed(this.cachedOpenRoofState.Current))
					{
						this.cachedOpenRoofCount++;
					}
				}
				if (this.cachedOpenRoofCount < threshold)
				{
					this.cachedOpenRoofState.Dispose();
					this.cachedOpenRoofState = null;
				}
			}
			return this.cachedOpenRoofCount;
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x000885C8 File Offset: 0x000867C8
		public int ExposedVacuumCount(int threshold)
		{
			if (this.cachedExposedCount == -1 && this.cachedExposedState == null)
			{
				this.cachedExposedCount = 0;
				this.cachedExposedState = this.Cells.GetEnumerator();
			}
			if (this.cachedExposedCount < threshold && this.cachedExposedState != null)
			{
				RoofGrid roofGrid = this.Map.roofGrid;
				TerrainGrid terrainGrid = this.Map.terrainGrid;
				while (this.cachedExposedCount < threshold && this.cachedExposedState.MoveNext())
				{
					IntVec3 c = this.cachedExposedState.Current;
					if (!roofGrid.Roofed(c) || terrainGrid.TerrainAt(c).exposesToVacuum)
					{
						this.cachedExposedCount++;
					}
				}
				if (this.cachedExposedCount < threshold)
				{
					this.cachedExposedState.Dispose();
					this.cachedExposedState = null;
				}
			}
			return this.cachedExposedCount;
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00088694 File Offset: 0x00086894
		internal void DebugDraw()
		{
			int hashCode = this.GetHashCode();
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)hashCode * 0.01f);
			}
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x000886F0 File Offset: 0x000868F0
		internal string DebugString()
		{
			return string.Concat(new string[]
			{
				"District ID=",
				this.ID.ToString(),
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				"\n  RegionCount=",
				this.RegionCount.ToString(),
				"\n  RegionType=",
				this.RegionType.ToString(),
				"\n  CellCount=",
				this.CellCount.ToString(),
				"\n  OpenRoofCount=",
				this.OpenRoofCountStopAt(int.MaxValue).ToString(),
				"\n  numRegionsTouchingMapEdge=",
				this.numRegionsTouchingMapEdge.ToString(),
				"\n  lastChangeTick=",
				this.lastChangeTick.ToString(),
				"\n  Room=",
				(this.Room != null) ? this.Room.ID.ToString() : "null"
			});
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00088814 File Offset: 0x00086A14
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"District(districtID=",
				this.ID.ToString(),
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				", lastChangeTick=",
				this.lastChangeTick.ToString(),
				")"
			});
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x0008889E File Offset: 0x00086A9E
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478890);
		}

		// Token: 0x04001373 RID: 4979
		public sbyte mapIndex = -1;

		// Token: 0x04001374 RID: 4980
		private Room roomInt;

		// Token: 0x04001375 RID: 4981
		private List<Region> regions = new List<Region>();

		// Token: 0x04001376 RID: 4982
		public int ID = -16161616;

		// Token: 0x04001377 RID: 4983
		public int lastChangeTick = -1;

		// Token: 0x04001378 RID: 4984
		private int numRegionsTouchingMapEdge;

		// Token: 0x04001379 RID: 4985
		private int cachedOpenRoofCount = -1;

		// Token: 0x0400137A RID: 4986
		private int cachedExposedCount = -1;

		// Token: 0x0400137B RID: 4987
		private IEnumerator<IntVec3> cachedOpenRoofState;

		// Token: 0x0400137C RID: 4988
		private IEnumerator<IntVec3> cachedExposedState;

		// Token: 0x0400137D RID: 4989
		private int cachedCellCount = -1;

		// Token: 0x0400137E RID: 4990
		public int newOrReusedRoomIndex = -1;

		// Token: 0x0400137F RID: 4991
		private static int nextDistrictID;

		// Token: 0x04001380 RID: 4992
		private HashSet<District> uniqueNeighborsSet = new HashSet<District>();

		// Token: 0x04001381 RID: 4993
		private List<District> uniqueNeighbors = new List<District>();
	}
}
