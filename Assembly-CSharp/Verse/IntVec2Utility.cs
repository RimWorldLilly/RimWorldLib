using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000068 RID: 104
	public static class IntVec2Utility
	{
		// Token: 0x0600039A RID: 922 RVA: 0x000177A4 File Offset: 0x000159A4
		public static IntVec2 ToIntVec2(this Vector3 v)
		{
			return new IntVec2((int)v.x, (int)v.z);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000177B9 File Offset: 0x000159B9
		public static IntVec2 ToIntVec2(this Vector2 v)
		{
			return new IntVec2((int)v.x, (int)v.y);
		}
	}
}
