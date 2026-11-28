using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015E9 RID: 5609
	public class BiomeWorker_AridShrubland : BiomeWorker
	{
		// Token: 0x06008B89 RID: 35721 RVA: 0x00329B25 File Offset: 0x00327D25
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_AridShrubland.Allowed(tile))
			{
				return 0f;
			}
			return BiomeWorker_AridShrubland.Score(tile);
		}

		// Token: 0x06008B8A RID: 35722 RVA: 0x00329B49 File Offset: 0x00327D49
		public static bool Allowed(Tile tile)
		{
			return tile.temperature >= -10f && tile.rainfall >= 600f && tile.rainfall < 2000f;
		}

		// Token: 0x06008B8B RID: 35723 RVA: 0x00329B77 File Offset: 0x00327D77
		public static float Score(Tile tile)
		{
			return 22.5f + (tile.temperature - 20f) * 2.2f + (tile.rainfall - 600f) / 100f;
		}
	}
}
