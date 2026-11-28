using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02002155 RID: 8533
	public class QuestTextRequest
	{
		// Token: 0x04008728 RID: 34600
		public string keyword;

		// Token: 0x04008729 RID: 34601
		public Action<string> setter;

		// Token: 0x0400872A RID: 34602
		public List<Rule> extraRules;
	}
}
