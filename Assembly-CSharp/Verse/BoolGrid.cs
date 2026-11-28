using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000278 RID: 632
	public class BoolGrid : IExposable
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0006992D File Offset: 0x00067B2D
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x00069935 File Offset: 0x00067B35
		public int Width
		{
			get
			{
				return this.width;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x0006993D File Offset: 0x00067B3D
		public int Height
		{
			get
			{
				return this.height;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x00069945 File Offset: 0x00067B45
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				BoolGrid.<get_ActiveCells>d__13 <get_ActiveCells>d__ = new BoolGrid.<get_ActiveCells>d__13(-2);
				<get_ActiveCells>d__.<>4__this = this;
				return <get_ActiveCells>d__;
			}
		}

		// Token: 0x1700037E RID: 894
		public bool this[int index]
		{
			get
			{
				return this.arr[index];
			}
			set
			{
				this.Set(index, value);
			}
		}

		// Token: 0x1700037F RID: 895
		public bool this[IntVec3 c]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(c, this.width)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x17000380 RID: 896
		public bool this[int x, int z]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(x, z, this.width)];
			}
			set
			{
				this.Set(CellIndicesUtility.CellToIndex(x, z, this.width), value);
			}
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x000699B4 File Offset: 0x00067BB4
		public BoolGrid()
		{
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x000699C3 File Offset: 0x00067BC3
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x000699D9 File Offset: 0x00067BD9
		public BoolGrid(int x, int z)
		{
			this.ClearAndResizeTo(x, z);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x000699F0 File Offset: 0x00067BF0
		public bool MapSizeMatches(Map map)
		{
			return this.width == map.Size.x && this.height == map.Size.z;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00069A1A File Offset: 0x00067C1A
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map))
			{
				if (this.arr != null)
				{
					this.Clear();
				}
				return;
			}
			this.ClearAndResizeTo(map.Size.x, map.Size.z);
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x00069A50 File Offset: 0x00067C50
		public void ClearAndResizeTo(int x, int z)
		{
			if (this.width == x && this.height == z)
			{
				if (this.arr != null)
				{
					this.Clear();
				}
				return;
			}
			this.width = x;
			this.height = z;
			this.arr = new bool[this.width * this.height];
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x00069ABC File Offset: 0x00067CBC
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.trueCountInt, "trueCount", 0, false);
			Scribe_Values.Look<int>(ref this.width, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.height, "mapSizeZ", 0, false);
			DataExposeUtility.LookBoolArray(ref this.arr, this.width * this.height, "arr");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.minPossibleTrueIndexDirty = true;
			}
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00069B2B File Offset: 0x00067D2B
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00069B56 File Offset: 0x00067D56
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.width), value);
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00069B6C File Offset: 0x00067D6C
		public virtual void Set(int index, bool value)
		{
			if (this.arr[index] == value)
			{
				return;
			}
			this.arr[index] = value;
			if (value)
			{
				this.trueCountInt++;
				if (this.trueCountInt == 1 || index < this.minPossibleTrueIndexCached)
				{
					this.minPossibleTrueIndexCached = index;
					return;
				}
			}
			else
			{
				this.trueCountInt--;
				if (index == this.minPossibleTrueIndexCached)
				{
					this.minPossibleTrueIndexDirty = true;
				}
			}
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00069BD8 File Offset: 0x00067DD8
		public void Invert()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i] = !this.arr[i];
			}
			this.trueCountInt = this.arr.Length - this.trueCountInt;
			this.minPossibleTrueIndexDirty = true;
		}

		// Token: 0x0400109C RID: 4252
		private bool[] arr;

		// Token: 0x0400109D RID: 4253
		private int trueCountInt;

		// Token: 0x0400109E RID: 4254
		private int width;

		// Token: 0x0400109F RID: 4255
		private int height;

		// Token: 0x040010A0 RID: 4256
		private int minPossibleTrueIndexCached = -1;

		// Token: 0x040010A1 RID: 4257
		private bool minPossibleTrueIndexDirty;
	}
}
