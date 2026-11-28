using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015F0 RID: 5616
	public class BiomeWorker_Grasslands : BiomeWorker
	{
		// Token: 0x06008B9D RID: 35741 RVA: 0x00329ECB File Offset: 0x003280CB
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_Grasslands.Allowed(tile))
			{
				return 0f;
			}
			return BiomeWorker_Grasslands.Score(tile);
		}

		// Token: 0x06008B9E RID: 35742 RVA: 0x00329EF0 File Offset: 0x003280F0
		public static bool Allowed(Tile tile)
		{
			return tile.temperature >= -10f && tile.temperature <= 55f && tile.rainfall >= 800f && tile.rainfall < 2500f && tile.hilliness != Hilliness.Mountainous && tile.hilliness != Hilliness.Impassable;
		}

		// Token: 0x06008B9F RID: 35743 RVA: 0x00329F4A File Offset: 0x0032814A
		public static float Score(Tile tile)
		{
			return 22.5f + (tile.temperature - 20f) * 2.4f + (tile.rainfall - 800f) / 100f;
		}
	}
}
