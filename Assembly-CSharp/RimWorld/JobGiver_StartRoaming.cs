using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ABA RID: 2746
	public class JobGiver_StartRoaming : ThinkNode_JobGiver
	{
		// Token: 0x06005294 RID: 21140 RVA: 0x001EC3F8 File Offset: 0x001EA5F8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!MentalStateWorker_Roaming.CanRoamNow(pawn))
			{
				return null;
			}
			if (!pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Roaming, null, false, false, false, null, false, false, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Wait, 10, false);
		}
	}
}
