using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x02000028 RID: 40
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(NativeHeapDebugView<, >))]
	public struct UnsafeHeap<[IsUnmanaged] T, [IsUnmanaged] U> : IDisposable where T : struct, ValueType where U : struct, ValueType, IComparer<T>
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00009436 File Offset: 0x00007636
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00009444 File Offset: 0x00007644
		public unsafe int Capacity
		{
			get
			{
				return this.Data->Capacity;
			}
			set
			{
				TableValue* ptr = (TableValue*)UnsafeUtility.Malloc((long)(UnsafeUtility.SizeOf<TableValue>() * value), UnsafeUtility.AlignOf<TableValue>(), this.Allocator);
				HeapNode<T>* ptr2 = (HeapNode<T>*)UnsafeUtility.Malloc((long)(UnsafeUtility.SizeOf<HeapNode<T>>() * value), UnsafeUtility.AlignOf<HeapNode<T>>(), this.Allocator);
				int num = (this.Data->Capacity < value) ? this.Data->Capacity : value;
				UnsafeUtility.MemCpy((void*)ptr, (void*)this.Data->Table, (long)(num * UnsafeUtility.SizeOf<TableValue>()));
				UnsafeUtility.MemCpy((void*)ptr2, (void*)this.Data->Heap, (long)(num * UnsafeUtility.SizeOf<HeapNode<T>>()));
				for (int i = 0; i < value - this.Data->Capacity; i++)
				{
					ptr2[(IntPtr)(i + this.Data->Capacity) * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = new HeapNode<T>
					{
						TableIndex = i + this.Data->Capacity
					};
				}
				UnsafeUtility.Free((void*)this.Data->Table, this.Allocator);
				UnsafeUtility.Free((void*)this.Data->Heap, this.Allocator);
				this.Data->Table = ptr;
				this.Data->Heap = ptr2;
				this.Data->Capacity = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00009574 File Offset: 0x00007774
		public unsafe int Count
		{
			get
			{
				return this.Data->Count;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00009581 File Offset: 0x00007781
		// (set) Token: 0x0600011C RID: 284 RVA: 0x0000958E File Offset: 0x0000778E
		public unsafe U Comparator
		{
			get
			{
				return this.Data->Comparator;
			}
			set
			{
				this.Data->Comparator = value;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000959C File Offset: 0x0000779C
		public UnsafeHeap(Allocator allocator, int initialCapacity = 128, U comparator = default(U))
		{
			this = new UnsafeHeap<T, U>(initialCapacity, comparator, allocator);
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000095A7 File Offset: 0x000077A7
		public bool IsCreated
		{
			get
			{
				return this.Data != null;
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000095B8 File Offset: 0x000077B8
		public unsafe void Dispose()
		{
			this.Data->Count = 0;
			this.Data->Capacity = 0;
			UnsafeUtility.Free((void*)this.Data->Heap, this.Allocator);
			UnsafeUtility.Free((void*)this.Data->Table, this.Allocator);
			UnsafeUtility.Free((void*)this.Data, this.Allocator);
			this.Data = null;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00009622 File Offset: 0x00007822
		public unsafe void Clear()
		{
			this.Data->Count = 0;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00009630 File Offset: 0x00007830
		public T Peek()
		{
			T result;
			if (!this.TryPeek(out result))
			{
				throw new InvalidOperationException("Cannot Peek NativeHeap when the count is zero.");
			}
			return result;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00009653 File Offset: 0x00007853
		public unsafe bool TryPeek(out T t)
		{
			if (this.Data->Count == 0)
			{
				t = default(T);
				return false;
			}
			t = this.Data->Heap->Item;
			return true;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00009684 File Offset: 0x00007884
		public T Pop()
		{
			T result;
			if (!this.TryPop(out result))
			{
				throw new InvalidOperationException("Cannot Pop NativeHeap when the count is zero.");
			}
			return result;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000096A8 File Offset: 0x000078A8
		public unsafe bool TryPop(out T t)
		{
			if (this.Data->Count == 0)
			{
				t = default(T);
				return false;
			}
			HeapNode<T> heapNode = *this.Data->Heap;
			HeapData<T, U>* data = this.Data;
			int num = data->Count - 1;
			data->Count = num;
			int num2 = num;
			HeapNode<T> node = this.Data->Heap[(IntPtr)num2 * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
			this.Data->Heap[(IntPtr)num2 * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = heapNode;
			this.InsertAndBubbleDown(node, 0);
			t = heapNode.Item;
			return true;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000973C File Offset: 0x0000793C
		public unsafe NativeHeapIndex Insert(in T t)
		{
			if (this.Data->Count == this.Data->Capacity)
			{
				this.Capacity *= 2;
			}
			HeapNode<T> heapNode = this.Data->Heap[(IntPtr)this.Data->Count * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
			heapNode.Item = t;
			HeapData<T, U>* data = this.Data;
			int count = data->Count;
			data->Count = count + 1;
			int insertIndex = count;
			this.InsertAndBubbleUp(heapNode, insertIndex);
			return new NativeHeapIndex
			{
				TableIndex = heapNode.TableIndex
			};
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000097D4 File Offset: 0x000079D4
		public unsafe T Remove(NativeHeapIndex index)
		{
			int heapIndex = this.Data->Table[index.TableIndex].HeapIndex;
			HeapNode<T> heapNode = this.Data->Heap[(IntPtr)heapIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
			IntPtr heap = this.Data->Heap;
			HeapData<T, U>* data = this.Data;
			int num = data->Count - 1;
			data->Count = num;
			HeapNode<T> heapNode2 = *(heap + (IntPtr)num * (IntPtr)sizeof(HeapNode<T>));
			UnsafeUtility.WriteArrayElement<HeapNode<T>>((void*)this.Data->Heap, this.Data->Count, heapNode);
			if (heapIndex != 0)
			{
				int num2 = (heapIndex - 1) / 2;
				HeapNode<T> heapNode3 = this.Data->Heap[(IntPtr)num2 * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
				if (this.Data->Comparator.Compare(heapNode2.Item, heapNode3.Item) < 0)
				{
					this.InsertAndBubbleUp(heapNode2, heapIndex);
					return heapNode.Item;
				}
			}
			this.InsertAndBubbleDown(heapNode2, heapIndex);
			return heapNode.Item;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000098D0 File Offset: 0x00007AD0
		internal unsafe UnsafeHeap(int initialCapacity, U comparator, Allocator allocator)
		{
			this.Data = (HeapData<T, U>*)UnsafeUtility.Malloc((long)UnsafeUtility.SizeOf<HeapData<T, U>>(), UnsafeUtility.AlignOf<HeapData<T, U>>(), allocator);
			this.Data->Heap = (HeapNode<T>*)UnsafeUtility.Malloc((long)(UnsafeUtility.SizeOf<HeapNode<T>>() * initialCapacity), UnsafeUtility.AlignOf<HeapNode<T>>(), allocator);
			this.Data->Table = (TableValue*)UnsafeUtility.Malloc((long)(UnsafeUtility.SizeOf<TableValue>() * initialCapacity), UnsafeUtility.AlignOf<TableValue>(), allocator);
			this.Allocator = allocator;
			for (int i = 0; i < initialCapacity; i++)
			{
				this.Data->Heap[(IntPtr)i * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = new HeapNode<T>
				{
					TableIndex = i
				};
			}
			this.Data->Count = 0;
			this.Data->Capacity = initialCapacity;
			this.Data->Comparator = comparator;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00009994 File Offset: 0x00007B94
		internal unsafe void InsertAndBubbleDown(HeapNode<T> node, int insertIndex)
		{
			for (;;)
			{
				int num = insertIndex * 2 + 1;
				int num2 = insertIndex * 2 + 2;
				if (num >= this.Data->Count)
				{
					break;
				}
				if (num2 >= this.Data->Count || this.Data->Comparator.Compare(this.Data->Heap[(IntPtr)num * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)].Item, this.Data->Heap[(IntPtr)num2 * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)].Item) <= 0)
				{
					HeapNode<T> heapNode = this.Data->Heap[(IntPtr)num * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
					if (this.Data->Comparator.Compare(node.Item, heapNode.Item) <= 0)
					{
						break;
					}
					this.Data->Heap[(IntPtr)insertIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = heapNode;
					this.Data->Table[heapNode.TableIndex].HeapIndex = insertIndex;
					insertIndex = num;
				}
				else
				{
					HeapNode<T> heapNode2 = this.Data->Heap[(IntPtr)num2 * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
					if (this.Data->Comparator.Compare(node.Item, heapNode2.Item) <= 0)
					{
						break;
					}
					this.Data->Heap[(IntPtr)insertIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = heapNode2;
					this.Data->Table[heapNode2.TableIndex].HeapIndex = insertIndex;
					insertIndex = num2;
				}
			}
			this.Data->Heap[(IntPtr)insertIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = node;
			this.Data->Table[node.TableIndex].HeapIndex = insertIndex;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00009B60 File Offset: 0x00007D60
		internal unsafe void InsertAndBubbleUp(HeapNode<T> node, int insertIndex)
		{
			while (insertIndex != 0)
			{
				int num = (insertIndex - 1) / 2;
				HeapNode<T> heapNode = this.Data->Heap[(IntPtr)num * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)];
				if (this.Data->Comparator.Compare(heapNode.Item, node.Item) <= 0)
				{
					break;
				}
				this.Data->Heap[(IntPtr)insertIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = heapNode;
				this.Data->Table[heapNode.TableIndex].HeapIndex = insertIndex;
				insertIndex = num;
			}
			this.Data->Heap[(IntPtr)insertIndex * (IntPtr)sizeof(HeapNode<T>) / (IntPtr)sizeof(HeapNode<T>)] = node;
			this.Data->Table[node.TableIndex].HeapIndex = insertIndex;
		}

		// Token: 0x04000050 RID: 80
		public const int DEFAULT_CAPACITY = 128;

		// Token: 0x04000051 RID: 81
		internal const int VALIDATION_ERROR_WRONG_INSTANCE = 1;

		// Token: 0x04000052 RID: 82
		internal const int VALIDATION_ERROR_INVALID = 2;

		// Token: 0x04000053 RID: 83
		internal const int VALIDATION_ERROR_REMOVED = 3;

		// Token: 0x04000054 RID: 84
		[NativeDisableUnsafePtrRestriction]
		internal unsafe HeapData<T, U>* Data;

		// Token: 0x04000055 RID: 85
		internal Allocator Allocator;
	}
}
