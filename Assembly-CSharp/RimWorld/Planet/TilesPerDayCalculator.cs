using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023A4 RID: 9124
	public static class TilesPerDayCalculator
	{
		// Token: 0x0600E301 RID: 58113 RVA: 0x00546F7C File Offset: 0x0054517C
		public static float ApproxTilesPerDay(int caravanTicksPerMove, PlanetTile tile, PlanetTile nextTile, StringBuilder explanation = null, string caravanTicksPerMoveExplanation = null, bool immobile = false)
		{
			if (!nextTile.Valid)
			{
				nextTile = Find.WorldGrid.FindMostReasonableAdjacentTileForDisplayedPathCost(tile);
			}
			int num = Mathf.CeilToInt((float)Caravan_PathFollower.CostToMove(caravanTicksPerMove, tile, nextTile, null, false, explanation, caravanTicksPerMoveExplanation, immobile) / 1f);
			if (num == 0)
			{
				return 0f;
			}
			return 60000f / (float)num;
		}

		// Token: 0x0600E302 RID: 58114 RVA: 0x00546FDC File Offset: 0x005451DC
		public static float ApproxTilesPerDay(Caravan caravan, StringBuilder explanation = null)
		{
			return TilesPerDayCalculator.ApproxTilesPerDay(caravan.TicksPerMove, caravan.Tile, caravan.pather.Moving ? caravan.pather.nextTile : PlanetTile.Invalid, explanation, (explanation != null) ? caravan.TicksPerMoveExplanation : null, caravan.ImmobilizedByMass || caravan.Shuttle != null);
		}

		// Token: 0x0600E303 RID: 58115 RVA: 0x0054703C File Offset: 0x0054523C
		public static float ApproxTilesPerDay(List<TransferableOneWay> transferables, float massUsage, float massCapacity, PlanetTile tile, PlanetTile nextTile, bool isShuttle, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						TilesPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsage, massCapacity, isShuttle, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null, massUsage > massCapacity);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600E304 RID: 58116 RVA: 0x00547100 File Offset: 0x00545300
		public static float ApproxTilesPerDayLeftAfterTransfer(List<TransferableOneWay> transferables, float massUsageLeftAfterTransfer, float massCapacityLeftAfterTransfer, PlanetTile tile, PlanetTile nextTile, bool isShuttle, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						TilesPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsageLeftAfterTransfer, massCapacityLeftAfterTransfer, isShuttle, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null, massUsageLeftAfterTransfer > massCapacityLeftAfterTransfer);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600E305 RID: 58117 RVA: 0x005471CF File Offset: 0x005453CF
		public static float ApproxTilesPerDayLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, float massUsageLeftAfterTradeableTransfer, float massCapacityLeftAfterTradeableTransfer, PlanetTile tile, PlanetTile nextTile, bool isShuttle, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, TilesPerDayCalculator.tmpThingCounts);
			float result = TilesPerDayCalculator.ApproxTilesPerDay(TilesPerDayCalculator.tmpThingCounts, massUsageLeftAfterTradeableTransfer, massCapacityLeftAfterTradeableTransfer, tile, nextTile, isShuttle, explanation);
			TilesPerDayCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600E306 RID: 58118 RVA: 0x00547208 File Offset: 0x00545408
		public static float ApproxTilesPerDay(List<ThingCount> thingCounts, float massUsage, float massCapacity, PlanetTile tile, PlanetTile nextTile, bool isShuttle, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						TilesPerDayCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsage, massCapacity, isShuttle, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null, massUsage > massCapacity);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x040090DF RID: 37087
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040090E0 RID: 37088
		private static readonly List<ThingCount> tmpThingCounts = new List<ThingCount>();
	}
}
