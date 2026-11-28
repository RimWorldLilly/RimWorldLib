using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KTrie
{
	// Token: 0x0200240B RID: 9227
	public class Trie<TKey, TValue> : IDictionary<IEnumerable<TKey>, TValue>, ICollection<KeyValuePair<IEnumerable<TKey>, TValue>>, IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>>, IEnumerable
	{
		// Token: 0x0600E708 RID: 59144 RVA: 0x0055D5B8 File Offset: 0x0055B7B8
		public Trie() : this(EqualityComparer<TKey>.Default)
		{
		}

		// Token: 0x0600E709 RID: 59145 RVA: 0x0055D5C5 File Offset: 0x0055B7C5
		public Trie(IEqualityComparer<TKey> comparer)
		{
			this._trie = new TrieSet<TKey>(comparer);
		}

		// Token: 0x170028E2 RID: 10466
		// (get) Token: 0x0600E70A RID: 59146 RVA: 0x0055D5D9 File Offset: 0x0055B7D9
		public int Count
		{
			get
			{
				return this._trie.Count;
			}
		}

		// Token: 0x170028E3 RID: 10467
		// (get) Token: 0x0600E70B RID: 59147 RVA: 0x0055D5E6 File Offset: 0x0055B7E6
		bool ICollection<KeyValuePair<IEnumerable<!0>, !1>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170028E4 RID: 10468
		// (get) Token: 0x0600E70C RID: 59148 RVA: 0x0055D5E9 File Offset: 0x0055B7E9
		public ICollection<IEnumerable<TKey>> Keys
		{
			get
			{
				return this._trie.ToList<IEnumerable<TKey>>();
			}
		}

		// Token: 0x170028E5 RID: 10469
		// (get) Token: 0x0600E70D RID: 59149 RVA: 0x0055D5F6 File Offset: 0x0055B7F6
		public ICollection<TValue> Values
		{
			get
			{
				return (from Trie<TKey, TValue>.TrieEntryPrivate te in this._trie
				select te.Value).ToArray<TValue>();
			}
		}

		// Token: 0x170028E6 RID: 10470
		public TValue this[IEnumerable<TKey> key]
		{
			get
			{
				TValue result;
				if (this.TryGetValue(key, out result))
				{
					return result;
				}
				throw new KeyNotFoundException("The given key was not present in the trie.");
			}
			set
			{
				IEnumerable<TKey> enumerable;
				if (this._trie.TryGetItem(key, out enumerable))
				{
					((Trie<TKey, TValue>.TrieEntryPrivate)enumerable).Value = value;
					return;
				}
				this.Add(key, value);
			}
		}

		// Token: 0x0600E710 RID: 59152 RVA: 0x0055D682 File Offset: 0x0055B882
		public IEnumerable<TrieEntry<TKey, TValue>> GetByPrefix(IEnumerable<TKey> prefix)
		{
			return from Trie<TKey, TValue>.TrieEntryPrivate i in this._trie.GetByPrefix(prefix)
			select new TrieEntry<TKey, TValue>(i, i.Value);
		}

		// Token: 0x0600E711 RID: 59153 RVA: 0x0055D6B9 File Offset: 0x0055B8B9
		public IEnumerator<KeyValuePair<IEnumerable<TKey>, TValue>> GetEnumerator()
		{
			return (from Trie<TKey, TValue>.TrieEntryPrivate i in this._trie
			select new KeyValuePair<IEnumerable<TKey>, TValue>(i, i.Value)).GetEnumerator();
		}

		// Token: 0x0600E712 RID: 59154 RVA: 0x0055D6EF File Offset: 0x0055B8EF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600E713 RID: 59155 RVA: 0x0055D6F7 File Offset: 0x0055B8F7
		void ICollection<KeyValuePair<IEnumerable<!0>, !1>>.Add(KeyValuePair<IEnumerable<TKey>, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x0600E714 RID: 59156 RVA: 0x0055D70D File Offset: 0x0055B90D
		public void Clear()
		{
			this._trie.Clear();
		}

		// Token: 0x0600E715 RID: 59157 RVA: 0x0055D71C File Offset: 0x0055B91C
		bool ICollection<KeyValuePair<IEnumerable<!0>, !1>>.Contains(KeyValuePair<IEnumerable<TKey>, TValue> item)
		{
			IEnumerable<TKey> enumerable;
			if (this._trie.TryGetItem(item.Key, out enumerable))
			{
				TValue value = ((Trie<TKey, TValue>.TrieEntryPrivate)enumerable).Value;
				if (EqualityComparer<TValue>.Default.Equals(item.Value, value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E716 RID: 59158 RVA: 0x0055D764 File Offset: 0x0055B964
		public void CopyTo(KeyValuePair<IEnumerable<TKey>, TValue>[] array, int arrayIndex)
		{
			Array.Copy((from Trie<TKey, TValue>.TrieEntryPrivate i in this._trie
			select new KeyValuePair<IEnumerable<TKey>, TValue>(i, i.Value)).ToArray<KeyValuePair<IEnumerable<TKey>, TValue>>(), 0, array, arrayIndex, this.Count);
		}

		// Token: 0x0600E717 RID: 59159 RVA: 0x0055D7B4 File Offset: 0x0055B9B4
		bool ICollection<KeyValuePair<IEnumerable<!0>, !1>>.Remove(KeyValuePair<IEnumerable<TKey>, TValue> item)
		{
			IEnumerable<TKey> enumerable;
			if (this._trie.TryGetItem(item.Key, out enumerable))
			{
				TValue value = ((Trie<TKey, TValue>.TrieEntryPrivate)enumerable).Value;
				if (EqualityComparer<TValue>.Default.Equals(item.Value, value))
				{
					return this.Remove(item.Key);
				}
			}
			return false;
		}

		// Token: 0x0600E718 RID: 59160 RVA: 0x0055D806 File Offset: 0x0055BA06
		public bool ContainsKey(IEnumerable<TKey> key)
		{
			return this._trie.Contains(key);
		}

		// Token: 0x0600E719 RID: 59161 RVA: 0x0055D814 File Offset: 0x0055BA14
		public void Add(IEnumerable<TKey> key, TValue value)
		{
			this._trie.Add(new Trie<TKey, TValue>.TrieEntryPrivate(key)
			{
				Value = value
			});
		}

		// Token: 0x0600E71A RID: 59162 RVA: 0x0055D82E File Offset: 0x0055BA2E
		public bool Remove(IEnumerable<TKey> key)
		{
			return this._trie.Remove(key);
		}

		// Token: 0x0600E71B RID: 59163 RVA: 0x0055D83C File Offset: 0x0055BA3C
		public bool TryGetValue(IEnumerable<TKey> key, out TValue value)
		{
			IEnumerable<TKey> enumerable;
			bool flag = this._trie.TryGetItem(key, out enumerable);
			value = (flag ? ((Trie<TKey, TValue>.TrieEntryPrivate)enumerable).Value : default(TValue));
			return flag;
		}

		// Token: 0x04009289 RID: 37513
		private readonly TrieSet<TKey> _trie;

		// Token: 0x02003F1E RID: 16158
		private sealed class TrieEntryPrivate : IEnumerable<!0>, IEnumerable
		{
			// Token: 0x06016526 RID: 91430 RVA: 0x006EEC84 File Offset: 0x006ECE84
			public TrieEntryPrivate(IEnumerable<TKey> key)
			{
				this.Key = key;
			}

			// Token: 0x170037ED RID: 14317
			// (get) Token: 0x06016527 RID: 91431 RVA: 0x006EEC93 File Offset: 0x006ECE93
			private IEnumerable<TKey> Key { get; }

			// Token: 0x170037EE RID: 14318
			// (get) Token: 0x06016528 RID: 91432 RVA: 0x006EEC9B File Offset: 0x006ECE9B
			// (set) Token: 0x06016529 RID: 91433 RVA: 0x006EECA3 File Offset: 0x006ECEA3
			public TValue Value { get; set; }

			// Token: 0x0601652A RID: 91434 RVA: 0x006EECAC File Offset: 0x006ECEAC
			public IEnumerator<TKey> GetEnumerator()
			{
				return this.Key.GetEnumerator();
			}

			// Token: 0x0601652B RID: 91435 RVA: 0x006EECB9 File Offset: 0x006ECEB9
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}
