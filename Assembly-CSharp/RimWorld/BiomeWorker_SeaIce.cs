using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020015F2 RID: 5618
	public class BiomeWorker_SeaIce : BiomeWorker
	{
		// Token: 0x06008BA4 RID: 35748 RVA: 0x00329FB2 File Offset: 0x003281B2
		public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
		{
			if (!tile.WaterCovered)
			{
				return -100f;
			}
			if (!this.AllowedAt(planetTile))
			{
				return -100f;
			}
			return BiomeWorker_IceSheet.PermaIceScore(tile) - 23f;
		}

		// Token: 0x06008BA5 RID: 35749 RVA: 0x00329FE0 File Offset: 0x003281E0
		private bool AllowedAt(PlanetTile tile)
		{
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tile);
			Vector3 surfaceViewCenter = Find.WorldGrid.SurfaceViewCenter;
			float value = Vector3.Angle(surfaceViewCenter, tileCenter);
			float surfaceViewAngle = Find.WorldGrid.SurfaceViewAngle;
			float num = Mathf.Min(7.5f, surfaceViewAngle * 0.12f);
			float num2 = Mathf.InverseLerp(surfaceViewAngle - num, surfaceViewAngle, value);
			if (num2 <= 0f)
			{
				return true;
			}
			if (this.cachedSeaIceAllowedNoise == null || this.cachedSeaIceAllowedNoiseForSeed != Find.World.info.Seed)
			{
				this.cachedSeaIceAllowedNoise = new Perlin(0.017000000923871994, 2.0, 0.5, 6, Find.World.info.Seed, QualityMode.Medium);
				this.cachedSeaIceAllowedNoiseForSeed = Find.World.info.Seed;
			}
			float headingFromTo = Find.WorldGrid.GetHeadingFromTo(surfaceViewCenter, tileCenter);
			float num3 = (float)this.cachedSeaIceAllowedNoise.GetValue((double)headingFromTo, 0.0, 0.0) * 0.5f + 0.5f;
			return num2 <= num3;
		}

		// Token: 0x040055AF RID: 21935
		private ModuleBase cachedSeaIceAllowedNoise;

		// Token: 0x040055B0 RID: 21936
		private int cachedSeaIceAllowedNoiseForSeed;
	}
}
