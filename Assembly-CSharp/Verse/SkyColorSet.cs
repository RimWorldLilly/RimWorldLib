using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000157 RID: 343
	public struct SkyColorSet
	{
		// Token: 0x06000A75 RID: 2677 RVA: 0x0003A019 File Offset: 0x00038219
		public SkyColorSet(Color sky, Color shadow, Color overlay, float saturation)
		{
			this.sky = sky;
			this.shadow = shadow;
			this.overlay = overlay;
			this.saturation = saturation;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0003A038 File Offset: 0x00038238
		public static SkyColorSet Lerp(SkyColorSet A, SkyColorSet B, float t)
		{
			return new SkyColorSet
			{
				sky = Color.Lerp(A.sky, B.sky, t),
				shadow = Color.Lerp(A.shadow, B.shadow, t),
				overlay = Color.Lerp(A.overlay, B.overlay, t),
				saturation = Mathf.Lerp(A.saturation, B.saturation, t)
			};
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0003A0B4 File Offset: 0x000382B4
		public static SkyColorSet LerpDarken(SkyColorSet A, SkyColorSet B, float t)
		{
			return new SkyColorSet
			{
				sky = Color.Lerp(A.sky, A.sky.Min(B.sky), t),
				shadow = Color.Lerp(A.shadow, A.shadow.Min(B.shadow), t),
				overlay = Color.Lerp(A.overlay, A.overlay.Min(B.overlay), t),
				saturation = Mathf.Lerp(A.saturation, Mathf.Min(A.saturation, B.saturation), t)
			};
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0003A15C File Offset: 0x0003835C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(sky=",
				this.sky.ToString(),
				", shadow=",
				this.shadow.ToString(),
				", overlay=",
				this.overlay.ToString(),
				", sat=",
				this.saturation.ToString(),
				")"
			});
		}

		// Token: 0x0400071C RID: 1820
		public Color sky;

		// Token: 0x0400071D RID: 1821
		public Color shadow;

		// Token: 0x0400071E RID: 1822
		public Color overlay;

		// Token: 0x0400071F RID: 1823
		public float saturation;
	}
}
