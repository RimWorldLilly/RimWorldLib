using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200017B RID: 379
	public class IdeoStoryPatternDef : Def
	{
		// Token: 0x06000B16 RID: 2838 RVA: 0x0003C853 File Offset: 0x0003AA53
		public override IEnumerable<string> ConfigErrors()
		{
			IdeoStoryPatternDef.<ConfigErrors>d__3 <ConfigErrors>d__ = new IdeoStoryPatternDef.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400091F RID: 2335
		[NoTranslate]
		public List<string> segments = new List<string>();

		// Token: 0x04000920 RID: 2336
		public List<string> noCapitalizeFirstSentence = new List<string>();

		// Token: 0x04000921 RID: 2337
		public RulePack rules;
	}
}
