using System;
using Unity.Mathematics;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000090 RID: 144
	public static class EasingFunctions
	{
		// Token: 0x0600066A RID: 1642 RVA: 0x000259A3 File Offset: 0x00023BA3
		public static float EaseInOutQuad(float v)
		{
			if ((double)v >= 0.5)
			{
				return 1f - Mathf.Pow(-2f * v + 2f, 4f) / 2f;
			}
			return 8f * v * v * v * v;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000259E4 File Offset: 0x00023BE4
		public static float EaseInOutQuint(float x)
		{
			if ((double)x >= 0.5)
			{
				return 1f - Mathf.Pow(-2f * x + 2f, 5f) / 2f;
			}
			return 16f * x * x * x * x * x;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00025A31 File Offset: 0x00023C31
		public static float EaseInCubic(float x)
		{
			return x * x * x;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00025A38 File Offset: 0x00023C38
		public static float EaseOutCubic(float x)
		{
			return 1f - math.pow(1f - x, 3f);
		}
	}
}
