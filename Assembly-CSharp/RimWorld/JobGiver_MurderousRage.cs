using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA2 RID: 2978
	public class JobGiver_MurderousRage : ThinkNode_JobGiver
	{
		// Token: 0x06005615 RID: 22037 RVA: 0x00200424 File Offset: 0x001FE624
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_MurderousRage mentalState_MurderousRage = pawn.MentalState as MentalState_MurderousRage;
			if (mentalState_MurderousRage == null || !mentalState_MurderousRage.IsTargetStillValidAndReachable())
			{
				return null;
			}
			Thing spawnedParentOrMe = mentalState_MurderousRage.target.SpawnedParentOrMe;
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, spawnedParentOrMe);
			job.canBashDoors = true;
			job.killIncappedTarget = true;
			if (spawnedParentOrMe != mentalState_MurderousRage.target)
			{
				job.maxNumMeleeAttacks = 2;
			}
			return job;
		}
	}
}
