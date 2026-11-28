using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BB5 RID: 2997
	public abstract class WorkGiver_InteractAnimal : WorkGiver_Scanner
	{
		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06005677 RID: 22135 RVA: 0x00202515 File Offset: 0x00200715
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06005678 RID: 22136 RVA: 0x00202518 File Offset: 0x00200718
		public static void ResetStaticData()
		{
			WorkGiver_InteractAnimal.NoUsableFoodTrans = "NoUsableFood".Translate();
			WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans = "AnimalInteractedTooRecently".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans = "CantInteractAnimalDowned".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans = "CantInteractAnimalAsleep".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans = "CantInteractAnimalBusy".Translate();
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x0020258C File Offset: 0x0020078C
		protected virtual bool CanInteractWithAnimal(Pawn pawn, Pawn animal, bool forced)
		{
			string text;
			if (WorkGiver_InteractAnimal.CanInteractWithAnimal(pawn, animal, out text, forced, this.canInteractWhileSleeping, false, false))
			{
				return true;
			}
			if (text != null)
			{
				JobFailReason.Is(text, null);
			}
			return false;
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x002025BC File Offset: 0x002007BC
		public static bool CanInteractWithAnimal(Pawn pawn, Pawn animal, out string jobFailReason, bool forced, bool canInteractWhileSleeping = false, bool ignoreSkillRequirements = false, bool canInteractWhileRoaming = false)
		{
			jobFailReason = null;
			if (!pawn.CanReserve(animal, 1, -1, null, forced))
			{
				return false;
			}
			if (animal.Downed)
			{
				jobFailReason = WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans;
				return false;
			}
			if (!animal.Awake() && !canInteractWhileSleeping)
			{
				jobFailReason = WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans;
				return false;
			}
			if (!animal.CanCasuallyInteractNow(false, canInteractWhileSleeping, canInteractWhileRoaming, false))
			{
				jobFailReason = WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans;
				return false;
			}
			int num = TrainableUtility.MinimumHandlingSkill(animal);
			if (!ignoreSkillRequirements && num > pawn.skills.GetSkill(SkillDefOf.Animals).Level)
			{
				jobFailReason = "AnimalsSkillTooLow".Translate(num);
				return false;
			}
			return true;
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x0020265C File Offset: 0x0020085C
		protected bool HasFoodToInteractAnimal(Pawn pawn, Pawn tamee)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			int num = 0;
			float num2 = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee);
			float num3 = 0f;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				if (tamee.WillEat(thing, pawn, true, false) && thing.def.ingestible.preferability <= FoodPreferability.RawTasty && !thing.def.IsDrug)
				{
					for (int j = 0; j < thing.stackCount; j++)
					{
						num3 += thing.GetStatValue(StatDefOf.Nutrition, true, -1);
						if (num3 >= num2)
						{
							num++;
							num3 = 0f;
						}
						if (num >= 2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600567C RID: 22140 RVA: 0x00202714 File Offset: 0x00200914
		protected Job TakeFoodForAnimalInteractJob(Pawn pawn, Pawn tamee)
		{
			ThingDef foodDef;
			Thing thing = FoodUtility.BestFoodSourceOnMap(pawn, tamee, false, out foodDef, FoodPreferability.RawTasty, false, false, false, false, false, false, false, false, false, false, false, FoodPreferability.Undefined, new float?(JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2f * 4f), false);
			if (thing == null)
			{
				return null;
			}
			float wantedNutrition = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2f * 4f;
			float nutrition = FoodUtility.GetNutrition(tamee, thing, foodDef);
			int count = FoodUtility.StackCountForNutrition(wantedNutrition, nutrition);
			Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, thing);
			job.count = count;
			return job;
		}

		// Token: 0x04003314 RID: 13076
		protected static string NoUsableFoodTrans;

		// Token: 0x04003315 RID: 13077
		protected static string AnimalInteractedTooRecentlyTrans;

		// Token: 0x04003316 RID: 13078
		private static string CantInteractAnimalDownedTrans;

		// Token: 0x04003317 RID: 13079
		private static string CantInteractAnimalAsleepTrans;

		// Token: 0x04003318 RID: 13080
		private static string CantInteractAnimalBusyTrans;

		// Token: 0x04003319 RID: 13081
		protected bool canInteractWhileSleeping;
	}
}
