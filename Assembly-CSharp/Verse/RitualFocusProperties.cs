using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000156 RID: 342
	public class RitualFocusProperties
	{
		// Token: 0x04000719 RID: 1817
		public IntRange spectateDistance = new IntRange(2, 2);

		// Token: 0x0400071A RID: 1818
		public SpectateRectSide allowedSpectateSides = SpectateRectSide.Horizontal;

		// Token: 0x0400071B RID: 1819
		public bool consumable;
	}
}
