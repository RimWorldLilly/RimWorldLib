using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C4 RID: 5060
	public interface IPlantToGrowSettable
	{
		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x06007D79 RID: 32121
		Map Map { get; }

		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x06007D7A RID: 32122
		IEnumerable<IntVec3> Cells { get; }

		// Token: 0x06007D7B RID: 32123
		ThingDef GetPlantDefToGrow();

		// Token: 0x06007D7C RID: 32124
		void SetPlantDefToGrow(ThingDef plantDef);

		// Token: 0x06007D7D RID: 32125
		bool CanAcceptSowNow();
	}
}
