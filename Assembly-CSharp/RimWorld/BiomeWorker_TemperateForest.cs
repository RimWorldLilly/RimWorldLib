using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015F7 RID: 5623
	public class BiomeWorker_TemperateForest : BiomeWorker
	{
		// Token: 0x06008BB4 RID: 35764 RVA: 0x0032A39B File Offset: 0x0032859B
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_TemperateForest.Allowed(tile))
			{
				return 0f;
			}
			return BiomeWorker_TemperateForest.Score(tile);
		}

		// Token: 0x06008BB5 RID: 35765 RVA: 0x0032A3BF File Offset: 0x003285BF
		public static bool Allowed(Tile tile)
		{
			return tile.temperature >= -10f && tile.rainfall >= 600f;
		}

		// Token: 0x06008BB6 RID: 35766 RVA: 0x0032A3E0 File Offset: 0x003285E0
		public static float Score(Tile tile)
		{
			return 15f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 180f;
		}
	}
}
