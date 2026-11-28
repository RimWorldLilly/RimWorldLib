using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015EC RID: 5612
	public class BiomeWorker_Desert : BiomeWorker
	{
		// Token: 0x06008B91 RID: 35729 RVA: 0x00329C53 File Offset: 0x00327E53
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_Desert.Allowed(tile))
			{
				return 0f;
			}
			return BiomeWorker_Desert.Score(tile);
		}

		// Token: 0x06008B92 RID: 35730 RVA: 0x00329C77 File Offset: 0x00327E77
		public static bool Allowed(Tile tile)
		{
			return tile.rainfall < 600f;
		}

		// Token: 0x06008B93 RID: 35731 RVA: 0x00329C89 File Offset: 0x00327E89
		public static float Score(Tile tile)
		{
			return tile.temperature + 0.0001f;
		}
	}
}
