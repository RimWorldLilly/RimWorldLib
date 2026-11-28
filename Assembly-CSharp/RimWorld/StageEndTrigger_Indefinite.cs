using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001667 RID: 5735
	public class StageEndTrigger_Indefinite : StageEndTrigger
	{
		// Token: 0x0600900F RID: 36879 RVA: 0x0034EE38 File Offset: 0x0034D038
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom((TriggerSignal signal) => false);
		}
	}
}
