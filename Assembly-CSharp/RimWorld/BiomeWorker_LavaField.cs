using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020015F3 RID: 5619
	public class BiomeWorker_LavaField : BiomeWorker
	{
		// Token: 0x06008BA7 RID: 35751 RVA: 0x0032A0FC File Offset: 0x003282FC
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!this.AllowedAt(planetTile))
			{
				return 0f;
			}
			float num;
			if (BiomeWorker_Desert.Allowed(tile))
			{
				num = BiomeWorker_Desert.Score(tile) - 0.0001f;
			}
			else
			{
				if (!BiomeWorker_AridShrubland.Allowed(tile))
				{
					return 0f;
				}
				num = BiomeWorker_AridShrubland.Score(tile) - 0.0001f;
			}
			if (tile.hilliness >= Hilliness.SmallHills)
			{
				num += 0.0002f;
			}
			return num;
		}

		// Token: 0x06008BA8 RID: 35752 RVA: 0x0032A16C File Offset: 0x0032836C
		private bool AllowedAt(PlanetTile tile)
		{
			if (this.perlin == null)
			{
				this.perlin = new Perlin(0.30000001192092896, 0.5, 0.5, 4, true, false, Gen.HashCombineInt(Find.World.info.Seed, 967525656), QualityMode.Medium);
			}
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tile);
			return (float)this.perlin.GetValue((double)tileCenter.x, (double)tileCenter.y, (double)tileCenter.z) >= 0.9f;
		}

		// Token: 0x040055B1 RID: 21937
		private Perlin perlin;

		// Token: 0x040055B2 RID: 21938
		private const float NoiseThreshold = 0.9f;

		// Token: 0x040055B3 RID: 21939
		private const int SeedPart = 967525656;
	}
}
