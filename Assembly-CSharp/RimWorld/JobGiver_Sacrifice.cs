using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B0E RID: 2830
	public class JobGiver_Sacrifice : ThinkNode_JobGiver
	{
		// Token: 0x060053E2 RID: 21474 RVA: 0x001F383C File Offset: 0x001F1A3C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if (!pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Sacrifice, pawn2, pawn.mindState.duty.focus);
		}
	}
}
