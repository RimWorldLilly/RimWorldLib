using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000690 RID: 1680
	public static class ColorExtension
	{
		// Token: 0x06003512 RID: 13586 RVA: 0x00158F90 File Offset: 0x00157190
		public static Color ToOpaque(this Color c)
		{
			c.a = 1f;
			return c;
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x00158F9F File Offset: 0x0015719F
		public static Color ToTransparent(this Color c, float transparency)
		{
			c.a = transparency;
			return c;
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x00158FAC File Offset: 0x001571AC
		public static Color Min(this Color c, Color other)
		{
			c.r = Mathf.Min(c.r, other.r);
			c.g = Mathf.Min(c.g, other.g);
			c.b = Mathf.Min(c.b, other.b);
			return c;
		}
	}
}
