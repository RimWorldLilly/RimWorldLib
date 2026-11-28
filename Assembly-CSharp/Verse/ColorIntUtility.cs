using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000063 RID: 99
	public static class ColorIntUtility
	{
		// Token: 0x0600033C RID: 828 RVA: 0x00016EF6 File Offset: 0x000150F6
		public static ColorInt AsColorInt(this Color32 col)
		{
			return new ColorInt(col);
		}
	}
}
