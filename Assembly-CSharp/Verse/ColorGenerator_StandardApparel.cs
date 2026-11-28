using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000132 RID: 306
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00036CA0 File Offset: 0x00034EA0
		public override Color ExemplaryColor
		{
			get
			{
				return new Color(0.7f, 0.7f, 0.7f);
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00036CB8 File Offset: 0x00034EB8
		public override Color NewRandomizedColor()
		{
			if (Rand.Value < 0.1f)
			{
				return Color.white;
			}
			if (Rand.Value < 0.1f)
			{
				return new Color(0.4f, 0.4f, 0.4f);
			}
			Color white = Color.white;
			float num = Rand.Range(0f, 0.6f);
			white.r -= num * Rand.Value;
			white.g -= num * Rand.Value;
			white.b -= num * Rand.Value;
			if (Rand.Value < 0.2f)
			{
				white.r *= 0.4f;
				white.g *= 0.4f;
				white.b *= 0.4f;
			}
			return white;
		}

		// Token: 0x0400058E RID: 1422
		private const float DarkAmp = 0.4f;
	}
}
