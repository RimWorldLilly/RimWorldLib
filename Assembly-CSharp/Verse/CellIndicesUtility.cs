using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x0200068E RID: 1678
	public static class CellIndicesUtility
	{
		// Token: 0x06003509 RID: 13577 RVA: 0x00158EA2 File Offset: 0x001570A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int CellToIndex(IntVec3 c, int sizeX)
		{
			return c.z * sizeX + c.x;
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x00158EB3 File Offset: 0x001570B3
		public static int CellToIndex(int x, int z, int sizeX)
		{
			return z * sizeX + x;
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x00158EBC File Offset: 0x001570BC
		public static IntVec3 IndexToCell(int ind, int sizeX)
		{
			int newX = ind % sizeX;
			int newZ = ind / sizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}
