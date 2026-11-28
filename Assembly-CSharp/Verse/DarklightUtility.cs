using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000249 RID: 585
	public static class DarklightUtility
	{
		// Token: 0x0600113C RID: 4412 RVA: 0x00062ED4 File Offset: 0x000610D4
		public static bool IsDarklight(Color color)
		{
			if (color.r > color.g || color.r > color.b)
			{
				return false;
			}
			float num;
			float num2;
			if (color.g > color.b)
			{
				num = color.g;
				num2 = color.b;
			}
			else
			{
				num = color.b;
				num2 = color.g;
			}
			return num != 0f && color.r <= num / 2f && num2 / num > 0.85f;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00062F53 File Offset: 0x00061153
		public static bool IsDarklightAt(IntVec3 position, Map map)
		{
			return position.InBounds(map) && position.Roofed(map) && map.glowGrid.PsychGlowAt(position) >= PsychGlow.Lit && DarklightUtility.IsDarklight(map.glowGrid.VisualGlowAt(position));
		}

		// Token: 0x04000FC3 RID: 4035
		private static FloatRange DarklightHueRange = new FloatRange(0.49f, 0.51f);

		// Token: 0x04000FC4 RID: 4036
		public static readonly Color DefaultDarklight = new Color32(78, 226, 229, byte.MaxValue);
	}
}
