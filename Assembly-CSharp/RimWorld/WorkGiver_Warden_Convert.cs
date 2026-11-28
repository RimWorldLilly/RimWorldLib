using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BDE RID: 3038
	public class WorkGiver_Warden_Convert : WorkGiver_Warden
	{
		// Token: 0x06005744 RID: 22340 RVA: 0x002057F0 File Offset: 0x002039F0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("WorkGiver_Warden_Convert"))
			{
				return false;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return false;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.InMentalState)
			{
				JobFailReason.Is("PawnIsInMentalState".Translate(pawn2), null);
				return false;
			}
			if (pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Convert) && !pawn2.guest.ScheduledForInteraction)
			{
				JobFailReason.Is("PrisonerInteractedTooRecently".Translate(), null);
				return false;
			}
			return base.HasJobOnThing(pawn, t, forced);
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x00205884 File Offset: 0x00203A84
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("WorkGiver_Warden_Convert"))
			{
				return null;
			}
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Convert) && pawn2.guest.ScheduledForInteraction && pawn2.guest.IsPrisoner && (!pawn2.Downed || pawn2.InBed()) && pawn2.Ideo != pawn.Ideo && pawn.Ideo == pawn2.guest.ideoForConversion && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.CanReserve(t, 1, -1, null, false) && pawn2.Awake())
			{
				return JobMaker.MakeJob(JobDefOf.PrisonerConvert, t);
			}
			return null;
		}
	}
}
