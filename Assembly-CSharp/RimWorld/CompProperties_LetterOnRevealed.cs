using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DD RID: 6365
	public class CompProperties_LetterOnRevealed : CompProperties
	{
		// Token: 0x06009FA0 RID: 40864 RVA: 0x00393B03 File Offset: 0x00391D03
		public CompProperties_LetterOnRevealed()
		{
			this.compClass = typeof(CompLetterOnRevealed);
		}

		// Token: 0x04005E5E RID: 24158
		[MustTranslate]
		public string label;

		// Token: 0x04005E5F RID: 24159
		[MustTranslate]
		public string text;

		// Token: 0x04005E60 RID: 24160
		public LetterDef letterDef;
	}
}
