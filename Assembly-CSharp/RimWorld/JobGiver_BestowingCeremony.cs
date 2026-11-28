using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B32 RID: 2866
	public class JobGiver_BestowingCeremony : ThinkNode_JobGiver
	{
		// Token: 0x06005453 RID: 21587 RVA: 0x001F5934 File Offset: 0x001F3B34
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.BestowingCeremony, duty.focus.Pawn, duty.focusSecond);
		}
	}
}
