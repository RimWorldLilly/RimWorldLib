using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019EC RID: 6636
	public abstract class CompProperties_GrayStatue : CompProperties
	{
		// Token: 0x04006274 RID: 25204
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04006275 RID: 25205
		[MustTranslate]
		public string letterText;

		// Token: 0x04006276 RID: 25206
		public LetterDef letterDef;

		// Token: 0x04006277 RID: 25207
		public FloatRange triggerRadiusRange = new FloatRange(3f, 8f);
	}
}
