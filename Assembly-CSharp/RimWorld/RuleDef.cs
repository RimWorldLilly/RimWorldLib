using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDD RID: 4061
	public class RuleDef : Def
	{
		// Token: 0x04004003 RID: 16387
		[NoTranslate]
		public string symbol;

		// Token: 0x04004004 RID: 16388
		public List<SymbolResolver> resolvers;
	}
}
