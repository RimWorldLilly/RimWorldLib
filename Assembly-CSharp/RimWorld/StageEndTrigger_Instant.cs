using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001660 RID: 5728
	public class StageEndTrigger_Instant : StageEndTrigger
	{
		// Token: 0x06008FFD RID: 36861 RVA: 0x0034EAC2 File Offset: 0x0034CCC2
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom((TriggerSignal signal) => true);
		}
	}
}
