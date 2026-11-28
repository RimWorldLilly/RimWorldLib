using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace LudeonTK
{
	// Token: 0x0200005E RID: 94
	[NativeContainer]
	[DebuggerDisplay("Count = {Count}, Capacity = {Capacity}, IsCreated = {IsCreated}")]
	public struct NativePriorityQueue<[IsUnmanaged] TElement, [IsUnmanaged] TPriority, [IsUnmanaged] TComparer> : IDisposable where TElement : struct, ValueType where TPriority : struct, ValueType where TComparer : struct, ValueType, IComparer<TPriority>
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00013AF6 File Offset: 0x00011CF6
		public bool IsCreated
		{
			get
			{
				return this.m_ElementBuffer != null;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00013B05 File Offset: 0x00011D05
		public int Capacity
		{
			get
			{
				return this.m_Capacity;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00013B0D File Offset: 0x00011D0D
		public unsafe int Count
		{
			get
			{
				if (this.m_Data == null)
				{
					return 0;
				}
				return this.m_Data->m_Count;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00013B28 File Offset: 0x00011D28
		public unsafe NativePriorityQueue(int capacity, TComparer comparer, Allocator allocator)
		{
			long size = (long)UnsafeUtility.SizeOf<TElement>() * (long)capacity;
			this.m_ElementBuffer = UnsafeUtility.Malloc(size, UnsafeUtility.AlignOf<TElement>(), allocator);
			long size2 = (long)UnsafeUtility.SizeOf<TPriority>() * (long)capacity;
			this.m_PriorityBuffer = UnsafeUtility.Malloc(size2, UnsafeUtility.AlignOf<TPriority>(), allocator);
			this.m_Data = (NativePriorityQueue<TElement, TPriority, TComparer>.Data*)UnsafeUtility.Malloc((long)UnsafeUtility.SizeOf<NativePriorityQueue<TElement, TPriority, TComparer>.Data>(), UnsafeUtility.AlignOf<NativePriorityQueue<TElement, TPriority, TComparer>.Data>(), allocator);
			this.m_Data->m_Count = 0;
			this.m_Capacity = capacity;
			this.m_Comparer = comparer;
			this.m_AllocatorLabel = allocator;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00013BA5 File Offset: 0x00011DA5
		[WriteAccessRequired]
		public unsafe void Clear()
		{
			this.m_Data->m_Count = 0;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00013BB4 File Offset: 0x00011DB4
		[WriteAccessRequired]
		public unsafe void Enqueue(TElement element, TPriority priority)
		{
			int i = this.m_Data->m_Count;
			this.m_Data->m_Count = i + 1;
			TComparer comparer = this.m_Comparer;
			while (i > 0)
			{
				int num = i - 1 >> 2;
				TPriority tpriority = UnsafeUtility.ReadArrayElement<TPriority>(this.m_PriorityBuffer, num);
				if (comparer.Compare(priority, tpriority) >= 0)
				{
					break;
				}
				TElement value = UnsafeUtility.ReadArrayElement<TElement>(this.m_ElementBuffer, num);
				UnsafeUtility.WriteArrayElement<TElement>(this.m_ElementBuffer, i, value);
				UnsafeUtility.WriteArrayElement<TPriority>(this.m_PriorityBuffer, i, tpriority);
				i = num;
			}
			UnsafeUtility.WriteArrayElement<TElement>(this.m_ElementBuffer, i, element);
			UnsafeUtility.WriteArrayElement<TPriority>(this.m_PriorityBuffer, i, priority);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00013C52 File Offset: 0x00011E52
		[WriteAccessRequired]
		public void Dequeue(out TElement element, out TPriority priority)
		{
			element = UnsafeUtility.ReadArrayElement<TElement>(this.m_ElementBuffer, 0);
			priority = UnsafeUtility.ReadArrayElement<TPriority>(this.m_PriorityBuffer, 0);
			this.RemoveRootNode();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00013C80 File Offset: 0x00011E80
		private unsafe void RemoveRootNode()
		{
			int num = this.m_Data->m_Count - 1;
			this.m_Data->m_Count = num;
			if (num > 0)
			{
				TElement element = UnsafeUtility.ReadArrayElement<TElement>(this.m_ElementBuffer, num);
				TPriority priority = UnsafeUtility.ReadArrayElement<TPriority>(this.m_PriorityBuffer, num);
				this.MoveDown(element, priority, 0);
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00013CD0 File Offset: 0x00011ED0
		private void MoveDown(TElement element, TPriority priority, int nodeIndex)
		{
			TComparer comparer = this.m_Comparer;
			int count = this.Count;
			int num;
			while ((num = (nodeIndex << 2) + 1) < count)
			{
				TElement value = UnsafeUtility.ReadArrayElement<TElement>(this.m_ElementBuffer, num);
				TPriority tpriority = UnsafeUtility.ReadArrayElement<TPriority>(this.m_PriorityBuffer, num);
				int num2 = num;
				int num3 = math.min(num + 4, count);
				while (++num < num3)
				{
					TPriority tpriority2 = UnsafeUtility.ReadArrayElement<TPriority>(this.m_PriorityBuffer, num);
					if (comparer.Compare(tpriority2, tpriority) < 0)
					{
						value = UnsafeUtility.ReadArrayElement<TElement>(this.m_ElementBuffer, num);
						tpriority = tpriority2;
						num2 = num;
					}
				}
				if (comparer.Compare(priority, tpriority) <= 0)
				{
					break;
				}
				UnsafeUtility.WriteArrayElement<TElement>(this.m_ElementBuffer, nodeIndex, value);
				UnsafeUtility.WriteArrayElement<TPriority>(this.m_PriorityBuffer, nodeIndex, tpriority);
				nodeIndex = num2;
			}
			UnsafeUtility.WriteArrayElement<TElement>(this.m_ElementBuffer, nodeIndex, element);
			UnsafeUtility.WriteArrayElement<TPriority>(this.m_PriorityBuffer, nodeIndex, priority);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00013DB4 File Offset: 0x00011FB4
		[WriteAccessRequired]
		public unsafe void Dispose()
		{
			if (this.m_ElementBuffer == null)
			{
				throw new ObjectDisposedException("The collection is already disposed.");
			}
			if (this.m_AllocatorLabel == Allocator.Invalid)
			{
				throw new InvalidOperationException("The collection can not be Disposed because it was not allocated with a valid allocator.");
			}
			if (this.m_AllocatorLabel > Allocator.None)
			{
				UnsafeUtility.Free(this.m_ElementBuffer, this.m_AllocatorLabel);
				UnsafeUtility.Free(this.m_PriorityBuffer, this.m_AllocatorLabel);
				UnsafeUtility.Free((void*)this.m_Data, this.m_AllocatorLabel);
				this.m_AllocatorLabel = Allocator.Invalid;
			}
			this.m_ElementBuffer = null;
			this.m_PriorityBuffer = null;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00013E3C File Offset: 0x0001203C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal static void CheckAllocateArguments(int capacity, Allocator allocator)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", "Capacity must be >= 0");
			}
			if (allocator <= Allocator.None || allocator > Allocator.Persistent)
			{
				throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", "allocator");
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00013E6C File Offset: 0x0001206C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal static void CheckTotalSize(long totalSize)
		{
			if (totalSize > 2147483647L)
			{
				string paramName = "totalSize";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Allocation total size cannot exceed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(int.MaxValue);
				defaultInterpolatedStringHandler.AppendLiteral(" bytes");
				throw new ArgumentOutOfRangeException(paramName, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00013EC2 File Offset: 0x000120C2
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal static void CheckNotEmpty(int count)
		{
			if (count == 0)
			{
				throw new InvalidOperationException("The container is empty");
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00013ED2 File Offset: 0x000120D2
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		internal static void CheckSufficientCapacity(int capacity, int index)
		{
			if (index >= capacity)
			{
				throw new InvalidOperationException("The container has a fixed capacity and is already full.");
			}
		}

		// Token: 0x0400015F RID: 351
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* m_ElementBuffer;

		// Token: 0x04000160 RID: 352
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* m_PriorityBuffer;

		// Token: 0x04000161 RID: 353
		[NativeDisableUnsafePtrRestriction]
		internal unsafe NativePriorityQueue<TElement, TPriority, TComparer>.Data* m_Data;

		// Token: 0x04000162 RID: 354
		internal int m_Capacity;

		// Token: 0x04000163 RID: 355
		internal TComparer m_Comparer;

		// Token: 0x04000164 RID: 356
		internal Allocator m_AllocatorLabel;

		// Token: 0x04000165 RID: 357
		private const int Arity = 4;

		// Token: 0x04000166 RID: 358
		private const int Log2Arity = 2;

		// Token: 0x02002481 RID: 9345
		internal struct Data
		{
			// Token: 0x04009517 RID: 38167
			public int m_Count;
		}
	}
}
