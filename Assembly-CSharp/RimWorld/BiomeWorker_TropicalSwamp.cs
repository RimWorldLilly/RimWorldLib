using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015FA RID: 5626
	public class BiomeWorker_TropicalSwamp : BiomeWorker
	{
		// Token: 0x06008BBC RID: 35772 RVA: 0x0032A514 File Offset: 0x00328714
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_TropicalSwamp.Allowed(tile))
			{
				return 0f;
			}
			return BiomeWorker_TropicalSwamp.Score(tile);
		}

		// Token: 0x06008BBD RID: 35773 RVA: 0x0032A538 File Offset: 0x00328738
		public static bool Allowed(Tile tile)
		{
			return tile.temperature >= 15f && tile.temperature <= 55f && tile.rainfall >= 2000f && tile.swampiness >= 0.5f;
		}

		// Token: 0x06008BBE RID: 35774 RVA: 0x0032A573 File Offset: 0x00328773
		public static float Score(Tile tile)
		{
			return 28f + (tile.temperature - 20f) * 1.5f + (tile.rainfall - 600f) / 165f + tile.swampiness * 3f;
		}
	}
}
