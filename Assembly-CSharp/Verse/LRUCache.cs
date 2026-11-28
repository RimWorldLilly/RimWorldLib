using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200006D RID: 109
	public class LRUCache<K, V>
	{
		// Token: 0x060003F5 RID: 1013 RVA: 0x00018936 File Offset: 0x00016B36
		public LRUCache(int capacity)
		{
			this.capacity = capacity;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0001895C File Offset: 0x00016B5C
		public bool TryGetValue(K key, out V result)
		{
			LinkedListNode<ValueTuple<K, V>> linkedListNode;
			if (this.cache.TryGetValue(key, out linkedListNode))
			{
				result = linkedListNode.Value.Item2;
				this.WasUsed(linkedListNode);
				return true;
			}
			result = default(V);
			return false;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001899C File Offset: 0x00016B9C
		public void Add(K key, V value)
		{
			if (this.cache.Count > this.capacity)
			{
				this.RemoveLeastUsed();
			}
			LinkedListNode<ValueTuple<K, V>> linkedListNode = new LinkedListNode<ValueTuple<K, V>>(new ValueTuple<K, V>(key, value));
			this.cache.Add(key, linkedListNode);
			this.leastRecentList.AddLast(linkedListNode);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x000189E8 File Offset: 0x00016BE8
		public void Clear()
		{
			this.cache.Clear();
			this.leastRecentList.Clear();
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00018A00 File Offset: 0x00016C00
		private void WasUsed(LinkedListNode<ValueTuple<K, V>> node)
		{
			this.leastRecentList.Remove(node);
			this.leastRecentList.AddLast(node);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00018A1C File Offset: 0x00016C1C
		private void RemoveLeastUsed()
		{
			LinkedListNode<ValueTuple<K, V>> first = this.leastRecentList.First;
			if (first != null)
			{
				this.leastRecentList.RemoveFirst();
				this.cache.Remove(first.Value.Item1);
			}
		}

		// Token: 0x04000189 RID: 393
		private readonly Dictionary<K, LinkedListNode<ValueTuple<K, V>>> cache = new Dictionary<K, LinkedListNode<ValueTuple<K, V>>>();

		// Token: 0x0400018A RID: 394
		private readonly LinkedList<ValueTuple<K, V>> leastRecentList = new LinkedList<ValueTuple<K, V>>();

		// Token: 0x0400018B RID: 395
		private readonly int capacity;
	}
}
