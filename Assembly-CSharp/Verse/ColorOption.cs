using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000134 RID: 308
	public class ColorOption
	{
		// Token: 0x060009E7 RID: 2535 RVA: 0x00036EBC File Offset: 0x000350BC
		public Color RandomizedColor()
		{
			if (this.only.a >= 0f)
			{
				return this.only;
			}
			return new Color(Rand.Range(this.min.r, this.max.r), Rand.Range(this.min.g, this.max.g), Rand.Range(this.min.b, this.max.b), Rand.Range(this.min.a, this.max.a));
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00036F53 File Offset: 0x00035153
		public void SetSingle(Color color)
		{
			this.only = color;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00036F5C File Offset: 0x0003515C
		public void SetMin(Color color)
		{
			this.min = color;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00036F65 File Offset: 0x00035165
		public void SetMax(Color color)
		{
			this.max = color;
		}

		// Token: 0x04000590 RID: 1424
		public float weight = 1f;

		// Token: 0x04000591 RID: 1425
		public Color min = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000592 RID: 1426
		public Color max = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000593 RID: 1427
		public Color only = new Color(-1f, -1f, -1f, -1f);
	}
}
