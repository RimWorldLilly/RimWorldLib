using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x0200002B RID: 43
	internal struct HeapData<[IsUnmanaged] T, U> where T : struct, ValueType
	{
		// Token: 0x04000058 RID: 88
		public int Count;

		// Token: 0x04000059 RID: 89
		public int Capacity;

		// Token: 0x0400005A RID: 90
		public unsafe HeapNode<T>* Heap;

		// Token: 0x0400005B RID: 91
		public unsafe TableValue* Table;

		// Token: 0x0400005C RID: 92
		public U Comparator;
	}
}
