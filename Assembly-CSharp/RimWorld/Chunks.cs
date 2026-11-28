using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001291 RID: 4753
	public class Chunks
	{
		// Token: 0x170013EC RID: 5100
		public CellRect this[IntVec3 cell]
		{
			get
			{
				return this.CellToRect(cell);
			}
		}

		// Token: 0x170013ED RID: 5101
		public CellRect this[int index]
		{
			get
			{
				return this.data[index];
			}
		}

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x060075DC RID: 30172 RVA: 0x002974EE File Offset: 0x002956EE
		public int UsedCount
		{
			get
			{
				return this.usedCount;
			}
		}

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x060075DD RID: 30173 RVA: 0x002974F6 File Offset: 0x002956F6
		public int ApproximateArea
		{
			get
			{
				return this.usedCount * this.regionSize * this.regionSize;
			}
		}

		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x060075DE RID: 30174 RVA: 0x0029750C File Offset: 0x0029570C
		public IReadOnlyList<CellRect> EnumeratedRects
		{
			get
			{
				if (this.enumerated.Empty<CellRect>())
				{
					this.RecacheEnumeratedChunks();
				}
				return this.enumerated;
			}
		}

		// Token: 0x060075DF RID: 30175 RVA: 0x00297528 File Offset: 0x00295728
		public Chunks(CellRect rect, int regionSize)
		{
			int subdivisionCount = MapGenUtility.GetSubdivisionCount(rect, ref regionSize);
			this.regionSize = regionSize;
			this.chunksX = 1;
			for (int i = 0; i < subdivisionCount; i++)
			{
				this.chunksX *= 2;
			}
			this.data = new CellRect[this.chunksX * this.chunksX];
			this.used.EnsureCapacity(this.data.Length);
			this.rect = new CellRect(0, 0, this.chunksX, this.chunksX);
			for (int j = 0; j < this.data.Length; j++)
			{
				IntVec3 a = this.IndexToCell(j);
				IntVec3 first = rect.Min + a * regionSize;
				IntVec3 second = rect.Min + a * regionSize + new IntVec3(regionSize, 0, regionSize);
				this.data[j] = CellRect.FromLimits(first, second);
				this.boundary = ((this.boundary == default(CellRect)) ? this.data[j] : this.boundary.Encapsulate(this.data[j]));
			}
		}

		// Token: 0x060075E0 RID: 30176 RVA: 0x00297676 File Offset: 0x00295876
		public bool Contains(int ind)
		{
			return ind >= 0 && ind < this.chunksX * this.chunksX;
		}

		// Token: 0x060075E1 RID: 30177 RVA: 0x0029768E File Offset: 0x0029588E
		public bool Contains(IntVec3 cell)
		{
			return this.rect.Contains(cell);
		}

		// Token: 0x060075E2 RID: 30178 RVA: 0x0029769C File Offset: 0x0029589C
		public bool Used(int ind)
		{
			return this.Contains(ind) && this.Used(this.IndexToCell(ind));
		}

		// Token: 0x060075E3 RID: 30179 RVA: 0x002976B6 File Offset: 0x002958B6
		public bool Used(IntVec3 cell)
		{
			return this.Contains(cell) && this.used.Contains(cell);
		}

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x060075E4 RID: 30180 RVA: 0x002976D0 File Offset: 0x002958D0
		public IntVec3 RandomChunk
		{
			get
			{
				return this.rect.RandomCell;
			}
		}

		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x060075E5 RID: 30181 RVA: 0x002976EB File Offset: 0x002958EB
		public IntVec3 RandomUsedChunk
		{
			get
			{
				return this.used.RandomElement<IntVec3>();
			}
		}

		// Token: 0x060075E6 RID: 30182 RVA: 0x002976F8 File Offset: 0x002958F8
		public void SetUsed(int index, bool set)
		{
			if (!this.Contains(index))
			{
				return;
			}
			this.SetUsed(this.IndexToCell(index), set);
		}

		// Token: 0x060075E7 RID: 30183 RVA: 0x00297714 File Offset: 0x00295914
		public void SetUsed(IntVec3 cell, bool set)
		{
			if (!this.Contains(cell))
			{
				return;
			}
			if (set && !this.used.Contains(cell))
			{
				this.usedCount++;
				this.used.Add(cell);
				this.enumerated.Clear();
				return;
			}
			if (!set && this.used.Contains(cell))
			{
				this.usedCount--;
				this.used.Remove(cell);
				this.enumerated.Clear();
			}
		}

		// Token: 0x060075E8 RID: 30184 RVA: 0x0029779A File Offset: 0x0029599A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CellRect CellToRect(IntVec3 c)
		{
			return this.data[this.CellToIndex(c)];
		}

		// Token: 0x060075E9 RID: 30185 RVA: 0x002977AE File Offset: 0x002959AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.chunksX);
		}

		// Token: 0x060075EA RID: 30186 RVA: 0x002977BC File Offset: 0x002959BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.chunksX);
		}

		// Token: 0x060075EB RID: 30187 RVA: 0x002977CC File Offset: 0x002959CC
		public bool TryGetFreeEdge([TupleElementNames(new string[]
		{
			"chunk",
			"rot"
		})] out ValueTuple<IntVec3, Rot4> edge, bool inBounds = false, int spaces = 1, Predicate<IntVec3> validator = null)
		{
			List<ValueTuple<IntVec3, Rot4>> list = new List<ValueTuple<IntVec3, Rot4>>();
			foreach (IntVec3 intVec in this.used.InRandomOrder(null))
			{
				Rot4 item;
				if (this.TryGetFreeEdge(intVec, out item, inBounds, spaces, validator))
				{
					list.Add(new ValueTuple<IntVec3, Rot4>(intVec, item));
				}
			}
			if (list.Empty<ValueTuple<IntVec3, Rot4>>())
			{
				edge = default(ValueTuple<IntVec3, Rot4>);
				return false;
			}
			edge = list.RandomElement<ValueTuple<IntVec3, Rot4>>();
			return true;
		}

		// Token: 0x060075EC RID: 30188 RVA: 0x00297858 File Offset: 0x00295A58
		public bool TryGetFreeEdge(IntVec3 chunk, out Rot4 rot, bool inBounds = false, int spaces = 1, Predicate<IntVec3> validator = null)
		{
			List<Rot4> list = new List<Rot4>();
			for (int i = 0; i < 4; i++)
			{
				rot = new Rot4(i);
				bool flag = true;
				for (int j = 0; j < spaces; j++)
				{
					IntVec3 intVec = chunk + rot.FacingCell * (j + 1);
					if (this.Used(intVec) || (inBounds && !this.rect.Contains(intVec)) || (validator != null && !validator(intVec)))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(rot);
				}
			}
			if (list.Empty<Rot4>())
			{
				rot = default(Rot4);
				return false;
			}
			rot = list.RandomElement<Rot4>();
			return true;
		}

		// Token: 0x060075ED RID: 30189 RVA: 0x00297904 File Offset: 0x00295B04
		private void RecacheEnumeratedChunks()
		{
			this.enumerated.Clear();
			foreach (CellRect cellRect in this.rect.EnumerateRectanglesCovering(new Predicate<IntVec3>(this.<RecacheEnumeratedChunks>g__Validator|34_0)))
			{
				CellRect cellRect2 = CellRect.Empty;
				foreach (IntVec3 cell in cellRect.Cells)
				{
					CellRect cellRect3 = this[cell];
					cellRect2 = ((cellRect2 == CellRect.Empty) ? cellRect3 : cellRect2.Encapsulate(cellRect3));
				}
				this.enumerated.Add(cellRect2);
			}
			this.enumerated.SortByDescending((CellRect r) => r.Area);
		}

		// Token: 0x060075EE RID: 30190 RVA: 0x00297A00 File Offset: 0x00295C00
		[CompilerGenerated]
		private bool <RecacheEnumeratedChunks>g__Validator|34_0(IntVec3 index)
		{
			return this.Used(index);
		}

		// Token: 0x040049E8 RID: 18920
		private readonly int chunksX;

		// Token: 0x040049E9 RID: 18921
		public readonly int regionSize;

		// Token: 0x040049EA RID: 18922
		public readonly CellRect rect;

		// Token: 0x040049EB RID: 18923
		public readonly CellRect boundary;

		// Token: 0x040049EC RID: 18924
		private readonly CellRect[] data;

		// Token: 0x040049ED RID: 18925
		private readonly HashSet<IntVec3> used = new HashSet<IntVec3>();

		// Token: 0x040049EE RID: 18926
		private readonly List<CellRect> enumerated = new List<CellRect>();

		// Token: 0x040049EF RID: 18927
		private int usedCount;
	}
}
