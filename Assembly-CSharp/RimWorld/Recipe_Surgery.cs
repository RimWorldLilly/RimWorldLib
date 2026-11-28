using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AE RID: 5294
	public class Recipe_Surgery : RecipeWorker
	{
		// Token: 0x060081A7 RID: 33191 RVA: 0x002E4274 File Offset: 0x002E2474
		protected bool CheckSurgeryFail(Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			if (this.recipe.surgeryOutcomeEffect == null)
			{
				return false;
			}
			SurgeryOutcome outcome = this.recipe.surgeryOutcomeEffect.GetOutcome(this.recipe, surgeon, patient, ingredients, part, bill);
			if (outcome != null && outcome.failure)
			{
				if (this.recipe.addsHediffOnFailure != null)
				{
					patient.health.AddHediff(this.recipe.addsHediffOnFailure, part, null, null);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060081A8 RID: 33192 RVA: 0x002E42F0 File Offset: 0x002E24F0
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn = thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if ((this.recipe.genderPrerequisite ?? pawn.gender) != pawn.gender)
			{
				return false;
			}
			if (this.recipe.mustBeFertile && pawn.Sterile())
			{
				return false;
			}
			if (!this.recipe.allowedForQuestLodgers && pawn.IsQuestLodger())
			{
				return false;
			}
			if (this.recipe.minAllowedAge > 0 && pawn.ageTracker.AgeBiologicalYears < this.recipe.minAllowedAge)
			{
				return false;
			}
			if (this.recipe.developmentalStageFilter != null && !this.recipe.developmentalStageFilter.Value.Has(pawn.DevelopmentalStage))
			{
				return false;
			}
			if (this.recipe.humanlikeOnly && !pawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (ModsConfig.AnomalyActive)
			{
				if (this.recipe.mutantBlacklist != null && pawn.IsMutant && this.recipe.mutantBlacklist.Contains(pawn.mutant.Def))
				{
					return false;
				}
				if (this.recipe.mutantPrerequisite != null && (!pawn.IsMutant || !this.recipe.mutantPrerequisite.Contains(pawn.mutant.Def)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060081A9 RID: 33193 RVA: 0x002E444A File Offset: 0x002E264A
		public virtual bool CompletableEver(Pawn surgeryTarget)
		{
			return true;
		}

		// Token: 0x060081AA RID: 33194 RVA: 0x002E444D File Offset: 0x002E264D
		protected virtual void OnSurgerySuccess(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}
	}
}
