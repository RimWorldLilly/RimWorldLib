using System;

namespace Verse
{
	// Token: 0x020001C1 RID: 449
	public class ThingStyleCategoryWithPriority : IExposable
	{
		// Token: 0x06000C59 RID: 3161 RVA: 0x00041111 File Offset: 0x0003F311
		public ThingStyleCategoryWithPriority()
		{
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00041119 File Offset: 0x0003F319
		public ThingStyleCategoryWithPriority(StyleCategoryDef category, float priority)
		{
			this.category = category;
			this.priority = priority;
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0004112F File Offset: 0x0003F32F
		public void ExposeData()
		{
			Scribe_Defs.Look<StyleCategoryDef>(ref this.category, "category");
			Scribe_Values.Look<float>(ref this.priority, "priority", 0f, false);
		}

		// Token: 0x04000BB2 RID: 2994
		public StyleCategoryDef category;

		// Token: 0x04000BB3 RID: 2995
		public float priority;
	}
}
