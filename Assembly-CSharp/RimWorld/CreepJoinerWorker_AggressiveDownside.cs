using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001485 RID: 5253
	public class CreepJoinerWorker_AggressiveDownside : BaseCreepJoinerWorker
	{
		// Token: 0x060080EF RID: 33007 RVA: 0x002DFEA8 File Offset: 0x002DE0A8
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			base.Tracker.DoAggressive();
		}
	}
}
