using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KTrie
{
	// Token: 0x0200240D RID: 9229
	public class TrieSet<T> : ICollection<IEnumerable<T>>, IEnumerable<IEnumerable<T>>, IEnumerable
	{
		// Token: 0x0600E71F RID: 59167 RVA: 0x0055D898 File Offset: 0x0055BA98
		public TrieSet() : this(EqualityComparer<T>.Default)
		{
		}

		// Token: 0x0600E720 RID: 59168 RVA: 0x0055D8A8 File Offset: 0x0055BAA8
		public TrieSet(IEqualityComparer<T> comparer)
		{
			this._comparer = comparer;
			this._root = new TrieSet<T>.TrieNode(default(T), comparer);
		}

		// Token: 0x170028E9 RID: 10473
		// (get) Token: 0x0600E721 RID: 59169 RVA: 0x0055D8D7 File Offset: 0x0055BAD7
		// (set) Token: 0x0600E722 RID: 59170 RVA: 0x0055D8DF File Offset: 0x0055BADF
		public int Count { get; private set; }

		// Token: 0x170028EA RID: 10474
		// (get) Token: 0x0600E723 RID: 59171 RVA: 0x0055D8E8 File Offset: 0x0055BAE8
		bool ICollection<IEnumerable<!0>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E724 RID: 59172 RVA: 0x0055D8EB File Offset: 0x0055BAEB
		public IEnumerator<IEnumerable<T>> GetEnumerator()
		{
			return TrieSet<T>.GetAllNodes(this._root).Select(new Func<TrieSet<T>.TrieNode, IEnumerable<T>>(TrieSet<T>.GetFullKey)).GetEnumerator();
		}

		// Token: 0x0600E725 RID: 59173 RVA: 0x0055D90E File Offset: 0x0055BB0E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600E726 RID: 59174 RVA: 0x0055D918 File Offset: 0x0055BB18
		public void Add(IEnumerable<T> key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			TrieSet<T>.TrieNode trieNode = this._root;
			foreach (T key2 in key)
			{
				trieNode = this.AddItem(trieNode, key2);
			}
			if (trieNode.IsTerminal)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("An element with the same key already exists: '");
				defaultInterpolatedStringHandler.AppendFormatted<IEnumerable<T>>(key);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "key");
			}
			trieNode.IsTerminal = true;
			trieNode.Item = key;
			int count = this.Count;
			this.Count = count + 1;
		}

		// Token: 0x0600E727 RID: 59175 RVA: 0x0055D9DC File Offset: 0x0055BBDC
		public void AddRange(IEnumerable<IEnumerable<T>> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (IEnumerable<T> key in collection)
			{
				this.Add(key);
			}
		}

		// Token: 0x0600E728 RID: 59176 RVA: 0x0055DA34 File Offset: 0x0055BC34
		public void Clear()
		{
			this._root.Children.Clear();
			this.Count = 0;
		}

		// Token: 0x0600E729 RID: 59177 RVA: 0x0055DA50 File Offset: 0x0055BC50
		public bool Contains(IEnumerable<T> item)
		{
			TrieSet<T>.TrieNode node = this.GetNode(item);
			return node != null && node.IsTerminal;
		}

		// Token: 0x0600E72A RID: 59178 RVA: 0x0055DA70 File Offset: 0x0055BC70
		public void CopyTo(IEnumerable<T>[] array, int arrayIndex)
		{
			Array.Copy(TrieSet<T>.GetAllNodes(this._root).Select(new Func<TrieSet<T>.TrieNode, IEnumerable<T>>(TrieSet<T>.GetFullKey)).ToArray<IEnumerable<T>>(), 0, array, arrayIndex, this.Count);
		}

		// Token: 0x0600E72B RID: 59179 RVA: 0x0055DAA4 File Offset: 0x0055BCA4
		public bool Remove(IEnumerable<T> key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			TrieSet<T>.TrieNode node = this.GetNode(key);
			if (node == null)
			{
				return false;
			}
			if (!node.IsTerminal)
			{
				return false;
			}
			this.RemoveNode(node);
			return true;
		}

		// Token: 0x0600E72C RID: 59180 RVA: 0x0055DAE0 File Offset: 0x0055BCE0
		public bool TryGetItem(IEnumerable<T> key, out IEnumerable<T> item)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			TrieSet<T>.TrieNode node = this.GetNode(key);
			item = null;
			if (node == null)
			{
				return false;
			}
			if (!node.IsTerminal)
			{
				return false;
			}
			item = node.Item;
			return true;
		}

		// Token: 0x0600E72D RID: 59181 RVA: 0x0055DB1E File Offset: 0x0055BD1E
		internal bool TryGetNode(IEnumerable<T> key, out TrieSet<T>.TrieNode node)
		{
			node = this.GetNode(key);
			return node != null && node.IsTerminal;
		}

		// Token: 0x0600E72E RID: 59182 RVA: 0x0055DB3C File Offset: 0x0055BD3C
		public IEnumerable<IEnumerable<T>> GetByPrefix(IEnumerable<T> prefix)
		{
			if (prefix == null)
			{
				throw new ArgumentNullException("prefix");
			}
			TrieSet<T>.TrieNode root = this._root;
			foreach (T key in prefix)
			{
				if (!root.Children.TryGetValue(key, out root))
				{
					return Enumerable.Empty<IEnumerable<T>>();
				}
			}
			return TrieSet<T>.GetByPrefix(root);
		}

		// Token: 0x0600E72F RID: 59183 RVA: 0x0055DBB4 File Offset: 0x0055BDB4
		private static IEnumerable<TrieSet<T>.TrieNode> GetAllNodes(TrieSet<T>.TrieNode node)
		{
			TrieSet<T>.<GetAllNodes>d__21 <GetAllNodes>d__ = new TrieSet<T>.<GetAllNodes>d__21(-2);
			<GetAllNodes>d__.<>3__node = node;
			return <GetAllNodes>d__;
		}

		// Token: 0x0600E730 RID: 59184 RVA: 0x0055DBC4 File Offset: 0x0055BDC4
		private static IEnumerable<IEnumerable<T>> GetByPrefix(TrieSet<T>.TrieNode node)
		{
			TrieSet<T>.<GetByPrefix>d__22 <GetByPrefix>d__ = new TrieSet<T>.<GetByPrefix>d__22(-2);
			<GetByPrefix>d__.<>3__node = node;
			return <GetByPrefix>d__;
		}

		// Token: 0x0600E731 RID: 59185 RVA: 0x0055DBD4 File Offset: 0x0055BDD4
		private static IEnumerable<T> GetFullKey(TrieSet<T>.TrieNode node)
		{
			return node.Item;
		}

		// Token: 0x0600E732 RID: 59186 RVA: 0x0055DBDC File Offset: 0x0055BDDC
		private TrieSet<T>.TrieNode GetNode(IEnumerable<T> key)
		{
			TrieSet<T>.TrieNode root = this._root;
			foreach (T key2 in key)
			{
				if (!root.Children.TryGetValue(key2, out root))
				{
					return null;
				}
			}
			return root;
		}

		// Token: 0x0600E733 RID: 59187 RVA: 0x0055DC3C File Offset: 0x0055BE3C
		private void RemoveNode(TrieSet<T>.TrieNode node)
		{
			this.Remove(node);
			int count = this.Count;
			this.Count = count - 1;
		}

		// Token: 0x0600E734 RID: 59188 RVA: 0x0055DC60 File Offset: 0x0055BE60
		private TrieSet<T>.TrieNode AddItem(TrieSet<T>.TrieNode node, T key)
		{
			TrieSet<T>.TrieNode trieNode;
			if (!node.Children.TryGetValue(key, out trieNode))
			{
				trieNode = new TrieSet<T>.TrieNode(key, this._comparer)
				{
					Parent = node
				};
				node.Children.Add(key, trieNode);
			}
			return trieNode;
		}

		// Token: 0x0600E735 RID: 59189 RVA: 0x0055DCA0 File Offset: 0x0055BEA0
		private void Remove(TrieSet<T>.TrieNode node, T key)
		{
			foreach (KeyValuePair<T, TrieSet<T>.TrieNode> item in node.Children)
			{
				if (this._comparer.Equals(key, item.Key))
				{
					node.Children.Remove(item);
					break;
				}
			}
		}

		// Token: 0x0600E736 RID: 59190 RVA: 0x0055DD0C File Offset: 0x0055BF0C
		private void Remove(TrieSet<T>.TrieNode node)
		{
			for (;;)
			{
				node.IsTerminal = false;
				if (node.Children.Count != 0 || node.Parent == null)
				{
					break;
				}
				this.Remove(node.Parent, node.Key);
				if (node.Parent.IsTerminal)
				{
					break;
				}
				node = node.Parent;
			}
		}

		// Token: 0x0400928C RID: 37516
		private readonly IEqualityComparer<T> _comparer;

		// Token: 0x0400928D RID: 37517
		private readonly TrieSet<T>.TrieNode _root;

		// Token: 0x02003F20 RID: 16160
		internal sealed class TrieNode
		{
			// Token: 0x06016532 RID: 91442 RVA: 0x006EED07 File Offset: 0x006ECF07
			public TrieNode(T key, IEqualityComparer<T> comparer)
			{
				this.Key = key;
				this.Children = new Dictionary<T, TrieSet<T>.TrieNode>(comparer);
			}

			// Token: 0x170037EF RID: 14319
			// (get) Token: 0x06016533 RID: 91443 RVA: 0x006EED22 File Offset: 0x006ECF22
			// (set) Token: 0x06016534 RID: 91444 RVA: 0x006EED2A File Offset: 0x006ECF2A
			public bool IsTerminal { get; set; }

			// Token: 0x170037F0 RID: 14320
			// (get) Token: 0x06016535 RID: 91445 RVA: 0x006EED33 File Offset: 0x006ECF33
			public T Key { get; }

			// Token: 0x170037F1 RID: 14321
			// (get) Token: 0x06016536 RID: 91446 RVA: 0x006EED3B File Offset: 0x006ECF3B
			// (set) Token: 0x06016537 RID: 91447 RVA: 0x006EED43 File Offset: 0x006ECF43
			public IEnumerable<T> Item { get; set; }

			// Token: 0x170037F2 RID: 14322
			// (get) Token: 0x06016538 RID: 91448 RVA: 0x006EED4C File Offset: 0x006ECF4C
			public IDictionary<T, TrieSet<T>.TrieNode> Children { get; }

			// Token: 0x170037F3 RID: 14323
			// (get) Token: 0x06016539 RID: 91449 RVA: 0x006EED54 File Offset: 0x006ECF54
			// (set) Token: 0x0601653A RID: 91450 RVA: 0x006EED5C File Offset: 0x006ECF5C
			public TrieSet<T>.TrieNode Parent { get; set; }
		}
	}
}
