using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B57 RID: 2903
	public class JobGiver_PackFood : ThinkNode_JobGiver
	{
		// Token: 0x060054D3 RID: 21715 RVA: 0x001F8FAC File Offset: 0x001F71AC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.inventory == null)
			{
				return null;
			}
			float invNutrition = JobGiver_PackFood.GetInventoryPackableFoodNutrition(pawn);
			if (invNutrition > 0.4f)
			{
				return null;
			}
			if (pawn.Map.resourceCounter.TotalHumanEdibleNutrition < (float)pawn.Map.mapPawns.ColonistsSpawnedCount * 1.5f)
			{
				return null;
			}
			if (MassUtility.IsOverEncumbered(pawn))
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 20f, delegate(Thing t)
			{
				if (!JobGiver_PackFood.IsGoodPackableFoodFor(t, pawn, true) || t.IsForbidden(pawn) || !pawn.CanReserve(t, 1, -1, null, false) || !t.IsSociallyProper(pawn))
				{
					return false;
				}
				if (invNutrition + t.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)t.stackCount < 0.8f)
				{
					return false;
				}
				List<FoodUtility.ThoughtFromIngesting> list = FoodUtility.ThoughtsFromIngesting(pawn, t, FoodUtility.GetFinalIngestibleDef(t, false));
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].thought.stages[0].baseMoodEffect < 0f)
					{
						return false;
					}
				}
				return true;
			}, (Thing x) => FoodUtility.FoodOptimality(pawn, x, FoodUtility.GetFinalIngestibleDef(x, false), 0f, false), 24, 30, false);
			if (thing == null)
			{
				return null;
			}
			int num = Mathf.FloorToInt((pawn.needs.food.MaxLevel - invNutrition) / thing.GetStatValue(StatDefOf.Nutrition, true, -1));
			num = Mathf.Min(num, thing.stackCount);
			num = Mathf.Max(num, 1);
			Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, thing);
			job.count = num;
			return job;
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x001F90F0 File Offset: 0x001F72F0
		public static float GetInventoryPackableFoodNutrition(Pawn pawn)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			float num = 0f;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (JobGiver_PackFood.IsGoodPackableFoodFor(innerContainer[i], pawn, false))
				{
					num += innerContainer[i].GetStatValue(StatDefOf.Nutrition, true, -1) * (float)innerContainer[i].stackCount;
				}
			}
			return num;
		}

		// Token: 0x060054D5 RID: 21717 RVA: 0x001F9158 File Offset: 0x001F7358
		public static bool IsGoodPackableFoodFor(Thing food, Pawn forPawn, bool checkMass)
		{
			return (!checkMass || MassUtility.CountToPickUpUntilOverEncumbered(forPawn, food) != 0) && (food.def.IsNutritionGivingIngestible && food.def.EverHaulable && food.def.ingestible.preferability >= FoodPreferability.MealAwful) && forPawn.WillEat(food, null, false, false);
		}

		// Token: 0x040032AA RID: 12970
		public const float MaxInvNutritionToConsiderLookingForFood = 0.4f;

		// Token: 0x040032AB RID: 12971
		private const float MinFinalInvNutritionToPickUp = 0.8f;

		// Token: 0x040032AC RID: 12972
		private const float MinNutritionPerColonistToDo = 1.5f;

		// Token: 0x040032AD RID: 12973
		public const FoodPreferability MinFoodPreferability = FoodPreferability.MealAwful;
	}
}
