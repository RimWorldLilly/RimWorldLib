using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015FB RID: 5627
	public class BiomeWorker_Tundra : BiomeWorker
	{
		// Token: 0x06008BC0 RID: 35776 RVA: 0x0032A5B5 File Offset: 0x003287B5
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return -tile.temperature;
		}
	}
}
