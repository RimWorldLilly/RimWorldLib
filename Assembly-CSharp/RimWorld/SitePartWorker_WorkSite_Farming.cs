using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200177F RID: 6015
	public class SitePartWorker_WorkSite_Farming : SitePartWorker_WorkSite
	{
		// Token: 0x17001989 RID: 6537
		// (get) Token: 0x060095A6 RID: 38310 RVA: 0x0036677F File Offset: 0x0036497F
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowFarmingCamps
				select p;
			}
		}

		// Token: 0x1700198A RID: 6538
		// (get) Token: 0x060095A7 RID: 38311 RVA: 0x003667AA File Offset: 0x003649AA
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Farmers;
			}
		}

		// Token: 0x060095A8 RID: 38312 RVA: 0x003667B1 File Offset: 0x003649B1
		public override bool CanSpawnOn(PlanetTile tile)
		{
			return Find.WorldGrid[tile].PrimaryBiome.allowFarmingCamps && this.LootThings(tile).Any<SitePartWorker_WorkSite.CampLootThingStruct>();
		}

		// Token: 0x060095A9 RID: 38313 RVA: 0x003667D8 File Offset: 0x003649D8
		public override IEnumerable<SitePartWorker_WorkSite.CampLootThingStruct> LootThings(PlanetTile tile)
		{
			SitePartWorker_WorkSite_Farming.<LootThings>d__5 <LootThings>d__ = new SitePartWorker_WorkSite_Farming.<LootThings>d__5(-2);
			<LootThings>d__.<>3__tile = tile;
			return <LootThings>d__;
		}
	}
}
