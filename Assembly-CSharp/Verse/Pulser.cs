using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000651 RID: 1617
	public static class Pulser
	{
		// Token: 0x060032B3 RID: 12979 RVA: 0x001434AF File Offset: 0x001416AF
		public static float PulseBrightness(float frequency, float amplitude)
		{
			return Pulser.PulseBrightness(frequency, amplitude, Time.realtimeSinceStartup);
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x001434C0 File Offset: 0x001416C0
		public static float PulseBrightness(float frequency, float amplitude, float time)
		{
			float num = time * 6.2831855f;
			num *= frequency;
			float t = (1f - Mathf.Cos(num)) * 0.5f;
			return Mathf.Lerp(1f - amplitude, 1f, t);
		}
	}
}
