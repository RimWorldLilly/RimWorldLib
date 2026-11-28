using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001781 RID: 6017
	public class SitePartWorker_WorkSite_Logging : SitePartWorker_WorkSite
	{
		// Token: 0x1700198D RID: 6541
		// (get) Token: 0x060095B0 RID: 38320 RVA: 0x00366881 File Offset: 0x00364A81
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowLoggingCamps
				select p;
			}
		}

		// Token: 0x1700198E RID: 6542
		// (get) Token: 0x060095B1 RID: 38321 RVA: 0x003668AC File Offset: 0x00364AAC
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Loggers;
			}
		}

		// Token: 0x060095B2 RID: 38322 RVA: 0x003668B3 File Offset: 0x00364AB3
		public override bool CanSpawnOn(PlanetTile tile)
		{
			return Find.WorldGrid[tile].PrimaryBiome.TreeDensity >= BiomeDefOf.Tundra.TreeDensity;
		}
	}
}
