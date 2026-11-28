using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000852 RID: 2130
	public static class CorpseObsessionMentalStateUtility
	{
		// Token: 0x060042F4 RID: 17140 RVA: 0x001A6D98 File Offset: 0x001A4F98
		public static Corpse GetClosestCorpseToDigUp(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (ModsConfig.AnomalyActive && Find.Anomaly.TryGetUnnaturalCorpseTrackerForHaunted(pawn, out unnaturalCorpseTracker) && CorpseObsessionMentalStateUtility.IsCorpseValid(unnaturalCorpseTracker.Corpse, pawn, false))
			{
				return unnaturalCorpseTracker.Corpse;
			}
			Building_Grave building_Grave = (Building_Grave)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Grave), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing x)
			{
				Building_Grave building_Grave2 = (Building_Grave)x;
				return building_Grave2.HasCorpse && CorpseObsessionMentalStateUtility.IsCorpseValid(building_Grave2.Corpse, pawn, true);
			}, null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (building_Grave == null)
			{
				return null;
			}
			return building_Grave.Corpse;
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x001A6E54 File Offset: 0x001A5054
		public static bool IsCorpseValid(Corpse corpse, Pawn pawn, bool ignoreReachability = false)
		{
			if (corpse == null || corpse.Destroyed || !corpse.InnerPawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (pawn.carryTracker.CarriedThing == corpse)
			{
				return true;
			}
			if (corpse.Spawned)
			{
				return pawn.CanReserve(corpse, 1, -1, null, false) && (ignoreReachability || pawn.CanReach(corpse, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn));
			}
			Building_Grave building_Grave = corpse.ParentHolder as Building_Grave;
			return building_Grave != null && building_Grave.Spawned && pawn.CanReserve(building_Grave, 1, -1, null, false) && (ignoreReachability || pawn.CanReach(building_Grave, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn));
		}
	}
}
