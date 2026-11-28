using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000DC RID: 220
	public abstract class ConditionalStatAffecter
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600084D RID: 2125
		public abstract string Label { get; }

		// Token: 0x0600084E RID: 2126
		public abstract bool Applies(StatRequest req);

		// Token: 0x040003BD RID: 957
		public List<StatModifier> statFactors;

		// Token: 0x040003BE RID: 958
		public List<StatModifier> statOffsets;
	}
}
