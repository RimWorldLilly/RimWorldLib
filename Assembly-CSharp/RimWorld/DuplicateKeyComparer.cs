using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020020A9 RID: 8361
	public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
	{
		// Token: 0x0600D2C8 RID: 53960 RVA: 0x004D881C File Offset: 0x004D6A1C
		public int Compare(TKey x, TKey y)
		{
			int num = x.CompareTo(y);
			if (num == 0)
			{
				return -1;
			}
			return num;
		}
	}
}
