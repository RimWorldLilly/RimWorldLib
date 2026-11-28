using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000075 RID: 117
	public static class GenAdj
	{
		// Token: 0x06000469 RID: 1129 RVA: 0x0001A4D0 File Offset: 0x000186D0
		static GenAdj()
		{
			GenAdj.SetupAdjacencyTables();
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001A560 File Offset: 0x00018760
		private static void SetupAdjacencyTables()
		{
			GenAdj.CardinalDirections[0] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirections[1] = new IntVec3(1, 0, 0);
			GenAdj.CardinalDirections[2] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirections[3] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[0] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirectionsAndInside[1] = new IntVec3(1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[2] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirectionsAndInside[3] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[4] = new IntVec3(0, 0, 0);
			GenAdj.CardinalDirectionsAround[0] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirectionsAround[1] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAround[2] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirectionsAround[3] = new IntVec3(1, 0, 0);
			GenAdj.DiagonalDirections[0] = new IntVec3(-1, 0, -1);
			GenAdj.DiagonalDirections[1] = new IntVec3(-1, 0, 1);
			GenAdj.DiagonalDirections[2] = new IntVec3(1, 0, 1);
			GenAdj.DiagonalDirections[3] = new IntVec3(1, 0, -1);
			GenAdj.DiagonalDirectionsAround[0] = new IntVec3(-1, 0, -1);
			GenAdj.DiagonalDirectionsAround[1] = new IntVec3(-1, 0, 1);
			GenAdj.DiagonalDirectionsAround[2] = new IntVec3(1, 0, 1);
			GenAdj.DiagonalDirectionsAround[3] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCells[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCells[1] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCells[2] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCells[3] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCells[4] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCells[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCells[6] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCells[7] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInside[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAndInside[1] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAndInside[2] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAndInside[3] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAndInside[4] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAndInside[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAndInside[6] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAndInside[7] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInside[8] = new IntVec3(0, 0, 0);
			GenAdj.AdjacentCellsAround[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAround[1] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAround[2] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAround[3] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAround[4] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAround[5] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAround[6] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAround[7] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[0] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[1] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[2] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAroundBottom[3] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[4] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[6] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAroundBottom[7] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[8] = new IntVec3(0, 0, 0);
			GenAdj.AdjacentCellsAndInsideForUV[0] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAndInsideForUV[1] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInsideForUV[2] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAndInsideForUV[3] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAndInsideForUV[4] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAndInsideForUV[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAndInsideForUV[6] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAndInsideForUV[7] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAndInsideForUV[8] = new IntVec3(0, 0, 0);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001AA30 File Offset: 0x00018C30
		public static List<IntVec3> AdjacentCells8WayRandomized()
		{
			if (GenAdj.adjRandomOrderList == null)
			{
				GenAdj.adjRandomOrderList = new List<IntVec3>();
				for (int i = 0; i < 8; i++)
				{
					GenAdj.adjRandomOrderList.Add(GenAdj.AdjacentCells[i]);
				}
			}
			GenAdj.adjRandomOrderList.Shuffle<IntVec3>();
			return GenAdj.adjRandomOrderList;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001AA7E File Offset: 0x00018C7E
		public static IEnumerable<IntVec3> CellsOccupiedBy(Thing t)
		{
			GenAdj.<CellsOccupiedBy>d__14 <CellsOccupiedBy>d__ = new GenAdj.<CellsOccupiedBy>d__14(-2);
			<CellsOccupiedBy>d__.<>3__t = t;
			return <CellsOccupiedBy>d__;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001AA8E File Offset: 0x00018C8E
		public static IEnumerable<IntVec3> CellsOccupiedBy(IntVec3 center, Rot4 rotation, IntVec2 size)
		{
			GenAdj.<CellsOccupiedBy>d__15 <CellsOccupiedBy>d__ = new GenAdj.<CellsOccupiedBy>d__15(-2);
			<CellsOccupiedBy>d__.<>3__center = center;
			<CellsOccupiedBy>d__.<>3__rotation = rotation;
			<CellsOccupiedBy>d__.<>3__size = size;
			return <CellsOccupiedBy>d__;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001AAAC File Offset: 0x00018CAC
		public static IEnumerable<IntVec3> CellsAdjacent8Way(TargetInfo pack)
		{
			GenAdj.<CellsAdjacent8Way>d__16 <CellsAdjacent8Way>d__ = new GenAdj.<CellsAdjacent8Way>d__16(-2);
			<CellsAdjacent8Way>d__.<>3__pack = pack;
			return <CellsAdjacent8Way>d__;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001AABC File Offset: 0x00018CBC
		public static IEnumerable<IntVec3> CellsAdjacent8Way(Thing t)
		{
			return GenAdj.CellsAdjacent8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001AADA File Offset: 0x00018CDA
		public static IEnumerable<IntVec3> CellsAdjacent8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			GenAdj.<CellsAdjacent8Way>d__18 <CellsAdjacent8Way>d__ = new GenAdj.<CellsAdjacent8Way>d__18(-2);
			<CellsAdjacent8Way>d__.<>3__thingCenter = thingCenter;
			<CellsAdjacent8Way>d__.<>3__thingRot = thingRot;
			<CellsAdjacent8Way>d__.<>3__thingSize = thingSize;
			return <CellsAdjacent8Way>d__;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001AAF8 File Offset: 0x00018CF8
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(Thing t)
		{
			return GenAdj.CellsAdjacentCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001AB16 File Offset: 0x00018D16
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.<CellsAdjacentCardinal>d__20 <CellsAdjacentCardinal>d__ = new GenAdj.<CellsAdjacentCardinal>d__20(-2);
			<CellsAdjacentCardinal>d__.<>3__center = center;
			<CellsAdjacentCardinal>d__.<>3__rot = rot;
			<CellsAdjacentCardinal>d__.<>3__size = size;
			return <CellsAdjacentCardinal>d__;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001AB34 File Offset: 0x00018D34
		public static IEnumerable<IntVec3> CellsAdjacentAlongEdge(IntVec3 center, Rot4 rot, IntVec2 size, int dir)
		{
			GenAdj.<CellsAdjacentAlongEdge>d__21 <CellsAdjacentAlongEdge>d__ = new GenAdj.<CellsAdjacentAlongEdge>d__21(-2);
			<CellsAdjacentAlongEdge>d__.<>3__center = center;
			<CellsAdjacentAlongEdge>d__.<>3__rot = rot;
			<CellsAdjacentAlongEdge>d__.<>3__size = size;
			<CellsAdjacentAlongEdge>d__.<>3__dir = dir;
			return <CellsAdjacentAlongEdge>d__;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001AB59 File Offset: 0x00018D59
		public static IEnumerable<IntVec3> CellsAdjacent8WayAndInside(this Thing thing)
		{
			GenAdj.<CellsAdjacent8WayAndInside>d__22 <CellsAdjacent8WayAndInside>d__ = new GenAdj.<CellsAdjacent8WayAndInside>d__22(-2);
			<CellsAdjacent8WayAndInside>d__.<>3__thing = thing;
			return <CellsAdjacent8WayAndInside>d__;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001AB69 File Offset: 0x00018D69
		public static void GetAdjacentCorners(LocalTargetInfo target, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			if (target.HasThing)
			{
				GenAdj.GetAdjacentCorners(target.Thing.OccupiedRect(), out BL, out TL, out TR, out BR);
				return;
			}
			GenAdj.GetAdjacentCorners(CellRect.SingleCell(target.Cell), out BL, out TL, out TR, out BR);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001ABA4 File Offset: 0x00018DA4
		private static void GetAdjacentCorners(CellRect rect, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			BL = new IntVec3(rect.minX - 1, 0, rect.minZ - 1);
			TL = new IntVec3(rect.minX - 1, 0, rect.maxZ + 1);
			TR = new IntVec3(rect.maxX + 1, 0, rect.maxZ + 1);
			BR = new IntVec3(rect.maxX + 1, 0, rect.minZ - 1);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001AC22 File Offset: 0x00018E22
		public static IntVec3 RandomAdjacentCell8Way(this IntVec3 root)
		{
			return root + GenAdj.AdjacentCells[Rand.RangeInclusive(0, 7)];
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0001AC3B File Offset: 0x00018E3B
		public static IntVec3 RandomAdjacentCellCardinal(this IntVec3 root)
		{
			return root + GenAdj.CardinalDirections[Rand.RangeInclusive(0, 3)];
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001AC54 File Offset: 0x00018E54
		public static IntVec3 RandomAdjacentCell8Way(this Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			CellRect cellRect2 = cellRect.ExpandedBy(1);
			IntVec3 randomCell;
			do
			{
				randomCell = cellRect2.RandomCell;
			}
			while (cellRect.Contains(randomCell));
			return randomCell;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001AC84 File Offset: 0x00018E84
		public static IntVec3 RandomAdjacentCellCardinal(this Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			IntVec3 randomCell = cellRect.RandomCell;
			if (Rand.Value < 0.5f)
			{
				if (Rand.Value < 0.5f)
				{
					randomCell.x = cellRect.minX - 1;
				}
				else
				{
					randomCell.x = cellRect.maxX + 1;
				}
			}
			else if (Rand.Value < 0.5f)
			{
				randomCell.z = cellRect.minZ - 1;
			}
			else
			{
				randomCell.z = cellRect.maxZ + 1;
			}
			return randomCell;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001AD07 File Offset: 0x00018F07
		public static bool TryFindRandomAdjacentCell8WayWithRoom(Thing t, out IntVec3 result)
		{
			return GenAdj.TryFindRandomAdjacentCell8WayWithRoom(t.Position, t.Rotation, t.def.size, t.MapHeld, out result);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001AD2C File Offset: 0x00018F2C
		public static bool TryFindRandomAdjacentCell8WayWithRoom(IntVec3 center, Rot4 rot, IntVec2 size, Map map, out IntVec3 result)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			GenAdj.validCells.Clear();
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(center, rot, size))
			{
				if (intVec.InBounds(map) && intVec.GetRoom(map) != null)
				{
					GenAdj.validCells.Add(intVec);
				}
			}
			return GenAdj.validCells.TryRandomElement(out result);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001ADB4 File Offset: 0x00018FB4
		public static bool AdjacentTo8WayOrInside(this IntVec3 me, LocalTargetInfo other)
		{
			if (other.HasThing)
			{
				return me.AdjacentTo8WayOrInside(other.Thing);
			}
			return me.AdjacentTo8WayOrInside(other.Cell);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001ADDC File Offset: 0x00018FDC
		public static bool AdjacentTo8Way(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			if (num == 0 && num2 == 0)
			{
				return false;
			}
			if (num < 0)
			{
				num *= -1;
			}
			if (num2 < 0)
			{
				num2 *= -1;
			}
			return num <= 1 && num2 <= 1;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001AE2C File Offset: 0x0001902C
		public static bool AdjacentTo8WayOrInside(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			if (num < 0)
			{
				num *= -1;
			}
			if (num2 < 0)
			{
				num2 *= -1;
			}
			return num <= 1 && num2 <= 1;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001AE74 File Offset: 0x00019074
		public static bool IsAdjacentToCardinalOrInside(this IntVec3 me, CellRect other)
		{
			if (other.IsEmpty)
			{
				return false;
			}
			CellRect cellRect = other.ExpandedBy(1);
			return cellRect.Contains(me) && !cellRect.IsCorner(me);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001AEAC File Offset: 0x000190AC
		public static bool IsAdjacentToCardinalOrInside(this Thing t1, Thing t2)
		{
			return GenAdj.IsAdjacentToCardinalOrInside(t1.OccupiedRect(), t2.OccupiedRect());
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001AEC0 File Offset: 0x000190C0
		public static bool IsAdjacentToCardinalOrInside(CellRect rect1, CellRect rect2)
		{
			if (rect1.IsEmpty || rect2.IsEmpty)
			{
				return false;
			}
			CellRect cellRect = rect1.ExpandedBy(1);
			int minX = cellRect.minX;
			int maxX = cellRect.maxX;
			int minZ = cellRect.minZ;
			int maxZ = cellRect.maxZ;
			int i = minX;
			int j = minZ;
			while (i <= maxX)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
				i++;
			}
			i--;
			for (j++; j <= maxZ; j++)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			j--;
			for (i--; i >= minX; i--)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			i++;
			for (j--; j > minZ; j--)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001B057 File Offset: 0x00019257
		public static bool AdjacentTo8WayOrInside(this IntVec3 root, Thing t)
		{
			return root.AdjacentTo8WayOrInside(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001B078 File Offset: 0x00019278
		public static bool AdjacentTo8WayOrInside(this IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2 - 1;
			int num2 = center.z - (size.z - 1) / 2 - 1;
			int num3 = num + size.x + 1;
			int num4 = num2 + size.z + 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0001B0F4 File Offset: 0x000192F4
		public static bool AdjacentTo8WayOrInside(this Thing a, Thing b)
		{
			return GenAdj.AdjacentTo8WayOrInside(a.OccupiedRect(), b.OccupiedRect());
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0001B108 File Offset: 0x00019308
		public static bool AdjacentTo8WayOrInside(CellRect rect1, CellRect rect2)
		{
			return !rect1.IsEmpty && !rect2.IsEmpty && rect1.ExpandedBy(1).Overlaps(rect2);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001B13A File Offset: 0x0001933A
		public static bool IsInside(this IntVec3 root, Thing t)
		{
			return GenAdj.IsInside(root, t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001B15C File Offset: 0x0001935C
		public static bool IsInside(IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2;
			int num2 = center.z - (size.z - 1) / 2;
			int num3 = num + size.x - 1;
			int num4 = num2 + size.z - 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001B1D6 File Offset: 0x000193D6
		public static CellRect OccupiedRect(this Thing t)
		{
			return GenAdj.OccupiedRect(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001B1F4 File Offset: 0x000193F4
		public static CellRect OccupiedDrawRect(this Thing t)
		{
			Vector2 drawSize = t.DrawSize;
			return GenAdj.OccupiedRect(t.Position, t.Rotation, new IntVec2(Mathf.CeilToInt(drawSize.x), Mathf.CeilToInt(drawSize.y)));
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001B234 File Offset: 0x00019434
		public static Bounds DrawBounds(this Thing t)
		{
			Vector3 center = t.TrueCenter();
			Vector3 size = t.Rotation.AsQuat * t.def.size.ToVector3();
			return new Bounds(center, size);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001B271 File Offset: 0x00019471
		public static CellRect OccupiedRect(IntVec3 center, Rot4 rot, int width, int height)
		{
			return GenAdj.OccupiedRect(center, rot, new IntVec2(width, height));
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001B281 File Offset: 0x00019481
		public static CellRect OccupiedRect(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			return new CellRect(center.x - (size.x - 1) / 2, center.z - (size.z - 1) / 2, size.x, size.z);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001B2C0 File Offset: 0x000194C0
		public static CellRect RectAbout(this IntVec3 center, int width, int height)
		{
			return GenAdj.OccupiedRect(center, Rot4.North, new IntVec2(width, height));
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001B2D4 File Offset: 0x000194D4
		public static CellRect RectAbout(this IntVec3 center, int width, int height, Rot4 rot)
		{
			return GenAdj.OccupiedRect(center, rot, new IntVec2(width, height));
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001B2E4 File Offset: 0x000194E4
		public static CellRect RectAbout(this IntVec3 center, IntVec2 size)
		{
			return GenAdj.OccupiedRect(center, Rot4.North, size);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001B2F2 File Offset: 0x000194F2
		public static CellRect RectAbout(this IntVec3 center, IntVec2 size, Rot4 rot)
		{
			return GenAdj.OccupiedRect(center, rot, size);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001B2FC File Offset: 0x000194FC
		public static void AdjustForRotation(ref IntVec3 center, ref IntVec2 size, Rot4 rot)
		{
			GenAdj.AdjustForRotation(ref center, ref size, Rot4.North, rot);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0001B30C File Offset: 0x0001950C
		public static void AdjustForRotation(ref IntVec3 center, ref IntVec2 size, Rot4 reference, Rot4 rot)
		{
			if (size.x == 1 && size.z == 1)
			{
				return;
			}
			int num;
			int z;
			if (rot.IsHorizontal)
			{
				ref int ptr = ref size.x;
				num = size.z;
				z = size.x;
				ptr = num;
				size.z = z;
			}
			z = reference.AsInt;
			num = rot.AsInt;
			ValueTuple<int, int> valueTuple;
			switch (z)
			{
			case 0:
				switch (num)
				{
				case 1:
					valueTuple = new ValueTuple<int, int>(0, -1);
					goto IL_15F;
				case 2:
					valueTuple = new ValueTuple<int, int>(-1, -1);
					goto IL_15F;
				case 3:
					valueTuple = new ValueTuple<int, int>(-1, 0);
					goto IL_15F;
				}
				break;
			case 1:
				switch (num)
				{
				case 1:
					valueTuple = new ValueTuple<int, int>(-1, 0);
					goto IL_15F;
				case 2:
					valueTuple = new ValueTuple<int, int>(-1, 1);
					goto IL_15F;
				case 3:
					valueTuple = new ValueTuple<int, int>(0, 1);
					goto IL_15F;
				}
				break;
			case 2:
				switch (num)
				{
				case 1:
					valueTuple = new ValueTuple<int, int>(0, 1);
					goto IL_15F;
				case 2:
					valueTuple = new ValueTuple<int, int>(1, 1);
					goto IL_15F;
				case 3:
					valueTuple = new ValueTuple<int, int>(1, 0);
					goto IL_15F;
				}
				break;
			case 3:
				switch (num)
				{
				case 1:
					valueTuple = new ValueTuple<int, int>(1, 0);
					goto IL_15F;
				case 2:
					valueTuple = new ValueTuple<int, int>(1, -1);
					goto IL_15F;
				case 3:
					valueTuple = new ValueTuple<int, int>(0, -1);
					goto IL_15F;
				}
				break;
			}
			valueTuple = new ValueTuple<int, int>(0, 0);
			IL_15F:
			ValueTuple<int, int> valueTuple2 = valueTuple;
			int item = valueTuple2.Item1;
			int item2 = valueTuple2.Item2;
			if (size.x % 2 == 0)
			{
				center.x += item;
			}
			if (size.z % 2 == 0)
			{
				center.z += item2;
			}
		}

		// Token: 0x040001AB RID: 427
		public static IntVec3[] CardinalDirections = new IntVec3[4];

		// Token: 0x040001AC RID: 428
		public static IntVec3[] CardinalDirectionsAndInside = new IntVec3[5];

		// Token: 0x040001AD RID: 429
		public static IntVec3[] CardinalDirectionsAround = new IntVec3[4];

		// Token: 0x040001AE RID: 430
		public static IntVec3[] DiagonalDirections = new IntVec3[4];

		// Token: 0x040001AF RID: 431
		public static IntVec3[] DiagonalDirectionsAround = new IntVec3[4];

		// Token: 0x040001B0 RID: 432
		public static IntVec3[] AdjacentCells = new IntVec3[8];

		// Token: 0x040001B1 RID: 433
		public static IntVec3[] AdjacentCellsAndInside = new IntVec3[9];

		// Token: 0x040001B2 RID: 434
		public static IntVec3[] AdjacentCellsAround = new IntVec3[8];

		// Token: 0x040001B3 RID: 435
		public static IntVec3[] AdjacentCellsAroundBottom = new IntVec3[9];

		// Token: 0x040001B4 RID: 436
		public static IntVec3[] AdjacentCellsAndInsideForUV = new IntVec3[9];

		// Token: 0x040001B5 RID: 437
		private static List<IntVec3> adjRandomOrderList;

		// Token: 0x040001B6 RID: 438
		private static List<IntVec3> validCells = new List<IntVec3>();
	}
}
