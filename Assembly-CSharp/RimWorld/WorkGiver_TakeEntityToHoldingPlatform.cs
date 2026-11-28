using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C42 RID: 3138
	public class WorkGiver_TakeEntityToHoldingPlatform : WorkGiver_Scanner
	{
		// Token: 0x0600593F RID: 22847 RVA: 0x0020E49F File Offset: 0x0020C69F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x06005940 RID: 22848 RVA: 0x0020E4A9 File Offset: 0x0020C6A9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.HoldingPlatformTarget);
			}
		}

		// Token: 0x06005941 RID: 22849 RVA: 0x0020E4B4 File Offset: 0x0020C6B4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				JobFailReason.Is("IncapableOfCapacity".Translate(PawnCapacityDefOf.Manipulation.label).CapitalizeFirst(), null);
				return false;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = t.TryGetComp<CompHoldingPlatformTarget>();
			if (((compHoldingPlatformTarget != null) ? compHoldingPlatformTarget.targetHolder : null) == null || compHoldingPlatformTarget.targetHolder.Destroyed || compHoldingPlatformTarget.targetHolder.MapHeld != t.MapHeld || compHoldingPlatformTarget.EntityHolder.HeldPawn != null)
			{
				return false;
			}
			if (!pawn.CanReserveAndReach(t, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, forced))
			{
				return false;
			}
			if (!pawn.CanReserveAndReach(compHoldingPlatformTarget.targetHolder, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, forced))
			{
				return false;
			}
			Pawn pawn2 = t as Pawn;
			return pawn2 == null || pawn2.ThreatDisabled(pawn);
		}

		// Token: 0x06005942 RID: 22850 RVA: 0x0020E594 File Offset: 0x0020C794
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompHoldingPlatformTarget compHoldingPlatformTarget = t.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.CarryToEntityHolder, compHoldingPlatformTarget.targetHolder, t);
			job.count = 1;
			return job;
		}
	}
}
