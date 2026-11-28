using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015EB RID: 5611
	public class BiomeWorker_ColdBog : BiomeWorker
	{
		// Token: 0x06008B8F RID: 35727 RVA: 0x00329BF0 File Offset: 0x00327DF0
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
			if (tile.swampiness < 0.5f)
			{
				return 0f;
			}
			return -tile.temperature + 13f + tile.swampiness * 8f;
		}
	}
}
