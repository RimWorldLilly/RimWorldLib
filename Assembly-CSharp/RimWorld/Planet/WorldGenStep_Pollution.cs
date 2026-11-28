using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02002350 RID: 9040
	public class WorldGenStep_Pollution : WorldGenStep
	{
		// Token: 0x1700274F RID: 10063
		// (get) Token: 0x0600DFC8 RID: 57288 RVA: 0x00533AB8 File Offset: 0x00531CB8
		public override int SeedPart
		{
			get
			{
				return 759372056;
			}
		}

		// Token: 0x0600DFC9 RID: 57289 RVA: 0x00533AC0 File Offset: 0x00531CC0
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			float pollution = Find.World.info.pollution;
			if (pollution <= 0f)
			{
				for (int i = 0; i < layer.TilesCount; i++)
				{
					Tile tile = layer[i];
					float pollutionOffset = tile.PrimaryBiome.pollutionOffset;
					tile.pollution = Mathf.Clamp01(pollutionOffset);
				}
				return;
			}
			Perlin perlin = new Perlin(0.10000000149011612, 2.0, 0.5, 6, seed.GetHashCode(), QualityMode.Medium);
			this.tmpTiles.Clear();
			this.tmpTileNoise.Clear();
			foreach (Tile tile2 in layer.Tiles)
			{
				if (tile2.PrimaryBiome.allowPollution)
				{
					this.tmpTileNoise.Add(tile2.tile, perlin.GetValue(layer.GetTileCenter(tile2.tile)));
					this.tmpTiles.Add(tile2.tile);
				}
			}
			this.tmpTiles.SortByDescending((PlanetTile t) => this.tmpTileNoise[t]);
			int num = Mathf.RoundToInt((float)this.tmpTiles.Count * pollution);
			for (int j = 0; j < num; j++)
			{
				Tile tile3 = layer[this.tmpTiles[j]];
				float value = Mathf.Lerp(0.25f, 1f, this.tmpTileNoise[this.tmpTiles[j]]);
				tile3.pollution = Mathf.Clamp01(value);
			}
			this.tmpTiles.Clear();
			this.tmpTileNoise.Clear();
		}

		// Token: 0x04008F99 RID: 36761
		private const float MinPollution = 0.25f;

		// Token: 0x04008F9A RID: 36762
		private const float MaxPollution = 1f;

		// Token: 0x04008F9B RID: 36763
		private const float PerlinFrequency = 0.1f;

		// Token: 0x04008F9C RID: 36764
		private readonly List<PlanetTile> tmpTiles = new List<PlanetTile>();

		// Token: 0x04008F9D RID: 36765
		private readonly Dictionary<PlanetTile, float> tmpTileNoise = new Dictionary<PlanetTile, float>();
	}
}
