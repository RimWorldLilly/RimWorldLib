using System;

namespace Verse
{
	// Token: 0x020001C0 RID: 448
	public class ThingDefStyle
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x000410F9 File Offset: 0x0003F2F9
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00041101 File Offset: 0x0003F301
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.styleDef;
			}
		}

		// Token: 0x04000BB0 RID: 2992
		private ThingDef thingDef;

		// Token: 0x04000BB1 RID: 2993
		private ThingStyleDef styleDef;
	}
}
