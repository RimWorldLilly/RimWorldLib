using System;

namespace Verse
{
	// Token: 0x02000453 RID: 1107
	public class HediffCompProperties_LetterOnDeath : HediffCompProperties
	{
		// Token: 0x06001FA0 RID: 8096 RVA: 0x000BFEE4 File Offset: 0x000BE0E4
		public HediffCompProperties_LetterOnDeath()
		{
			this.compClass = typeof(HediffComp_LetterOnDeath);
		}

		// Token: 0x0400189E RID: 6302
		public LetterDef letterDef;

		// Token: 0x0400189F RID: 6303
		[MustTranslate]
		public string letterText;

		// Token: 0x040018A0 RID: 6304
		[MustTranslate]
		public string letterLabel;

		// Token: 0x040018A1 RID: 6305
		public bool onlyIfNoMechanitorDied;
	}
}
