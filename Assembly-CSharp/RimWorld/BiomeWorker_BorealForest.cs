using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015EA RID: 5610
	public class BiomeWorker_BorealForest : BiomeWorker
	{
		// Token: 0x06008B8D RID: 35725 RVA: 0x00329BAC File Offset: 0x00327DAC
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < -10f)
			{
				return 0f;
			}
			if (tile.rainfall < 600f)
			{
				return 0f;
			}
			return 15f;
		}
	}
}
