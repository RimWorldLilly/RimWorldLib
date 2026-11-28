using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000AF RID: 175
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x06000749 RID: 1865 RVA: 0x00029DEC File Offset: 0x00027FEC
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			List<T> list = this.items;
			int num = this.readIndex;
			this.readIndex = num + 1;
			return list[num];
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00029E38 File Offset: 0x00028038
		public void Clear()
		{
			this.readIndex = 0;
		}

		// Token: 0x040002BC RID: 700
		private readonly List<T> items = new List<T>();

		// Token: 0x040002BD RID: 701
		private int readIndex;
	}
}
