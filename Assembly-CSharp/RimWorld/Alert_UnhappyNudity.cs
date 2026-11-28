using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C23 RID: 7203
	public class Alert_UnhappyNudity : Alert_Thought
	{
		// Token: 0x17002126 RID: 8486
		// (get) Token: 0x0600B7B5 RID: 47029 RVA: 0x0040C02A File Offset: 0x0040A22A
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}

		// Token: 0x0600B7B6 RID: 47030 RVA: 0x0040C031 File Offset: 0x0040A231
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}
	}
}
