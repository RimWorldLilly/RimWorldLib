using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AA6 RID: 2726
	public static class JobInBedUtility
	{
		// Token: 0x06005222 RID: 21026 RVA: 0x001E9758 File Offset: 0x001E7958
		public static void KeepLyingDown(this JobDriver driver, TargetIndex bedIndex)
		{
			driver.AddFinishAction(delegate(JobCondition jobCondition)
			{
				Pawn pawn = driver.pawn;
				if (!pawn.Drafted)
				{
					pawn.jobs.jobQueue.EnqueueFirst(JobMaker.MakeJob(JobDefOf.LayDown, pawn.CurJob.GetTarget(bedIndex)), null);
				}
			});
		}

		// Token: 0x06005223 RID: 21027 RVA: 0x001E9790 File Offset: 0x001E7990
		public static bool InBedOrRestSpotNow(Pawn pawn, LocalTargetInfo bedOrRestSpot)
		{
			if (!bedOrRestSpot.IsValid || !pawn.Spawned)
			{
				return false;
			}
			if (bedOrRestSpot.HasThing)
			{
				return bedOrRestSpot.Thing.Map == pawn.Map && RestUtility.GetBedSleepingSlotPosFor(pawn, (Building_Bed)bedOrRestSpot.Thing) == pawn.Position;
			}
			return bedOrRestSpot.Cell == pawn.Position;
		}
	}
}
