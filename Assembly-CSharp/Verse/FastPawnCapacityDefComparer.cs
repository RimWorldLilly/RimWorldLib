using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200019A RID: 410
	public class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x06000B6C RID: 2924 RVA: 0x0003D928 File Offset: 0x0003BB28
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0003D92E File Offset: 0x0003BB2E
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x040009EE RID: 2542
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();
	}
}
