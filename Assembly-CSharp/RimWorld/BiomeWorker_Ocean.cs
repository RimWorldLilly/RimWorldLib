using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015F4 RID: 5620
	public class BiomeWorker_Ocean : BiomeWorker
	{
		// Token: 0x06008BAA RID: 35754 RVA: 0x0032A203 File Offset: 0x00328403
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (!tile.WaterCovered)
			{
				return -100f;
			}
			return 0f;
		}
	}
}
