using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA7 RID: 2983
	public class JobGiver_Tantrum : ThinkNode_JobGiver
	{
		// Token: 0x0600561F RID: 22047 RVA: 0x0020065C File Offset: 0x001FE85C
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_Tantrum mentalState_Tantrum = pawn.MentalState as MentalState_Tantrum;
			if (mentalState_Tantrum == null || mentalState_Tantrum.target == null || !mentalState_Tantrum.target.Spawned || !pawn.CanReach(mentalState_Tantrum.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Verb verbToUse = null;
			Pawn pawn2 = mentalState_Tantrum.target as Pawn;
			if (pawn2 != null)
			{
				if (pawn2.Downed)
				{
					return null;
				}
				if (!SocialInteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
				{
					return null;
				}
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, mentalState_Tantrum.target);
			job.maxNumMeleeAttacks = 1;
			job.verbToUse = verbToUse;
			return job;
		}
	}
}
