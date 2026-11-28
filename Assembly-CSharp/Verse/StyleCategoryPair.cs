using System;

namespace Verse
{
	// Token: 0x020001C2 RID: 450
	public class StyleCategoryPair : IExposable
	{
		// Token: 0x06000C5C RID: 3164 RVA: 0x00041157 File Offset: 0x0003F357
		public void ExposeData()
		{
			Scribe_Defs.Look<StyleCategoryDef>(ref this.category, "category");
			Scribe_Defs.Look<ThingStyleDef>(ref this.styleDef, "styleDef");
		}

		// Token: 0x04000BB4 RID: 2996
		public StyleCategoryDef category;

		// Token: 0x04000BB5 RID: 2997
		public ThingStyleDef styleDef;
	}
}
