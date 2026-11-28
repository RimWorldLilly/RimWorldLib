using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Verse
{
	// Token: 0x0200035E RID: 862
	public static class CellConnectionExtensions
	{
		// Token: 0x06001A6C RID: 6764 RVA: 0x000A1A70 File Offset: 0x0009FC70
		public static IntVec3 Offset(this CellConnection connection)
		{
			if (connection == CellConnection.Self)
			{
				return IntVec3.Zero;
			}
			if (math.countbits((int)connection) != 1)
			{
				throw new ArgumentException("connection");
			}
			return CellConnectionExtensions.OffsetFromBitIndex[connection.BitIndex()];
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x000A1AA0 File Offset: 0x0009FCA0
		public static CellConnection Inverse(this CellConnection connection)
		{
			if (connection <= CellConnection.NorthEast)
			{
				switch (connection)
				{
				case CellConnection.North:
					return CellConnection.South;
				case CellConnection.South:
					return CellConnection.North;
				case CellConnection.North | CellConnection.South:
					break;
				case CellConnection.East:
					return CellConnection.West;
				default:
					if (connection == CellConnection.West)
					{
						return CellConnection.East;
					}
					if (connection == CellConnection.NorthEast)
					{
						return CellConnection.SouthWest;
					}
					break;
				}
			}
			else
			{
				if (connection == CellConnection.SouthEast)
				{
					return CellConnection.NorthWest;
				}
				if (connection == CellConnection.SouthWest)
				{
					return CellConnection.NorthEast;
				}
				if (connection == CellConnection.NorthWest)
				{
					return CellConnection.SouthEast;
				}
			}
			throw new ArgumentException("connection");
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x000A1B0A File Offset: 0x0009FD0A
		public static CellConnectionEnumerator GetEnumerator(this CellConnection connections)
		{
			return new CellConnectionEnumerator(connections);
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x000A1B14 File Offset: 0x0009FD14
		public static CellConnection? ConnectionFromCells(IntVec3 first, IntVec3 second)
		{
			IntVec3 b = second - first;
			for (int i = 0; i < CellConnectionExtensions.OffsetFromBitIndex.Length; i++)
			{
				if (CellConnectionExtensions.OffsetFromBitIndex[i] == b)
				{
					return new CellConnection?(CellConnectionExtensions.FlagFromBitIndex[i]);
				}
			}
			return null;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x000A1B64 File Offset: 0x0009FD64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Diagonal(this CellConnection connection)
		{
			return (connection & CellConnection.DiagonalNeighbors) > CellConnection.North;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x000A1B70 File Offset: 0x0009FD70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Cardinal(this CellConnection connection)
		{
			return (connection & CellConnection.CardinalNeighbors) > CellConnection.North;
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x000A1B79 File Offset: 0x0009FD79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int BitIndex(this CellConnection connection)
		{
			return math.tzcnt((uint)connection);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x000A1B81 File Offset: 0x0009FD81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int BitLoopEndIndex(this CellConnection connections)
		{
			return 8 - (math.lzcnt((uint)connections) - 8) % 8;
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x000A1B8F File Offset: 0x0009FD8F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasBit(this CellConnection connections, CellConnection testFlag)
		{
			return (connections & testFlag) > CellConnection.Self;
		}

		// Token: 0x0400158B RID: 5515
		public static readonly IntVec3[] OffsetFromBitIndex = new IntVec3[]
		{
			new IntVec3(0, 0, 1),
			new IntVec3(0, 0, -1),
			new IntVec3(1, 0, 0),
			new IntVec3(-1, 0, 0),
			new IntVec3(1, 0, 1),
			new IntVec3(1, 0, -1),
			new IntVec3(-1, 0, -1),
			new IntVec3(-1, 0, 1)
		};

		// Token: 0x0400158C RID: 5516
		public static readonly CellConnection[] FlagFromBitIndex = new CellConnection[]
		{
			CellConnection.North,
			CellConnection.South,
			CellConnection.East,
			CellConnection.West,
			CellConnection.NorthEast,
			CellConnection.SouthEast,
			CellConnection.SouthWest,
			CellConnection.NorthWest
		};

		// Token: 0x0400158D RID: 5517
		public static readonly CellConnection[] InverseFromBitIndex = new CellConnection[]
		{
			CellConnection.South,
			CellConnection.North,
			CellConnection.West,
			CellConnection.East,
			CellConnection.SouthWest,
			CellConnection.NorthWest,
			CellConnection.NorthEast,
			CellConnection.SouthEast
		};
	}
}
