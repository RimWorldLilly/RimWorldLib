using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200028F RID: 655
	public class ThingDefComparer : IEqualityComparer<ThingDef>
	{
		// Token: 0x060013CA RID: 5066 RVA: 0x000701D2 File Offset: 0x0006E3D2
		public bool Equals(ThingDef x, ThingDef y)
		{
			return (x == null && y == null) || (x != null && y != null && x.shortHash == y.shortHash);
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x000701F2 File Offset: 0x0006E3F2
		public int GetHashCode(ThingDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04001115 RID: 4373
		public static readonly ThingDefComparer Instance = new ThingDefComparer();
	}
}
