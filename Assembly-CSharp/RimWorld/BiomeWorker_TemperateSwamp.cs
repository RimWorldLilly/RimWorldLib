using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020015F8 RID: 5624
	public class BiomeWorker_TemperateSwamp : BiomeWorker
	{
		// Token: 0x06008BB8 RID: 35768 RVA: 0x0032A410 File Offset: 0x00328610
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
			if (tile.swampiness < 0.5f)
			{
				return 0f;
			}
			return 15f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 180f + tile.swampiness * 3f;
		}
	}
}
