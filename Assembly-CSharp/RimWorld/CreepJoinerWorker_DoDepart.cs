using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200148A RID: 5258
	public class CreepJoinerWorker_DoDepart : BaseCreepJoinerWorker
	{
		// Token: 0x060080FD RID: 33021 RVA: 0x002E0082 File Offset: 0x002DE282
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			base.Tracker.DoLeave();
		}
	}
}
