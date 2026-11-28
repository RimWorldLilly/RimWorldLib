using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022E6 RID: 8934
	public class FeatureWorker_Island : FeatureWorker_FloodFill
	{
		// Token: 0x0600DC71 RID: 56433 RVA: 0x00522064 File Offset: 0x00520264
		protected override bool IsRoot(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			return primaryBiome != BiomeDefOf.Ocean && primaryBiome != BiomeDefOf.Lake;
		}

		// Token: 0x0600DC72 RID: 56434 RVA: 0x00522097 File Offset: 0x00520297
		protected override bool IsPossiblyAllowed(PlanetTile tile)
		{
			return Find.WorldGrid[tile].PrimaryBiome == BiomeDefOf.Lake;
		}
	}
}
