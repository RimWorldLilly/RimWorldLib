using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C1E RID: 3102
	public class WorkGiver_HaulToBiosculpterPod : WorkGiver_Scanner
	{
		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x0600586C RID: 22636 RVA: 0x0020B232 File Offset: 0x00209432
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.BiosculpterPod);
			}
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x0600586D RID: 22637 RVA: 0x0020B23E File Offset: 0x0020943E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600586E RID: 22638 RVA: 0x0020B244 File Offset: 0x00209444
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("Biosculpting"))
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
			CompBiosculpterPod compBiosculpterPod = t.TryGetComp<CompBiosculpterPod>();
			if (compBiosculpterPod == null || !compBiosculpterPod.PowerOn || compBiosculpterPod.State != BiosculpterPodState.LoadingNutrition || (!forced && !compBiosculpterPod.autoLoadNutrition))
			{
				return false;
			}
			if (t.IsBurning())
			{
				return false;
			}
			if (compBiosculpterPod.RequiredNutritionRemaining <= 0f)
			{
				return false;
			}
			if (this.FindNutrition(pawn, compBiosculpterPod).Thing == null)
			{
				JobFailReason.Is("NoFood".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x0020B2F8 File Offset: 0x002094F8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompBiosculpterPod compBiosculpterPod = t.TryGetComp<CompBiosculpterPod>();
			if (compBiosculpterPod == null)
			{
				return null;
			}
			if (compBiosculpterPod.RequiredNutritionRemaining > 0f)
			{
				ThingCount thingCount = this.FindNutrition(pawn, compBiosculpterPod);
				if (thingCount.Thing != null)
				{
					Job job = HaulAIUtility.HaulToContainerJob(pawn, thingCount.Thing, t);
					job.count = Mathf.Min(job.count, thingCount.Count);
					return job;
				}
			}
			return null;
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x0020B358 File Offset: 0x00209558
		private ThingCount FindNutrition(Pawn pawn, CompBiosculpterPod pod)
		{
			WorkGiver_HaulToBiosculpterPod.<>c__DisplayClass6_0 CS$<>8__locals1 = new WorkGiver_HaulToBiosculpterPod.<>c__DisplayClass6_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.pod = pod;
			Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<FindNutrition>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				return default(ThingCount);
			}
			int b = Mathf.CeilToInt(CS$<>8__locals1.pod.RequiredNutritionRemaining / thing.GetStatValue(StatDefOf.Nutrition, true, -1));
			return new ThingCount(thing, Mathf.Min(thing.stackCount, b), false);
		}
	}
}
