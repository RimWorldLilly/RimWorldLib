using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C1C RID: 3100
	public class WorkGiver_HaulToSubcoreScanner : WorkGiver_Scanner
	{
		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06005863 RID: 22627 RVA: 0x0020AF39 File Offset: 0x00209139
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.SubcoreScanner);
			}
		}

		// Token: 0x06005864 RID: 22628 RVA: 0x0020AF44 File Offset: 0x00209144
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Haul to subcore scanner"))
			{
				return false;
			}
			Building_SubcoreScanner building_SubcoreScanner = t as Building_SubcoreScanner;
			return building_SubcoreScanner != null && building_SubcoreScanner.State == SubcoreScannerState.WaitingForIngredients && pawn.CanReserve(t, 1, -1, null, forced) && this.FindIngredients(pawn, building_SubcoreScanner).Thing != null;
		}

		// Token: 0x06005865 RID: 22629 RVA: 0x0020AF9C File Offset: 0x0020919C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_SubcoreScanner building_SubcoreScanner = t as Building_SubcoreScanner;
			if (building_SubcoreScanner == null || building_SubcoreScanner.State != SubcoreScannerState.WaitingForIngredients)
			{
				return null;
			}
			ThingCount thingCount = this.FindIngredients(pawn, building_SubcoreScanner);
			if (thingCount.Thing != null)
			{
				Job job = HaulAIUtility.HaulToContainerJob(pawn, thingCount.Thing, t);
				job.count = Mathf.Min(job.count, thingCount.Count);
				return job;
			}
			return null;
		}

		// Token: 0x06005866 RID: 22630 RVA: 0x0020AFF8 File Offset: 0x002091F8
		private ThingCount FindIngredients(Pawn pawn, Building_SubcoreScanner scanner)
		{
			WorkGiver_HaulToSubcoreScanner.<>c__DisplayClass4_0 CS$<>8__locals1 = new WorkGiver_HaulToSubcoreScanner.<>c__DisplayClass4_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.scanner = scanner;
			Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<FindIngredients>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				return default(ThingCount);
			}
			int requiredCountOf = CS$<>8__locals1.scanner.GetRequiredCountOf(thing.def);
			return new ThingCount(thing, Mathf.Min(thing.stackCount, requiredCountOf), false);
		}
	}
}
