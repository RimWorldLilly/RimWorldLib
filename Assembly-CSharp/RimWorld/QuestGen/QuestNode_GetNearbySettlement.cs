using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021CB RID: 8651
	public class QuestNode_GetNearbySettlement : QuestNode
	{
		// Token: 0x0600D759 RID: 55129 RVA: 0x004FE99C File Offset: 0x004FCB9C
		private Settlement RandomNearbyTradeableSettlement(PlanetTile originTile, Slate slate)
		{
			QuestNode_GetNearbySettlement.<>c__DisplayClass0_0 CS$<>8__locals1 = new QuestNode_GetNearbySettlement.<>c__DisplayClass0_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.slate = slate;
			CS$<>8__locals1.originTile = originTile;
			return Find.WorldObjects.SettlementBases.Where(new Func<Settlement, bool>(CS$<>8__locals1.<RandomNearbyTradeableSettlement>g__Validator|0)).RandomElementWithFallback(null);
		}

		// Token: 0x0600D75A RID: 55130 RVA: 0x004FE9E8 File Offset: 0x004FCBE8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			Settlement settlement = this.RandomNearbyTradeableSettlement(map.Tile, slate);
			QuestGen.slate.Set<Settlement>(this.storeAs.GetValue(slate), settlement, false);
			if (!string.IsNullOrEmpty(this.storeFactionAs.GetValue(slate)))
			{
				QuestGen.slate.Set<Faction>(this.storeFactionAs.GetValue(slate), settlement.Faction, false);
			}
			if (!this.storeFactionLeaderAs.GetValue(slate).NullOrEmpty())
			{
				QuestGen.slate.Set<Pawn>(this.storeFactionLeaderAs.GetValue(slate), settlement.Faction.leader, false);
			}
			if (!this.storeCanCaravanAs.GetValue(slate).NullOrEmpty())
			{
				bool var = settlement.Tile.Valid && map.Tile.Valid && settlement.Tile.Layer == map.Tile.Layer && settlement.Tile.LayerDef.SurfaceTiles;
				QuestGen.slate.Set<bool>(this.storeCanCaravanAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x0600D75B RID: 55131 RVA: 0x004FEB20 File Offset: 0x004FCD20
		protected override bool TestRunInt(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			Settlement settlement = this.RandomNearbyTradeableSettlement(map.Tile, slate);
			if (settlement != null)
			{
				slate.Set<Settlement>(this.storeAs.GetValue(slate), settlement, false);
				if (!string.IsNullOrEmpty(this.storeFactionAs.GetValue(slate)))
				{
					slate.Set<Faction>(this.storeFactionAs.GetValue(slate), settlement.Faction, false);
				}
				if (!string.IsNullOrEmpty(this.storeFactionLeaderAs.GetValue(slate)))
				{
					slate.Set<Pawn>(this.storeFactionLeaderAs.GetValue(slate), settlement.Faction.leader, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x040088AB RID: 34987
		public SlateRef<bool> allowActiveTradeRequest = true;

		// Token: 0x040088AC RID: 34988
		public SlateRef<bool> canBeSpace;

		// Token: 0x040088AD RID: 34989
		public SlateRef<bool> requireSameOrAdjacentLayer;

		// Token: 0x040088AE RID: 34990
		public SlateRef<float> maxTileDistance;

		// Token: 0x040088AF RID: 34991
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088B0 RID: 34992
		[NoTranslate]
		public SlateRef<string> storeFactionAs;

		// Token: 0x040088B1 RID: 34993
		[NoTranslate]
		public SlateRef<string> storeFactionLeaderAs;

		// Token: 0x040088B2 RID: 34994
		[NoTranslate]
		public SlateRef<string> storeCanCaravanAs;

		// Token: 0x040088B3 RID: 34995
		public SlateRef<List<PlanetLayerDef>> layerWhitelist;

		// Token: 0x040088B4 RID: 34996
		public SlateRef<List<PlanetLayerDef>> layerBlacklist;
	}
}
