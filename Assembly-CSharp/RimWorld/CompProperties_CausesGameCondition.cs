using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC4 RID: 3780
	public class CompProperties_CausesGameCondition : CompProperties
	{
		// Token: 0x06006362 RID: 25442 RVA: 0x0023B3A8 File Offset: 0x002395A8
		public CompProperties_CausesGameCondition()
		{
			this.compClass = typeof(CompCauseGameCondition);
		}

		// Token: 0x04003795 RID: 14229
		public GameConditionDef conditionDef;

		// Token: 0x04003796 RID: 14230
		public int worldRange;

		// Token: 0x04003797 RID: 14231
		public bool hideSource;

		// Token: 0x04003798 RID: 14232
		public bool preventConditionStacking = true;
	}
}
