using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015F6 RID: 5622
	public class BiomeWorker_Space : BiomeWorker
	{
		// Token: 0x06008BB1 RID: 35761 RVA: 0x0032A37D File Offset: 0x0032857D
		public override bool CanPlaceOnLayer(BiomeDef biome, PlanetLayer layer)
		{
			return layer.Def == PlanetLayerDefOf.Orbit;
		}

		// Token: 0x06008BB2 RID: 35762 RVA: 0x0032A38C File Offset: 0x0032858C
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			return -100f;
		}
	}
}
