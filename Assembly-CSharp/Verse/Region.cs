using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002F5 RID: 757
	public sealed class Region
	{
		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x060016F8 RID: 5880 RVA: 0x00089F68 File Offset: 0x00088168
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

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x00089F85 File Offset: 0x00088185
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				Region.<get_Cells>d__31 <get_Cells>d__ = new Region.<get_Cells>d__31(-2);
				<get_Cells>d__.<>4__this = this;
				return <get_Cells>d__;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x00089F98 File Offset: 0x00088198
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					RegionGrid regionGrid = this.Map.regionGrid;
					for (int i = this.extentsClose.minZ; i <= this.extentsClose.maxZ; i++)
					{
						for (int j = this.extentsClose.minX; j <= this.extentsClose.maxX; j++)
						{
							IntVec3 c = new IntVec3(j, 0, i);
							if (regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c) == this)
							{
								this.cachedCellCount++;
							}
						}
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x0008A029 File Offset: 0x00088229
		public IEnumerable<Region> Neighbors
		{
			get
			{
				Region.<get_Neighbors>d__35 <get_Neighbors>d__ = new Region.<get_Neighbors>d__35(-2);
				<get_Neighbors>d__.<>4__this = this;
				return <get_Neighbors>d__;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x060016FC RID: 5884 RVA: 0x0008A039 File Offset: 0x00088239
		public IEnumerable<Region> NeighborsOfSameType
		{
			get
			{
				Region.<get_NeighborsOfSameType>d__37 <get_NeighborsOfSameType>d__ = new Region.<get_NeighborsOfSameType>d__37(-2);
				<get_NeighborsOfSameType>d__.<>4__this = this;
				return <get_NeighborsOfSameType>d__;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x0008A049 File Offset: 0x00088249
		public Room Room
		{
			get
			{
				District district = this.District;
				if (district == null)
				{
					return null;
				}
				return district.Room;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060016FE RID: 5886 RVA: 0x0008A05C File Offset: 0x0008825C
		// (set) Token: 0x060016FF RID: 5887 RVA: 0x0008A064 File Offset: 0x00088264
		public District District
		{
			get
			{
				return this.districtInt;
			}
			set
			{
				if (value == this.districtInt)
				{
					return;
				}
				District district = this.districtInt;
				if (district != null)
				{
					district.RemoveRegion(this);
				}
				this.districtInt = value;
				District district2 = this.districtInt;
				if (district2 == null)
				{
					return;
				}
				district2.AddRegion(this);
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001700 RID: 5888 RVA: 0x0008A09C File Offset: 0x0008829C
		public IntVec3 RandomCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				for (int i = 0; i < 1000; i++)
				{
					IntVec3 randomCell = this.extentsClose.RandomCell;
					if (directGrid[cellIndices.CellToIndex(randomCell)] == this)
					{
						return randomCell;
					}
				}
				return this.AnyCell;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001701 RID: 5889 RVA: 0x0008A0F4 File Offset: 0x000882F4
		public IntVec3 AnyCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				foreach (IntVec3 intVec in this.extentsClose)
				{
					if (directGrid[cellIndices.CellToIndex(intVec)] == this)
					{
						return intVec;
					}
				}
				Log.Error("Couldn't find any cell in region " + this.ToString());
				return this.extentsClose.RandomCell;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x0008A190 File Offset: 0x00088390
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("id: " + this.id.ToString());
				stringBuilder.AppendLine("mapIndex: " + this.mapIndex.ToString());
				stringBuilder.AppendLine("links count: " + this.links.Count.ToString());
				stringBuilder.AppendLine("type: " + this.type.ToString());
				stringBuilder.AppendLine("touchesMapEdge: " + this.touchesMapEdge.ToString());
				foreach (RegionLink regionLink in this.links)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					string str = "  --";
					RegionLink regionLink2 = regionLink;
					stringBuilder2.AppendLine(str + ((regionLink2 != null) ? regionLink2.ToString() : null));
				}
				stringBuilder.AppendLine("valid: " + this.valid.ToString());
				stringBuilder.AppendLine("makeTick: " + this.debug_makeTick.ToString());
				stringBuilder.AppendLine("districtID: " + ((this.District != null) ? this.District.ID.ToString() : "null district!"));
				stringBuilder.AppendLine("roomID: " + ((this.Room != null) ? this.Room.ID.ToString() : "null room!"));
				StringBuilder stringBuilder3 = stringBuilder;
				string str2 = "extentsClose: ";
				CellRect cellRect = this.extentsClose;
				stringBuilder3.AppendLine(str2 + cellRect.ToString());
				StringBuilder stringBuilder4 = stringBuilder;
				string str3 = "extentsLimit: ";
				cellRect = this.extentsLimit;
				stringBuilder4.AppendLine(str3 + cellRect.ToString());
				stringBuilder.AppendLine("ListerThings:");
				if (this.listerThings.AllThings != null)
				{
					for (int i = 0; i < this.listerThings.AllThings.Count; i++)
					{
						StringBuilder stringBuilder5 = stringBuilder;
						string str4 = "  --";
						Thing thing = this.listerThings.AllThings[i];
						stringBuilder5.AppendLine(str4 + ((thing != null) ? thing.ToString() : null));
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x0008A3EC File Offset: 0x000885EC
		public bool DebugIsNew
		{
			get
			{
				return this.debug_makeTick > Find.TickManager.TicksGame - 60;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001704 RID: 5892 RVA: 0x0008A403 File Offset: 0x00088603
		public ListerThings ListerThings
		{
			get
			{
				return this.listerThings;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x0008A40B File Offset: 0x0008860B
		public bool IsDoorway
		{
			get
			{
				return this.door != null;
			}
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x0008A418 File Offset: 0x00088618
		private Region()
		{
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x0008A4AC File Offset: 0x000886AC
		public static Region MakeNewUnfilled(IntVec3 root, Map map)
		{
			Region region = new Region();
			region.debug_makeTick = Find.TickManager.TicksGame;
			region.id = Region.nextId;
			Region.nextId++;
			region.mapIndex = (sbyte)map.Index;
			region.precalculatedHashCode = Gen.HashCombineInt(region.id, 1295813358);
			region.extentsClose.minX = root.x;
			region.extentsClose.maxX = root.x;
			region.extentsClose.minZ = root.z;
			region.extentsClose.maxZ = root.z;
			region.extentsLimit.minX = root.x - root.x % 12;
			region.extentsLimit.maxX = root.x + 12 - (root.x + 12) % 12 - 1;
			region.extentsLimit.minZ = root.z - root.z % 12;
			region.extentsLimit.maxZ = root.z + 12 - (root.z + 12) % 12 - 1;
			region.extentsLimit.ClipInsideMap(map);
			return region;
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x0008A5D8 File Offset: 0x000887D8
		public bool Allows(TraverseParms tp, bool isDestination)
		{
			if (tp.mode != TraverseMode.PassAllDestroyableThings && tp.mode != TraverseMode.PassAllDestroyablePlayerOwnedThings && tp.mode != TraverseMode.PassAllDestroyableThingsNotWater && !this.type.Passable())
			{
				return false;
			}
			if (tp.maxDanger < Danger.Deadly && tp.pawn != null)
			{
				Danger danger = this.DangerFor(tp.pawn);
				if (isDestination || danger == Danger.Deadly)
				{
					Region region = tp.pawn.GetRegion(RegionType.Set_All);
					if ((region == null || danger > region.DangerFor(tp.pawn)) && danger > tp.maxDanger)
					{
						return false;
					}
				}
			}
			bool flag = this.type == RegionType.Fence && tp.fenceBlocked && !tp.canBashFences;
			switch (tp.mode)
			{
			case TraverseMode.ByPawn:
			{
				if (this.door == null)
				{
					return !flag;
				}
				if (tp.pawn == null)
				{
					return this.door.FreePassage || this.door.Open || tp.canBashDoors;
				}
				AvoidGrid avoidGrid;
				if (tp.pawn.TryGetAvoidGrid(out avoidGrid, true))
				{
					foreach (IntVec3 intVec in this.door.OccupiedRect())
					{
						if (avoidGrid.Grid[this.Map.cellIndices.CellToIndex(intVec)] == 255 && intVec.GetRegion(this.Map, RegionType.Set_Passable) == this)
						{
							return false;
						}
					}
				}
				if (tp.pawn.HostileTo(this.door))
				{
					return this.door.CanPhysicallyPass(tp.pawn) || tp.canBashDoors;
				}
				return this.door.CanPhysicallyPass(tp.pawn) && !this.door.IsForbiddenToPass(tp.pawn);
			}
			case TraverseMode.PassDoors:
				return !flag;
			case TraverseMode.NoPassClosedDoors:
			case TraverseMode.NoPassClosedDoorsOrWater:
				return (this.door == null || this.door.FreePassage) && !flag;
			case TraverseMode.PassAllDestroyableThings:
				return true;
			case TraverseMode.PassAllDestroyablePlayerOwnedThings:
				return this.door == null || this.door.Faction != Faction.OfPlayer;
			case TraverseMode.PassAllDestroyableThingsNotWater:
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x0008A830 File Offset: 0x00088A30
		public IEnumerable<CellRect> EnumerateRectangleCover()
		{
			Region.<EnumerateRectangleCover>d__58 <EnumerateRectangleCover>d__ = new Region.<EnumerateRectangleCover>d__58(-2);
			<EnumerateRectangleCover>d__.<>4__this = this;
			return <EnumerateRectangleCover>d__;
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x0008A840 File Offset: 0x00088A40
		public Danger DangerFor(Pawn p)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Danger result;
				if (this.cachedDangersForTick != GenTicks.TicksGame)
				{
					this.cachedDangers.Clear();
					this.cachedDangersForTick = GenTicks.TicksGame;
				}
				else if (this.cachedDangers.TryGetValue(p, out result))
				{
					return result;
				}
			}
			Room room = this.Room;
			float temperature = room.Temperature;
			FloatRange value;
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Region.cachedSafeTemperatureRangesForTick != GenTicks.TicksGame)
				{
					Region.cachedSafeTemperatureRanges.Clear();
					Region.cachedSafeTemperatureRangesForTick = GenTicks.TicksGame;
				}
				if (!Region.cachedSafeTemperatureRanges.TryGetValue(p, out value))
				{
					value = p.SafeTemperatureRange();
					Region.cachedSafeTemperatureRanges.Add(p, value);
				}
			}
			else
			{
				value = p.SafeTemperatureRange();
			}
			Danger danger;
			if (value.Includes(temperature))
			{
				danger = Danger.None;
			}
			else
			{
				danger = (value.ExpandedBy(80f).Includes(temperature) ? Danger.Some : Danger.Deadly);
			}
			if (room.Vacuum > 0.5f && p.ConcernedByVacuum)
			{
				danger = Danger.Deadly;
			}
			this.cachedDangers[p] = danger;
			return danger;
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0008A93C File Offset: 0x00088B3C
		public float GetBaseDesiredPlantsCount(bool allowCache = true)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (allowCache && ticksGame - this.cachedBaseDesiredPlantsCountForTick < 2500)
			{
				return this.cachedBaseDesiredPlantsCount;
			}
			this.cachedBaseDesiredPlantsCount = 0f;
			Map map = this.Map;
			foreach (IntVec3 c in this.Cells)
			{
				this.cachedBaseDesiredPlantsCount += map.wildPlantSpawner.GetBaseDesiredPlantsCountAt(c);
			}
			this.cachedBaseDesiredPlantsCountForTick = ticksGame;
			return this.cachedBaseDesiredPlantsCount;
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x0008A9E0 File Offset: 0x00088BE0
		public AreaOverlap OverlapWith(Area a)
		{
			if (a.TrueCount == 0)
			{
				return AreaOverlap.None;
			}
			if (this.Map != a.Map)
			{
				return AreaOverlap.None;
			}
			if (this.cachedAreaOverlaps == null)
			{
				this.cachedAreaOverlaps = new Dictionary<Area, AreaOverlap>();
			}
			AreaOverlap areaOverlap;
			if (!this.cachedAreaOverlaps.TryGetValue(a, out areaOverlap))
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.Cells)
				{
					num2++;
					if (a[c])
					{
						num++;
					}
				}
				if (num == 0)
				{
					areaOverlap = AreaOverlap.None;
				}
				else if (num == num2)
				{
					areaOverlap = AreaOverlap.Entire;
				}
				else
				{
					areaOverlap = AreaOverlap.Partial;
				}
				this.cachedAreaOverlaps.Add(a, areaOverlap);
			}
			return areaOverlap;
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x0008AA9C File Offset: 0x00088C9C
		public void Notify_AreaChanged(Area a)
		{
			if (this.cachedAreaOverlaps == null)
			{
				return;
			}
			if (this.cachedAreaOverlaps.ContainsKey(a))
			{
				this.cachedAreaOverlaps.Remove(a);
			}
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0008AAC4 File Offset: 0x00088CC4
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning("Tried to decrement map index for region " + this.id.ToString() + ", but mapIndex=" + this.mapIndex.ToString());
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x0008AB14 File Offset: 0x00088D14
		public void Notify_MyMapRemoved()
		{
			this.listerThings.Clear();
			this.mapIndex = -1;
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x0008AB28 File Offset: 0x00088D28
		public static void ClearStaticData()
		{
			Region.cachedSafeTemperatureRanges.Clear();
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x0008AB34 File Offset: 0x00088D34
		public override string ToString()
		{
			string str;
			if (this.door != null)
			{
				str = this.door.ToString();
			}
			else
			{
				str = "null";
			}
			return string.Concat(new string[]
			{
				"Region(id=",
				this.id.ToString(),
				", mapIndex=",
				this.mapIndex.ToString(),
				", center=",
				this.extentsClose.CenterCell.ToString(),
				", links=",
				this.links.Count.ToString(),
				", cells=",
				this.CellCount.ToString(),
				", touchesMapEdge=",
				this.touchesMapEdge.ToString(),
				(this.door != null) ? (", portal=" + str) : null,
				")"
			});
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x0008AC2C File Offset: 0x00088E2C
		public void DebugDraw()
		{
			if (DebugViewSettings.drawRegionTraversal && Find.TickManager.TicksGame < this.debug_lastTraverseTick + 60)
			{
				float a = 1f - (float)(Find.TickManager.TicksGame - this.debug_lastTraverseTick) / 60f;
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), new Color(0f, 0f, 1f, a), null, null, 2900);
			}
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x0008ACA8 File Offset: 0x00088EA8
		public void DebugDrawMouseover()
		{
			int num = Mathf.RoundToInt(Time.realtimeSinceStartup * 2f) % 2;
			if (DebugViewSettings.drawRegions)
			{
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), this.DebugColor(), null, null, 2900);
				foreach (Region region in this.Neighbors)
				{
					GenDraw.DrawFieldEdges(region.Cells.ToList<IntVec3>(), Color.grey, null, null, 2900);
				}
			}
			if (DebugViewSettings.drawRegionLinks)
			{
				foreach (RegionLink regionLink in this.links)
				{
					if (num == 1)
					{
						List<IntVec3> list = regionLink.span.Cells.ToList<IntVec3>();
						Material mat = DebugSolidColorMats.MaterialOf(Color.magenta * new Color(1f, 1f, 1f, 0.25f));
						foreach (IntVec3 c in list)
						{
							CellRenderer.RenderCell(c, mat);
						}
						GenDraw.DrawFieldEdges(list, Color.white, null, null, 2900);
					}
				}
			}
			if (DebugViewSettings.drawRegionThings)
			{
				foreach (Thing thing in this.listerThings.AllThings)
				{
					CellRenderer.RenderSpot(thing.TrueCenter(), (float)(thing.thingIDNumber % 256) / 256f, 0.15f);
				}
			}
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x0008AEA8 File Offset: 0x000890A8
		private Color DebugColor()
		{
			Color result;
			if (!this.valid)
			{
				result = Color.red;
			}
			else if (this.DebugIsNew)
			{
				result = Color.yellow;
			}
			else
			{
				result = Color.green;
			}
			return result;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0008AEDC File Offset: 0x000890DC
		public void Debug_Notify_Traversed()
		{
			this.debug_lastTraverseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x0008AEEE File Offset: 0x000890EE
		public override int GetHashCode()
		{
			return this.precalculatedHashCode;
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x0008AEF8 File Offset: 0x000890F8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Region region = obj as Region;
			return region != null && region.id == this.id;
		}

		// Token: 0x0400139E RID: 5022
		public RegionType type = RegionType.Normal;

		// Token: 0x0400139F RID: 5023
		public int id = -1;

		// Token: 0x040013A0 RID: 5024
		public sbyte mapIndex = -1;

		// Token: 0x040013A1 RID: 5025
		private District districtInt;

		// Token: 0x040013A2 RID: 5026
		public List<RegionLink> links = new List<RegionLink>();

		// Token: 0x040013A3 RID: 5027
		public CellRect extentsClose;

		// Token: 0x040013A4 RID: 5028
		public CellRect extentsLimit;

		// Token: 0x040013A5 RID: 5029
		public Building_Door door;

		// Token: 0x040013A6 RID: 5030
		private int precalculatedHashCode;

		// Token: 0x040013A7 RID: 5031
		public bool touchesMapEdge;

		// Token: 0x040013A8 RID: 5032
		private int cachedCellCount = -1;

		// Token: 0x040013A9 RID: 5033
		public bool valid = true;

		// Token: 0x040013AA RID: 5034
		private readonly ListerThings listerThings = new ListerThings(ListerThingsUse.Region, null);

		// Token: 0x040013AB RID: 5035
		public readonly uint[] closedIndex = new uint[RegionTraverser.NumWorkers];

		// Token: 0x040013AC RID: 5036
		public uint reachedIndex;

		// Token: 0x040013AD RID: 5037
		public int newRegionGroupIndex = -1;

		// Token: 0x040013AE RID: 5038
		private Dictionary<Area, AreaOverlap> cachedAreaOverlaps;

		// Token: 0x040013AF RID: 5039
		public int mark;

		// Token: 0x040013B0 RID: 5040
		private readonly Dictionary<Pawn, Danger> cachedDangers = new Dictionary<Pawn, Danger>();

		// Token: 0x040013B1 RID: 5041
		private int cachedDangersForTick;

		// Token: 0x040013B2 RID: 5042
		private float cachedBaseDesiredPlantsCount;

		// Token: 0x040013B3 RID: 5043
		private int cachedBaseDesiredPlantsCountForTick = -999999;

		// Token: 0x040013B4 RID: 5044
		private static readonly Dictionary<Pawn, FloatRange> cachedSafeTemperatureRanges = new Dictionary<Pawn, FloatRange>();

		// Token: 0x040013B5 RID: 5045
		private static int cachedSafeTemperatureRangesForTick;

		// Token: 0x040013B6 RID: 5046
		private int debug_makeTick = -1000;

		// Token: 0x040013B7 RID: 5047
		private int debug_lastTraverseTick = -1000;

		// Token: 0x040013B8 RID: 5048
		private static int nextId = 1;

		// Token: 0x040013B9 RID: 5049
		public const int GridSize = 12;
	}
}
