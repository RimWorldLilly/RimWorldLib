using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B30 RID: 2864
	public class JobGiver_AnimaLinking : ThinkNode_JobGiver
	{
		// Token: 0x0600544E RID: 21582 RVA: 0x001F5840 File Offset: 0x001F3A40
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			if (!pawn.CanReserveAndReach(duty.focus, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Thing thing = duty.focusSecond.Thing;
			CompPsylinkable compPsylinkable = (thing != null) ? thing.TryGetComp<CompPsylinkable>() : null;
			if (compPsylinkable == null || !compPsylinkable.CanPsylink(pawn, null, true).Accepted)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.LinkPsylinkable, duty.focusSecond, duty.focus);
		}
	}
}
