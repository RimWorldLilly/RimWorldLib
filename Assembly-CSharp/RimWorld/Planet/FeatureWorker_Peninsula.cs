using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022E8 RID: 8936
	public class FeatureWorker_Peninsula : FeatureWorker_Protrusion
	{
		// Token: 0x0600DC76 RID: 56438 RVA: 0x005220F0 File Offset: 0x005202F0
		protected override bool IsRoot(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			return primaryBiome != BiomeDefOf.Ocean && primaryBiome != BiomeDefOf.Lake;
		}
	}
}
