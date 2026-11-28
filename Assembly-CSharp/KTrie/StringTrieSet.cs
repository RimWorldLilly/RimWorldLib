using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KTrie
{
	// Token: 0x0200240A RID: 9226
	public class StringTrieSet : ICollection<string>, IEnumerable<string>, IEnumerable
	{
		// Token: 0x0600E6FB RID: 59131 RVA: 0x0055D498 File Offset: 0x0055B698
		public StringTrieSet() : this(EqualityComparer<char>.Default)
		{
		}

		// Token: 0x0600E6FC RID: 59132 RVA: 0x0055D4A5 File Offset: 0x0055B6A5
		public StringTrieSet(IEqualityComparer<char> comparer)
		{
			this._trie = new TrieSet<char>(comparer);
		}

		// Token: 0x170028E0 RID: 10464
		// (get) Token: 0x0600E6FD RID: 59133 RVA: 0x0055D4B9 File Offset: 0x0055B6B9
		public int Count
		{
			get
			{
				return this._trie.Count;
			}
		}

		// Token: 0x170028E1 RID: 10465
		// (get) Token: 0x0600E6FE RID: 59134 RVA: 0x0055D4C6 File Offset: 0x0055B6C6
		bool ICollection<string>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E6FF RID: 59135 RVA: 0x0055D4C9 File Offset: 0x0055B6C9
		public IEnumerable<string> GetByPrefix(string prefix)
		{
			return from c in this._trie.GetByPrefix(prefix)
			select new string(c.ToArray<char>());
		}

		// Token: 0x0600E700 RID: 59136 RVA: 0x0055D4FB File Offset: 0x0055B6FB
		public IEnumerator<string> GetEnumerator()
		{
			return (from c in this._trie
			select new string(c.ToArray<char>())).GetEnumerator();
		}

		// Token: 0x0600E701 RID: 59137 RVA: 0x0055D52C File Offset: 0x0055B72C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600E702 RID: 59138 RVA: 0x0055D534 File Offset: 0x0055B734
		public void Add(string item)
		{
			this._trie.Add(item);
		}

		// Token: 0x0600E703 RID: 59139 RVA: 0x0055D542 File Offset: 0x0055B742
		public void AddRange(IEnumerable<string> item)
		{
			this._trie.AddRange(item);
		}

		// Token: 0x0600E704 RID: 59140 RVA: 0x0055D550 File Offset: 0x0055B750
		public void Clear()
		{
			this._trie.Clear();
		}

		// Token: 0x0600E705 RID: 59141 RVA: 0x0055D55D File Offset: 0x0055B75D
		public bool Contains(string item)
		{
			return this._trie.Contains(item);
		}

		// Token: 0x0600E706 RID: 59142 RVA: 0x0055D56B File Offset: 0x0055B76B
		public void CopyTo(string[] array, int arrayIndex)
		{
			Array.Copy((from c in this._trie
			select new string(c.ToArray<char>())).ToArray<string>(), 0, array, arrayIndex, this.Count);
		}

		// Token: 0x0600E707 RID: 59143 RVA: 0x0055D5AA File Offset: 0x0055B7AA
		public bool Remove(string item)
		{
			return this._trie.Remove(item);
		}

		// Token: 0x04009288 RID: 37512
		private readonly TrieSet<char> _trie;
	}
}
