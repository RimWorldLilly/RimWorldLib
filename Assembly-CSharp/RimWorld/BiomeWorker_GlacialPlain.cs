using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020015EE RID: 5614
	public class BiomeWorker_GlacialPlain : BiomeWorker
	{
		// Token: 0x06008B97 RID: 35735 RVA: 0x00329CF8 File Offset: 0x00327EF8
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
			if (tile.rainfall <= 600f)
			{
				return 0f;
			}
			float num = 0f;
			if (tile.hilliness == Hilliness.SmallHills)
			{
				num = 10f;
			}
			else if (tile.hilliness == Hilliness.LargeHills)
			{
				num = 15f;
			}
			else if (tile.hilliness == Hilliness.Mountainous)
			{
				num = 18f;
			}
			else if (tile.hilliness == Hilliness.Impassable)
			{
				num = 18f;
			}
			return 15f + -tile.temperature + num;
		}

		// Token: 0x06008B98 RID: 35736 RVA: 0x00329D8C File Offset: 0x00327F8C
		private bool AllowedAt(PlanetTile tileID)
		{
			return this.NoiseAt(tileID) >= 0.9f;
		}

		// Token: 0x06008B99 RID: 35737 RVA: 0x00329DA0 File Offset: 0x00327FA0
		private float NoiseAt(PlanetTile tileID)
		{
			if (this.perlin == null)
			{
				this.perlin = new Perlin(0.30000001192092896, 0.4000000059604645, 0.5, 5, true, false, Gen.HashCombineInt(Find.World.info.Seed, 44319114), QualityMode.Medium);
			}
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tileID);
			return (float)this.perlin.GetValue((double)tileCenter.x, (double)tileCenter.y, (double)tileCenter.z);
		}

		// Token: 0x040055A8 RID: 21928
		private Perlin perlin;

		// Token: 0x040055A9 RID: 21929
		private const float NoiseThreshold = 0.9f;

		// Token: 0x040055AA RID: 21930
		private const int SeedPart = 44319114;

		// Token: 0x040055AB RID: 21931
		private const float HillinessSmallHillsElevation = 10f;

		// Token: 0x040055AC RID: 21932
		private const float HillinessLargeHillsElevation = 15f;

		// Token: 0x040055AD RID: 21933
		private const float HillinessMountainousElevation = 18f;

		// Token: 0x040055AE RID: 21934
		private const float HillinessImpassableElevation = 18f;
	}
}
