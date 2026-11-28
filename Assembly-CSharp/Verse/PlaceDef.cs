using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001A8 RID: 424
	public class PlaceDef : Def
	{
		// Token: 0x04000AE6 RID: 2790
		public RulePack placeRules;

		// Token: 0x04000AE7 RID: 2791
		[NoTranslate]
		public List<string> tags = new List<string>();
	}
}
