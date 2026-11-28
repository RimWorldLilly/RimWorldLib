using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015EF RID: 5615
	public class BiomeWorker_Glowforest : BiomeWorker
	{
		// Token: 0x06008B9B RID: 35739 RVA: 0x00329E30 File Offset: 0x00328030
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < 10f || tile.temperature > 55f)
			{
				return 0f;
			}
			if (tile.rainfall < 1000f)
			{
				return 0f;
			}
			if (tile.swampiness < 0.6f)
			{
				return 0f;
			}
			return 23f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 220f + tile.swampiness * 3f;
		}
	}
}
