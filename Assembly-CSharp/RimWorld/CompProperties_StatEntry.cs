using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001367 RID: 4967
	public class CompProperties_StatEntry : CompProperties
	{
		// Token: 0x06007B20 RID: 31520 RVA: 0x002BD24F File Offset: 0x002BB44F
		public CompProperties_StatEntry()
		{
			this.compClass = typeof(CompStatEntry);
		}

		// Token: 0x04004DBA RID: 19898
		public StatCategoryDef statCategoryDef;

		// Token: 0x04004DBB RID: 19899
		[MustTranslate]
		public string statLabel;

		// Token: 0x04004DBC RID: 19900
		[MustTranslate]
		public string reportText;

		// Token: 0x04004DBD RID: 19901
		public string valueString;

		// Token: 0x04004DBE RID: 19902
		public Func<CompStatEntry, string> valueFunc;

		// Token: 0x04004DBF RID: 19903
		public int displayPriorityInCategory;
	}
}
