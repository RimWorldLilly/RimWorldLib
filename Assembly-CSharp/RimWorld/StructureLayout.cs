using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DelaunatorSharp;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001279 RID: 4729
	public class StructureLayout : IExposable
	{
		// Token: 0x170013DA RID: 5082
		// (get) Token: 0x06007508 RID: 29960 RVA: 0x00291536 File Offset: 0x0028F736
		public List<LayoutRoom> Rooms
		{
			get
			{
				return this.rooms;
			}
		}

		// Token: 0x170013DB RID: 5083
		// (get) Token: 0x06007509 RID: 29961 RVA: 0x0029153E File Offset: 0x0028F73E
		public int Width
		{
			get
			{
				return this.roomIds.GetLength(0);
			}
		}

		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x0600750A RID: 29962 RVA: 0x0029154C File Offset: 0x0028F74C
		public int Height
		{
			get
			{
				return this.roomIds.GetLength(1);
			}
		}

		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x0600750B RID: 29963 RVA: 0x0029155C File Offset: 0x0028F75C
		public int Area
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.rooms.Count; i++)
				{
					num += this.rooms[i].Area;
				}
				return num;
			}
		}

		// Token: 0x0600750C RID: 29964 RVA: 0x00291596 File Offset: 0x0028F796
		public StructureLayout(LayoutStructureSketch sketch)
		{
			this.sketch = sketch;
		}

		// Token: 0x0600750D RID: 29965 RVA: 0x002915B0 File Offset: 0x0028F7B0
		public StructureLayout(LayoutStructureSketch sketch, CellRect rect)
		{
			this.sketch = sketch;
			this.container = rect;
			this.cellTypes = new RoomLayoutCellType[rect.Width, rect.Height];
			this.roomIds = new int[rect.Width, rect.Height];
			for (int i = 0; i < rect.Width; i++)
			{
				for (int j = 0; j < rect.Height; j++)
				{
					this.roomIds[i, j] = -1;
				}
			}
		}

		// Token: 0x0600750E RID: 29966 RVA: 0x00291640 File Offset: 0x0028F840
		public bool HasRoomWithDef(LayoutRoomDef def)
		{
			return this.GetFirstRoomOfDef(def) != null;
		}

		// Token: 0x0600750F RID: 29967 RVA: 0x0029164C File Offset: 0x0028F84C
		public bool TryGetFirstRoomOfDef(LayoutRoomDef def, out LayoutRoom room)
		{
			room = this.GetFirstRoomOfDef(def);
			return room != null;
		}

		// Token: 0x06007510 RID: 29968 RVA: 0x0029165C File Offset: 0x0028F85C
		public LayoutRoom GetFirstRoomOfDef(LayoutRoomDef def)
		{
			foreach (LayoutRoom layoutRoom in this.rooms)
			{
				if (layoutRoom.defs != null && layoutRoom.defs.Contains(def))
				{
					return layoutRoom;
				}
			}
			return null;
		}

		// Token: 0x06007511 RID: 29969 RVA: 0x002916C8 File Offset: 0x0028F8C8
		public LayoutRoom AddRoom(List<CellRect> rects, LayoutRoomDef requiredDef = null)
		{
			for (int i = 0; i < rects.Count; i++)
			{
				rects[i] = rects[i].ClipInsideRect(this.container);
			}
			LayoutRoom layoutRoom = new LayoutRoom(this.sketch, rects)
			{
				requiredDef = requiredDef
			};
			this.rooms.Add(layoutRoom);
			return layoutRoom;
		}

		// Token: 0x06007512 RID: 29970 RVA: 0x00291723 File Offset: 0x0028F923
		public void RemoveRoom(LayoutRoom room)
		{
			this.rooms.Remove(room);
		}

		// Token: 0x06007513 RID: 29971 RVA: 0x00291732 File Offset: 0x0028F932
		public void Add(IntVec3 position, RoomLayoutCellType cellType)
		{
			if (this.cellTypes.InBounds(position.x, position.z))
			{
				this.cellTypes[position.x, position.z] = cellType;
			}
		}

		// Token: 0x06007514 RID: 29972 RVA: 0x00291768 File Offset: 0x0028F968
		public bool IsGoodForHorizontalDoor(IntVec3 p)
		{
			return this.IsWallAt(p + IntVec3.West) && this.IsWallAt(p + IntVec3.East) && !this.IsWallAt(p + IntVec3.North) && !this.IsWallAt(p + IntVec3.South);
		}

		// Token: 0x06007515 RID: 29973 RVA: 0x002917C4 File Offset: 0x0028F9C4
		public bool IsGoodForVerticalDoor(IntVec3 p)
		{
			return this.IsWallAt(p + IntVec3.North) && this.IsWallAt(p + IntVec3.South) && !this.IsWallAt(p + IntVec3.East) && !this.IsWallAt(p + IntVec3.West);
		}

		// Token: 0x06007516 RID: 29974 RVA: 0x00291820 File Offset: 0x0028FA20
		public bool IsGoodForDoor(IntVec3 p)
		{
			return this.IsGoodForHorizontalDoor(p) || this.IsGoodForVerticalDoor(p);
		}

		// Token: 0x06007517 RID: 29975 RVA: 0x00291834 File Offset: 0x0028FA34
		public bool IsWallAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Wall;
		}

		// Token: 0x06007518 RID: 29976 RVA: 0x0029186B File Offset: 0x0028FA6B
		public bool IsFloorAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Floor;
		}

		// Token: 0x06007519 RID: 29977 RVA: 0x002918A2 File Offset: 0x0028FAA2
		public bool IsDoorAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Door;
		}

		// Token: 0x0600751A RID: 29978 RVA: 0x002918D9 File Offset: 0x0028FAD9
		public bool IsEmptyAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Empty;
		}

		// Token: 0x0600751B RID: 29979 RVA: 0x00291910 File Offset: 0x0028FB10
		public bool IsOutside(IntVec3 position)
		{
			return !this.cellTypes.InBounds(position.x, position.z) || this.cellTypes[position.x, position.z] == RoomLayoutCellType.Empty;
		}

		// Token: 0x0600751C RID: 29980 RVA: 0x00291947 File Offset: 0x0028FB47
		public int GetRoomIdAt(IntVec3 position)
		{
			if (!this.roomIds.InBounds(position.x, position.z))
			{
				return -2;
			}
			return this.roomIds[position.x, position.z];
		}

		// Token: 0x0600751D RID: 29981 RVA: 0x0029197C File Offset: 0x0028FB7C
		public bool CanRemoveWithoutDisconnection(LayoutRoom room)
		{
			return !this.WouldDisconnectRoomsIfRemoved(room);
		}

		// Token: 0x0600751E RID: 29982 RVA: 0x0029198C File Offset: 0x0028FB8C
		public bool TryMinimizeLayoutWithoutDisconnection()
		{
			if (this.rooms.Count == 1)
			{
				return false;
			}
			for (int i = this.rooms.Count - 1; i >= 0; i--)
			{
				if (this.rooms[i].requiredDef == null && !this.WouldDisconnectRoomsIfRemoved(this.rooms[i]))
				{
					this.rooms.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600751F RID: 29983 RVA: 0x002919F8 File Offset: 0x0028FBF8
		private bool WouldDisconnectRoomsIfRemoved(LayoutRoom room)
		{
			StructureLayout.tmpRooms.Clear();
			StructureLayout.tmpRooms.AddRange(this.rooms);
			StructureLayout.tmpRooms.Remove(room);
			StructureLayout.tmpSeenRooms.Clear();
			StructureLayout.tmpRoomQueue.Clear();
			StructureLayout.tmpRoomQueue.Enqueue(StructureLayout.tmpRooms.First<LayoutRoom>());
			while (StructureLayout.tmpRoomQueue.Count > 0)
			{
				LayoutRoom layoutRoom = StructureLayout.tmpRoomQueue.Dequeue();
				StructureLayout.tmpSeenRooms.Add(layoutRoom);
				foreach (LayoutRoom layoutRoom2 in StructureLayout.tmpRooms)
				{
					if (layoutRoom != layoutRoom2 && !StructureLayout.tmpSeenRooms.Contains(layoutRoom2) && layoutRoom.IsAdjacentTo(layoutRoom2, 3))
					{
						StructureLayout.tmpRoomQueue.Enqueue(layoutRoom2);
					}
				}
			}
			int count = StructureLayout.tmpRooms.Count;
			int count2 = StructureLayout.tmpSeenRooms.Count;
			StructureLayout.tmpRooms.Clear();
			StructureLayout.tmpSeenRooms.Clear();
			return count2 != count;
		}

		// Token: 0x06007520 RID: 29984 RVA: 0x00291B0C File Offset: 0x0028FD0C
		public bool IsAdjacentToLayoutEdge(LayoutRoom room)
		{
			for (int i = 0; i < room.rects.Count; i++)
			{
				if (room.rects[i].minX == this.container.minX || room.rects[i].maxX == this.container.maxX || room.rects[i].minZ == this.container.minZ || room.rects[i].maxZ == this.container.maxZ)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007521 RID: 29985 RVA: 0x00291BB0 File Offset: 0x0028FDB0
		public void FinalizeRooms(bool avoidDoubleWalls = true)
		{
			for (int i = 0; i < 4; i++)
			{
				Rot4 dir = new Rot4(i);
				foreach (LayoutRoom layoutRoom in this.rooms)
				{
					for (int j = 0; j < layoutRoom.rects.Count; j++)
					{
						foreach (IntVec3 intVec in layoutRoom.rects[j].GetEdgeCells(dir))
						{
							IntVec3 facingCell = dir.FacingCell + intVec;
							if (!avoidDoubleWalls || (!this.IsWallAt(facingCell) && !layoutRoom.rects.Any((CellRect r) => r.Contains(facingCell))))
							{
								bool flag = false;
								foreach (CellRect lhs in layoutRoom.rects)
								{
									if (!(lhs == layoutRoom.rects[j]) && lhs.Contains(intVec))
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									this.Add(intVec, RoomLayoutCellType.Wall);
								}
							}
						}
					}
				}
			}
			for (int k = 0; k < this.rooms.Count; k++)
			{
				LayoutRoom layoutRoom2 = this.rooms[k];
				foreach (CellRect cellRect in layoutRoom2.rects)
				{
					foreach (IntVec3 intVec2 in cellRect.Cells)
					{
						this.roomIds[intVec2.x, intVec2.z] = k;
						layoutRoom2.id = k;
						if (!this.IsWallAt(intVec2))
						{
							this.Add(intVec2, RoomLayoutCellType.Floor);
						}
					}
				}
			}
			for (int l = this.container.minX; l < this.container.maxX; l++)
			{
				for (int m = this.container.minZ; m < this.container.maxZ; m++)
				{
					IntVec3 intVec3 = new IntVec3(l, 0, m);
					if (!this.IsWallAt(intVec3) && this.IsFloorAt(intVec3))
					{
						int num = 0;
						foreach (IntVec3 b in GenAdj.CardinalDirections)
						{
							if (this.IsWallAt(intVec3 + b))
							{
								num++;
							}
						}
						int num2 = 0;
						int num3 = 0;
						foreach (IntVec3 b2 in GenAdj.DiagonalDirections)
						{
							if (this.IsWallAt(intVec3 + b2))
							{
								num2++;
							}
							else if (!this.IsFloorAt(intVec3 + b2))
							{
								num3++;
							}
						}
						if (num > 1 && (num2 < 2 || num3 > 0))
						{
							this.Add(intVec3, RoomLayoutCellType.Wall);
						}
					}
				}
			}
		}

		// Token: 0x06007522 RID: 29986 RVA: 0x00291F94 File Offset: 0x00290194
		public bool CellWithinRangeOfUndamageableRoom(IntVec3 cell, float range)
		{
			foreach (LayoutRoom layoutRoom in this.Rooms)
			{
				if (layoutRoom.DontDestroyWallsDoors)
				{
					foreach (CellRect cellRect in layoutRoom.rects)
					{
						if (cellRect.ClosestDistanceTo(cell) <= range)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007523 RID: 29987 RVA: 0x00292038 File Offset: 0x00290238
		public int GetLogicalRoomConnectionCount(LayoutRoom room)
		{
			if (this.neighbours == null || this.neighbours.connections.NullOrEmpty<Vector2, List<Vector2>>())
			{
				return 0;
			}
			int num = 0;
			foreach (KeyValuePair<Vector2, List<Vector2>> keyValuePair in this.neighbours.connections)
			{
				foreach (CellRect cellRect in room.rects)
				{
					if (cellRect.Contains(new IntVec3(Mathf.RoundToInt(keyValuePair.Key.x), 0, Mathf.RoundToInt(keyValuePair.Key.y))))
					{
						num += keyValuePair.Value.Count;
					}
				}
			}
			return num;
		}

		// Token: 0x06007524 RID: 29988 RVA: 0x00292128 File Offset: 0x00290328
		[return: TupleElementNames(new string[]
		{
			null,
			"a",
			"b"
		})]
		public IEnumerable<ValueTuple<LayoutRoom, CellRect, CellRect>> GetLogicalRoomConnections(LayoutRoom room)
		{
			StructureLayout.<GetLogicalRoomConnections>d__43 <GetLogicalRoomConnections>d__ = new StructureLayout.<GetLogicalRoomConnections>d__43(-2);
			<GetLogicalRoomConnections>d__.<>4__this = this;
			<GetLogicalRoomConnections>d__.<>3__room = room;
			return <GetLogicalRoomConnections>d__;
		}

		// Token: 0x06007525 RID: 29989 RVA: 0x00292140 File Offset: 0x00290340
		public void RemoveLogicalConnectionsBetweenRooms(LayoutRoom a, LayoutRoom b)
		{
			if (this.neighbours == null || this.neighbours.connections.NullOrEmpty<Vector2, List<Vector2>>())
			{
				return;
			}
			foreach (KeyValuePair<Vector2, List<Vector2>> keyValuePair in this.neighbours.connections)
			{
				Vector2 vector;
				List<Vector2> list;
				keyValuePair.Deconstruct(out vector, out list);
				Vector2 point = vector;
				List<Vector2> connections = list;
				StructureLayout.RemoveLinksBetween(a, b, point, connections);
				StructureLayout.RemoveLinksBetween(b, a, point, connections);
			}
		}

		// Token: 0x06007526 RID: 29990 RVA: 0x002921D0 File Offset: 0x002903D0
		private static void RemoveLinksBetween(LayoutRoom a, LayoutRoom b, Vector2 point, List<Vector2> connections)
		{
			IntVec3 c = new IntVec3(Mathf.RoundToInt(point.x), 0, Mathf.RoundToInt(point.y));
			foreach (CellRect cellRect in a.rects)
			{
				if (cellRect.Contains(c))
				{
					foreach (Vector2 vector in connections)
					{
						IntVec3 position = new IntVec3(Mathf.RoundToInt(vector.x), 0, Mathf.RoundToInt(vector.y));
						if (b.Contains(position, 0))
						{
							connections.Remove(vector);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06007527 RID: 29991 RVA: 0x002922B4 File Offset: 0x002904B4
		public bool TryGetRoom(IntVec3 pos, out LayoutRoom room)
		{
			foreach (LayoutRoom layoutRoom in this.Rooms)
			{
				foreach (CellRect cellRect in layoutRoom.rects)
				{
					if (cellRect.Contains(pos))
					{
						room = layoutRoom;
						return true;
					}
				}
			}
			room = null;
			return false;
		}

		// Token: 0x06007528 RID: 29992 RVA: 0x00292354 File Offset: 0x00290554
		public void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.container, "container", default(CellRect), false);
			Scribe_Collections.Look<LayoutRoom>(ref this.rooms, "rooms", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.rooms.Count - 1; i >= 0; i--)
				{
					if (this.rooms[i] == null)
					{
						this.rooms.RemoveAt(i);
					}
					else if (this.rooms[i].sketch == null)
					{
						this.rooms[i].sketch = this.sketch;
					}
				}
			}
		}

		// Token: 0x04004965 RID: 18789
		public LayoutStructureSketch sketch;

		// Token: 0x04004966 RID: 18790
		public CellRect container;

		// Token: 0x04004967 RID: 18791
		private List<LayoutRoom> rooms = new List<LayoutRoom>();

		// Token: 0x04004968 RID: 18792
		public RoomLayoutCellType[,] cellTypes;

		// Token: 0x04004969 RID: 18793
		public int[,] roomIds;

		// Token: 0x0400496A RID: 18794
		public Delaunator delaunator;

		// Token: 0x0400496B RID: 18795
		public RelativeNeighborhoodGraph neighbours;

		// Token: 0x0400496C RID: 18796
		private const int MinAdjacencyForDisconnectedRoom = 3;

		// Token: 0x0400496D RID: 18797
		private static readonly List<LayoutRoom> tmpRooms = new List<LayoutRoom>();

		// Token: 0x0400496E RID: 18798
		private static readonly Queue<LayoutRoom> tmpRoomQueue = new Queue<LayoutRoom>();

		// Token: 0x0400496F RID: 18799
		private static readonly HashSet<LayoutRoom> tmpSeenRooms = new HashSet<LayoutRoom>();
	}
}
