using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001724 RID: 5924
	public abstract class RitualStageTickActionMaker
	{
		// Token: 0x060093CB RID: 37835
		public abstract IEnumerable<ActionOnTick> GenerateTimedActions(LordJob_Ritual ritual, RitualStage stage);
	}
}
