using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022E4 RID: 8932
	public class FeatureWorker_Biome : FeatureWorker_FloodFill
	{
		// Token: 0x0600DC62 RID: 56418 RVA: 0x00521A5E File Offset: 0x0051FC5E
		protected override bool IsRoot(PlanetTile tile)
		{
			return this.def.rootBiomes.Contains(Find.WorldGrid[tile].PrimaryBiome);
		}

		// Token: 0x0600DC63 RID: 56419 RVA: 0x00521A80 File Offset: 0x0051FC80
		protected override bool IsPossiblyAllowed(PlanetTile tile)
		{
			return this.def.acceptableBiomes.Contains(Find.WorldGrid[tile].PrimaryBiome);
		}
	}
}
