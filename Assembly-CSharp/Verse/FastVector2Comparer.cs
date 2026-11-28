using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000507 RID: 1287
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x0600261C RID: 9756 RVA: 0x000E8491 File Offset: 0x000E6691
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000E849A File Offset: 0x000E669A
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04001B6E RID: 7022
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();
	}
}
