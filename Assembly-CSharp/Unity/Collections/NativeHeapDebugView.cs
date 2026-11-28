using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000029 RID: 41
	internal class NativeHeapDebugView<[IsUnmanaged] T, [IsUnmanaged] U> where T : struct, ValueType where U : struct, ValueType, IComparer<T>
	{
		// Token: 0x0600012A RID: 298 RVA: 0x00009C37 File Offset: 0x00007E37
		public NativeHeapDebugView(UnsafeHeap<T, U> heap)
		{
			this._heap = heap;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00009C46 File Offset: 0x00007E46
		public int Count
		{
			get
			{
				return this._heap.Count;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00009C53 File Offset: 0x00007E53
		public int Capacity
		{
			get
			{
				return this._heap.Capacity;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00009C60 File Offset: 0x00007E60
		public U Comparator
		{
			get
			{
				return this._heap.Comparator;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00009C70 File Offset: 0x00007E70
		public unsafe T[] Items
		{
			get
			{
				T[] array = new T[this._heap.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._heap.Data->Heap[(IntPtr)i * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)].Item;
				}
				return array;
			}
		}

		// Token: 0x04000056 RID: 86
		private UnsafeHeap<T, U> _heap;
	}
}
