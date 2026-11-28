using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200177E RID: 6014
	public class SitePartWorker_WorkSite_Hunting : SitePartWorker_WorkSite
	{
		// Token: 0x17001987 RID: 6535
		// (get) Token: 0x060095A1 RID: 38305 RVA: 0x0036670E File Offset: 0x0036490E
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowHuntingCamps
				select p;
			}
		}

		// Token: 0x17001988 RID: 6536
		// (get) Token: 0x060095A2 RID: 38306 RVA: 0x00366739 File Offset: 0x00364939
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Hunters;
			}
		}

		// Token: 0x060095A3 RID: 38307 RVA: 0x00366740 File Offset: 0x00364940
		public override bool CanSpawnOn(PlanetTile tile)
		{
			return Find.WorldGrid[tile].AnimalDensity > BiomeDefOf.Desert.animalDensity && base.CanSpawnOn(tile);
		}

		// Token: 0x060095A4 RID: 38308 RVA: 0x00366767 File Offset: 0x00364967
		public override IEnumerable<SitePartWorker_WorkSite.CampLootThingStruct> LootThings(PlanetTile tile)
		{
			SitePartWorker_WorkSite_Hunting.<LootThings>d__5 <LootThings>d__ = new SitePartWorker_WorkSite_Hunting.<LootThings>d__5(-2);
			<LootThings>d__.<>3__tile = tile;
			return <LootThings>d__;
		}
	}
}
