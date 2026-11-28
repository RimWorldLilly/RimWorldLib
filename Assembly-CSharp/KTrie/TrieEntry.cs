using System;
using System.Collections.Generic;

namespace KTrie
{
	// Token: 0x0200240C RID: 9228
	public struct TrieEntry<TKey, TValue>
	{
		// Token: 0x0600E71C RID: 59164 RVA: 0x0055D878 File Offset: 0x0055BA78
		public TrieEntry(IEnumerable<TKey> key, TValue value)
		{
			this.Key = key;
			this.Value = value;
		}

		// Token: 0x170028E7 RID: 10471
		// (get) Token: 0x0600E71D RID: 59165 RVA: 0x0055D888 File Offset: 0x0055BA88
		public readonly IEnumerable<TKey> Key { get; }

		// Token: 0x170028E8 RID: 10472
		// (get) Token: 0x0600E71E RID: 59166 RVA: 0x0055D890 File Offset: 0x0055BA90
		public readonly TValue Value { get; }
	}
}
