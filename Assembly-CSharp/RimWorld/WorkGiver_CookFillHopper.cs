using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BFC RID: 3068
	public class WorkGiver_CookFillHopper : WorkGiver_Scanner
	{
		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x060057AF RID: 22447 RVA: 0x0020738D File Offset: 0x0020558D
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Hopper);
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x060057B0 RID: 22448 RVA: 0x00207399 File Offset: 0x00205599
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060057B1 RID: 22449 RVA: 0x0020739C File Offset: 0x0020559C
		public WorkGiver_CookFillHopper()
		{
			if (WorkGiver_CookFillHopper.TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans == null)
			{
				WorkGiver_CookFillHopper.TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans = "TheOnlyAvailableFoodIsInStorageOfHigherPriority".Translate();
			}
			if (WorkGiver_CookFillHopper.NoFoodToFillHopperTrans == null)
			{
				WorkGiver_CookFillHopper.NoFoodToFillHopperTrans = "NoFoodToFillHopper".Translate();
			}
		}

		// Token: 0x060057B2 RID: 22450 RVA: 0x002073DC File Offset: 0x002055DC
		public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
		{
			ISlotGroupParent slotGroupParent = thing as ISlotGroupParent;
			if (slotGroupParent == null)
			{
				return null;
			}
			if (!pawn.CanReserve(thing.Position, 1, -1, null, false))
			{
				return null;
			}
			float num = 0f;
			List<Thing> list = pawn.Map.thingGrid.ThingsListAt(thing.Position);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing2.def))
				{
					num = (float)thing2.stackCount / (float)thing2.def.stackLimit;
				}
			}
			if (num > 0.35f)
			{
				JobFailReason.Is("AlreadyFilledLower".Translate(), null);
				return null;
			}
			return WorkGiver_CookFillHopper.HopperFillFoodJob(pawn, slotGroupParent, forced);
		}

		// Token: 0x060057B3 RID: 22451 RVA: 0x00207494 File Offset: 0x00205694
		public static Job HopperFillFoodJob(Pawn pawn, ISlotGroupParent hopperSgp, bool forced)
		{
			Building building = (Building)hopperSgp;
			if (!pawn.CanReserveAndReach(building.Position, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				return null;
			}
			ThingDef thingDef = null;
			Thing firstItem = building.Position.GetFirstItem(building.Map);
			if (firstItem != null)
			{
				if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(firstItem.def))
				{
					thingDef = firstItem.def;
				}
				else
				{
					if (firstItem.IsForbidden(pawn))
					{
						return null;
					}
					return HaulAIUtility.HaulAsideJobFor(pawn, firstItem);
				}
			}
			List<Thing> list;
			if (thingDef == null)
			{
				list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree);
			}
			else
			{
				list = pawn.Map.listerThings.ThingsOfDef(thingDef);
			}
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.IsNutritionGivingIngestible && (thing.def.ingestible.preferability == FoodPreferability.RawBad || thing.def.ingestible.preferability == FoodPreferability.RawTasty) && HaulAIUtility.PawnCanAutomaticallyHaul(pawn, thing, forced) && pawn.Map.haulDestinationManager.SlotGroupAt(building.Position).Settings.AllowedToAccept(thing))
				{
					if (StoreUtility.CurrentStoragePriorityOf(thing, forced) >= hopperSgp.GetSlotGroup().Settings.Priority)
					{
						flag = true;
						JobFailReason.Is(WorkGiver_CookFillHopper.TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans, null);
					}
					else
					{
						Job job = HaulAIUtility.HaulToCellStorageJob(pawn, thing, building.Position, true);
						if (job != null)
						{
							return job;
						}
					}
				}
			}
			if (!flag)
			{
				JobFailReason.Is(WorkGiver_CookFillHopper.NoFoodToFillHopperTrans, null);
			}
			return null;
		}

		// Token: 0x04003337 RID: 13111
		private static string TheOnlyAvailableFoodIsInStorageOfHigherPriorityTrans;

		// Token: 0x04003338 RID: 13112
		private static string NoFoodToFillHopperTrans;
	}
}
