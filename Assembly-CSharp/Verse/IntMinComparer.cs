using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000365 RID: 869
	public struct IntMinComparer : IComparer<int>
	{
		// Token: 0x06001AAA RID: 6826 RVA: 0x000A34FE File Offset: 0x000A16FE
		public int Compare(int x, int y)
		{
			if (x < y)
			{
				return -1;
			}
			if (x > y)
			{
				return 1;
			}
			return 0;
		}
	}
}
