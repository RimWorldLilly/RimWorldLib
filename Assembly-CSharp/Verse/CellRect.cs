using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000061 RID: 97
	public struct CellRect : IEquatable<CellRect>, IEnumerable<IntVec3>, IEnumerable
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002AD RID: 685 RVA: 0x00014172 File Offset: 0x00012372
		public static CellRect Empty
		{
			get
			{
				return new CellRect(0, 0, 0, 0);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0001417D File Offset: 0x0001237D
		public readonly bool IsEmpty
		{
			get
			{
				return this.Width <= 0 || this.Height <= 0;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00014196 File Offset: 0x00012396
		public readonly int Area
		{
			get
			{
				return this.Width * this.Height;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x000141A5 File Offset: 0x000123A5
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x000141C6 File Offset: 0x000123C6
		public int Width
		{
			readonly get
			{
				if (this.minX > this.maxX)
				{
					return 0;
				}
				return this.maxX - this.minX + 1;
			}
			set
			{
				this.maxX = this.minX + Mathf.Max(value, 0) - 1;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x000141DE File Offset: 0x000123DE
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x000141FF File Offset: 0x000123FF
		public int Height
		{
			readonly get
			{
				if (this.minZ > this.maxZ)
				{
					return 0;
				}
				return this.maxZ - this.minZ + 1;
			}
			set
			{
				this.maxZ = this.minZ + Mathf.Max(value, 0) - 1;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00014217 File Offset: 0x00012417
		public IntVec2 Size
		{
			get
			{
				return new IntVec2(this.Width, this.Height);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0001422A File Offset: 0x0001242A
		public readonly IEnumerable<IntVec3> Corners
		{
			get
			{
				CellRect.<get_Corners>d__19 <get_Corners>d__ = new CellRect.<get_Corners>d__19(-2);
				<get_Corners>d__.<>3__<>4__this = this;
				return <get_Corners>d__;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0001423F File Offset: 0x0001243F
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x00014253 File Offset: 0x00012453
		public IntVec3 Min
		{
			get
			{
				return new IntVec3(this.minX, 0, this.minZ);
			}
			set
			{
				this.minX = value.x;
				this.minZ = value.z;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0001426D File Offset: 0x0001246D
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x00014281 File Offset: 0x00012481
		public IntVec3 Max
		{
			get
			{
				return new IntVec3(this.maxX, 0, this.maxZ);
			}
			set
			{
				this.maxX = value.x;
				this.maxZ = value.z;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0001429B File Offset: 0x0001249B
		public IntVec3 RandomCell
		{
			get
			{
				return new IntVec3(Rand.RangeInclusive(this.minX, this.maxX), 0, Rand.RangeInclusive(this.minZ, this.maxZ));
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002BB RID: 699 RVA: 0x000142C5 File Offset: 0x000124C5
		public IntVec3 CenterCell
		{
			get
			{
				return new IntVec3(this.minX + this.Width / 2, 0, this.minZ + this.Height / 2);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002BC RID: 700 RVA: 0x000142EB File Offset: 0x000124EB
		public Vector3 CenterVector3
		{
			get
			{
				return new Vector3((float)this.minX + (float)this.Width / 2f, 0f, (float)this.minZ + (float)this.Height / 2f);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00014321 File Offset: 0x00012521
		public Vector3 RandomVector3
		{
			get
			{
				return new Vector3(Rand.Range((float)this.minX, (float)this.maxX + 1f), 0f, Rand.Range((float)this.minZ, (float)this.maxZ + 1f));
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0001435F File Offset: 0x0001255F
		public readonly bool AreSidesEqualOrGreater(int width, int height)
		{
			return this.Width >= width && this.Height >= height;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002BF RID: 703 RVA: 0x00014378 File Offset: 0x00012578
		public readonly IEnumerable<IntVec3> Cells
		{
			get
			{
				CellRect.<get_Cells>d__36 <get_Cells>d__ = new CellRect.<get_Cells>d__36(-2);
				<get_Cells>d__.<>3__<>4__this = this;
				return <get_Cells>d__;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0001438D File Offset: 0x0001258D
		public readonly IEnumerable<IntVec2> Cells2D
		{
			get
			{
				CellRect.<get_Cells2D>d__38 <get_Cells2D>d__ = new CellRect.<get_Cells2D>d__38(-2);
				<get_Cells2D>d__.<>3__<>4__this = this;
				return <get_Cells2D>d__;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x000143A2 File Offset: 0x000125A2
		public readonly IEnumerable<IntVec3> EdgeCells
		{
			get
			{
				CellRect.<get_EdgeCells>d__40 <get_EdgeCells>d__ = new CellRect.<get_EdgeCells>d__40(-2);
				<get_EdgeCells>d__.<>3__<>4__this = this;
				return <get_EdgeCells>d__;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x000143B7 File Offset: 0x000125B7
		public IEnumerable<IntVec3> EdgeCellsNoCorners
		{
			get
			{
				CellRect.<get_EdgeCellsNoCorners>d__42 <get_EdgeCellsNoCorners>d__ = new CellRect.<get_EdgeCellsNoCorners>d__42(-2);
				<get_EdgeCellsNoCorners>d__.<>3__<>4__this = this;
				return <get_EdgeCellsNoCorners>d__;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x000143CC File Offset: 0x000125CC
		public int EdgeCellsCount
		{
			get
			{
				if (this.Area == 0)
				{
					return 0;
				}
				if (this.Area == 1)
				{
					return 1;
				}
				return this.Width * 2 + (this.Height - 2) * 2;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x000143F6 File Offset: 0x000125F6
		public IEnumerable<IntVec3> AdjacentCellsCardinal
		{
			get
			{
				CellRect.<get_AdjacentCellsCardinal>d__46 <get_AdjacentCellsCardinal>d__ = new CellRect.<get_AdjacentCellsCardinal>d__46(-2);
				<get_AdjacentCellsCardinal>d__.<>3__<>4__this = this;
				return <get_AdjacentCellsCardinal>d__;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0001440B File Offset: 0x0001260B
		public IEnumerable<IntVec3> AdjacentCells
		{
			get
			{
				CellRect.<get_AdjacentCells>d__48 <get_AdjacentCells>d__ = new CellRect.<get_AdjacentCells>d__48(-2);
				<get_AdjacentCells>d__.<>3__<>4__this = this;
				return <get_AdjacentCells>d__;
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00014420 File Offset: 0x00012620
		public IntVec3 FarthestPoint(IntVec3 startingPoint, Rot4 direction)
		{
			if (!this.Contains(startingPoint))
			{
				throw new ArgumentException(startingPoint.ToString());
			}
			switch (direction.AsInt)
			{
			case 0:
				return new IntVec3(startingPoint.x, 0, this.maxZ);
			case 1:
				return new IntVec3(this.maxX, 0, startingPoint.z);
			case 2:
				return new IntVec3(startingPoint.x, 0, this.minZ);
			case 3:
				return new IntVec3(this.minX, 0, startingPoint.z);
			default:
				throw new ArgumentException(direction.ToString());
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000144C7 File Offset: 0x000126C7
		public static bool operator ==(CellRect lhs, CellRect rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000144D1 File Offset: 0x000126D1
		public static bool operator !=(CellRect lhs, CellRect rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000144DD File Offset: 0x000126DD
		public CellRect(int minX, int minZ, int width, int height)
		{
			this.minX = minX;
			this.minZ = minZ;
			this.maxX = minX + width - 1;
			this.maxZ = minZ + height - 1;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00014504 File Offset: 0x00012704
		public static CellRect WholeMap(Map map)
		{
			return new CellRect(0, 0, map.Size.x, map.Size.z);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00014524 File Offset: 0x00012724
		public static CellRect FromLimits(int minX, int minZ, int maxX, int maxZ)
		{
			return new CellRect
			{
				minX = Mathf.Min(minX, maxX),
				minZ = Mathf.Min(minZ, maxZ),
				maxX = Mathf.Max(maxX, minX),
				maxZ = Mathf.Max(maxZ, minZ)
			};
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00014574 File Offset: 0x00012774
		public static CellRect FromLimits(IntVec3 first, IntVec3 second)
		{
			return new CellRect
			{
				minX = Mathf.Min(first.x, second.x),
				minZ = Mathf.Min(first.z, second.z),
				maxX = Mathf.Max(first.x, second.x),
				maxZ = Mathf.Max(first.z, second.z)
			};
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000145EC File Offset: 0x000127EC
		public static CellRect FromCellList(IEnumerable<IntVec3> cells)
		{
			CellRect cellRect = new CellRect
			{
				minX = int.MaxValue,
				minZ = int.MaxValue
			};
			foreach (IntVec3 intVec in cells)
			{
				if (intVec.x < cellRect.minX)
				{
					cellRect.minX = intVec.x;
				}
				if (intVec.z < cellRect.minZ)
				{
					cellRect.minZ = intVec.z;
				}
				if (intVec.x > cellRect.maxX)
				{
					cellRect.maxX = intVec.x;
				}
				if (intVec.z > cellRect.maxZ)
				{
					cellRect.maxZ = intVec.z;
				}
			}
			return cellRect;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000146BC File Offset: 0x000128BC
		public static CellRect CenteredOn(IntVec3 center, int radius)
		{
			return new CellRect
			{
				minX = center.x - radius,
				maxX = center.x + radius,
				minZ = center.z - radius,
				maxZ = center.z + radius
			};
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00014710 File Offset: 0x00012910
		public static CellRect CenteredOn(IntVec3 center, int width, int height)
		{
			CellRect cellRect = new CellRect
			{
				minX = center.x - width / 2,
				minZ = center.z - height / 2
			};
			cellRect.maxX = cellRect.minX + width - 1;
			cellRect.maxZ = cellRect.minZ + height - 1;
			return cellRect;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0001476C File Offset: 0x0001296C
		public static CellRect CenteredOn(IntVec3 center, IntVec2 size)
		{
			return CellRect.CenteredOn(center, size.x, size.z);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00014780 File Offset: 0x00012980
		public static CellRect ViewRect(Map map)
		{
			if (Current.ProgramState != ProgramState.Playing || Find.CurrentMap != map || WorldRendererUtility.WorldSelected)
			{
				return CellRect.Empty;
			}
			return Find.CameraDriver.CurrentViewRect;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x000147A9 File Offset: 0x000129A9
		public static CellRect SingleCell(IntVec3 c)
		{
			return new CellRect(c.x, c.z, 1, 1);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x000147BE File Offset: 0x000129BE
		public bool InBounds(Map map)
		{
			return this.minX >= 0 && this.minZ >= 0 && this.maxX < map.Size.x && this.maxZ < map.Size.z;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000147FC File Offset: 0x000129FC
		public bool FullyContainedWithin(CellRect within)
		{
			CellRect rhs = this;
			rhs.ClipInsideRect(within);
			return this == rhs;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00014828 File Offset: 0x00012A28
		public bool Overlaps(CellRect other)
		{
			return !this.IsEmpty && !other.IsEmpty && (this.minX <= other.maxX && this.maxX >= other.minX && this.maxZ >= other.minZ) && this.minZ <= other.maxZ;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00014888 File Offset: 0x00012A88
		public bool OverlapsCardinal(CellRect other)
		{
			if (this.IsEmpty || other.IsEmpty)
			{
				return false;
			}
			CellRect other2 = this;
			other2.minZ++;
			other2.maxZ--;
			CellRect other3 = this;
			other3.minX++;
			other3.maxX--;
			return other.Overlaps(other2) || other.Overlaps(other3);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000148FC File Offset: 0x00012AFC
		public bool Overlaps(Bounds other)
		{
			return !this.IsEmpty && !(other.size == Vector3.zero) && ((float)this.minX <= other.max.x && (float)this.maxX >= other.min.x && (float)this.maxZ >= other.min.z) && (float)this.minZ <= other.max.z;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00014980 File Offset: 0x00012B80
		public Bounds ToBounds()
		{
			return new Bounds
			{
				center = this.CenterVector3,
				size = new Vector3((float)this.Width, 1f, (float)this.Height)
			};
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x000149C4 File Offset: 0x00012BC4
		public bool IsOnEdge(IntVec3 c)
		{
			return (c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ) || (c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ) || (c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX) || (c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00014A84 File Offset: 0x00012C84
		public bool IsOnEdge(IntVec3 c, Rot4 rot)
		{
			if (rot == Rot4.West)
			{
				return c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ;
			}
			if (rot == Rot4.East)
			{
				return c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
			}
			if (rot == Rot4.South)
			{
				return c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX;
			}
			return c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00014B78 File Offset: 0x00012D78
		public bool IsOnEdge(IntVec3 c, int edgeWidth)
		{
			return this.Contains(c) && (c.x < this.minX + edgeWidth || c.z < this.minZ + edgeWidth || c.x >= this.maxX + 1 - edgeWidth || c.z >= this.maxZ + 1 - edgeWidth);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00014BDC File Offset: 0x00012DDC
		public bool IsCorner(IntVec3 c)
		{
			return (c.x == this.minX && c.z == this.minZ) || (c.x == this.maxX && c.z == this.minZ) || (c.x == this.minX && c.z == this.maxZ) || (c.x == this.maxX && c.z == this.maxZ);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00014C60 File Offset: 0x00012E60
		public IntVec3 ClosestCorner(IntVec3 cell)
		{
			IntVec3 corner = this.GetCorner(Rot4.North, true);
			IntVec3 corner2 = this.GetCorner(Rot4.East, true);
			IntVec3 corner3 = this.GetCorner(Rot4.South, true);
			IntVec3 corner4 = this.GetCorner(Rot4.West, true);
			IntVec3 intVec = cell - corner;
			IntVec3 intVec2 = cell - corner2;
			IntVec3 intVec3 = cell - corner3;
			IntVec3 intVec4 = cell - corner4;
			return GenMath.MinBy<IntVec3>(corner, intVec.SqrMagnitude, corner2, intVec2.SqrMagnitude, corner3, intVec3.SqrMagnitude, corner4, intVec4.SqrMagnitude);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00014CEC File Offset: 0x00012EEC
		public Rot4 GetClosestEdge(IntVec3 c)
		{
			int num = Mathf.Abs(c.x - this.minX);
			int num2 = Mathf.Abs(c.x - this.maxX);
			int num3 = Mathf.Abs(c.z - this.maxZ);
			int num4 = Mathf.Abs(c.z - this.minZ);
			return GenMath.MinBy<Rot4>(Rot4.West, (float)num, Rot4.East, (float)num2, Rot4.North, (float)num3, Rot4.South, (float)num4);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00014D68 File Offset: 0x00012F68
		public Rot4 GetEdgeCellRot(IntVec3 c)
		{
			if (c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ)
			{
				return Rot4.East;
			}
			if (c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ)
			{
				return Rot4.West;
			}
			if (c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX)
			{
				return Rot4.North;
			}
			if (c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX)
			{
				return Rot4.South;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(79, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Attempted to get edge cell rotation but cell was not on an edge, cell: ");
			defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(c);
			defaultInterpolatedStringHandler.AppendLiteral(", rect: ");
			defaultInterpolatedStringHandler.AppendFormatted(this.ToString());
			Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			return Rot4.North;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00014E83 File Offset: 0x00013083
		public bool IsDiagonalCorner(IntVec3 c)
		{
			return (c.x == this.minX || c.x == this.maxX) && (c.z == this.minZ || c.z == this.maxZ);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00014EC1 File Offset: 0x000130C1
		public IntVec3 GetCenterCellOnEdge(Rot4 rot)
		{
			return this.GetCellOnEdge(rot, this.CenterCell);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00014ED0 File Offset: 0x000130D0
		public IntVec3 GetCellOnEdge(Rot4 rot, IntVec3 point)
		{
			if (rot == Rot4.North)
			{
				return new IntVec3(point.x, point.y, this.maxZ);
			}
			if (rot == Rot4.East)
			{
				return new IntVec3(this.maxX, point.y, point.z);
			}
			if (rot == Rot4.South)
			{
				return new IntVec3(point.x, point.y, this.minZ);
			}
			if (rot == Rot4.West)
			{
				return new IntVec3(this.minX, point.y, point.z);
			}
			return IntVec3.Invalid;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00014F78 File Offset: 0x00013178
		public IntVec3 GetCenterCellOnEdge(Rot4 rot, int offset)
		{
			if (rot == Rot4.North)
			{
				return new IntVec3(this.CenterCell.x + offset, this.CenterCell.y, this.maxZ);
			}
			if (rot == Rot4.East)
			{
				return new IntVec3(this.maxX, this.CenterCell.y, this.CenterCell.z + offset);
			}
			if (rot == Rot4.South)
			{
				return new IntVec3(this.CenterCell.x + offset, this.CenterCell.y, this.minZ);
			}
			if (rot == Rot4.West)
			{
				return new IntVec3(this.minX, this.CenterCell.y, this.CenterCell.z + offset);
			}
			return IntVec3.Invalid;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0001504E File Offset: 0x0001324E
		public IEnumerable<IntVec3> GetCenterCellsOnEdge(Rot4 rot, int range)
		{
			CellRect.<GetCenterCellsOnEdge>d__79 <GetCenterCellsOnEdge>d__ = new CellRect.<GetCenterCellsOnEdge>d__79(-2);
			<GetCenterCellsOnEdge>d__.<>3__<>4__this = this;
			<GetCenterCellsOnEdge>d__.<>3__rot = rot;
			<GetCenterCellsOnEdge>d__.<>3__range = range;
			return <GetCenterCellsOnEdge>d__;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00015071 File Offset: 0x00013271
		public IEnumerable<IntVec3> GetCellsCenterOutOnEdge(Rot4 rot)
		{
			return this.GetCenterCellsOnEdge(rot, this.GetSideLength(rot));
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00015081 File Offset: 0x00013281
		public int GetSideLength(Rot4 rot)
		{
			if (rot == Rot4.North || rot == Rot4.South)
			{
				return this.Width;
			}
			return this.Height;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000150AA File Offset: 0x000132AA
		public IEnumerable<IntVec3> GetCellsOnEdge(Rot4 rot)
		{
			CellRect.<GetCellsOnEdge>d__82 <GetCellsOnEdge>d__ = new CellRect.<GetCellsOnEdge>d__82(-2);
			<GetCellsOnEdge>d__.<>3__<>4__this = this;
			<GetCellsOnEdge>d__.<>3__rot = rot;
			return <GetCellsOnEdge>d__;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000150C8 File Offset: 0x000132C8
		public CellRect ClipInsideMap(Map map)
		{
			if (this.minX < 0)
			{
				this.minX = 0;
			}
			if (this.minZ < 0)
			{
				this.minZ = 0;
			}
			if (this.maxX > map.Size.x - 1)
			{
				this.maxX = map.Size.x - 1;
			}
			if (this.maxZ > map.Size.z - 1)
			{
				this.maxZ = map.Size.z - 1;
			}
			return this;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0001514C File Offset: 0x0001334C
		public CellRect ClipInsideRect(CellRect otherRect)
		{
			if (this.minX < otherRect.minX)
			{
				this.minX = otherRect.minX;
			}
			if (this.maxX > otherRect.maxX)
			{
				this.maxX = otherRect.maxX;
			}
			if (this.minZ < otherRect.minZ)
			{
				this.minZ = otherRect.minZ;
			}
			if (this.maxZ > otherRect.maxZ)
			{
				this.maxZ = otherRect.maxZ;
			}
			return this;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000151C8 File Offset: 0x000133C8
		public Rect ToScreenRect(Camera camera)
		{
			Vector3 vector = camera.WorldToScreenPoint(this.Min.ToVector3());
			Vector3 vector2 = camera.WorldToScreenPoint(this.Max.ToVector3() + new Vector3(1f, 0f, 1f));
			return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00015238 File Offset: 0x00013438
		[return: TupleElementNames(new string[]
		{
			"bottom",
			"up"
		})]
		public ValueTuple<CellRect, CellRect> SplitVertical(int separation = 0)
		{
			IntVec3 centerCell = this.CenterCell;
			CellRect item = this;
			CellRect item2 = this;
			item.maxZ = centerCell.z - separation;
			item2.minZ = centerCell.z + separation;
			return new ValueTuple<CellRect, CellRect>(item, item2);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00015280 File Offset: 0x00013480
		[return: TupleElementNames(new string[]
		{
			"left",
			"right"
		})]
		public ValueTuple<CellRect, CellRect> SplitHorizontal(int separation = 0)
		{
			IntVec3 centerCell = this.CenterCell;
			CellRect item = this;
			CellRect item2 = this;
			item.maxX = centerCell.x - separation;
			item2.minX = centerCell.x + separation;
			return new ValueTuple<CellRect, CellRect>(item, item2);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000152C8 File Offset: 0x000134C8
		[return: TupleElementNames(new string[]
		{
			"leftBottom",
			"leftUp",
			"rightBottom",
			"rightUp"
		})]
		public ValueTuple<CellRect, CellRect, CellRect, CellRect> Subdivide(int separation)
		{
			ValueTuple<CellRect, CellRect> valueTuple = this.SplitHorizontal(separation);
			CellRect item = valueTuple.Item1;
			CellRect item2 = valueTuple.Item2;
			ValueTuple<CellRect, CellRect> valueTuple2 = item.SplitVertical(separation);
			CellRect item3 = valueTuple2.Item1;
			CellRect item4 = valueTuple2.Item2;
			ValueTuple<CellRect, CellRect> valueTuple3 = item2.SplitVertical(separation);
			CellRect item5 = valueTuple3.Item1;
			CellRect item6 = valueTuple3.Item2;
			return new ValueTuple<CellRect, CellRect, CellRect, CellRect>(item3, item4, item5, item6);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00015320 File Offset: 0x00013520
		public List<CellRect> Subdivide(int times, int separation)
		{
			List<CellRect> list = new List<CellRect>
			{
				this
			};
			for (int i = 0; i < times; i++)
			{
				int count = list.Count;
				for (int j = 0; j < count; j++)
				{
					ValueTuple<CellRect, CellRect, CellRect, CellRect> valueTuple = list[j].Subdivide(separation);
					CellRect item = valueTuple.Item1;
					CellRect item2 = valueTuple.Item2;
					CellRect item3 = valueTuple.Item3;
					CellRect item4 = valueTuple.Item4;
					list.Add(item);
					list.Add(item2);
					list.Add(item3);
					list.Add(item4);
				}
				list.RemoveRange(0, count);
			}
			return list;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000153B8 File Offset: 0x000135B8
		public void SubdivideToMaxLength(int size, List<CellRect> rects)
		{
			rects.Clear();
			rects.Add(this);
			for (int i = 0; i < rects.Count; i++)
			{
				CellRect cellRect = rects[i];
				while (cellRect.Width > size)
				{
					rects.RemoveAt(i);
					ValueTuple<CellRect, CellRect> valueTuple = cellRect.SplitHorizontal(1);
					CellRect item = valueTuple.Item1;
					CellRect item2 = valueTuple.Item2;
					cellRect = item;
					rects.Add(item);
					rects.Add(item2);
				}
				while (cellRect.Height > size)
				{
					rects.RemoveAt(i);
					ValueTuple<CellRect, CellRect> valueTuple2 = cellRect.SplitVertical(1);
					CellRect item3 = valueTuple2.Item1;
					CellRect item4 = valueTuple2.Item2;
					cellRect = item3;
					rects.Add(item3);
					rects.Add(item4);
				}
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0001546C File Offset: 0x0001366C
		public bool TryContractToRemove(IntVec3 point, out CellRect rect)
		{
			rect = this;
			IntVec3 centerCell = this.CenterCell;
			if (!(point == centerCell))
			{
				if (point.x <= this.maxX && point.x >= this.minX)
				{
					if (point.x >= centerCell.x)
					{
						rect.maxX = point.x - 1;
					}
					else
					{
						rect.minX = point.x + 1;
					}
				}
				if (point.z <= this.maxZ && point.z >= this.minZ)
				{
					if (point.z >= centerCell.z)
					{
						rect.maxZ = point.z - 1;
					}
					else
					{
						rect.minZ = point.z + 1;
					}
				}
				return true;
			}
			if (this.Width < 3 && this.Height < 3)
			{
				rect = default(CellRect);
				return false;
			}
			if (this.Width > this.Height)
			{
				if (this.minX < point.x - 1)
				{
					rect.maxX = point.x - 1;
				}
				else
				{
					rect.minX = point.x + 1;
				}
			}
			else if (this.minZ < point.z - 1)
			{
				rect.maxZ = point.z - 1;
			}
			else
			{
				rect.minZ = point.z + 1;
			}
			return true;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000155B4 File Offset: 0x000137B4
		public readonly bool Contains(IntVec3 c)
		{
			return c.x >= this.minX && c.x <= this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x000155F3 File Offset: 0x000137F3
		public readonly bool InlineWith(CellRect other)
		{
			return (this.minX == other.minX && this.maxX == other.maxX) || (this.minZ == other.minZ && this.maxZ == other.maxZ);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00015634 File Offset: 0x00013834
		public IntVec3 GetCorner(Rot4 rot, bool clockwise = true)
		{
			if (rot == Rot4.North)
			{
				return new IntVec3(clockwise ? this.maxX : this.minX, 0, this.maxZ);
			}
			if (rot == Rot4.East)
			{
				return new IntVec3(clockwise ? this.maxX : this.minX, 0, this.minZ);
			}
			if (rot == Rot4.South)
			{
				return new IntVec3(clockwise ? this.minX : this.maxX, 0, this.minZ);
			}
			if (rot == Rot4.West)
			{
				return new IntVec3(clockwise ? this.minX : this.maxX, 0, this.maxZ);
			}
			return IntVec3.Invalid;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000156F4 File Offset: 0x000138F4
		public void GetInternalCorners(out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			BL = new IntVec3(this.minX, 0, this.minZ);
			TL = new IntVec3(this.minX, 0, this.maxZ);
			TR = new IntVec3(this.maxX, 0, this.maxZ);
			BR = new IntVec3(this.maxX, 0, this.minZ);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00015764 File Offset: 0x00013964
		public void GetAdjacentCorners(out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			BL = new IntVec3(this.minX - 1, 0, this.minZ - 1);
			TL = new IntVec3(this.minX - 1, 0, this.maxZ + 1);
			TR = new IntVec3(this.maxX + 1, 0, this.maxZ + 1);
			BR = new IntVec3(this.maxX + 1, 0, this.minZ - 1);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000157E4 File Offset: 0x000139E4
		public float ClosestDistSquaredTo(IntVec3 c)
		{
			if (this.Contains(c))
			{
				return 0f;
			}
			if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					return (float)(c - new IntVec3(this.minX, 0, this.minZ)).LengthHorizontalSquared;
				}
				if (c.z > this.maxZ)
				{
					return (float)(c - new IntVec3(this.minX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				return (float)((this.minX - c.x) * (this.minX - c.x));
			}
			else if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					return (float)(c - new IntVec3(this.maxX, 0, this.minZ)).LengthHorizontalSquared;
				}
				if (c.z > this.maxZ)
				{
					return (float)(c - new IntVec3(this.maxX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				return (float)((c.x - this.maxX) * (c.x - this.maxX));
			}
			else
			{
				if (c.z < this.minZ)
				{
					return (float)((this.minZ - c.z) * (this.minZ - c.z));
				}
				return (float)((c.z - this.maxZ) * (c.z - this.maxZ));
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0001595D File Offset: 0x00013B5D
		public float ClosestDistanceTo(IntVec3 c)
		{
			return this.ClosestCellTo(c).DistanceTo(c);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0001596C File Offset: 0x00013B6C
		public float DistanceToEdge(IntVec3 v)
		{
			return (float)Mathf.Max(Mathf.Min(Mathf.Min(Mathf.Min(v.x, v.z), this.Width - v.x - 1), this.Height - v.z - 1), 0);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000159BC File Offset: 0x00013BBC
		public IntVec3 ClosestCellTo(IntVec3 c)
		{
			if (this.Contains(c))
			{
				return c;
			}
			if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(this.minX, 0, this.minZ);
				}
				if (c.z > this.maxZ)
				{
					return new IntVec3(this.minX, 0, this.maxZ);
				}
				return new IntVec3(this.minX, 0, c.z);
			}
			else if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(this.maxX, 0, this.minZ);
				}
				if (c.z > this.maxZ)
				{
					return new IntVec3(this.maxX, 0, this.maxZ);
				}
				return new IntVec3(this.maxX, 0, c.z);
			}
			else
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(c.x, 0, this.minZ);
				}
				return new IntVec3(c.x, 0, this.maxZ);
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00015AD0 File Offset: 0x00013CD0
		public bool InNoBuildEdgeArea(Map map)
		{
			return !this.IsEmpty && (this.minX < 10 || this.minZ < 10 || this.maxX >= map.Size.x - 10 || this.maxZ >= map.Size.z - 10);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00015B2C File Offset: 0x00013D2C
		public IEnumerable<IntVec3> GetEdgeCells(Rot4 dir)
		{
			CellRect.<GetEdgeCells>d__102 <GetEdgeCells>d__ = new CellRect.<GetEdgeCells>d__102(-2);
			<GetEdgeCells>d__.<>3__<>4__this = this;
			<GetEdgeCells>d__.<>3__dir = dir;
			return <GetEdgeCells>d__;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00015B48 File Offset: 0x00013D48
		public readonly CellRect GetEdgeRect(Rot4 rot)
		{
			if (rot == Rot4.North)
			{
				return CellRect.FromLimits(new IntVec3(this.minX, 0, this.maxZ), new IntVec3(this.maxX, 0, this.maxZ));
			}
			if (rot == Rot4.South)
			{
				return CellRect.FromLimits(new IntVec3(this.minX, 0, this.minZ), new IntVec3(this.maxX, 0, this.minZ));
			}
			if (rot == Rot4.West)
			{
				return CellRect.FromLimits(new IntVec3(this.minX, 0, this.minZ), new IntVec3(this.minX, 0, this.maxZ));
			}
			return CellRect.FromLimits(new IntVec3(this.maxX, 0, this.minZ), new IntVec3(this.maxX, 0, this.maxZ));
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00015C24 File Offset: 0x00013E24
		public bool TryFindRandomInnerRectTouchingEdge(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.EdgeCells.Where(delegate(IntVec3 x)
			{
				if (predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return predicate(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00015D3C File Offset: 0x00013F3C
		public readonly bool TryFindRandomInnerRectTouchingCorner(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.Corners.Where(delegate(IntVec3 x)
			{
				if (predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return predicate(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00015E54 File Offset: 0x00014054
		public readonly bool TryFindRandomCell(out IntVec3 cell, Predicate<IntVec3> validator = null)
		{
			foreach (IntVec3 intVec in this.Cells.InRandomOrder(null))
			{
				if (validator == null || validator(intVec))
				{
					cell = intVec;
					return true;
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00015EC8 File Offset: 0x000140C8
		public readonly bool TryFindRandomInnerRect(IntVec2 size, out CellRect rect, Predicate<CellRect> validator = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.Cells.Where(delegate(IntVec3 x)
			{
				if (validator == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return validator(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00015FE0 File Offset: 0x000141E0
		public readonly bool TryFindNearestInnerRectTo(IntVec2 size, IntVec3 position, out CellRect rect)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			CellRect cellRect2 = CellRect.Empty;
			float num = float.MaxValue;
			foreach (IntVec3 intVec in cellRect.Cells)
			{
				CellRect cellRect3 = new CellRect(intVec.x, intVec.z, size.x, size.z);
				int num2 = position.DistanceToSquared(cellRect3.CenterCell);
				if ((float)num2 < num)
				{
					cellRect2 = cellRect3;
					num = (float)num2;
				}
			}
			rect = cellRect2;
			return true;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000160F8 File Offset: 0x000142F8
		public readonly CellRect ExpandedBy(int dist)
		{
			CellRect result = this;
			result.minX -= dist;
			result.minZ -= dist;
			result.maxX += dist;
			result.maxZ += dist;
			return result;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00016140 File Offset: 0x00014340
		public readonly CellRect ExpandedBy(int x, int z)
		{
			CellRect result = this;
			result.minX -= x;
			result.minZ -= z;
			result.maxX += x;
			result.maxZ += z;
			return result;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00016188 File Offset: 0x00014388
		public readonly CellRect ExpandedBy(IntVec3 offset)
		{
			CellRect result = this;
			result.minX -= offset.x;
			result.minZ -= offset.z;
			result.maxX += offset.x;
			result.maxZ += offset.z;
			return result;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000161E4 File Offset: 0x000143E4
		public readonly CellRect MaxExpandedBy(int dist)
		{
			CellRect result = this;
			result.maxX += dist;
			result.maxZ += dist;
			return result;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00016214 File Offset: 0x00014414
		public readonly CellRect MaxExpandedBy(int x, int z)
		{
			CellRect result = this;
			result.maxX += x;
			result.maxZ += z;
			return result;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00016244 File Offset: 0x00014444
		public readonly CellRect MaxExpandedBy(IntVec3 offset)
		{
			CellRect result = this;
			result.maxX += offset.x;
			result.maxZ += offset.z;
			return result;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0001627C File Offset: 0x0001447C
		public readonly CellRect MinExpandedBy(int dist)
		{
			CellRect result = this;
			result.minX -= dist;
			result.minZ -= dist;
			return result;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000162AC File Offset: 0x000144AC
		public readonly CellRect MinExpandedBy(int x, int z)
		{
			CellRect result = this;
			result.minX -= x;
			result.minZ -= z;
			return result;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000162DC File Offset: 0x000144DC
		public readonly CellRect MinExpandedBy(IntVec3 offset)
		{
			CellRect result = this;
			result.minX -= offset.x;
			result.minZ -= offset.z;
			return result;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00016313 File Offset: 0x00014513
		public readonly CellRect ContractedBy(int x, int z)
		{
			return this.ExpandedBy(new IntVec3(-x, 0, -z));
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00016325 File Offset: 0x00014525
		public readonly CellRect ContractedBy(IntVec3 dist)
		{
			return this.ExpandedBy(-dist);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00016333 File Offset: 0x00014533
		public readonly CellRect ContractedBy(int dist)
		{
			return this.ExpandedBy(-dist);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00016340 File Offset: 0x00014540
		public CellRect ContractedBy(int x, int z, int negativeX, int negativeZ)
		{
			CellRect result = this;
			result.minX += x;
			result.minZ += z;
			result.maxX -= negativeX;
			result.maxZ -= negativeZ;
			return result;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00016386 File Offset: 0x00014586
		public CellRect MovedBy(int x, int z)
		{
			return this.MovedBy(new IntVec3(x, 0, z));
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00016396 File Offset: 0x00014596
		public CellRect MovedBy(IntVec2 offset)
		{
			return this.MovedBy(offset.ToIntVec3);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000163A8 File Offset: 0x000145A8
		public CellRect MovedBy(IntVec3 offset)
		{
			CellRect result = this;
			result.minX += offset.x;
			result.minZ += offset.z;
			result.maxX += offset.x;
			result.maxZ += offset.z;
			return result;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00016404 File Offset: 0x00014604
		public CellRect Encapsulate(CellRect otherRect)
		{
			CellRect result = this;
			result.minX = Mathf.Min(this.minX, otherRect.minX);
			result.minZ = Mathf.Min(this.minZ, otherRect.minZ);
			result.maxX = Mathf.Max(this.maxX, otherRect.maxX);
			result.maxZ = Mathf.Max(this.maxZ, otherRect.maxZ);
			return result;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0001647C File Offset: 0x0001467C
		public CellRect Encapsulate(IntVec3 point)
		{
			CellRect result = this;
			result.minX = Mathf.Min(this.minX, point.x);
			result.minZ = Mathf.Min(this.minZ, point.z);
			result.maxX = Mathf.Max(this.maxX, point.x);
			result.maxZ = Mathf.Max(this.maxZ, point.z);
			return result;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x000164F1 File Offset: 0x000146F1
		public IEnumerable<IntVec3> UnionCells(CellRect container)
		{
			CellRect.<UnionCells>d__127 <UnionCells>d__ = new CellRect.<UnionCells>d__127(-2);
			<UnionCells>d__.<>3__<>4__this = this;
			<UnionCells>d__.<>3__container = container;
			return <UnionCells>d__;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0001650D File Offset: 0x0001470D
		public IEnumerable<IntVec3> DifferenceCells(CellRect container)
		{
			CellRect.<DifferenceCells>d__128 <DifferenceCells>d__ = new CellRect.<DifferenceCells>d__128(-2);
			<DifferenceCells>d__.<>3__<>4__this = this;
			<DifferenceCells>d__.<>3__container = container;
			return <DifferenceCells>d__;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00016529 File Offset: 0x00014729
		public int IndexOf(IntVec3 location)
		{
			return location.x - this.minX + (location.z - this.minZ) * this.Width;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00016550 File Offset: 0x00014750
		public void DebugDraw()
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Vector3 vector = new Vector3((float)this.minX, y, (float)this.minZ);
			Vector3 vector2 = new Vector3((float)this.minX, y, (float)(this.maxZ + 1));
			Vector3 vector3 = new Vector3((float)(this.maxX + 1), y, (float)(this.maxZ + 1));
			Vector3 vector4 = new Vector3((float)(this.maxX + 1), y, (float)this.minZ);
			GenDraw.DrawLineBetween(vector, vector2);
			GenDraw.DrawLineBetween(vector2, vector3);
			GenDraw.DrawLineBetween(vector3, vector4);
			GenDraw.DrawLineBetween(vector4, vector);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000165E3 File Offset: 0x000147E3
		public CellRect.Enumerator GetEnumerator()
		{
			return new CellRect.Enumerator(this);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000165F0 File Offset: 0x000147F0
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000165FD File Offset: 0x000147FD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001660C File Offset: 0x0001480C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.minX.ToString(),
				",",
				this.minZ.ToString(),
				",",
				this.maxX.ToString(),
				",",
				this.maxZ.ToString(),
				")"
			});
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00016685 File Offset: 0x00014885
		public void Reset()
		{
			this = default(CellRect);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00016690 File Offset: 0x00014890
		public static CellRect FromString(string str)
		{
			str = str.TrimStart('(');
			str = str.TrimEnd(')');
			string[] array = str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			int num = Convert.ToInt32(array[0], invariantCulture);
			int num2 = Convert.ToInt32(array[1], invariantCulture);
			int num3 = Convert.ToInt32(array[2], invariantCulture);
			int num4 = Convert.ToInt32(array[3], invariantCulture);
			return new CellRect(num, num2, num3 - num + 1, num4 - num2 + 1);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x000166FA File Offset: 0x000148FA
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.minX), this.maxX), this.minZ), this.maxZ);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0001672C File Offset: 0x0001492C
		public override bool Equals(object obj)
		{
			if (obj is CellRect)
			{
				CellRect other = (CellRect)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00016753 File Offset: 0x00014953
		public bool Equals(CellRect other)
		{
			return this.minX == other.minX && this.maxX == other.maxX && this.minZ == other.minZ && this.maxZ == other.maxZ;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0001678F File Offset: 0x0001498F
		public static CellRect FromCell(IntVec3 cell)
		{
			return new CellRect(cell.x, cell.z, 1, 1);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000167A4 File Offset: 0x000149A4
		public readonly IEnumerable<CellRect> EnumerateRectanglesCovering(Predicate<IntVec3> validator)
		{
			CellRect.<EnumerateRectanglesCovering>d__143 <EnumerateRectanglesCovering>d__ = new CellRect.<EnumerateRectanglesCovering>d__143(-2);
			<EnumerateRectanglesCovering>d__.<>3__<>4__this = this;
			<EnumerateRectanglesCovering>d__.<>3__validator = validator;
			return <EnumerateRectanglesCovering>d__;
		}

		// Token: 0x0400016C RID: 364
		public int minX;

		// Token: 0x0400016D RID: 365
		public int maxX;

		// Token: 0x0400016E RID: 366
		public int minZ;

		// Token: 0x0400016F RID: 367
		public int maxZ;

		// Token: 0x04000170 RID: 368
		private static readonly HashSet<IntVec3> tmpUnvistedCells = new HashSet<IntVec3>();

		// Token: 0x02002482 RID: 9346
		public struct Enumerator : IEnumerator<IntVec3>, IEnumerator, IDisposable
		{
			// Token: 0x17002956 RID: 10582
			// (get) Token: 0x0600E9D9 RID: 59865 RVA: 0x0056B3FB File Offset: 0x005695FB
			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x17002957 RID: 10583
			// (get) Token: 0x0600E9DA RID: 59866 RVA: 0x0056B40F File Offset: 0x0056960F
			object IEnumerator.Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x0600E9DB RID: 59867 RVA: 0x0056B428 File Offset: 0x00569628
			public Enumerator(CellRect ir)
			{
				this.ir = ir;
				this.x = ir.minX - 1;
				this.z = ir.minZ;
			}

			// Token: 0x0600E9DC RID: 59868 RVA: 0x0056B44C File Offset: 0x0056964C
			public bool MoveNext()
			{
				this.x++;
				if (this.x > this.ir.maxX)
				{
					this.x = this.ir.minX;
					this.z++;
				}
				return this.z <= this.ir.maxZ;
			}

			// Token: 0x0600E9DD RID: 59869 RVA: 0x0056B4AF File Offset: 0x005696AF
			public void Reset()
			{
				this.x = this.ir.minX - 1;
				this.z = this.ir.minZ;
			}

			// Token: 0x0600E9DE RID: 59870 RVA: 0x0056B4D5 File Offset: 0x005696D5
			void IDisposable.Dispose()
			{
			}

			// Token: 0x04009518 RID: 38168
			private CellRect ir;

			// Token: 0x04009519 RID: 38169
			private int x;

			// Token: 0x0400951A RID: 38170
			private int z;
		}
	}
}
