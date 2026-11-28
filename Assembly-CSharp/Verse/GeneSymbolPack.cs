using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000BF RID: 191
	public class GeneSymbolPack
	{
		// Token: 0x04000341 RID: 833
		public List<GeneSymbolPack.WeightedSymbol> prefixSymbols;

		// Token: 0x04000342 RID: 834
		public List<GeneSymbolPack.WeightedSymbol> suffixSymbols;

		// Token: 0x04000343 RID: 835
		public List<GeneSymbolPack.WeightedSymbol> wholeNameSymbols;

		// Token: 0x020024EA RID: 9450
		public class WeightedSymbol
		{
			// Token: 0x04009710 RID: 38672
			[MustTranslate]
			public string symbol;

			// Token: 0x04009711 RID: 38673
			public float weight = 1f;
		}
	}
}
