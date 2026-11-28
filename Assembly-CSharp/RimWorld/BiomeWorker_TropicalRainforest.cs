using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015F9 RID: 5625
	public class BiomeWorker_TropicalRainforest : BiomeWorker
	{
		// Token: 0x06008BBA RID: 35770 RVA: 0x0032A4A0 File Offset: 0x003286A0
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < 15f)
			{
				return 0f;
			}
			if (tile.rainfall < 2000f)
			{
				return 0f;
			}
			return 28f + (tile.temperature - 20f) * 1.5f + (tile.rainfall - 600f) / 165f;
		}
	}
}
