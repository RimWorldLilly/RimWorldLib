using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200200F RID: 8207
	public class RoyalAid
	{
		// Token: 0x04008426 RID: 33830
		public int favorCost;

		// Token: 0x04008427 RID: 33831
		public int points;

		// Token: 0x04008428 RID: 33832
		public FloatRange? overrideAcceptableTemperatureRange;

		// Token: 0x04008429 RID: 33833
		public int pawnCount;

		// Token: 0x0400842A RID: 33834
		public PawnKindDef pawnKindDef;

		// Token: 0x0400842B RID: 33835
		public float targetingRange;

		// Token: 0x0400842C RID: 33836
		public bool targetingRequireLOS = true;

		// Token: 0x0400842D RID: 33837
		public float aidDurationDays;

		// Token: 0x0400842E RID: 33838
		public float radius;

		// Token: 0x0400842F RID: 33839
		public int intervalTicks;

		// Token: 0x04008430 RID: 33840
		public int explosionCount;

		// Token: 0x04008431 RID: 33841
		public int warmupTicks;

		// Token: 0x04008432 RID: 33842
		public FloatRange explosionRadiusRange;

		// Token: 0x04008433 RID: 33843
		public List<ThingDefCountClass> itemsToDrop;
	}
}
