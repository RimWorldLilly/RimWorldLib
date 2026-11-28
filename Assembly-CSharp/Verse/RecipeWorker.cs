using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000152 RID: 338
	public class RecipeWorker
	{
		// Token: 0x06000A53 RID: 2643 RVA: 0x000397B0 File Offset: 0x000379B0
		public virtual bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return true;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x000397B3 File Offset: 0x000379B3
		public virtual AcceptanceReport AvailableReport(Thing thing, BodyPartRecord part = null)
		{
			return this.AvailableOnNow(thing, part);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x000397C2 File Offset: 0x000379C2
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return Enumerable.Empty<BodyPartRecord>();
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x000397C9 File Offset: 0x000379C9
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x000397CB File Offset: 0x000379CB
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return (pawn.Faction != billDoerFaction || pawn.IsQuestLodger()) && this.recipe.isViolation;
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x000397EB File Offset: 0x000379EB
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x000397F8 File Offset: 0x000379F8
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00039801 File Offset: 0x00037A01
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00039803 File Offset: 0x00037A03
		public virtual void CheckForWarnings(Pawn billDoer)
		{
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00039805 File Offset: 0x00037A05
		public virtual float GetIngredientCount(IngredientCount ing, Bill bill)
		{
			return ing.GetBaseCount();
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0003980D File Offset: 0x00037A0D
		public virtual TaggedString GetConfirmation(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00039818 File Offset: 0x00037A18
		protected void ReportViolation(Pawn pawn, Pawn billDoer, Faction factionToInform, int goodwillImpact, HistoryEventDef overrideEventDef = null)
		{
			if (factionToInform != null && billDoer != null && billDoer.Faction == Faction.OfPlayer)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(factionToInform, goodwillImpact, true, !factionToInform.temporary, overrideEventDef ?? HistoryEventDefOf.PerformedHarmfulSurgery, null);
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "SurgeryViolation", pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0003987F File Offset: 0x00037A7F
		public virtual string LabelFromUniqueIngredients(Bill bill)
		{
			return null;
		}

		// Token: 0x04000717 RID: 1815
		public RecipeDef recipe;
	}
}
