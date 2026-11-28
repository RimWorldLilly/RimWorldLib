using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000364 RID: 868
	public struct FloatMinComparer : IComparer<float>
	{
		// Token: 0x06001AA9 RID: 6825 RVA: 0x000A34EF File Offset: 0x000A16EF
		public int Compare(float x, float y)
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
