using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B2 RID: 5298
	public class Recipe_ImplantIUD : Recipe_AddHediff
	{
		// Token: 0x060081B1 RID: 33201 RVA: 0x002E4674 File Offset: 0x002E2874
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && pawn.ageTracker.AgeBiologicalYears >= 16 && base.AvailableOnNow(thing, part);
		}

		// Token: 0x060081B2 RID: 33202 RVA: 0x002E46A8 File Offset: 0x002E28A8
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			base.ApplyOnPawn(pawn, part, billDoer, ingredients, bill);
			if (pawn.RaceProps.Humanlike)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnantHuman, false);
				if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex == 0)
				{
					PregnancyUtility.TryTerminatePregnancy(pawn);
				}
			}
		}

		// Token: 0x060081B3 RID: 33203 RVA: 0x002E46F8 File Offset: 0x002E28F8
		public override TaggedString GetConfirmation(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnantHuman, false);
				if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex == 0)
				{
					return "ConfirmationPawnPregnancyTerminated".Translate(pawn.Named("PAWN"));
				}
			}
			return base.GetConfirmation(pawn);
		}
	}
}
