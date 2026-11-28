using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA6 RID: 2982
	public class JobGiver_SocialFighting : ThinkNode_JobGiver
	{
		// Token: 0x0600561D RID: 22045 RVA: 0x002005DC File Offset: 0x001FE7DC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			Pawn otherPawn = ((MentalState_SocialFighting)pawn.MentalState).otherPawn;
			if (!otherPawn.Spawned || otherPawn.Map != pawn.Map)
			{
				return null;
			}
			Verb verbToUse;
			if (!SocialInteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.SocialFight, otherPawn);
			job.maxNumMeleeAttacks = 1;
			job.verbToUse = verbToUse;
			return job;
		}
	}
}
