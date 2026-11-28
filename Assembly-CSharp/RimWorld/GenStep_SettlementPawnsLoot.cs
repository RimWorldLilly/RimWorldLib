using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020012CB RID: 4811
	public class GenStep_SettlementPawnsLoot : GenStep
	{
		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x06007764 RID: 30564 RVA: 0x002A1C64 File Offset: 0x0029FE64
		public override int SeedPart
		{
			get
			{
				return 4615611;
			}
		}

		// Token: 0x06007765 RID: 30565 RVA: 0x002A1C6C File Offset: 0x0029FE6C
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect cellRect;
			if (!MapGenerator.TryGetVar<CellRect>("SpawnRect", out cellRect))
			{
				Log.Error("GenStep_SettlementPawnsLoot tried to execute but no SpawnRect was found in the map generator. This CellRect must be set.");
				return;
			}
			Faction faction = this.GetFaction(map);
			if (this.generatePawns)
			{
				Lord lord = LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, cellRect.CenterCell, 25000, false), map, null);
				CellRect rect = cellRect;
				Faction faction2 = faction;
				Lord lord2 = lord;
				PawnGroupKindDef settlement = PawnGroupKindDefOf.Settlement;
				PawnGroupMakerParms pawnGroupMakerParams = null;
				bool flag = this.requiresRoof;
				MapGenUtility.GeneratePawns(map, rect, faction2, lord2, settlement, pawnGroupMakerParams, null, null, null, flag);
			}
			FloatRange? floatRange = this.lootMarketValue;
			if (floatRange == null || !floatRange.GetValueOrDefault().IsZeros)
			{
				ThingSetMakerDef thingSetMakerDef;
				if ((thingSetMakerDef = this.lootThingSetMaker) == null)
				{
					thingSetMakerDef = (faction.def.settlementLootMaker ?? ThingSetMakerDefOf.MapGen_AbandonedColonyStockpile);
				}
				ThingSetMakerDef thingSetMakerDef2 = thingSetMakerDef;
				CellRect rect2 = cellRect;
				ThingSetMakerDef setMakerDef = thingSetMakerDef2;
				FloatRange? marketValueRange = this.lootMarketValue;
				Faction faction3 = faction;
				bool flag = this.requiresRoof;
				MapGenUtility.GenerateLoot(map, rect2, setMakerDef, marketValueRange, null, faction3, flag);
			}
		}

		// Token: 0x06007766 RID: 30566 RVA: 0x002A1D54 File Offset: 0x0029FF54
		private Faction GetFaction(Map map)
		{
			Faction result;
			if (this.factionDef != null)
			{
				result = Find.FactionManager.FirstFactionOfDef(this.factionDef);
			}
			else if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				result = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				result = map.ParentFaction;
			}
			return result;
		}

		// Token: 0x04004AE4 RID: 19172
		public FactionDef factionDef;

		// Token: 0x04004AE5 RID: 19173
		public bool generatePawns = true;

		// Token: 0x04004AE6 RID: 19174
		public ThingSetMakerDef lootThingSetMaker;

		// Token: 0x04004AE7 RID: 19175
		public FloatRange? lootMarketValue;

		// Token: 0x04004AE8 RID: 19176
		public bool requiresRoof;
	}
}
