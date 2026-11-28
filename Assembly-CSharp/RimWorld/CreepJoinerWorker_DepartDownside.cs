using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001486 RID: 5254
	public class CreepJoinerWorker_DepartDownside : BaseCreepJoinerWorker
	{
		// Token: 0x060080F1 RID: 33009 RVA: 0x002DFEBD File Offset: 0x002DE0BD
		public override bool CanDoResponse()
		{
			return base.Pawn.Spawned && base.Pawn.Map.CanEverExit;
		}

		// Token: 0x060080F2 RID: 33010 RVA: 0x002DFEDE File Offset: 0x002DE0DE
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			base.Tracker.DoLeave();
		}
	}
}
