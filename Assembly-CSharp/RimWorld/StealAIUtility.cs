using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B24 RID: 2852
	public static class StealAIUtility
	{
		// Token: 0x0600541E RID: 21534 RVA: 0x001F4920 File Offset: 0x001F2B20
		public static bool TryFindBestItemToSteal(IntVec3 root, Map map, float maxDist, out Thing item, Pawn thief, List<Thing> disallowed = null)
		{
			if (map == null)
			{
				item = null;
				return false;
			}
			if (thief != null && !thief.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				item = null;
				return false;
			}
			if ((thief != null && !map.reachability.CanReachMapEdge(thief.Position, TraverseParms.For(thief, Danger.Some, TraverseMode.ByPawn, false, false, false, true))) || (thief == null && !map.reachability.CanReachMapEdge(root, TraverseParms.For(TraverseMode.PassDoors, Danger.Some, false, false, false, true, false))))
			{
				item = null;
				return false;
			}
			Predicate<Thing> validator = (Thing t) => (thief == null || thief.CanReserve(t, 1, -1, null, false)) && (disallowed == null || !disallowed.Contains(t)) && t.def.stealable && !t.IsBurning();
			item = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(root, map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEverOrMinifiable), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false, false, false, true, false), maxDist, validator, (Thing x) => StealAIUtility.GetValue(x), 15, 15, false);
			if (item != null && StealAIUtility.GetValue(item) < 320f)
			{
				item = null;
			}
			return item != null;
		}

		// Token: 0x0600541F RID: 21535 RVA: 0x001F4A38 File Offset: 0x001F2C38
		public static float TotalMarketValueAround(List<Pawn> pawns)
		{
			float num = 0f;
			StealAIUtility.tmpToSteal.Clear();
			for (int i = 0; i < pawns.Count; i++)
			{
				Thing thing;
				if (pawns[i].Spawned && StealAIUtility.TryFindBestItemToSteal(pawns[i].Position, pawns[i].Map, 7f, out thing, pawns[i], StealAIUtility.tmpToSteal))
				{
					num += StealAIUtility.GetValue(thing);
					StealAIUtility.tmpToSteal.Add(thing);
				}
			}
			StealAIUtility.tmpToSteal.Clear();
			return num;
		}

		// Token: 0x06005420 RID: 21536 RVA: 0x001F4AC8 File Offset: 0x001F2CC8
		public static float StartStealingMarketValueThreshold(Lord lord)
		{
			Rand.PushState();
			Rand.Seed = lord.loadID;
			float randomInRange = StealAIUtility.StealThresholdValuePerCombatPowerRange.RandomInRange;
			Rand.PopState();
			float num = 0f;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				num += Mathf.Max(lord.ownedPawns[i].kindDef.combatPower, 100f);
			}
			return num * randomInRange;
		}

		// Token: 0x06005421 RID: 21537 RVA: 0x001F4B3A File Offset: 0x001F2D3A
		public static float GetValue(Thing thing)
		{
			return thing.MarketValue * (float)thing.stackCount;
		}

		// Token: 0x0400325C RID: 12892
		private const float MinMarketValueToTake = 320f;

		// Token: 0x0400325D RID: 12893
		private static readonly FloatRange StealThresholdValuePerCombatPowerRange = new FloatRange(2f, 10f);

		// Token: 0x0400325E RID: 12894
		private const float MinCombatPowerPerPawn = 100f;

		// Token: 0x0400325F RID: 12895
		private static List<Thing> tmpToSteal = new List<Thing>();
	}
}
