using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001489 RID: 5257
	public class CreepJoinerWorker_DoAggressive : BaseCreepJoinerWorker
	{
		// Token: 0x060080FB RID: 33019 RVA: 0x002E006D File Offset: 0x002DE26D
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			base.Tracker.DoAggressive();
		}
	}
}
