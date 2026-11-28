using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C20 RID: 3104
	public class WorkGiver_HaulToGrowthVat : WorkGiver_Scanner
	{
		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06005879 RID: 22649 RVA: 0x0020B57E File Offset: 0x0020977E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GrowthVat);
			}
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x0600587A RID: 22650 RVA: 0x0020B58A File Offset: 0x0020978A
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600587B RID: 22651 RVA: 0x0020B590 File Offset: 0x00209790
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Growth vat"))
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			if (t.IsBurning())
			{
				return false;
			}
			Building_GrowthVat building_GrowthVat = t as Building_GrowthVat;
			if (building_GrowthVat == null)
			{
				return false;
			}
			if (building_GrowthVat.NutritionNeeded <= 2.5f)
			{
				return building_GrowthVat.selectedEmbryo != null && !building_GrowthVat.innerContainer.Contains(building_GrowthVat.selectedEmbryo) && this.CanHaulSelectedThing(pawn, building_GrowthVat.selectedEmbryo);
			}
			if (this.FindNutrition(pawn, building_GrowthVat).Thing == null)
			{
				JobFailReason.Is("NoFood".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600587C RID: 22652 RVA: 0x0020B654 File Offset: 0x00209854
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_GrowthVat building_GrowthVat = t as Building_GrowthVat;
			if (building_GrowthVat == null)
			{
				return null;
			}
			if (building_GrowthVat.NutritionNeeded > 0f)
			{
				ThingCount thingCount = this.FindNutrition(pawn, building_GrowthVat);
				if (thingCount.Thing != null)
				{
					Job job = HaulAIUtility.HaulToContainerJob(pawn, thingCount.Thing, t);
					job.count = Mathf.Min(job.count, thingCount.Count);
					return job;
				}
			}
			if (building_GrowthVat.selectedEmbryo != null && !building_GrowthVat.innerContainer.Contains(building_GrowthVat.selectedEmbryo) && this.CanHaulSelectedThing(pawn, building_GrowthVat.selectedEmbryo))
			{
				Job job2 = HaulAIUtility.HaulToContainerJob(pawn, building_GrowthVat.selectedEmbryo, t);
				job2.count = 1;
				return job2;
			}
			return null;
		}

		// Token: 0x0600587D RID: 22653 RVA: 0x0020B6F3 File Offset: 0x002098F3
		private bool CanHaulSelectedThing(Pawn pawn, Thing selectedThing)
		{
			return selectedThing.Spawned && selectedThing.Map == pawn.Map && !selectedThing.IsForbidden(pawn) && pawn.CanReserveAndReach(selectedThing, PathEndMode.OnCell, Danger.Deadly, 1, 1, null, false);
		}

		// Token: 0x0600587E RID: 22654 RVA: 0x0020B730 File Offset: 0x00209930
		private ThingCount FindNutrition(Pawn pawn, Building_GrowthVat vat)
		{
			WorkGiver_HaulToGrowthVat.<>c__DisplayClass8_0 CS$<>8__locals1 = new WorkGiver_HaulToGrowthVat.<>c__DisplayClass8_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.vat = vat;
			Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<FindNutrition>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				return default(ThingCount);
			}
			int b = Mathf.CeilToInt(CS$<>8__locals1.vat.NutritionNeeded / thing.GetStatValue(StatDefOf.Nutrition, true, -1));
			return new ThingCount(thing, Mathf.Min(thing.stackCount, b), false);
		}

		// Token: 0x0400334F RID: 13135
		private const float NutritionBuffer = 2.5f;
	}
}
