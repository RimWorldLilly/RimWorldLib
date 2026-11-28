using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020015F5 RID: 5621
	public class BiomeWorker_Scarlands : BiomeWorker
	{
		// Token: 0x06008BAC RID: 35756 RVA: 0x0032A220 File Offset: 0x00328420
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (!BiomeWorker_TemperateForest.Allowed(tile) || !this.AllowedAt(planetTile))
			{
				return 0f;
			}
			return BiomeWorker_TemperateForest.Score(tile) + 20f;
		}

		// Token: 0x06008BAD RID: 35757 RVA: 0x0032A254 File Offset: 0x00328454
		private bool AllowedAt(PlanetTile tileID)
		{
			if (this.NoiseAt(tileID) < 0.9f)
			{
				return false;
			}
			Find.WorldGrid.GetTileNeighbors(tileID, BiomeWorker_Scarlands.SearchList);
			if (BiomeWorker_Scarlands.SearchList.Empty<PlanetTile>())
			{
				return false;
			}
			foreach (PlanetTile tileID2 in BiomeWorker_Scarlands.SearchList)
			{
				if (this.NoiseAt(tileID2) >= 0.9f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008BAE RID: 35758 RVA: 0x0032A2E4 File Offset: 0x003284E4
		private float NoiseAt(PlanetTile tileID)
		{
			if (this.perlin == null)
			{
				this.perlin = new Perlin(0.20000000298023224, 0.5, 0.10000000149011612, 4, true, false, Gen.HashCombineInt(Find.World.info.Seed, 417235213), QualityMode.Medium);
			}
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tileID);
			return (float)this.perlin.GetValue((double)tileCenter.x, (double)tileCenter.y, (double)tileCenter.z);
		}

		// Token: 0x040055B4 RID: 21940
		private Perlin perlin;

		// Token: 0x040055B5 RID: 21941
		private static readonly List<PlanetTile> SearchList = new List<PlanetTile>();

		// Token: 0x040055B6 RID: 21942
		private const float NoiseThreshold = 0.9f;

		// Token: 0x040055B7 RID: 21943
		private const int SeedPart = 417235213;
	}
}
