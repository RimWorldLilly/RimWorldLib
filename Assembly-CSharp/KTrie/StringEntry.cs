using System;

namespace KTrie
{
	// Token: 0x02002408 RID: 9224
	public struct StringEntry<TValue>
	{
		// Token: 0x0600E6E3 RID: 59107 RVA: 0x0055D230 File Offset: 0x0055B430
		public StringEntry(string key, TValue value)
		{
			this.Key = key;
			this.Value = value;
		}

		// Token: 0x170028D9 RID: 10457
		// (get) Token: 0x0600E6E4 RID: 59108 RVA: 0x0055D240 File Offset: 0x0055B440
		public readonly string Key { get; }

		// Token: 0x170028DA RID: 10458
		// (get) Token: 0x0600E6E5 RID: 59109 RVA: 0x0055D248 File Offset: 0x0055B448
		public readonly TValue Value { get; }
	}
}
