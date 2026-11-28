using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022E2 RID: 8930
	public class FeatureWorker_MountainRange : FeatureWorker_Cluster
	{
		// Token: 0x0600DC5B RID: 56411 RVA: 0x0052188C File Offset: 0x0051FA8C
		protected override bool IsRoot(PlanetTile tile)
		{
			return Find.WorldGrid[tile].hilliness != Hilliness.Flat;
		}

		// Token: 0x0600DC5C RID: 56412 RVA: 0x005218A4 File Offset: 0x0051FAA4
		protected override bool CanTraverse(PlanetTile tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].PrimaryBiome != BiomeDefOf.Ocean;
		}
	}
}
