using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD8 RID: 4056
	public class RiverDef : Def
	{
		// Token: 0x04003FEB RID: 16363
		public int spawnFlowThreshold = -1;

		// Token: 0x04003FEC RID: 16364
		public float spawnChance = 1f;

		// Token: 0x04003FED RID: 16365
		public int degradeThreshold;

		// Token: 0x04003FEE RID: 16366
		public RiverDef degradeChild;

		// Token: 0x04003FEF RID: 16367
		public List<RiverDef.Branch> branches;

		// Token: 0x04003FF0 RID: 16368
		public float widthOnWorld = 0.5f;

		// Token: 0x04003FF1 RID: 16369
		public float widthOnMap = 10f;

		// Token: 0x04003FF2 RID: 16370
		public float debugOpacity;

		// Token: 0x02002F85 RID: 12165
		public class Branch
		{
			// Token: 0x0400C135 RID: 49461
			public int minFlow;

			// Token: 0x0400C136 RID: 49462
			public RiverDef child;

			// Token: 0x0400C137 RID: 49463
			public float chance = 1f;
		}
	}
}
