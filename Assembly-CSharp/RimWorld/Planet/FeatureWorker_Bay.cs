using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022E7 RID: 8935
	public class FeatureWorker_Bay : FeatureWorker_Protrusion
	{
		// Token: 0x0600DC74 RID: 56436 RVA: 0x005220B8 File Offset: 0x005202B8
		protected override bool IsRoot(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			return primaryBiome == BiomeDefOf.Ocean || primaryBiome == BiomeDefOf.Lake;
		}
	}
}
