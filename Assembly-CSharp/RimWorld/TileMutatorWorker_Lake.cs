using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001330 RID: 4912
	public class TileMutatorWorker_Lake : TileMutatorWorker
	{
		// Token: 0x170014BE RID: 5310
		// (get) Token: 0x060079B9 RID: 31161 RVA: 0x002B5399 File Offset: 0x002B3599
		protected virtual float LakeRadius
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x170014BF RID: 5311
		// (get) Token: 0x060079BA RID: 31162 RVA: 0x002B53A0 File Offset: 0x002B35A0
		protected virtual float LakeFalloffDecay
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170014C0 RID: 5312
		// (get) Token: 0x060079BB RID: 31163 RVA: 0x002B53A7 File Offset: 0x002B35A7
		protected virtual bool GenerateDeepWater
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060079BC RID: 31164 RVA: 0x002B53AA File Offset: 0x002B35AA
		public TileMutatorWorker_Lake(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079BD RID: 31165 RVA: 0x002B53B4 File Offset: 0x002B35B4
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			map.waterInfo.lakeCenter = this.GetLakeCenter(map);
			this.lakeNoise = new DistFromPoint((float)map.Size.x * this.LakeRadius);
			this.lakeNoise = new ScaleBias(-1.0, 1.0, this.lakeNoise);
			this.lakeNoise = new Scale((double)TileMutatorWorker_Lake.LakeSquashRange.RandomInRange, 1.0, 1.0, this.lakeNoise);
			this.lakeNoise = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, this.lakeNoise);
			this.lakeNoise = new Translate((double)(-(double)map.waterInfo.lakeCenter.x), 0.0, (double)(-(double)map.waterInfo.lakeCenter.z), this.lakeNoise);
			this.lakeNoise = new Clamp(0.0, 1.0, this.lakeNoise);
			this.lakeNoise = new Power(this.lakeNoise, new Const((double)this.LakeFalloffDecay));
			this.lakeNoise = MapNoiseUtility.AddDisplacementNoise(this.lakeNoise, 0.006f, 40f, 2, map.Tile.tileId);
			this.lakeNoise = MapNoiseUtility.AddDisplacementNoise(this.lakeNoise, 0.015f, 15f, 4, map.Tile.tileId);
			NoiseDebugUI.StoreNoiseRender(this.lakeNoise, "lake");
		}

		// Token: 0x060079BE RID: 31166 RVA: 0x002B555C File Offset: 0x002B375C
		protected virtual IntVec3 GetLakeCenter(Map map)
		{
			Landmark landmark;
			if (Find.World.landmarks.landmarks.TryGetValue(map.Tile, out landmark) && landmark.isComboLandmark)
			{
				return map.Center;
			}
			Rand.PushState(map.Tile.tileId);
			IntVec3 result = new IntVec3(map.Center.x + Mathf.RoundToInt((float)map.Size.x * Rand.Range(-0.4f, 0.4f)), 0, map.Center.z + Mathf.RoundToInt((float)map.Size.z * Rand.Range(-0.4f, 0.4f)));
			Rand.PopState();
			return result;
		}

		// Token: 0x060079BF RID: 31167 RVA: 0x002B560C File Offset: 0x002B380C
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.GetValAt(intVec, map) > 0.45f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x060079C0 RID: 31168 RVA: 0x002B5674 File Offset: 0x002B3874
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 cell in map.AllCells)
			{
				this.ProcessCell(cell, map);
			}
		}

		// Token: 0x060079C1 RID: 31169 RVA: 0x002B56C4 File Offset: 0x002B38C4
		protected virtual void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell, map);
			if (this.GenerateDeepWater && valAt > 0.75f)
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.DeepFreshWaterTerrainAt(cell, map));
				return;
			}
			if (valAt > 0.5f)
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.ShallowFreshWaterTerrainAt(cell, map));
				return;
			}
			if (valAt > 0.45f && MapGenUtility.ShouldGenerateBeachSand(cell, map))
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.LakeshoreTerrainAt(cell, map));
			}
		}

		// Token: 0x060079C2 RID: 31170 RVA: 0x002B573E File Offset: 0x002B393E
		protected virtual float GetValAt(IntVec3 cell, Map map)
		{
			return (float)this.lakeNoise.GetValue((double)cell.x, 0.0, (double)cell.z);
		}

		// Token: 0x04004CC4 RID: 19652
		protected ModuleBase lakeNoise;

		// Token: 0x04004CC5 RID: 19653
		public const float LakeNoiseFrequency = 0.015f;

		// Token: 0x04004CC6 RID: 19654
		public const float LakeNoiseStrength = 15f;

		// Token: 0x04004CC7 RID: 19655
		public const float LakeMacroNoiseFrequency = 0.006f;

		// Token: 0x04004CC8 RID: 19656
		public const float LakeMacroNoiseStrength = 40f;

		// Token: 0x04004CC9 RID: 19657
		public const float DeepWaterThreshold = 0.75f;

		// Token: 0x04004CCA RID: 19658
		public const float WaterThreshold = 0.5f;

		// Token: 0x04004CCB RID: 19659
		public const float BeachThreshold = 0.45f;

		// Token: 0x04004CCC RID: 19660
		private const float LakeMaxOffset = 0.4f;

		// Token: 0x04004CCD RID: 19661
		public static readonly FloatRange LakeSquashRange = new FloatRange(1f, 1.3f);
	}
}
