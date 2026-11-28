using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015F1 RID: 5617
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		// Token: 0x06008BA1 RID: 35745 RVA: 0x00329F7F File Offset: 0x0032817F
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return BiomeWorker_IceSheet.PermaIceScore(tile);
		}

		// Token: 0x06008BA2 RID: 35746 RVA: 0x00329F95 File Offset: 0x00328195
		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
