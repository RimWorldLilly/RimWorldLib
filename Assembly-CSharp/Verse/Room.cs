using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000306 RID: 774
	public class Room
	{
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x0008D58A File Offset: 0x0008B78A
		public List<District> Districts
		{
			get
			{
				return this.districts;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x0008D592 File Offset: 0x0008B792
		public Map Map
		{
			get
			{
				if (!this.districts.Any<District>())
				{
					return null;
				}
				return this.districts[0].Map;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x0008D5B4 File Offset: 0x0008B7B4
		public int DistrictCount
		{
			get
			{
				return this.districts.Count;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600178E RID: 6030 RVA: 0x0008D5C1 File Offset: 0x0008B7C1
		public RoomTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x0008D5C9 File Offset: 0x0008B7C9
		// (set) Token: 0x06001790 RID: 6032 RVA: 0x0008D5D6 File Offset: 0x0008B7D6
		public float Temperature
		{
			get
			{
				return this.tempTracker.Temperature;
			}
			set
			{
				this.tempTracker.Temperature = value;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x0008D5E4 File Offset: 0x0008B7E4
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.TouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)this.CellCount * 0.25f);
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001792 RID: 6034 RVA: 0x0008D60D File Offset: 0x0008B80D
		public bool ExposedToSpace
		{
			get
			{
				return this.Map.Biome.inVacuum && (this.TouchesMapEdge || this.ExposedCountStopAt(1) > 0);
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001793 RID: 6035 RVA: 0x0008D637 File Offset: 0x0008B837
		public bool Dereferenced
		{
			get
			{
				return this.RegionCount == 0;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001794 RID: 6036 RVA: 0x0008D642 File Offset: 0x0008B842
		public bool IsHuge
		{
			get
			{
				return this.RegionCount > 60;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001795 RID: 6037 RVA: 0x0008D64E File Offset: 0x0008B84E
		public bool IsPrisonCell
		{
			get
			{
				return this.isPrisonCell;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x0008D658 File Offset: 0x0008B858
		// (set) Token: 0x06001797 RID: 6039 RVA: 0x0008D78C File Offset: 0x0008B98C
		public float Vacuum
		{
			get
			{
				if (!this.Map.Biome.inVacuum)
				{
					return 0f;
				}
				if (this.ExposedToSpace)
				{
					return this.vacuum = 1f;
				}
				if (this.IsDoorway)
				{
					if (!this.Map.regionAndRoomUpdater.Enabled)
					{
						return 0f;
					}
					float num = 0f;
					Region region = this.Regions[0];
					foreach (RegionLink regionLink in region.links)
					{
						Region otherRegion = regionLink.GetOtherRegion(region);
						Room room = (otherRegion != null) ? otherRegion.Room : null;
						if (room != null)
						{
							num += room.UnsanitizedVacuum;
						}
					}
					if (Mathf.Approximately(num, 0f) || region.links.Count == 0)
					{
						return 0f;
					}
					num /= (float)region.links.Count;
					return this.vacuum = ((num <= 0.01f) ? 0f : num);
				}
				else
				{
					if (this.vacuum > 0.01f)
					{
						return this.vacuum;
					}
					return 0f;
				}
			}
			set
			{
				this.vacuum = Mathf.Clamp(value, -0.1f, 1f);
				foreach (Thing thing in this.uniqueContainedThings)
				{
					ThingWithComps thingWithComps = thing as ThingWithComps;
					if (thingWithComps != null)
					{
						thingWithComps.BroadcastCompSignal("Vacuum");
					}
				}
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001798 RID: 6040 RVA: 0x0008D804 File Offset: 0x0008BA04
		internal float UnsanitizedVacuum
		{
			get
			{
				return this.vacuum;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x0008D80C File Offset: 0x0008BA0C
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				Room.<get_Cells>d__51 <get_Cells>d__ = new Room.<get_Cells>d__51(-2);
				<get_Cells>d__.<>4__this = this;
				return <get_Cells>d__;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x0008D81C File Offset: 0x0008BA1C
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.districts.Count; i++)
					{
						this.cachedCellCount += this.districts[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x0600179B RID: 6043 RVA: 0x0008D874 File Offset: 0x0008BA74
		public Region FirstRegion
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					List<Region> regions = this.districts[i].Regions;
					if (regions.Count > 0)
					{
						return regions[0];
					}
				}
				return null;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x0008D8BC File Offset: 0x0008BABC
		public List<Region> Regions
		{
			get
			{
				this.tmpRegions.Clear();
				for (int i = 0; i < this.districts.Count; i++)
				{
					List<Region> regions = this.districts[i].Regions;
					for (int j = 0; j < regions.Count; j++)
					{
						this.tmpRegions.Add(regions[j]);
					}
				}
				return this.tmpRegions;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600179D RID: 6045 RVA: 0x0008D928 File Offset: 0x0008BB28
		public int RegionCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.districts.Count; i++)
				{
					num += this.districts[i].RegionCount;
				}
				return num;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600179E RID: 6046 RVA: 0x0008D964 File Offset: 0x0008BB64
		public CellRect ExtentsClose
		{
			get
			{
				CellRect cellRect = new CellRect(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
				foreach (Region region in this.Regions)
				{
					if (region.extentsClose.minX < cellRect.minX)
					{
						cellRect.minX = region.extentsClose.minX;
					}
					if (region.extentsClose.minZ < cellRect.minZ)
					{
						cellRect.minZ = region.extentsClose.minZ;
					}
					if (region.extentsClose.maxX > cellRect.maxX)
					{
						cellRect.maxX = region.extentsClose.maxX;
					}
					if (region.extentsClose.maxZ > cellRect.maxZ)
					{
						cellRect.maxZ = region.extentsClose.maxZ;
					}
				}
				return cellRect;
			}
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x0008DA68 File Offset: 0x0008BC68
		private int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount != -1)
			{
				return this.cachedOpenRoofCount;
			}
			int num = 0;
			for (int i = 0; i < this.districts.Count; i++)
			{
				num += this.districts[i].OpenRoofCountStopAt(threshold);
				if (num >= threshold)
				{
					return num;
				}
				threshold -= num;
			}
			return num;
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0008DAC0 File Offset: 0x0008BCC0
		private int ExposedCountStopAt(int threshold)
		{
			if (this.cachedExposedThreshold > 0 && this.cachedExposedThreshold <= threshold)
			{
				return this.cachedExposedCount;
			}
			this.cachedExposedThreshold = threshold;
			int num = 0;
			for (int i = 0; i < this.districts.Count; i++)
			{
				num += this.districts[i].ExposedVacuumCount(threshold);
				if (num >= threshold)
				{
					return this.cachedExposedCount = num;
				}
				threshold -= num;
			}
			return this.cachedExposedCount = num;
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x0008DB38 File Offset: 0x0008BD38
		public int OpenRoofCount
		{
			get
			{
				if (this.cachedOpenRoofCount == -1)
				{
					this.cachedOpenRoofCount = this.OpenRoofCountStopAt(int.MaxValue);
				}
				return this.cachedOpenRoofCount;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060017A2 RID: 6050 RVA: 0x0008DB5A File Offset: 0x0008BD5A
		public IEnumerable<IntVec3> BorderCells
		{
			get
			{
				Room.<get_BorderCells>d__68 <get_BorderCells>d__ = new Room.<get_BorderCells>d__68(-2);
				<get_BorderCells>d__.<>4__this = this;
				return <get_BorderCells>d__;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060017A3 RID: 6051 RVA: 0x0008DB6A File Offset: 0x0008BD6A
		public IEnumerable<IntVec3> BorderCellsCardinal
		{
			get
			{
				Room.<get_BorderCellsCardinal>d__70 <get_BorderCellsCardinal>d__ = new Room.<get_BorderCellsCardinal>d__70(-2);
				<get_BorderCellsCardinal>d__.<>4__this = this;
				return <get_BorderCellsCardinal>d__;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060017A4 RID: 6052 RVA: 0x0008DB7C File Offset: 0x0008BD7C
		public IEnumerable<IntVec3> BorderCellsCached
		{
			get
			{
				if ((long)GenTicks.TicksGame - this.cachedBorderCellsTick > 120L)
				{
					this.cachedBorderCellsTick = (long)GenTicks.TicksGame;
					this.cachedBorderCells.Clear();
					this.cachedBorderCells.AddRange(this.BorderCells);
				}
				return this.cachedBorderCells;
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060017A5 RID: 6053 RVA: 0x0008DBCC File Offset: 0x0008BDCC
		public bool TouchesMapEdge
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].TouchesMapEdge)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060017A6 RID: 6054 RVA: 0x0008DC05 File Offset: 0x0008BE05
		public bool PsychologicallyOutdoors
		{
			get
			{
				return this.OpenRoofCountStopAt(300) >= 300 || (this.TouchesMapEdge && (float)this.OpenRoofCount / (float)this.CellCount >= 0.5f);
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060017A7 RID: 6055 RVA: 0x0008DC3C File Offset: 0x0008BE3C
		public bool OutdoorsForWork
		{
			get
			{
				return this.OpenRoofCountStopAt(101) > 100 || (float)this.OpenRoofCount > (float)this.CellCount * 0.25f;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x0008DC63 File Offset: 0x0008BE63
		public IEnumerable<Pawn> Owners
		{
			get
			{
				Room.<get_Owners>d__80 <get_Owners>d__ = new Room.<get_Owners>d__80(-2);
				<get_Owners>d__.<>4__this = this;
				return <get_Owners>d__;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x0008DC73 File Offset: 0x0008BE73
		public IEnumerable<Building_Bed> ContainedBeds
		{
			get
			{
				Room.<get_ContainedBeds>d__82 <get_ContainedBeds>d__ = new Room.<get_ContainedBeds>d__82(-2);
				<get_ContainedBeds>d__.<>4__this = this;
				return <get_ContainedBeds>d__;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x0008DC83 File Offset: 0x0008BE83
		public bool Fogged
		{
			get
			{
				return this.RegionCount != 0 && this.FirstRegion.AnyCell.Fogged(this.Map);
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x0008DCA5 File Offset: 0x0008BEA5
		public bool IsDoorway
		{
			get
			{
				return this.districts.Count == 1 && this.districts[0].IsDoorway;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060017AC RID: 6060 RVA: 0x0008DCC8 File Offset: 0x0008BEC8
		public Building_Door Door
		{
			get
			{
				if (!this.IsDoorway)
				{
					return null;
				}
				return this.districts[0].Regions[0].door;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x0008DCF0 File Offset: 0x0008BEF0
		public List<Thing> ContainedAndAdjacentThings
		{
			get
			{
				this.uniqueContainedThingsSet.Clear();
				this.uniqueContainedThings.Clear();
				List<Region> regions = this.Regions;
				for (int i = 0; i < regions.Count; i++)
				{
					List<Thing> allThings = regions[i].ListerThings.AllThings;
					if (allThings != null)
					{
						for (int j = 0; j < allThings.Count; j++)
						{
							Thing item = allThings[j];
							if (this.uniqueContainedThingsSet.Add(item))
							{
								this.uniqueContainedThings.Add(item);
							}
						}
					}
				}
				this.uniqueContainedThingsSet.Clear();
				return this.uniqueContainedThings;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x0008DD87 File Offset: 0x0008BF87
		public RoomRoleDef Role
		{
			get
			{
				if (this.statsAndRoleDirty)
				{
					this.UpdateRoomStatsAndRole();
				}
				return this.role;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x0008DDA0 File Offset: 0x0008BFA0
		public bool AnyPassable
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].Passable)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x0008DDDC File Offset: 0x0008BFDC
		public bool ProperRoom
		{
			get
			{
				if (this.TouchesMapEdge)
				{
					return false;
				}
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].RegionType == RegionType.Normal)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x0008DE20 File Offset: 0x0008C020
		public static Room MakeNew(Map map)
		{
			Room room = new Room();
			room.ID = Room.nextRoomID;
			room.tempTracker = new RoomTempTracker(room, map);
			map.regionAndRoomUpdater.roomLookup[room.ID] = room;
			Room.nextRoomID++;
			return room;
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x0008DE70 File Offset: 0x0008C070
		public void AddDistrict(District district)
		{
			if (this.districts.Contains(district))
			{
				Log.Error("Tried to add the same district twice to Room. district=" + ((district != null) ? district.ToString() : null) + ", room=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.districts.Add(district);
			if (this.districts.Count == 1)
			{
				this.Map.regionGrid.RoomAdded(this);
			}
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0008DEE8 File Offset: 0x0008C0E8
		public void RemoveDistrict(District district)
		{
			if (!this.districts.Contains(district))
			{
				Log.Error("Tried to remove district from Room but this district is not here. district=" + ((district != null) ? district.ToString() : null) + ", room=" + ((this != null) ? this.ToString() : null));
				return;
			}
			Map map = this.Map;
			this.districts.Remove(district);
			this.tmpRegions.Clear();
			if (this.districts.Count == 0 && map != null)
			{
				map.regionGrid.RoomRemoved(this);
			}
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0008DF74 File Offset: 0x0008C174
		public bool PushHeat(float energy)
		{
			if (this.UsesOutdoorTemperature)
			{
				return false;
			}
			this.Temperature += energy / (float)this.CellCount;
			return true;
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x0008DF98 File Offset: 0x0008C198
		public void Notify_ContainedThingSpawnedOrDespawned(Thing th)
		{
			if (th.def.category != ThingCategory.Mote && th.def.category != ThingCategory.Projectile && th.def.category != ThingCategory.Ethereal && th.def.category != ThingCategory.Pawn)
			{
				if (this.IsDoorway)
				{
					List<Region> regions = this.districts[0].Regions;
					for (int i = 0; i < regions[0].links.Count; i++)
					{
						Region otherRegion = regions[0].links[i].GetOtherRegion(regions[0]);
						if (otherRegion != null && !otherRegion.IsDoorway)
						{
							otherRegion.Room.Notify_ContainedThingSpawnedOrDespawned(th);
						}
					}
				}
				this.statsAndRoleDirty = true;
			}
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0008E05B File Offset: 0x0008C25B
		public void Notify_TerrainChanged()
		{
			this.cachedExposedCount = -1;
			this.cachedExposedThreshold = 0;
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0008E072 File Offset: 0x0008C272
		public void Notify_BedTypeChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0008E07B File Offset: 0x0008C27B
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedExposedCount = -1;
			this.cachedExposedThreshold = 0;
			this.tempTracker.RoofChanged();
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x0008E0A0 File Offset: 0x0008C2A0
		public void Notify_RoomShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.cachedExposedCount = -1;
			this.cachedExposedThreshold = 0;
			if (this.Dereferenced)
			{
				this.isPrisonCell = false;
				this.statsAndRoleDirty = true;
				return;
			}
			this.tempTracker.RoomChanged();
			if (Current.ProgramState == ProgramState.Playing && !this.Fogged && !GravshipPlacementUtility.placingGravship)
			{
				this.Map.autoBuildRoofAreaSetter.TryGenerateAreaFor(this);
			}
			this.isPrisonCell = false;
			if (Building_Bed.RoomCanBePrisonCell(this))
			{
				List<Thing> containedAndAdjacentThings = this.ContainedAndAdjacentThings;
				for (int i = 0; i < containedAndAdjacentThings.Count; i++)
				{
					Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
					if (building_Bed != null && building_Bed.ForPrisoners)
					{
						this.isPrisonCell = true;
						break;
					}
				}
			}
			List<Thing> list = this.Map.listerThings.ThingsOfDef(ThingDefOf.NutrientPasteDispenser);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Notify_ColorChanged();
			}
			if (Current.ProgramState == ProgramState.Playing && this.isPrisonCell)
			{
				foreach (Building_Bed building_Bed2 in this.ContainedBeds)
				{
					building_Bed2.ForPrisoners = true;
				}
			}
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0008E1F0 File Offset: 0x0008C3F0
		public bool ContainsCell(IntVec3 cell)
		{
			return this.Map != null && cell.GetRoom(this.Map) == this;
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x0008E20C File Offset: 0x0008C40C
		public bool ContainsThing(ThingDef def)
		{
			List<Region> regions = this.Regions;
			for (int i = 0; i < regions.Count; i++)
			{
				if (regions[i].ListerThings.ThingsOfDef(def).Any<Thing>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x0008E24D File Offset: 0x0008C44D
		public IEnumerable<Thing> ContainedThings(ThingDef def)
		{
			Room.<ContainedThings>d__111 <ContainedThings>d__ = new Room.<ContainedThings>d__111(-2);
			<ContainedThings>d__.<>4__this = this;
			<ContainedThings>d__.<>3__def = def;
			return <ContainedThings>d__;
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x0008E264 File Offset: 0x0008C464
		public IEnumerable<T> ContainedThings<T>() where T : Thing
		{
			Room.<ContainedThings>d__112<T> <ContainedThings>d__ = new Room.<ContainedThings>d__112<T>(-2);
			<ContainedThings>d__.<>4__this = this;
			return <ContainedThings>d__;
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0008E274 File Offset: 0x0008C474
		public IEnumerable<Thing> ContainedThingsList(IEnumerable<ThingDef> thingDefs)
		{
			Room.<ContainedThingsList>d__113 <ContainedThingsList>d__ = new Room.<ContainedThingsList>d__113(-2);
			<ContainedThingsList>d__.<>4__this = this;
			<ContainedThingsList>d__.<>3__thingDefs = thingDefs;
			return <ContainedThingsList>d__;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0008E28C File Offset: 0x0008C48C
		public int ThingCount(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			List<Region> regions = this.Regions;
			int num = 0;
			for (int i = 0; i < regions.Count; i++)
			{
				List<Thing> list = regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < list.Count; j++)
				{
					if (this.uniqueContainedThingsOfDef.Add(list[j]))
					{
						num += list[j].stackCount;
					}
				}
			}
			this.uniqueContainedThingsOfDef.Clear();
			return num;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0008E317 File Offset: 0x0008C517
		public float GetStat(RoomStatDef roomStat)
		{
			if (this.statsAndRoleDirty)
			{
				this.UpdateRoomStatsAndRole();
			}
			if (this.stats == null)
			{
				return roomStat.roomlessScore;
			}
			return this.stats[roomStat];
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x0008E344 File Offset: 0x0008C544
		public void DrawFieldEdges()
		{
			Room.fields.Clear();
			Room.fields.AddRange(this.Cells);
			Color color = this.isPrisonCell ? Room.PrisonFieldColor : Room.NonPrisonFieldColor;
			color.a = Pulser.PulseBrightness(1f, 0.6f);
			GenDraw.DrawFieldEdges(Room.fields, color, null, null, 2900);
			Room.fields.Clear();
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0008E3BC File Offset: 0x0008C5BC
		public RoomRoleDef GetRoomRoleIfBuildingPlaced(ThingDef buildingDef)
		{
			if (this.statsAndRoleDirty)
			{
				this.UpdateRoomStatsAndRole();
			}
			if (!this.ProperRoom || this.RegionCount > 60)
			{
				return RoomRoleDefOf.None;
			}
			return DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this) + x.Worker.GetScoreDeltaIfBuildingPlaced(this, buildingDef));
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0008E41C File Offset: 0x0008C61C
		private void UpdateRoomStatsAndRole()
		{
			this.statsAndRoleDirty = false;
			if (this.ProperRoom && this.RegionCount <= 60)
			{
				if (this.stats == null)
				{
					this.stats = new DefMap<RoomStatDef, float>();
				}
				foreach (RoomStatDef roomStatDef in from x in DefDatabase<RoomStatDef>.AllDefs
				orderby x.updatePriority descending
				select x)
				{
					this.stats[roomStatDef] = roomStatDef.Worker.GetScore(this);
				}
				this.role = DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this));
				return;
			}
			this.stats = null;
			this.role = RoomRoleDefOf.None;
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0008E500 File Offset: 0x0008C700
		public string GetRoomRoleLabel()
		{
			Pawn pawn = null;
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in this.Owners)
			{
				if (pawn == null)
				{
					pawn = pawn3;
				}
				else
				{
					pawn2 = pawn3;
				}
			}
			TaggedString taggedString;
			if (pawn == null)
			{
				taggedString = this.Role.PostProcessedLabel(this);
			}
			else if (pawn2 == null || pawn2 == pawn)
			{
				taggedString = "SomeonesRoom".Translate(pawn.LabelShort, this.Role.label, pawn.Named("PAWN"));
			}
			else
			{
				taggedString = "CouplesRoom".Translate(pawn.LabelShort, pawn2.LabelShort, this.Role.label, pawn.Named("PAWN1"), pawn2.Named("PAWN2"));
			}
			return taggedString;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0008E5F8 File Offset: 0x0008C7F8
		public string DebugString()
		{
			return string.Concat(new string[]
			{
				"Room ID=",
				this.ID.ToString(),
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				"\n  DistrictCount=",
				this.DistrictCount.ToString(),
				"\n  RegionCount=",
				this.RegionCount.ToString(),
				"\n  CellCount=",
				this.CellCount.ToString(),
				"\n  OpenRoofCount=",
				this.OpenRoofCount.ToString(),
				"\n  PsychologicallyOutdoors=",
				this.PsychologicallyOutdoors.ToString(),
				"\n  OutdoorsForWork=",
				this.OutdoorsForWork.ToString(),
				"\n  WellEnclosed=",
				this.ProperRoom.ToString(),
				"\n  ",
				this.tempTracker.DebugString(),
				DebugViewSettings.writeRoomRoles ? ("\n" + this.DebugRolesString()) : ""
			});
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x0008E740 File Offset: 0x0008C940
		private string DebugRolesString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ValueTuple<float, RoomRoleDef> valueTuple in from x in DefDatabase<RoomRoleDef>.AllDefs
			select new ValueTuple<float, RoomRoleDef>(x.Worker.GetScore(this), x) into tuple
			orderby tuple.Item1 descending
			select tuple)
			{
				float item = valueTuple.Item1;
				RoomRoleDef item2 = valueTuple.Item2;
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<float>(item);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<RoomRoleDef>(item2);
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x0008E808 File Offset: 0x0008CA08
		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)num * 0.01f);
			}
			this.tempTracker.DebugDraw();
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0008E878 File Offset: 0x0008CA78
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Room(roomID=",
				this.ID.ToString(),
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				")"
			});
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0008E8EB File Offset: 0x0008CAEB
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478891);
		}

		// Token: 0x040013F4 RID: 5108
		public int ID = -1;

		// Token: 0x040013F5 RID: 5109
		private List<District> districts = new List<District>();

		// Token: 0x040013F6 RID: 5110
		private RoomTempTracker tempTracker;

		// Token: 0x040013F7 RID: 5111
		private int cachedOpenRoofCount = -1;

		// Token: 0x040013F8 RID: 5112
		private int cachedExposedCount = -1;

		// Token: 0x040013F9 RID: 5113
		private int cachedExposedThreshold;

		// Token: 0x040013FA RID: 5114
		private int cachedCellCount = -1;

		// Token: 0x040013FB RID: 5115
		private bool isPrisonCell;

		// Token: 0x040013FC RID: 5116
		private bool statsAndRoleDirty = true;

		// Token: 0x040013FD RID: 5117
		private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

		// Token: 0x040013FE RID: 5118
		private RoomRoleDef role;

		// Token: 0x040013FF RID: 5119
		private List<IntVec3> cachedBorderCells = new List<IntVec3>();

		// Token: 0x04001400 RID: 5120
		private long cachedBorderCellsTick = -1L;

		// Token: 0x04001401 RID: 5121
		private float vacuum;

		// Token: 0x04001402 RID: 5122
		public bool vacuumOverlayDrawn;

		// Token: 0x04001403 RID: 5123
		public int vacuumOverlayRects;

		// Token: 0x04001404 RID: 5124
		private static int nextRoomID;

		// Token: 0x04001405 RID: 5125
		private const int RegionCountHuge = 60;

		// Token: 0x04001406 RID: 5126
		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;

		// Token: 0x04001407 RID: 5127
		public const float MinVaccum = -0.1f;

		// Token: 0x04001408 RID: 5128
		private const int MaxRegionsToAssignRoomRole = 60;

		// Token: 0x04001409 RID: 5129
		private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

		// Token: 0x0400140A RID: 5130
		private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

		// Token: 0x0400140B RID: 5131
		public const string VacuumChangedSignal = "Vacuum";

		// Token: 0x0400140C RID: 5132
		private List<Region> tmpRegions = new List<Region>();

		// Token: 0x0400140D RID: 5133
		private readonly HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

		// Token: 0x0400140E RID: 5134
		private readonly List<Thing> uniqueContainedThings = new List<Thing>();

		// Token: 0x0400140F RID: 5135
		private readonly HashSet<Thing> uniqueContainedThingsOfDef = new HashSet<Thing>();

		// Token: 0x04001410 RID: 5136
		private static List<IntVec3> fields = new List<IntVec3>();
	}
}
