using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001725 RID: 5925
	public class RitualStageAction_ReleaseLanterns : RitualStageTickActionMaker
	{
		// Token: 0x060093CD RID: 37837 RVA: 0x0035E86E File Offset: 0x0035CA6E
		public override IEnumerable<ActionOnTick> GenerateTimedActions(LordJob_Ritual ritual, RitualStage stage)
		{
			RitualStageAction_ReleaseLanterns.<GenerateTimedActions>d__2 <GenerateTimedActions>d__ = new RitualStageAction_ReleaseLanterns.<GenerateTimedActions>d__2(-2);
			<GenerateTimedActions>d__.<>4__this = this;
			<GenerateTimedActions>d__.<>3__ritual = ritual;
			<GenerateTimedActions>d__.<>3__stage = stage;
			return <GenerateTimedActions>d__;
		}

		// Token: 0x04005954 RID: 22868
		public int woodCost = 4;

		// Token: 0x04005955 RID: 22869
		public IntRange preliminaryTicks;
	}
}
