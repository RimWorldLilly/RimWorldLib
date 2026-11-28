using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015ED RID: 5613
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		// Token: 0x06008B95 RID: 35733 RVA: 0x00329CA0 File Offset: 0x00327EA0
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.rainfall >= 340f)
			{
				return 0f;
			}
			return tile.temperature * 2.7f - 13f - tile.rainfall * 0.14f;
		}
	}
}
