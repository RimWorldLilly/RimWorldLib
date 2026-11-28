using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000846 RID: 2118
	public static class Toils_Reserve
	{
		// Token: 0x060042A0 RID: 17056 RVA: 0x001A4638 File Offset: 0x001A2838
		public static Toil ReserveDestination(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("ReserveDestination");
			toil.initAction = delegate()
			{
				toil.actor.Map.pawnDestinationReservationManager.Reserve(toil.actor, toil.actor.CurJob, toil.actor.jobs.curJob.GetTarget(ind).Cell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x001A4698 File Offset: 0x001A2898
		public static Toil Reserve(TargetIndex ind, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			Toil toil = ToilMaker.MakeToil("Reserve");
			toil.initAction = delegate()
			{
				if (!toil.actor.Reserve(toil.actor.jobs.curJob.GetTarget(ind), toil.actor.CurJob, maxPawns, stackCount, layer, ignoreOtherReservations, false))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x001A4714 File Offset: 0x001A2914
		public static Toil ReserveDestinationOrThing(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("ReserveDestinationOrThing");
			toil.initAction = delegate()
			{
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(ind);
				if (target.HasThing)
				{
					if (!toil.actor.Reserve(target, toil.actor.CurJob, 1, -1, null, true, false))
					{
						toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
				}
				else
				{
					toil.actor.Map.pawnDestinationReservationManager.Reserve(toil.actor, toil.actor.CurJob, target.Cell);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060042A3 RID: 17059 RVA: 0x001A4774 File Offset: 0x001A2974
		public static Toil ReserveQueue(TargetIndex ind, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			Toil toil = ToilMaker.MakeToil("ReserveQueue");
			toil.initAction = delegate()
			{
				List<LocalTargetInfo> targetQueue = toil.actor.jobs.curJob.GetTargetQueue(ind);
				if (targetQueue != null)
				{
					for (int i = 0; i < targetQueue.Count; i++)
					{
						if (!toil.actor.Reserve(targetQueue[i], toil.actor.CurJob, maxPawns, stackCount, layer, true, false))
						{
							toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						}
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x001A47E8 File Offset: 0x001A29E8
		public static Toil Release(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("Release");
			toil.initAction = delegate()
			{
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(ind);
				toil.actor.Map.reservationManager.Release(target, toil.actor, toil.actor.CurJob);
				toil.actor.jobs.ReleaseReservations(target);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x001A4848 File Offset: 0x001A2A48
		public static Toil ReleaseDestination()
		{
			Toil toil = ToilMaker.MakeToil("ReleaseDestination");
			toil.initAction = delegate()
			{
				toil.actor.Map.pawnDestinationReservationManager.ReleaseClaimedBy(toil.actor, toil.actor.CurJob);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x001A48A0 File Offset: 0x001A2AA0
		public static Toil ReleaseDestinationOrThing(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("ReleaseDestinationOrThing");
			toil.initAction = delegate()
			{
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(ind);
				if (target.HasThing)
				{
					toil.actor.Map.reservationManager.Release(target, toil.actor, toil.actor.CurJob);
					toil.actor.jobs.ReleaseReservations(target);
					return;
				}
				toil.actor.Map.pawnDestinationReservationManager.ReleaseClaimedBy(toil.actor, toil.actor.CurJob);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}
	}
}
