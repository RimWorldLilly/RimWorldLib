using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000AE RID: 174
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x00029DA4 File Offset: 0x00027FA4
		public static T Get()
		{
			T result;
			if (!FullPool<T>.freeItems.TryDequeue(out result))
			{
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00029DC6 File Offset: 0x00027FC6
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Enqueue(item);
		}

		// Token: 0x040002BB RID: 699
		private static readonly Queue<T> freeItems = new Queue<T>();
	}
}
