using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Utility;

namespace RimWorld
{
	// Token: 0x02001265 RID: 4709
	public static class RoomLayoutGenerator
	{
		// Token: 0x060074B7 RID: 29879 RVA: 0x0028E97C File Offset: 0x0028CB7C
		public static StructureLayout GenerateRandomLayout(LayoutStructureSketch sketch, CellRect container, int minRoomWidth = 6, int minRoomHeight = 6, float areaPrunePercent = 0.2f, bool canRemoveRooms = false, bool generateDoors = true, LayoutRoomDef corridor = null, int corridorExpansion = 2, IntRange? maxMergeRoomsRange = null, CorridorShape corridorShapes = CorridorShape.All, bool canDisconnectRooms = true, bool singleRoom = false)
		{
			return RoomLayoutGenerator.GenerateRoomLayout(new RoomLayoutParams
			{
				sketch = sketch,
				container = container,
				areaPrunePercent = areaPrunePercent,
				minRoomHeight = Mathf.Max(minRoomWidth, 3),
				minRoomWidth = Mathf.Max(minRoomHeight, 3),
				canRemoveRooms = canRemoveRooms,
				generateDoors = generateDoors,
				corridor = corridor,
				corridorExpansion = corridorExpansion,
				maxMergeRoomsRange = maxMergeRoomsRange,
				corridorShapes = corridorShapes,
				canDisconnectRooms = canDisconnectRooms,
				singleRoom = singleRoom
			});
		}

		// Token: 0x060074B8 RID: 29880 RVA: 0x0028EA14 File Offset: 0x0028CC14
		public static StructureLayout GenerateRandomLayout(CellRect container, int minRoomWidth = 6, int minRoomHeight = 6, float areaPrunePercent = 0.2f, bool canRemoveRooms = false, bool generateDoors = true, LayoutRoomDef corridor = null, int corridorExpansion = 2, IntRange? maxMergeRoomsRange = null, CorridorShape corridorShapes = CorridorShape.All, bool canDisconnectRooms = true, bool singleRoom = false)
		{
			return RoomLayoutGenerator.GenerateRoomLayout(new RoomLayoutParams
			{
				container = container,
				areaPrunePercent = areaPrunePercent,
				minRoomHeight = Mathf.Max(minRoomWidth, 3),
				minRoomWidth = Mathf.Max(minRoomHeight, 3),
				canRemoveRooms = canRemoveRooms,
				generateDoors = generateDoors,
				corridor = corridor,
				corridorExpansion = corridorExpansion,
				maxMergeRoomsRange = maxMergeRoomsRange,
				corridorShapes = corridorShapes,
				canDisconnectRooms = canDisconnectRooms,
				singleRoom = singleRoom
			});
		}

