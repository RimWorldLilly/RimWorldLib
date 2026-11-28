using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FFC RID: 8188
	public struct RoyalTitleFoodRequirement
	{
		// Token: 0x1700255E RID: 9566
		// (get) Token: 0x0600CF6B RID: 53099 RVA: 0x004C60EE File Offset: 0x004C42EE
		public bool Defined
		{
			get
			{
				return this.minQuality > FoodPreferability.Undefined;
			}
		}

		// Token: 0x0600CF6C RID: 53100 RVA: 0x004C60FC File Offset: 0x004C42FC
		public bool Acceptable(ThingDef food)
		{
			return food.ingestible != null && (this.allowedDefs.Contains(food) || (this.allowedTypes != FoodTypeFlags.None && (this.allowedTypes & food.ingestible.foodType) != FoodTypeFlags.None) || food.ingestible.preferability >= this.minQuality);
		}

		// Token: 0x040083EF RID: 33775
		public FoodPreferability minQuality;

		// Token: 0x040083F0 RID: 33776
		public FoodTypeFlags allowedTypes;

		// Token: 0x040083F1 RID: 33777
		public List<ThingDef> allowedDefs;
	}
}
