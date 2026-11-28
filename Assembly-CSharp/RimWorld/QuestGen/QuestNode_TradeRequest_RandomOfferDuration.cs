using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200224A RID: 8778
	public class QuestNode_TradeRequest_RandomOfferDuration : QuestNode
	{
		// Token: 0x0600D91D RID: 55581 RVA: 0x00506A94 File Offset: 0x00504C94
		public static int RandomOfferDurationTicks(PlanetTile tileIdFrom, PlanetTile tileIdTo, out int travelTicks)
		{
			int randomInRange = SiteTuning.QuestSiteTimeoutDaysRange.RandomInRange;
			travelTicks = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(tileIdFrom, tileIdTo, null);
			float num = (float)travelTicks / 60000f;
			int num2 = Mathf.CeilToInt(Mathf.Max(num + 6f, num * 1.35f));
			if (num2 > SiteTuning.QuestSiteTimeoutDaysRange.max)
			{
				return -1;
			}
			int num3 = Mathf.Max(randomInRange, num2);
			return 60000 * num3;
		}

		// Token: 0x0600D91E RID: 55582 RVA: 0x00506AFC File Offset: 0x00504CFC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = slate.Get<Map>("map", null, false);
			PlanetTile tile = map.Tile;
			PlanetTile tile2 = this.settlement.GetValue(slate).Tile;
			if (!tile2.Valid || !tile.Valid || tile.Layer != tile2.Layer || !tile.LayerDef.SurfaceTiles)
			{
				return;
			}
			int var;
			slate.Set<int>(this.storeAs.GetValue(slate), QuestNode_TradeRequest_RandomOfferDuration.RandomOfferDurationTicks(map.Tile, this.settlement.GetValue(slate).Tile, out var), false);
			slate.Set<int>(this.storeEstimatedTravelTimeAs.GetValue(slate), var, false);
		}

		// Token: 0x0600D91F RID: 55583 RVA: 0x00506BB0 File Offset: 0x00504DB0
		protected override bool TestRunInt(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map.Tile.Layer != this.settlement.GetValue(slate).Tile.Layer)
			{
				return false;
			}
			int var;
			slate.Set<int>(this.storeAs.GetValue(slate), QuestNode_TradeRequest_RandomOfferDuration.RandomOfferDurationTicks(map.Tile, this.settlement.GetValue(slate).Tile, out var), false);
			slate.Set<int>(this.storeEstimatedTravelTimeAs.GetValue(slate), var, false);
			return true;
		}

		// Token: 0x04008AC6 RID: 35526
		private const float MinNonTravelTimeFractionOfTravelTime = 0.35f;

		// Token: 0x04008AC7 RID: 35527
		private const float MinNonTravelTimeDays = 6f;

		// Token: 0x04008AC8 RID: 35528
		public SlateRef<Settlement> settlement;

		// Token: 0x04008AC9 RID: 35529
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008ACA RID: 35530
		[NoTranslate]
		public SlateRef<string> storeEstimatedTravelTimeAs;
	}
}