		// Token: 0x060074B9 RID: 29881 RVA: 0x0028EAA4 File Offset: 0x0028CCA4
		private static StructureLayout GenerateRoomLayout(RoomLayoutParams parms)
		{
			StructureLayout structureLayout = new StructureLayout(parms.sketch, parms.container);
			if (parms.singleRoom)
			{
				structureLayout.AddRoom(new List<CellRect>
				{
					parms.container
				}, null);
			}
			else
			{
				if (parms.corridor == null)
				{
					RoomLayoutGenerator.SplitRandom(parms.container, RoomLayoutGenerator.tmpRoomRects, parms.minRoomWidth, parms.minRoomHeight);
					RoomLayoutGenerator.MergeRandom(RoomLayoutGenerator.tmpRoomRects, RoomLayoutGenerator.tmpMergedRoomRects, parms.maxMergeRoomsRange ?? RoomLayoutGenerator.DefaultMaxMergedRoomsRange, 5);
					using (List<List<CellRect>>.Enumerator enumerator = RoomLayoutGenerator.tmpMergedRoomRects.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							List<CellRect> rects = enumerator.Current;
							structureLayout.AddRoom(rects, null);
						}
						goto IL_1B2;
					}
				}
				RoomLayoutGenerator.SplitCorridor(parms.container, RoomLayoutGenerator.tmpCorridors, RoomLayoutGenerator.tmpSpaces, parms.corridorExpansion, parms.minRoomWidth, parms.minRoomHeight, parms.corridorShapes);
				RoomLayoutGenerator.MergeAddCorridors(parms, structureLayout);
				foreach (CellRect rectToSplit in RoomLayoutGenerator.tmpSpaces)
				{
					RoomLayoutGenerator.SplitRandom(rectToSplit, RoomLayoutGenerator.tmpRoomRects, parms.minRoomWidth, parms.minRoomHeight);
					RoomLayoutGenerator.MergeRandom(RoomLayoutGenerator.tmpRoomRects, RoomLayoutGenerator.tmpMergedRoomRects, parms.maxMergeRoomsRange ?? RoomLayoutGenerator.DefaultMaxMergedRoomsRange, 5);
					foreach (List<CellRect> rects2 in RoomLayoutGenerator.tmpMergedRoomRects)
					{
						structureLayout.AddRoom(rects2, null);
					}
				}
			}
			IL_1B2:
			float num = (float)structureLayout.Area - parms.areaPrunePercent * (float)structureLayout.Area;
			int num2 = 100;
			int num3 = 0;
			int num4 = structureLayout.Rooms.Count / 3;
			while (parms.canRemoveRooms && structureLayout.Rooms.Count > 4 && num3 < num4 && (float)structureLayout.Area > num && num2-- > 0)
			{
				LayoutRoom layoutRoom = structureLayout.Rooms.RandomElement<LayoutRoom>();
				if (layoutRoom.requiredDef == null && (parms.canDisconnectRooms || structureLayout.CanRemoveWithoutDisconnection(layoutRoom)))
				{
					structureLayout.RemoveRoom(layoutRoom);
					num3++;
				}
			}
			num2 = 0;
			while ((float)structureLayout.Area > num && num2-- > 0)
			{
				structureLayout.TryMinimizeLayoutWithoutDisconnection();
			}
			structureLayout.FinalizeRooms(true);
			if (parms.generateDoors)
			{
				RoomLayoutGenerator.CreateDoors(structureLayout, parms.entranceCount);
			}
			return structureLayout;
		}

		// Token: 0x060074BA RID: 29882 RVA: 0x0028ED54 File Offset: 0x0028CF54
		private static void MergeAddCorridors(RoomLayoutParams parms, StructureLayout layout)
		{
			while (RoomLayoutGenerator.tmpCorridors.Any<CellRect>())
			{
				List<CellRect> list = new List<CellRect>
				{
					RoomLayoutGenerator.tmpCorridors[0]
				};
				RoomLayoutGenerator.tmpCorridors.RemoveAt(0);
				for (int i = RoomLayoutGenerator.tmpCorridors.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					for (int j = 0; j < list.Count; j++)
					{
						if (RoomLayoutGenerator.tmpCorridors[i].Overlaps(list[j]))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						list.Add(RoomLayoutGenerator.tmpCorridors[i]);
						RoomLayoutGenerator.tmpCorridors.RemoveAt(i);
					}
				}
				LayoutRoom layoutRoom = layout.AddRoom(list, null);
				layoutRoom.requiredDef = parms.corridor;
				layoutRoom.noExteriorDoors = true;
			}
		}

