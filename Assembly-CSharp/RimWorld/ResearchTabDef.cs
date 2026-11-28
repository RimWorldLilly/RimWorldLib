using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD7 RID: 4055
	public class ResearchTabDef : Def
	{
		// Token: 0x04003FE6 RID: 16358
		[MustTranslate]
		public string generalTitle = "";

		// Token: 0x04003FE7 RID: 16359
		[MustTranslate]
		public string generalDescription = "";

		// Token: 0x04003FE8 RID: 16360
		public bool visibleByDefault = true;

		// Token: 0x04003FE9 RID: 16361
		public int minMonolithLevelVisible = -1;

		// Token: 0x04003FEA RID: 16362
		public string tutorTag;
	}
}
