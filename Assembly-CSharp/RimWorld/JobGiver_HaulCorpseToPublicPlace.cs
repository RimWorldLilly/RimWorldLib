using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B9D RID: 2973
	public class JobGiver_HaulCorpseToPublicPlace : ThinkNode_JobGiver
	{
		// Token: 0x06005608 RID: 22024 RVA: 0x001FFFBC File Offset: 0x001FE1BC
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_CorpseObsession mentalState_CorpseObsession = pawn.MentalState as MentalState_CorpseObsession;
			if (mentalState_CorpseObsession == null || mentalState_CorpseObsession.corpse == null || mentalState_CorpseObsession.alreadyHauledCorpse)
			{
				return null;
			}
			Corpse corpse = mentalState_CorpseObsession.corpse;
			Building_Grave building_Grave = mentalState_CorpseObsession.corpse.ParentHolder as Building_Grave;
			if (building_Grave != null)
			{
				if (!pawn.CanReserveAndReach(building_Grave, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
				{
					return null;
				}
			}
			else if (!pawn.CanReserveAndReach(corpse, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulCorpseToPublicPlace, corpse, building_Grave);
			job.count = 1;
			return job;
		}
	}
}