		// Token: 0x060074BB RID: 29883 RVA: 0x0028EE1C File Offset: 0x0028D01C
		private static void CreateDoors(StructureLayout layout, int entranceCount)
		{
			HashSet<int> hashSet = new HashSet<int>();
			RoomLayoutGenerator.tmpCells.Clear();
			RoomLayoutGenerator.tmpCells.AddRange(layout.container.Cells.InRandomOrder(null));
			for (int i = 0; i < RoomLayoutGenerator.tmpCells.Count; i++)
			{
				IntVec3 intVec = RoomLayoutGenerator.tmpCells[i];
				if (layout.IsWallAt(intVec))
				{
					if (layout.IsGoodForHorizontalDoor(intVec))
					{
						int roomIdAt = layout.GetRoomIdAt(intVec + IntVec3.North);
						int roomIdAt2 = layout.GetRoomIdAt(intVec + IntVec3.South);
						bool flag = layout.IsOutside(intVec + IntVec3.North) || layout.IsOutside(intVec + IntVec3.South);
						int item = Gen.HashOrderless(roomIdAt, roomIdAt2);
						if (hashSet.Contains(item) || (flag && entranceCount <= 0))
						{
							goto IL_16A;
						}
						layout.Add(intVec, RoomLayoutCellType.Door);
						hashSet.Add(item);
						if (flag)
						{
							entranceCount--;
						}
					}
					if (layout.IsGoodForVerticalDoor(intVec))
					{
						int roomIdAt3 = layout.GetRoomIdAt(intVec + IntVec3.East);
						int roomIdAt4 = layout.GetRoomIdAt(intVec + IntVec3.West);
						bool flag2 = layout.IsOutside(intVec + IntVec3.East) || layout.IsOutside(intVec + IntVec3.West);
						int item2 = Gen.HashOrderless(roomIdAt3, roomIdAt4);
						if (!hashSet.Contains(item2) && (!flag2 || entranceCount > 0))
						{
							layout.Add(intVec, RoomLayoutCellType.Door);
							hashSet.Add(item2);
							if (flag2)
							{
								entranceCount--;
							}
						}
					}
				}
				IL_16A:;
			}
			RoomLayoutGenerator.tmpCells.Clear();
		}

