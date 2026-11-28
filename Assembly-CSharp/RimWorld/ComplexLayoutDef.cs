using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000F85 RID: 3973
	public class ComplexLayoutDef : StructureLayoutDef
	{
		// Token: 0x04003C5F RID: 15455
		public List<ComplexThreat> threats;

		// Token: 0x04003C60 RID: 15456
		public float roomRewardCrateFactor = 0.5f;

		// Token: 0x04003C61 RID: 15457
		public float fixedHostileFactionChance = 0.25f;

		// Token: 0x04003C62 RID: 15458
		public ThingSetMakerDef rewardThingSetMakerDef;
	}
}
