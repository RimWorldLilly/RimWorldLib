using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064C RID: 1612
	public static class MouseoverUtility
	{
		// Token: 0x060032A4 RID: 12964 RVA: 0x00143152 File Offset: 0x00141352
		static MouseoverUtility()
		{
			MouseoverUtility.MakePermaCache();
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x00143159 File Offset: 0x00141359
		public static void Reset()
		{
			MouseoverUtility.MakePermaCache();
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x00143160 File Offset: 0x00141360
		private static void MakePermaCache()
		{
			MouseoverUtility.glowStrings = new string[101];
			for (int i = 0; i <= 100; i++)
			{
				MouseoverUtility.glowStrings[i] = GlowGrid.PsychGlowAtGlow((float)i / 100f).GetLabel() + " (" + ((float)i / 100f).ToStringPercent() + ")";
			}
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x001431BB File Offset: 0x001413BB
		public static string GetGlowLabelByValue(float value)
		{
			return MouseoverUtility.glowStrings[Mathf.RoundToInt(value * 100f)];
		}

		// Token: 0x0400205C RID: 8284
		private static string[] glowStrings;
	}
}
