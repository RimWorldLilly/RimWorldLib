using System;

namespace Verse
{
	// Token: 0x020001D1 RID: 465
	public class ThingStyleChance
	{
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00043B01 File Offset: 0x00041D01
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.styleDef;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x00043B09 File Offset: 0x00041D09
		public float Chance
		{
			get
			{
				return this.chance;
			}
		}

		// Token: 0x04000D23 RID: 3363
		private ThingStyleDef styleDef;

		// Token: 0x04000D24 RID: 3364
		private float chance = 1f;
	}
}
