using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B5F RID: 2911
	public class JobGiver_RescueNearby : ThinkNode_JobGiver
	{
		// Token: 0x060054F0 RID: 21744 RVA: 0x001F9BBC File Offset: 0x001F7DBC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_RescueNearby jobGiver_RescueNearby = (JobGiver_RescueNearby)base.DeepCopy(resolve);
			jobGiver_RescueNearby.radius = this.radius;
			return jobGiver_RescueNearby;
		}

		// Token: 0x060054F1 RID: 21745 RVA: 0x001F9BD8 File Offset: 0x001F7DD8
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_RescueNearby.<>c__DisplayClass3_0 CS$<>8__locals1 = new JobGiver_RescueNearby.<>c__DisplayClass3_0();
			CS$<>8__locals1.pawn = pawn;
			Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), this.radius, new Predicate<Thing>(CS$<>8__locals1.<TryGiveJob>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (pawn2 == null)
			{
				return null;
			}
			Building_Bed building_Bed = RestUtility.FindBedFor(pawn2, CS$<>8__locals1.pawn, false, false, pawn2.GuestStatus);
			if (building_Bed == null || !pawn2.CanReserve(building_Bed, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Rescue, pawn2, building_Bed);
			job.count = 1;
			return job;
		}

		// Token: 0x040032B4 RID: 12980
		private float radius = 30f;

		// Token: 0x040032B5 RID: 12981
		private const float MinDistFromEnemy = 25f;
	}
}