		// Token: 0x060074BC RID: 29884 RVA: 0x0028EFB4 File Offset: 0x0028D1B4
		private static void SplitCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, int expansion, int minRoomWidth, int minRoomHeight, CorridorShape shapes)
		{
			RoomLayoutGenerator.tmpCorridors.Clear();
			RoomLayoutGenerator.tmpShapes.Clear();
			spaces.Clear();
			foreach (Enum @enum in shapes.GetBitFlags())
			{
				CorridorShape item = (CorridorShape)@enum;
				RoomLayoutGenerator.tmpShapes.Add(item);
			}
			IntVec3 xExpansion = new IntVec3(expansion, 0, 0);
			IntVec3 zExpansion = new IntVec3(0, 0, expansion);
			int xRange = Mathf.Max(rect.Width / 2 - minRoomWidth - expansion - 1, 0);
			int zRange = Mathf.Max(rect.Height / 2 - minRoomHeight - expansion - 1, 0);
			float num;
			if (rect.Width >= rect.Height)
			{
				num = (float)rect.Height / (float)rect.Width;
			}
			else
			{
				num = (float)rect.Width / (float)rect.Height;
			}
			if (RoomLayoutGenerator.tmpShapes.Contains(CorridorShape.H) && (num < 0.5f || rect.Width - expansion * 2 - minRoomWidth * 3 - 2 < 0 || rect.Height - expansion * 2 - minRoomHeight * 3 - 2 < 0))
			{
				RoomLayoutGenerator.tmpShapes.Remove(CorridorShape.H);
			}
			if (RoomLayoutGenerator.tmpShapes.Empty<CorridorShape>())
			{
				Log.Error("Attempted to spawn a corridor with no valid options.");
				return;
			}
			CorridorShape corridorShape = RoomLayoutGenerator.tmpShapes.RandomElement<CorridorShape>();
			if (corridorShape == CorridorShape.Straight)
			{
				RoomLayoutGenerator.GenerateStraightCorridor(rect, corridors, spaces, zExpansion, xExpansion, xRange, zRange);
				return;
			}
			if (corridorShape == CorridorShape.Cross)
			{
				RoomLayoutGenerator.GenerateCrossCorridor(rect, corridors, spaces, zExpansion, xExpansion, xRange, zRange);
				return;
			}
			if (corridorShape == CorridorShape.T)
			{
				RoomLayoutGenerator.GenerateTCorridor(rect, corridors, spaces, zExpansion, xExpansion, xRange, zRange);
				return;
			}
			if (corridorShape == CorridorShape.H)
			{
				RoomLayoutGenerator.GenerateHCorridor(rect, corridors, spaces, zExpansion, xExpansion, xRange, zRange);
				return;
			}
			if (corridorShape == CorridorShape.AsymmetricCross)
			{
				RoomLayoutGenerator.GenerateAsymmetricCrossCorridor(rect, corridors, spaces, zExpansion, xExpansion, xRange, zRange);
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unhanlded corridor shape ");
			defaultInterpolatedStringHandler.AppendFormatted<CorridorShape>(corridorShape);
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060074BD RID: 29885 RVA: 0x0028F1A0 File Offset: 0x0028D3A0
		private static void GenerateStraightCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, IntVec3 zExpansion, IntVec3 xExpansion, int xRange, int zRange)
		{
			Rot4 rot = Rot4.East;
			if (rect.Height > rect.Width)
			{
				rot = Rot4.North;
			}
			int offset = (rot == Rot4.East) ? Rand.Range(-zRange, zRange) : Rand.Range(-xRange, xRange);
			IntVec3 centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			IntVec3 centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, centerCellOnEdge2.x, centerCellOnEdge2.z);
			cellRect = cellRect.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect, spaces, rot);
		}

		// Token: 0x060074BE RID: 29886 RVA: 0x0028F254 File Offset: 0x0028D454
		private static void GenerateCrossCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, IntVec3 zExpansion, IntVec3 xExpansion, int xRange, int zRange)
		{
			Rot4 rot = Rot4.East;
			int offset = Rand.Range(-zRange, zRange);
			IntVec3 intVec = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			IntVec3 intVec2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect = CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
			cellRect = cellRect.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect, spaces, rot);
			rot = Rot4.North;
			IntVec3 point = cellRect.CenterCell + new IntVec3(Rand.Range(-xRange, xRange), 0, 0);
			intVec = rect.GetCellOnEdge(rot.Opposite, point);
			intVec2 = rect.GetCellOnEdge(rot, point);
			CellRect cellRect2 = CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, cellRect.minZ);
			CellRect cellRect3 = CellRect.FromLimits(intVec.x, cellRect.maxZ, intVec2.x, intVec2.z);
			cellRect2 = cellRect2.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			cellRect3 = cellRect3.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect2);
			corridors.Add(cellRect3);
			RoomLayoutGenerator.DivideRectsByCorridor(spaces[(!spaces[0].Overlaps(cellRect2)) ? 1 : 0], cellRect2, spaces, rot);
			RoomLayoutGenerator.DivideRectsByCorridor(spaces[(!spaces[0].Overlaps(cellRect3)) ? 1 : 0], cellRect3, spaces, rot);
			spaces.RemoveAt(0);
			spaces.RemoveAt(0);
		}

		// Token: 0x060074BF RID: 29887 RVA: 0x0028F3F8 File Offset: 0x0028D5F8
		private static void GenerateAsymmetricCrossCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, IntVec3 zExpansion, IntVec3 xExpansion, int xRange, int zRange)
		{
			Rot4 rot = Rot4.West;
			if (rect.Height > rect.Width)
			{
				rot = Rot4.North;
			}
			int offset = rot.IsHorizontal ? Rand.Range(-zRange, zRange) : Rand.Range(-xRange, xRange);
			IntVec3 centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			IntVec3 centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, centerCellOnEdge2.x, centerCellOnEdge2.z);
			cellRect = cellRect.ExpandedBy(rot.IsHorizontal ? zExpansion : xExpansion);
			corridors.Add(cellRect);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect, spaces, rot);
			rot = rot.Rotated(RotationDirection.Clockwise);
			offset = (rot.IsHorizontal ? Rand.Range(-zRange, zRange) : Rand.Range(-xRange, xRange));
			centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect2;
			if (rot.IsHorizontal)
			{
				cellRect2 = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, cellRect.minX, centerCellOnEdge2.z);
			}
			else
			{
				cellRect2 = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, centerCellOnEdge2.x, cellRect.minZ);
			}
			offset = (rot.IsHorizontal ? Rand.Range(-zRange, zRange) : Rand.Range(-xRange, xRange));
			centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect3;
			if (rot.IsHorizontal)
			{
				cellRect3 = CellRect.FromLimits(cellRect.maxX, centerCellOnEdge.z, centerCellOnEdge2.x, centerCellOnEdge2.z);
			}
			else
			{
				cellRect3 = CellRect.FromLimits(centerCellOnEdge.x, cellRect.maxZ, centerCellOnEdge2.x, centerCellOnEdge2.z);
			}
			cellRect2 = cellRect2.ExpandedBy(rot.IsHorizontal ? zExpansion : xExpansion);
			cellRect3 = cellRect3.ExpandedBy(rot.IsHorizontal ? zExpansion : xExpansion);
			corridors.Add(cellRect2);
			corridors.Add(cellRect3);
			RoomLayoutGenerator.DivideRectsByCorridor(spaces[(!spaces[0].Overlaps(cellRect2)) ? 1 : 0], cellRect2, spaces, rot);
			RoomLayoutGenerator.DivideRectsByCorridor(spaces[(!spaces[0].Overlaps(cellRect3)) ? 1 : 0], cellRect3, spaces, rot);
			spaces.RemoveAt(0);
			spaces.RemoveAt(0);
		}

		// Token: 0x060074C0 RID: 29888 RVA: 0x0028F64C File Offset: 0x0028D84C
		private static void GenerateTCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, IntVec3 zExpansion, IntVec3 xExpansion, int xRange, int zRange)
		{
			Rot4 rot = Rot4.East;
			if (rect.Height > rect.Width)
			{
				rot = Rot4.North;
			}
			int offset = (rot == Rot4.East) ? Rand.Range(-zRange, zRange) : Rand.Range(-xRange, xRange);
			IntVec3 intVec = rect.GetCenterCellOnEdge(rot.Opposite, offset);
			IntVec3 intVec2 = rect.GetCenterCellOnEdge(rot, offset);
			CellRect cellRect = CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
			cellRect = cellRect.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect, spaces, rot);
			int i = Mathf.RoundToInt(cellRect.CenterCell.DistanceTo(intVec2)) - ((rot == Rot4.East) ? cellRect.Height : cellRect.Width) - ((rot == Rot4.East) ? zRange : xRange);
			if (Rand.Bool)
			{
				rot = rot.Opposite;
			}
			IntVec3 point = cellRect.CenterCell + rot.FacingCell * i;
			rot = rot.Rotated(RotationDirection.Clockwise);
			intVec = rect.GetCellOnEdge(rot.Opposite, point);
			intVec2 = rect.GetCellOnEdge(rot, point);
			CellRect cellRect2 = CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
			cellRect2 = cellRect2.ExpandedBy(rot.IsHorizontal ? zExpansion : xExpansion);
			if (rot.IsVertical && cellRect2.CenterCell.x >= rect.CenterCell.x)
			{
				cellRect.maxX = cellRect2.minX;
			}
			else if (rot.IsVertical)
			{
				cellRect.minX = cellRect2.maxX;
			}
			else if (cellRect2.CenterCell.z >= rect.CenterCell.z)
			{
				cellRect.maxZ = cellRect2.minZ;
			}
			else
			{
				cellRect.minZ = cellRect2.maxZ;
			}
			corridors.Add(cellRect);
			corridors.Add(cellRect2);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect2, spaces, rot);
			CellRect value = spaces[0];
			CellRect value2 = spaces[1];
			if (rot.IsVertical && cellRect2.CenterCell.x >= rect.CenterCell.x)
			{
				value.maxX = cellRect2.minX;
				value2.maxX = cellRect2.minX;
				spaces.RemoveAt(2);
			}
			else if (!rot.IsVertical && cellRect2.CenterCell.z >= rect.CenterCell.z)
			{
				value.maxZ = cellRect2.minZ;
				value2.maxZ = cellRect2.minZ;
				spaces.RemoveAt(3);
			}
			else if (rot.IsVertical)
			{
				value.minX = cellRect2.maxX;
				value2.minX = cellRect2.maxX;
				spaces.RemoveAt(2);
			}
			else
			{
				value.minZ = cellRect2.maxZ;
				value2.minZ = cellRect2.maxZ;
				spaces.RemoveAt(3);
			}
			spaces[0] = value;
			spaces[1] = value2;
		}

		// Token: 0x060074C1 RID: 29889 RVA: 0x0028F96C File Offset: 0x0028DB6C
		private static void GenerateHCorridor(CellRect rect, List<CellRect> corridors, List<CellRect> spaces, IntVec3 zExpansion, IntVec3 xExpansion, int xRange, int zRange)
		{
			Rot4 rot = Rot4.East;
			if (rect.Height > rect.Width)
			{
				rot = Rot4.North;
			}
			int num = (rot == Rot4.East) ? zRange : xRange;
			IntVec3 centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, -num);
			IntVec3 centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, -num);
			CellRect cellRect = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, centerCellOnEdge2.x, centerCellOnEdge2.z);
			cellRect = cellRect.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect, spaces, rot);
			centerCellOnEdge = rect.GetCenterCellOnEdge(rot.Opposite, num);
			centerCellOnEdge2 = rect.GetCenterCellOnEdge(rot, num);
			CellRect cellRect2 = CellRect.FromLimits(centerCellOnEdge.x, centerCellOnEdge.z, centerCellOnEdge2.x, centerCellOnEdge2.z);
			cellRect2 = cellRect2.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect2);
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect2, spaces, rot);
			rot = ((rot == Rot4.East) ? Rot4.North : Rot4.East);
			CellRect cellRect3;
			if (rot == Rot4.East)
			{
				int num2 = cellRect.CenterCell.z + Rand.Range(-zRange, zRange);
				cellRect3 = CellRect.FromLimits(cellRect.maxX, num2, cellRect2.minX, num2);
			}
			else
			{
				int num3 = cellRect.CenterCell.x + Rand.Range(-xRange, xRange);
				cellRect3 = CellRect.FromLimits(num3, cellRect.maxZ, num3, cellRect2.minZ);
			}
			cellRect3 = cellRect3.ExpandedBy((rot == Rot4.East) ? zExpansion : xExpansion);
			corridors.Add(cellRect3);
			spaces.RemoveAt(0);
			spaces.RemoveAt(2);
			if (rot == Rot4.East)
			{
				rect.minX = cellRect.maxX;
				rect.maxX = cellRect2.minX;
			}
			else
			{
				rect.minZ = cellRect.maxZ;
				rect.maxZ = cellRect2.minZ;
			}
			RoomLayoutGenerator.DivideRectsByCorridor(rect, cellRect3, spaces, rot);
		}

		// Token: 0x060074C2 RID: 29890 RVA: 0x0028FB88 File Offset: 0x0028DD88
		private static void DivideRectsByCorridor(CellRect rect, CellRect corridor, List<CellRect> spaces, Rot4 corridorRot)
		{
			CellRect item = rect;
			CellRect item2 = rect;
			if (!rect.Overlaps(corridor))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(113, 4);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to divide rects by corridor but rect and corridor are not overlapping. rect: ");
				defaultInterpolatedStringHandler.AppendFormatted<CellRect>(rect);
				defaultInterpolatedStringHandler.AppendLiteral(", corridor: ");
				defaultInterpolatedStringHandler.AppendFormatted<CellRect>(corridor);
				defaultInterpolatedStringHandler.AppendLiteral(", rot: ");
				defaultInterpolatedStringHandler.AppendFormatted<Rot4>(corridorRot);
				defaultInterpolatedStringHandler.AppendLiteral(", \nspaces:\n");
				defaultInterpolatedStringHandler.AppendFormatted((from r in spaces
				select r.ToString()).ToLineList(null, false));
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (corridorRot == Rot4.North)
			{
				item.minX = corridor.maxX;
				item2.maxX = corridor.minX;
			}
			else if (corridorRot == Rot4.East)
			{
				item.minZ = corridor.maxZ;
				item2.maxZ = corridor.minZ;
			}
			else if (corridorRot == Rot4.South)
			{
				item.maxX = corridor.minX;
				item2.minX = corridor.maxX;
			}
			else
			{
				item.maxZ = corridor.minZ;
				item2.minZ = corridor.maxZ;
			}
			spaces.Add(item);
			spaces.Add(item2);
		}

		// Token: 0x060074C3 RID: 29891 RVA: 0x0028FCDC File Offset: 0x0028DEDC
		private static void SplitRandom(CellRect rectToSplit, List<CellRect> rooms, int minWidth, int minHeight)
		{
			RoomLayoutGenerator.<>c__DisplayClass19_0 CS$<>8__locals1;
			CS$<>8__locals1.minHeight = minHeight;
			CS$<>8__locals1.minWidth = minWidth;
			rooms.Clear();
			Queue<CellRect> queue = new Queue<CellRect>();
			queue.Enqueue(rectToSplit);
			while (queue.Count > 0)
			{
				CellRect cellRect = queue.Dequeue();
				if (!RoomLayoutGenerator.<SplitRandom>g__CanSplit|19_0(cellRect, ref CS$<>8__locals1))
				{
					rooms.Add(cellRect);
				}
				else if (cellRect.Width > cellRect.Height)
				{
					int num = Rand.Range(CS$<>8__locals1.minWidth, cellRect.Width - CS$<>8__locals1.minWidth);
					CellRect item = new CellRect(cellRect.minX, cellRect.minZ, num, cellRect.Height);
					CellRect item2 = new CellRect(cellRect.minX - 1 + num, cellRect.minZ, cellRect.Width - num + 1, cellRect.Height);
					queue.Enqueue(item);
					queue.Enqueue(item2);
				}
				else
				{
					int num2 = Rand.Range(CS$<>8__locals1.minHeight, cellRect.Height - CS$<>8__locals1.minHeight);
					CellRect item3 = new CellRect(cellRect.minX, cellRect.minZ - 1 + num2, cellRect.Width, cellRect.Height - num2 + 1);
					CellRect item4 = new CellRect(cellRect.minX, cellRect.minZ, cellRect.Width, num2);
					queue.Enqueue(item3);
					queue.Enqueue(item4);
				}
			}
		}

		// Token: 0x060074C4 RID: 29892 RVA: 0x0028FE2C File Offset: 0x0028E02C
		private static void MergeRandom(List<CellRect> rects, List<List<CellRect>> mergedRects, IntRange maxMergedRooms, int minAdjacenyScore = 5)
		{
			mergedRects.Clear();
			rects.Shuffle<CellRect>();
			for (int i = 0; i < rects.Count; i++)
			{
				CellRect cellRect = rects[i];
				if (!RoomLayoutGenerator.UsedInMerge(mergedRects, cellRect))
				{
					List<CellRect> list = new List<CellRect>
					{
						cellRect
					};
					int num = Math.Max(1, maxMergedRooms.RandomInRange);
					int num2 = 0;
					while (num2 < rects.Count && list.Count < num)
					{
						CellRect cellRect2 = rects[num2];
						if (!(cellRect == cellRect2) && !RoomLayoutGenerator.UsedInMerge(mergedRects, cellRect2) && cellRect.GetAdjacencyScore(cellRect2) >= minAdjacenyScore)
						{
							list.Add(cellRect2);
						}
						num2++;
					}
					mergedRects.Add(list);
				}
			}
		}

		// Token: 0x060074C5 RID: 29893 RVA: 0x0028FEE0 File Offset: 0x0028E0E0
		private static bool UsedInMerge(List<List<CellRect>> mergedRects, CellRect rect)
		{
			for (int i = 0; i < mergedRects.Count; i++)
			{
				for (int j = 0; j < mergedRects[i].Count; j++)
				{
					if (mergedRects[i][j] == rect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060074C7 RID: 29895 RVA: 0x0028FF85 File Offset: 0x0028E185
		[CompilerGenerated]
		internal static bool <SplitRandom>g__CanSplit|19_0(CellRect r, ref RoomLayoutGenerator.<>c__DisplayClass19_0 A_1)
		{
			return r.Height > 2 * A_1.minHeight || r.Width > 2 * A_1.minWidth;
		}

		// Token: 0x04004949 RID: 18761
		private static readonly IntRange DefaultMaxMergedRoomsRange = new IntRange(1, 3);

		// Token: 0x0400494A RID: 18762
		private static readonly List<CellRect> tmpRoomRects = new List<CellRect>();

		// Token: 0x0400494B RID: 18763
		private static readonly List<CellRect> tmpCorridors = new List<CellRect>();

		// Token: 0x0400494C RID: 18764
		private static readonly List<CellRect> tmpSpaces = new List<CellRect>();

		// Token: 0x0400494D RID: 18765
		private static readonly List<List<CellRect>> tmpMergedRoomRects = new List<List<CellRect>>();

		// Token: 0x0400494E RID: 18766
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x0400494F RID: 18767
		private static readonly List<CorridorShape> tmpShapes = new List<CorridorShape>();
	}
}
