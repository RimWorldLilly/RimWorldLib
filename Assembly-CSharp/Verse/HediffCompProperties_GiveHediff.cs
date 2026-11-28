using System;

namespace Verse
{
	// Token: 0x02000437 RID: 1079
	public class HediffCompProperties_GiveHediff : HediffCompProperties
	{
		// Token: 0x06001F40 RID: 8000 RVA: 0x000BE4B1 File Offset: 0x000BC6B1
		public HediffCompProperties_GiveHediff()
		{
			this.compClass = typeof(HediffComp_GiveHediff);
		}

		// Token: 0x04001865 RID: 6245
		public HediffDef hediffDef;

		// Token: 0x04001866 RID: 6246
		public bool skipIfAlreadyExists;

		// Token: 0x04001867 RID: 6247
		public float atSeverity = 1f;

		// Token: 0x04001868 RID: 6248
		public bool disappearsAfterGiving;

		// Token: 0x04001869 RID: 6249
		public LetterDef letterDef;

		// Token: 0x0400186A RID: 6250
		[MustTranslate]
		public string letterLabel;

		// Token: 0x0400186B RID: 6251
		[MustTranslate]
		public string letterText;
	}
}
