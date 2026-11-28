using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200028E RID: 654
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x060013C6 RID: 5062 RVA: 0x000701B5 File Offset: 0x0006E3B5
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x000701BB File Offset: 0x0006E3BB
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}

		// Token: 0x04001114 RID: 4372
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();
	}
}
