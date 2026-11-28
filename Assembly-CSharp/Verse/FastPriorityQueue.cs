using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006D9 RID: 1753
	public class FastPriorityQueue<T>
	{
		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06003733 RID: 14131 RVA: 0x00161F08 File Offset: 0x00160108
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x00161F15 File Offset: 0x00160115
		public FastPriorityQueue()
		{
			this.comparer = Comparer<T>.Default;
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x00161F33 File Offset: 0x00160133
		public FastPriorityQueue(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x00161F50 File Offset: 0x00160150
		public void Push(T item)
		{
			int num = this.innerList.Count;
			this.innerList.Add(item);
			while (num != 0)
			{
				int num2 = (num - 1) / 2;
				if (this.CompareElements(num, num2) >= 0)
				{
					break;
				}
				this.SwapElements(num, num2);
				num = num2;
			}
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x00161F98 File Offset: 0x00160198
		public T Pop()
		{
			T result = this.innerList[0];
			int num = 0;
			int count = this.innerList.Count;
			this.innerList[0] = this.innerList[count - 1];
			this.innerList.RemoveAt(count - 1);
			count = this.innerList.Count;
			for (;;)
			{
				int num2 = num;
				int num3 = 2 * num + 1;
				int num4 = num3 + 1;
				if (num3 < count && this.CompareElements(num, num3) > 0)
				{
					num = num3;
				}
				if (num4 < count && this.CompareElements(num, num4) > 0)
				{
					num = num4;
				}
				if (num == num2)
				{
					break;
				}
				this.SwapElements(num, num2);
			}
			return result;
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x0016203A File Offset: 0x0016023A
		public void Clear()
		{
			this.innerList.Clear();
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x00162048 File Offset: 0x00160248
		protected void SwapElements(int i, int j)
		{
			List<T> list = this.innerList;
			List<T> list2 = this.innerList;
			T value = this.innerList[j];
			T value2 = this.innerList[i];
			list[i] = value;
			list2[j] = value2;
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x00162091 File Offset: 0x00160291
		protected int CompareElements(int i, int j)
		{
			return this.comparer.Compare(this.innerList[i], this.innerList[j]);
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x001620B6 File Offset: 0x001602B6
		public bool Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x04002446 RID: 9286
		protected List<T> innerList = new List<T>();

		// Token: 0x04002447 RID: 9287
		protected IComparer<T> comparer;
	}
}
