using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FAB RID: 4011
	public class IdeoDescriptionMaker
	{
		// Token: 0x04003D7D RID: 15741
		public List<IdeoDescriptionMaker.PatternEntry> patterns;

		// Token: 0x04003D7E RID: 15742
		public RulePack rules;

		// Token: 0x04003D7F RID: 15743
		public Dictionary<string, string> constants;

		// Token: 0x02002F72 RID: 12146
		public class PatternEntry
		{
			// Token: 0x0400C0E7 RID: 49383
			public IdeoStoryPatternDef def;

			// Token: 0x0400C0E8 RID: 49384
			public float weight = 1f;
		}
	}
}
