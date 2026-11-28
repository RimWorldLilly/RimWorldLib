using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000957 RID: 2391
	public class TransitionAction_EndAllJobs : TransitionAction
	{
		// Token: 0x06004951 RID: 18769 RVA: 0x001CA084 File Offset: 0x001C8284
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				if (pawn.jobs != null && pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}
	}
}
