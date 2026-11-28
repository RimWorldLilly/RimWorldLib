using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B1B RID: 2843
	public static class KidnapAIUtility
	{
		// Token: 0x06005408 RID: 21512 RVA: 0x001F4480 File Offset: 0x001F2680
		public static bool TryFindGoodKidnapVictim(Pawn kidnapper, float maxDist, out Pawn victim, List<Thing> disallowed = null)
		{
			if (!kidnapper.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !kidnapper.Map.reachability.CanReachMapEdge(kidnapper.Position, TraverseParms.For(kidnapper, Danger.Some, TraverseMode.ByPawn, false, false, false, true)))
			{
				victim = null;
				return false;
			}
			Predicate<Thing> validator = delegate(Thing t)
			{
				Pawn pawn = t as Pawn;
				return pawn.RaceProps.Humanlike && pawn.Downed && pawn.Faction == Faction.OfPlayer && pawn.Faction.HostileTo(kidnapper.Faction) && kidnapper.CanReserve(pawn, 1, -1, null, false) && (disallowed == null || !disallowed.Contains(pawn)) && (!ModsConfig.AnomalyActive || !pawn.IsSubhuman);
			};
			victim = (Pawn)GenClosest.ClosestThingReachable(kidnapper.Position, kidnapper.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false, false, false, true, false), maxDist, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
			return victim != null;
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x001F4548 File Offset: 0x001F2748
		public static Pawn ReachableWoundedGuest(Pawn searcher)
		{
			List<Pawn> list = searcher.Map.mapPawns.SpawnedPawnsInFaction(searcher.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				if (pawn.guest != null && !pawn.IsPrisoner && pawn.Downed && searcher.CanReserveAndReach(pawn, PathEndMode.OnCell, Danger.Some, 1, -1, null, false))
				{
					return pawn;
				}
			}
			return null;
		}
	}
}
