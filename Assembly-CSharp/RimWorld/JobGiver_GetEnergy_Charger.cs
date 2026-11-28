using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B45 RID: 2885
	public class JobGiver_GetEnergy_Charger : JobGiver_GetEnergy
	{
		// Token: 0x06005488 RID: 21640 RVA: 0x001F6770 File Offset: 0x001F4970
		public static Building_MechCharger GetClosestCharger(Pawn mech, Pawn carrier, bool forced)
		{
			if (!mech.Spawned || !carrier.Spawned)
			{
				return null;
			}
			Danger danger = forced ? Danger.Deadly : Danger.Some;
			return (Building_MechCharger)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForGroup(ThingRequestGroup.MechCharger), PathEndMode.InteractionCell, TraverseParms.For(carrier, danger, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing t)
			{
				Building_MechCharger building_MechCharger = (Building_MechCharger)t;
				if (!carrier.CanReach(t, PathEndMode.InteractionCell, danger, false, false, TraverseMode.ByPawn))
				{
					return false;
				}
				if (carrier != mech)
				{
					if (!forced && building_MechCharger.Map.reservationManager.ReservedBy(building_MechCharger, carrier, null))
					{
						return false;
					}
					if (forced && KeyBindingDefOf.QueueOrder.IsDownEvent && building_MechCharger.Map.reservationManager.ReservedBy(building_MechCharger, carrier, null))
					{
						return false;
					}
				}
				return !t.IsForbidden(carrier) && carrier.CanReserve(t, 1, -1, null, forced) && building_MechCharger.CanPawnChargeCurrently(mech);
			}, null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06005489 RID: 21641 RVA: 0x001F681C File Offset: 0x001F4A1C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!this.ShouldAutoRecharge(pawn))
			{
				return null;
			}
			Building_MechCharger closestCharger = JobGiver_GetEnergy_Charger.GetClosestCharger(pawn, pawn, false);
			if (closestCharger != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.MechCharge, closestCharger);
				job.overrideFacing = Rot4.South;
				return job;
			}
			return null;
		}
	}
}
