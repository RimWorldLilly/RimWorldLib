using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BBD RID: 3005
	public class WorkGiver_Tame : WorkGiver_InteractAnimal
	{
		// Token: 0x06005697 RID: 22167 RVA: 0x00202E1B File Offset: 0x0020101B
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_Tame.<PotentialWorkThingsGlobal>d__0 <PotentialWorkThingsGlobal>d__ = new WorkGiver_Tame.<PotentialWorkThingsGlobal>d__0(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x06005698 RID: 22168 RVA: 0x00202E2B File Offset: 0x0020102B
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Tame);
		}

		// Token: 0x06005699 RID: 22169 RVA: 0x00202E48 File Offset: 0x00201048
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !TameUtility.CanTame(pawn2))
			{
				return null;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Tame) == null)
			{
				return null;
			}
			if (!this.CanInteractWithAnimal(pawn, pawn2, forced))
			{
				return null;
			}
			if (TameUtility.TriedToTameTooRecently(pawn2))
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans, null);
				return null;
			}
			Thing thing = null;
			int count = -1;
			if (pawn2.RaceProps.EatsFood)
			{
				Pawn_NeedsTracker needs = pawn2.needs;
				if (((needs != null) ? needs.food : null) != null && !base.HasFoodToInteractAnimal(pawn, pawn2))
				{
					ThingDef foodDef;
					thing = FoodUtility.BestFoodSourceOnMap(pawn, pawn2, false, out foodDef, FoodPreferability.RawTasty, false, false, false, false, false, false, false, false, false, false, false, FoodPreferability.Undefined, new float?(JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn2) * 2f * 4f), false);
					if (thing == null)
					{
						JobFailReason.Is("NoFood".Translate(), null);
						return null;
					}
					float num = JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn2) * 2f * 4f;
					float nutrition = FoodUtility.GetNutrition(pawn2, thing, foodDef);
					count = Mathf.CeilToInt(num / nutrition);
				}
			}
			Job job = JobMaker.MakeJob(JobDefOf.Tame, t, null, thing);
			job.count = count;
			return job;
		}
	}
}
