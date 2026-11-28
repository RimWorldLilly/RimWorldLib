using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200238E RID: 9102
	public static class CaravanPawnsNeedsUtility
	{
		// Token: 0x0600E264 RID: 57956 RVA: 0x00540B48 File Offset: 0x0053ED48
		public static bool CanEatForNutritionEver(ThingDef food, Pawn pawn)
		{
			return food.IsNutritionGivingIngestible && pawn.WillEat(food, null, false, false) && food.ingestible.preferability > FoodPreferability.NeverForNutrition && (!food.IsDrug || !pawn.IsTeetotaler()) && food.ingestible.canAutoSelectAsFoodForCaravan && (!food.IsDrug || new HistoryEvent(HistoryEventDefOf.IngestedDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo()) && (!food.IsNonMedicalDrug || new HistoryEvent(HistoryEventDefOf.IngestedRecreationalDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo()) && (!food.IsDrug || food.ingestible.drugCategory != DrugCategory.Hard || new HistoryEvent(HistoryEventDefOf.IngestedHardDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo());
		}

		// Token: 0x0600E265 RID: 57957 RVA: 0x00540C1C File Offset: 0x0053EE1C
		public static bool CanEatForNutritionNow(ThingDef food, Pawn pawn)
		{
			return CaravanPawnsNeedsUtility.CanEatForNutritionEver(food, pawn) && (!pawn.RaceProps.Humanlike || pawn.needs.food.CurCategory >= HungerCategory.Starving || food.ingestible.preferability > FoodPreferability.DesperateOnlyForHumanlikes);
		}

		// Token: 0x0600E266 RID: 57958 RVA: 0x00540C5A File Offset: 0x0053EE5A
		public static bool CanEatForNutritionNow(Thing food, Pawn pawn)
		{
			return food.IngestibleNow && CaravanPawnsNeedsUtility.CanEatForNutritionNow(food.def, pawn);
		}

		// Token: 0x0600E267 RID: 57959 RVA: 0x00540C78 File Offset: 0x0053EE78
		public static float GetFoodScore(Thing food, Pawn pawn)
		{
			float num = CaravanPawnsNeedsUtility.GetFoodScore(food.def, pawn, food.GetStatValue(StatDefOf.Nutrition, true, -1));
			if (pawn.RaceProps.Humanlike)
			{
				CompRottable compRottable = food.TryGetComp<CompRottable>();
				int a = (compRottable != null) ? compRottable.TicksUntilRotAtCurrentTemp : int.MaxValue;
				float a2 = 1f - (float)Mathf.Min(a, 3600000) / 3600000f;
				num += Mathf.Min(a2, 0.999f);
			}
			return num;
		}

		// Token: 0x0600E268 RID: 57960 RVA: 0x00540CEC File Offset: 0x0053EEEC
		public static float GetFoodScore(ThingDef food, Pawn pawn, float singleFoodNutrition)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				float num = 0f;
				if (food == ThingDefOf.Kibble || food == ThingDefOf.Hay)
				{
					num = 5f;
				}
				else if (food.ingestible.preferability == FoodPreferability.DesperateOnlyForHumanlikes)
				{
					num = 4f;
				}
				else if (food.ingestible.preferability == FoodPreferability.RawBad)
				{
					num = 3f;
				}
				else if (food.ingestible.preferability == FoodPreferability.RawTasty)
				{
					num = 2f;
				}
				else if (food.ingestible.preferability < FoodPreferability.MealAwful)
				{
					num = 1f;
				}
				return num + Mathf.Min(singleFoodNutrition / 100f, 0.999f);
			}
			if (food.ingestible.lowPriorityCaravanFood)
			{
				return 2f;
			}
			return (float)food.ingestible.preferability;
		}
	}
}
