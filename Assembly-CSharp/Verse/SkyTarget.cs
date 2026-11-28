using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000320 RID: 800
	public struct SkyTarget
	{
		// Token: 0x06001890 RID: 6288 RVA: 0x00093D80 File Offset: 0x00091F80
		public SkyTarget(float glow, SkyColorSet colorSet, float lightsourceShineSize, float lightsourceShineIntensity)
		{
			this.glow = glow;
			this.lightsourceShineSize = lightsourceShineSize;
			this.lightsourceShineIntensity = lightsourceShineIntensity;
			this.colors = colorSet;
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x00093DA0 File Offset: 0x00091FA0
		public static SkyTarget Lerp(SkyTarget A, SkyTarget B, float t)
		{
			return new SkyTarget
			{
				colors = SkyColorSet.Lerp(A.colors, B.colors, t),
				glow = Mathf.Lerp(A.glow, B.glow, t),
				lightsourceShineSize = Mathf.Lerp(A.lightsourceShineSize, B.lightsourceShineSize, t),
				lightsourceShineIntensity = Mathf.Lerp(A.lightsourceShineIntensity, B.lightsourceShineIntensity, t)
			};
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x00093E1C File Offset: 0x0009201C
		public static SkyTarget LerpDarken(SkyTarget A, SkyTarget B, float t)
		{
			return new SkyTarget
			{
				colors = SkyColorSet.LerpDarken(A.colors, B.colors, t),
				glow = Mathf.Lerp(A.glow, Mathf.Min(A.glow, B.glow), t),
				lightsourceShineSize = Mathf.Lerp(A.lightsourceShineSize, Mathf.Min(A.lightsourceShineSize, B.lightsourceShineSize), t),
				lightsourceShineIntensity = Mathf.Lerp(A.lightsourceShineIntensity, Mathf.Min(A.lightsourceShineIntensity, B.lightsourceShineIntensity), t)
			};
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x00093EB8 File Offset: 0x000920B8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(glow=",
				this.glow.ToString("F2"),
				", colors=",
				this.colors.ToString(),
				", lightsourceShineSize=",
				this.lightsourceShineSize.ToString(),
				", lightsourceShineIntensity=",
				this.lightsourceShineIntensity.ToString(),
				")"
			});
		}

		// Token: 0x04001474 RID: 5236
		public float glow;

		// Token: 0x04001475 RID: 5237
		public SkyColorSet colors;

		// Token: 0x04001476 RID: 5238
		public float lightsourceShineSize;

		// Token: 0x04001477 RID: 5239
		public float lightsourceShineIntensity;
	}
}
