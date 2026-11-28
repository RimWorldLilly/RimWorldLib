using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000958 RID: 2392
	public class TransitionAction_EndAttackBuildingJobs : TransitionAction
	{
		// Token: 0x06004953 RID: 18771 RVA: 0x001CA0E8 File Offset: 0x001C82E8
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Pawn pawn = ownedPawns[i];
				if (pawn.jobs != null && pawn.jobs.curJob != null && pawn.jobs.curJob.def == JobDefOf.AttackMelee && pawn.jobs.curJob.targetA.Thing is Building)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}
	}
}
