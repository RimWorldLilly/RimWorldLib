using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BE8 RID: 3048
	public class WorkGiver_Warden_InterrogateIdentity : WorkGiver_Warden
	{
		// Token: 0x06005760 RID: 22368 RVA: 0x00206408 File Offset: 0x00204608
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, forced))
			{
				return false;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.InMentalState)
			{
				JobFailReason.Is("PawnIsInMentalState".Translate(pawn2), null);
				return false;
			}
			if (pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Interrogate) && !pawn2.guest.ScheduledForInteraction)
			{
				JobFailReason.Is("PrisonerInteractedTooRecently".Translate(), null);
				return false;
			}
			return base.HasJobOnThing(pawn, t, forced);
		}

		// Token: 0x06005761 RID: 22369 RVA: 0x00206498 File Offset: 0x00204698
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return null;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Interrogate) && pawn2.guest.ScheduledForInteraction && pawn2.guest.IsPrisoner && (!pawn2.Downed || pawn2.InBed()) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.CanReserve(t, 1, -1, null, false) && pawn2.Awake())
			{
				return JobMaker.MakeJob(JobDefOf.PrisonerInterrogateIdentity, t);
			}
			return null;
		}
	}
}
