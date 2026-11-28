using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000AC RID: 172
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00029D59 File Offset: 0x00027F59
		public static int FreeItemsCount
		{
			get
			{
				return SimplePool<T>.freeItems.Count;
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00029D68 File Offset: 0x00027F68
		public static T Get()
		{
			T result;
			if (!SimplePool<T>.freeItems.TryDequeue(out result))
			{
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00029D8A File Offset: 0x00027F8A
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Enqueue(item);
		}

		// Token: 0x040002BA RID: 698
		private static readonly Queue<T> freeItems = new Queue<T>();
	}
}
