using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x0200002C RID: 44
	internal struct HeapNode<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x0400005D RID: 93
		public T Item;

		// Token: 0x0400005E RID: 94
		public int TableIndex;
	}
}
