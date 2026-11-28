using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A1 RID: 5281
	public class Recipe_ExtractOvum : Recipe_AddHediff
	{
		// Token: 0x06008171 RID: 33137 RVA: 0x002E3220 File Offset: 0x002E1420
		public override AcceptanceReport AvailableReport(Thing thing, BodyPartRecord part = null)
		{
			if (!Find.Storyteller.difficulty.ChildrenAllowed)
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if ((this.recipe.genderPrerequisite ?? pawn.gender) != pawn.gender)
			{
				return false;
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.PregnantHuman, false))
			{
				return "CannotPregnant".Translate();
			}
			if (pawn.ageTracker.AgeBiologicalYears < this.recipe.minAllowedAge)
			{
				return "CannotMustBeAge".Translate(this.recipe.minAllowedAge);
			}
			if (pawn.Sterile())
			{
				return "CannotSterile".Translate();
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.OvumExtracted, false))
			{
				return "SurgeryDisableReasonOvumExtracted".Translate();
			}
			return base.AvailableReport(thing, part);
		}

		// Token: 0x06008172 RID: 33138 RVA: 0x002E3331 File Offset: 0x002E1531
		public override bool CompletableEver(Pawn surgeryTarget)
		{
			return base.IsValidNow(surgeryTarget, null, true);
		}

		// Token: 0x06008173 RID: 33139 RVA: 0x002E333C File Offset: 0x002E153C
		protected override void OnSurgerySuccess(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			HumanOvum thing = ThingMaker.MakeThing(ThingDefOf.HumanOvum, null) as HumanOvum;
			thing.TryGetComp<CompHasPawnSources>().AddSource(pawn);
			if (!GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near, null, (IntVec3 x) => x.InBounds(pawn.Map) && x.Standable(pawn.Map) && !x.Fogged(pawn.Map), null, 1))
			{
				Log.Error("Could not drop ovum near " + pawn.Position.ToString());
			}
		}
	}
}
