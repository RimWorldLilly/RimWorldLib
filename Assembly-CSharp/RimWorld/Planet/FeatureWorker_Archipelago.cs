using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022E0 RID: 8928
	public class FeatureWorker_Archipelago : FeatureWorker_Cluster
	{
		// Token: 0x0600DC48 RID: 56392 RVA: 0x00521294 File Offset: 0x0051F494
		protected override bool IsRoot(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			return primaryBiome != BiomeDefOf.Ocean && primaryBiome != BiomeDefOf.Lake;
		}

		// Token: 0x0600DC49 RID: 56393 RVA: 0x005212C7 File Offset: 0x0051F4C7
		protected override bool CanTraverse(PlanetTile tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			return true;
		}

		// Token: 0x0600DC4A RID: 56394 RVA: 0x005212D0 File Offset: 0x0051F4D0
		protected override bool IsMember(PlanetTile tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = true;
			bool flag;
			return base.IsMember(tile, out flag);
		}
	}
}
