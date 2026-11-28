using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x0200068D RID: 1677
	public readonly struct CellIndices
	{
		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060034FD RID: 13565 RVA: 0x00158DE6 File Offset: 0x00156FE6
		public int NumGridCells
		{
			get
			{
				return this.sizeX * this.sizeZ;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060034FE RID: 13566 RVA: 0x00158DF5 File Offset: 0x00156FF5
		public int SizeX
		{
			get
			{
				return this.sizeX;
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x060034FF RID: 13567 RVA: 0x00158DFD File Offset: 0x00156FFD
		public int SizeZ
		{
			get
			{
				return this.sizeZ;
			}
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x00158E05 File Offset: 0x00157005
		public CellIndices(Map map)
		{
			this.sizeX = map.Size.x;
			this.sizeZ = map.Size.z;
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x00158E29 File Offset: 0x00157029
		public CellIndices(int sizeX, int sizeZ)
		{
			this.sizeX = sizeX;
			this.sizeZ = sizeZ;
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x00158E39 File Offset: 0x00157039
		public bool Contains(IntVec3 c)
		{
			return this.Contains(CellIndicesUtility.CellToIndex(c, this.sizeX));
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x00158E4D File Offset: 0x0015704D
		public bool Contains(int ind)
		{
			return ind >= 0 && ind < this.sizeX * this.sizeZ;
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x00158E65 File Offset: 0x00157065
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.sizeX);
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x00158E73 File Offset: 0x00157073
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.sizeX);
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x00158E82 File Offset: 0x00157082
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.sizeX);
		}

		// Token: 0x170009E2 RID: 2530
		public int this[IntVec3 cell]
		{
			get
			{
				return this.CellToIndex(cell);
			}
		}

		// Token: 0x170009E3 RID: 2531
		public IntVec3 this[int index]
		{
			get
			{
				return this.IndexToCell(index);
			}
		}

		// Token: 0x04002276 RID: 8822
		private readonly int sizeX;

		// Token: 0x04002277 RID: 8823
		private readonly int sizeZ;
	}
}
