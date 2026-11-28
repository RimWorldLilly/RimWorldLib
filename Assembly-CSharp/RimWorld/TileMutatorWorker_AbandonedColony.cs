using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001309 RID: 4873
	public abstract class TileMutatorWorker_AbandonedColony : TileMutatorWorker
	{
		// Token: 0x06007914 RID: 30996 RVA: 0x002B02C6 File Offset: 0x002AE4C6
		public TileMutatorWorker_AbandonedColony(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007915 RID: 30997
		protected abstract Faction GetFaction();

		// Token: 0x06007916 RID: 30998 RVA: 0x002B02D0 File Offset: 0x002AE4D0
		public override void GenerateCriticalStructures(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Faction faction = this.GetFaction();
			MapGenUtility.PostProcessSettlementParams postProcessSettlementParams = new MapGenUtility.PostProcessSettlementParams
			{
				clearBuildingFaction = true,
				faction = faction,
				damageBuildings = true,
				canDamageWalls = false,
				noFuel = true,
				ageCorpses = true
			};
			new GenStep_Settlement
			{
				generatePawns = false,
				overrideFaction = faction,
				postProcessSettlementParams = postProcessSettlementParams,
				count = 1,
				lootThingSetMaker = ThingSetMakerDefOf.MapGen_AbandonedColonyStockpile,
				lootMarketValue = new float?(500f)
			}.Generate(map, default(GenStepParams));
			CellRect var = MapGenerator.GetVar<CellRect>("SettlementRect");
			MapGenUtility.ScatterCorpses(var, map, faction, TileMutatorWorker_AbandonedColony.CorpsesRange, TileMutatorWorker_AbandonedColony.DeadDaysRange);
			MapGenUtility.DestroyTurrets(map);
			MapGenUtility.DestroyProcessedFood(map);
			MapGenUtility.ForbidAllItems(map);
			MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Add(var);
		}

		// Token: 0x04004BF7 RID: 19447
		private static readonly IntRange CorpsesRange = new IntRange(0, 3);

		// Token: 0x04004BF8 RID: 19448
		private static readonly FloatRange DeadDaysRange = new FloatRange(20f, 60f);

		// Token: 0x04004BF9 RID: 19449
		private const float LootMarketValue = 500f;
	}
}
