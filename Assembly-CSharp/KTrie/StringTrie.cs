using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KTrie
{
	// Token: 0x02002409 RID: 9225
	public class StringTrie<TValue> : IDictionary<string, TValue>, ICollection<KeyValuePair<string, TValue>>, IEnumerable<KeyValuePair<string, TValue>>, IEnumerable
	{
		// Token: 0x0600E6E6 RID: 59110 RVA: 0x0055D250 File Offset: 0x0055B450
		public StringTrie(IEqualityComparer<char> comparer)
		{
			this._trie = new Trie<char, TValue>(comparer);
		}

		// Token: 0x0600E6E7 RID: 59111 RVA: 0x0055D264 File Offset: 0x0055B464
		public StringTrie() : this(EqualityComparer<char>.Default)
		{
		}

		// Token: 0x170028DB RID: 10459
		// (get) Token: 0x0600E6E8 RID: 59112 RVA: 0x0055D271 File Offset: 0x0055B471
		public int Count
		{
			get
			{
				return this._trie.Count;
			}
		}

		// Token: 0x170028DC RID: 10460
		// (get) Token: 0x0600E6E9 RID: 59113 RVA: 0x0055D27E File Offset: 0x0055B47E
		public ICollection<string> Keys
		{
			get
			{
				return (from i in this._trie.Keys
				select new string(i.ToArray<char>())).ToArray<string>();
			}
		}

		// Token: 0x170028DD RID: 10461
		// (get) Token: 0x0600E6EA RID: 59114 RVA: 0x0055D2B4 File Offset: 0x0055B4B4
		public ICollection<TValue> Values
		{
			get
			{
				return this._trie.Values.ToArray<TValue>();
			}
		}

		// Token: 0x170028DE RID: 10462
		// (get) Token: 0x0600E6EB RID: 59115 RVA: 0x0055D2C6 File Offset: 0x0055B4C6
		bool ICollection<KeyValuePair<string, !0>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170028DF RID: 10463
		public TValue this[string key]
		{
			get
			{
				return this._trie[key];
			}
			set
			{
				this._trie[key] = value;
			}
		}

		// Token: 0x0600E6EE RID: 59118 RVA: 0x0055D2E6 File Offset: 0x0055B4E6
		public void Add(string key, TValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._trie.Add(key, value);
		}

		// Token: 0x0600E6EF RID: 59119 RVA: 0x0055D304 File Offset: 0x0055B504
		public void AddRange(IEnumerable<StringEntry<TValue>> collection)
		{
			foreach (StringEntry<TValue> stringEntry in collection)
			{
				this._trie.Add(stringEntry.Key, stringEntry.Value);
			}
		}

		// Token: 0x0600E6F0 RID: 59120 RVA: 0x0055D360 File Offset: 0x0055B560
		public void Clear()
		{
			this._trie.Clear();
		}

		// Token: 0x0600E6F1 RID: 59121 RVA: 0x0055D36D File Offset: 0x0055B56D
		public bool ContainsKey(string key)
		{
			return this._trie.ContainsKey(key);
		}

		// Token: 0x0600E6F2 RID: 59122 RVA: 0x0055D37B File Offset: 0x0055B57B
		public IEnumerable<StringEntry<TValue>> GetByPrefix(string prefix)
		{
			return from i in this._trie.GetByPrefix(prefix)
			select new StringEntry<TValue>(new string(i.Key.ToArray<char>()), i.Value);
		}

		// Token: 0x0600E6F3 RID: 59123 RVA: 0x0055D3AD File Offset: 0x0055B5AD
		public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
		{
			return (from i in this._trie
			select new KeyValuePair<string, TValue>(new string(i.Key.ToArray<char>()), i.Value)).GetEnumerator();
		}

		// Token: 0x0600E6F4 RID: 59124 RVA: 0x0055D3DE File Offset: 0x0055B5DE
		public bool Remove(string key)
		{
			return this._trie.Remove(key);
		}

		// Token: 0x0600E6F5 RID: 59125 RVA: 0x0055D3EC File Offset: 0x0055B5EC
		public bool TryGetValue(string key, out TValue value)
		{
			return this._trie.TryGetValue(key, out value);
		}

		// Token: 0x0600E6F6 RID: 59126 RVA: 0x0055D3FB File Offset: 0x0055B5FB
		void ICollection<KeyValuePair<string, !0>>.Add(KeyValuePair<string, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x0600E6F7 RID: 59127 RVA: 0x0055D411 File Offset: 0x0055B611
		bool ICollection<KeyValuePair<string, !0>>.Contains(KeyValuePair<string, TValue> item)
		{
			return ((ICollection<KeyValuePair<IEnumerable<char>, TValue>>)this._trie).Contains(new KeyValuePair<IEnumerable<char>, TValue>(item.Key, item.Value));
		}

		// Token: 0x0600E6F8 RID: 59128 RVA: 0x0055D431 File Offset: 0x0055B631
		public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
		{
			Array.Copy((from i in this._trie
			select new KeyValuePair<string, TValue>(new string(i.Key.ToArray<char>()), i.Value)).ToArray<KeyValuePair<string, TValue>>(), 0, array, arrayIndex, this.Count);
		}

		// Token: 0x0600E6F9 RID: 59129 RVA: 0x0055D470 File Offset: 0x0055B670
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600E6FA RID: 59130 RVA: 0x0055D478 File Offset: 0x0055B678
		bool ICollection<KeyValuePair<string, !0>>.Remove(KeyValuePair<string, TValue> item)
		{
			return ((ICollection<KeyValuePair<IEnumerable<char>, TValue>>)this._trie).Remove(new KeyValuePair<IEnumerable<char>, TValue>(item.Key, item.Value));
		}

		// Token: 0x04009287 RID: 37511
		private readonly Trie<char, TValue> _trie;
	}
}
