using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149C RID: 5276
	public class Recipe_AdministerIngestible : Recipe_Surgery
	{
		// Token: 0x06008155 RID: 33109 RVA: 0x002E2810 File Offset: 0x002E0A10
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			Thing thing = ingredients[0];
			Pawn_NeedsTracker needs = pawn.needs;
			float? num;
			if (needs == null)
			{
				num = null;
			}
			else
			{
				Need_Food food = needs.food;
				num = ((food != null) ? new float?(food.NutritionWanted) : null);
			}
			float? num2 = num;
			float num3 = thing.Ingested(pawn, num2.GetValueOrDefault());
			if (!pawn.Dead)
			{
				Pawn_NeedsTracker needs2 = pawn.needs;
				if (((needs2 != null) ? needs2.food : null) != null)
				{
					pawn.needs.food.CurLevel += num3;
				}
				if (pawn.needs.mood != null)
				{
					if (pawn.IsTeetotaler() && ingredients[0].def.IsNonMedicalDrug)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ForcedMeToTakeDrugs, billDoer, null);
					}
					else if (pawn.IsProsthophobe() && ingredients[0].def == ThingDefOf.Luciferium)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ForcedMeToTakeLuciferium, billDoer, null);
					}
				}
			}
			if (billDoer != null)
			{
				if (ingredients[0].def.IsDrug)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AdministeredDrug, billDoer.Named(HistoryEventArgsNames.Doer)), true);
				}
				if (ingredients[0].def.IsDrug && ingredients[0].def.ingestible.drugCategory == DrugCategory.Hard)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AdministeredHardDrug, billDoer.Named(HistoryEventArgsNames.Doer)), true);
				}
				if (ingredients[0].def.IsNonMedicalDrug)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AdministeredRecreationalDrug, billDoer.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
		}

		// Token: 0x06008156 RID: 33110 RVA: 0x002E29E3 File Offset: 0x002E0BE3
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		// Token: 0x06008157 RID: 33111 RVA: 0x002E29E8 File Offset: 0x002E0BE8
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			if (pawn.Faction == billDoerFaction)
			{
				return false;
			}
			ThingDef thingDef = this.recipe.ingredients[0].filter.AllowedThingDefs.First<ThingDef>();
			if (thingDef.IsNonMedicalDrug)
			{
				if (AddictionUtility.HasChemicalDependency(pawn, thingDef))
				{
					return false;
				}
				foreach (CompProperties compProperties in thingDef.comps)
				{
					CompProperties_Drug compProperties_Drug = compProperties as CompProperties_Drug;
					if (compProperties_Drug != null)
					{
						ChemicalDef chemical = compProperties_Drug.chemical;
						if (((chemical != null) ? chemical.addictionHediff : null) != null && pawn.health.hediffSet.HasHediff(compProperties_Drug.chemical.addictionHediff, false))
						{
							return false;
						}
					}
				}
			}
			return thingDef.IsNonMedicalDrug;
		}

		// Token: 0x06008158 RID: 33112 RVA: 0x002E2ABC File Offset: 0x002E0CBC
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (!base.AvailableOnNow(thing, part))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn.IsMutant)
			{
				if (!pawn.mutant.Def.canUseDrugs)
				{
					return false;
				}
				ThingDef item = this.recipe.ingredients[0].filter.AllowedThingDefs.First<ThingDef>();
				if (!pawn.mutant.Def.drugWhitelist.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008159 RID: 33113 RVA: 0x002E2B34 File Offset: 0x002E0D34
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			ThingDef singleDef = this.recipe.ingredients[0].filter.BestThingRequest.singleDef;
			if (singleDef.IsNonMedicalDrug && pawn.PawnWouldBeUnhappyTakingDrug(singleDef))
			{
				return base.GetLabelWhenUsedOn(pawn, part) + " (" + "TeetotalerUnhappy".Translate() + ")";
			}
			if (pawn.IsProsthophobe() && singleDef == ThingDefOf.Luciferium)
			{
				return base.GetLabelWhenUsedOn(pawn, part) + " (" + "ProsthophobeUnhappy".Translate() + ")";
			}
			return base.GetLabelWhenUsedOn(pawn, part);
		}
	}
}
