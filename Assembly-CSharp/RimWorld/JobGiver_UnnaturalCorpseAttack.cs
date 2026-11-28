using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B28 RID: 2856
	public class JobGiver_UnnaturalCorpseAttack : ThinkNode_JobGiver
	{
		// Token: 0x06005430 RID: 21552 RVA: 0x001F50B8 File Offset: 0x001F32B8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!Find.Anomaly.TryGetUnnaturalCorpseTrackerForAwoken(pawn, out unnaturalCorpseTracker))
			{
				return null;
			}
			Pawn haunted = unnaturalCorpseTracker.Haunted;
			if (haunted.DestroyedOrNull())
			{
				return null;
			}
			if (!haunted.Spawned)
			{
				if (haunted.SpawnedOrAnyParentSpawned && pawn.CanReach(haunted.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, haunted.SpawnedParentOrMe);
					job.expiryInterval = 30;
					return job;
				}
				return null;
			}
			else
			{
				if (pawn.CanReach(haunted, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					Job job2 = JobMaker.MakeJob(JobDefOf.UnnaturalCorpseAttack, haunted);
					job2.expiryInterval = 120;
					job2.checkOverrideOnExpire = true;
					return job2;
				}
				return null;
			}
		}
	}
}
