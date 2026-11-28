using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001319 RID: 4889
	public class TileMutatorWorker_Basin : TileMutatorWorker_Lake
	{
		// Token: 0x170014AE RID: 5294
		// (get) Token: 0x06007946 RID: 31046 RVA: 0x002B17C0 File Offset: 0x002AF9C0
		protected override float LakeRadius
		{
			get
			{
				return 0.3f;
			}
		}

		// Token: 0x06007947 RID: 31047 RVA: 0x002B17C7 File Offset: 0x002AF9C7
		protected override IntVec3 GetLakeCenter(Map map)
		{
			return map.Center;
		}

		// Token: 0x06007948 RID: 31048 RVA: 0x002B17CF File Offset: 0x002AF9CF
		public TileMutatorWorker_Basin(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007949 RID: 31049 RVA: 0x002B17D8 File Offset: 0x002AF9D8
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			Rot4 rot = Rot4.Random;
			if (map.TileInfo.IsCoastal)
			{
				rot = Find.World.CoastDirectionAt(map.Tile).Opposite;
			}
			float angle = rot.AsAngle + (float)Rand.Range(-30, 30);
			this.basinNoise = MapNoiseUtility.CreateFalloffRadius((float)map.Size.x * 0.35f, map.Center.ToVector2(), 1f, false);
			List<float> list = new List<float>();
			list.Add(angle);
			ModuleBase moduleBase = new DistFromCone(5f, (float)map.Size.x * 0.05f);
			moduleBase = new Rotate(0.0, (double)(360f - angle), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			this.basinNoise = new SmoothMin(this.basinNoise, moduleBase, 0.2);
			int randomInRange = TileMutatorWorker_Basin.AdditionalEntranceCountRange.RandomInRange;
			Predicate<float> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				int num = 100;
				for (;;)
				{
					List<float> list2 = list;
					Predicate<float> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((float ua) => Mathf.Abs(Mathf.DeltaAngle(ua, angle)) < 70f));
					}
					if (!list2.Any(predicate) || num-- <= 0)
					{
						break;
					}
					angle = Rand.Range(0f, 360f);
				}
				list.Add(angle);
				moduleBase = new DistFromCone(5f, (float)map.Size.x * 0.02f);
				moduleBase = new Rotate(0.0, (double)angle, 0.0, moduleBase);
				moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
				this.basinNoise = new SmoothMin(this.basinNoise, moduleBase, 0.2);
			}
			NoiseDebugUI.StoreNoiseRender(this.basinNoise, "Basin base shape");
			float? num2 = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num3 = (num2 != null) ? num2 : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			if (num3 != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num3.Value, 0.1f, map);
				this.basinNoise = new SmoothMin(this.basinNoise, rhs, 0.1);
			}
			this.basinNoise = MapNoiseUtility.AddDisplacementNoise(this.basinNoise, 0.015f, 35f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.basinNoise, "Basin");
		}

		// Token: 0x0600794A RID: 31050 RVA: 0x002B1AC0 File Offset: 0x002AFCC0
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.basinNoise.GetValue(intVec);
				if (value > 0.5f)
				{
					elevation[intVec] = value;
				}
			}
			base.GeneratePostElevationFertility(map);
		}

		// Token: 0x04004C16 RID: 19478
		private ModuleBase basinNoise;

		// Token: 0x04004C17 RID: 19479
		private const float BasinRadius = 0.35f;

		// Token: 0x04004C18 RID: 19480
		private const float EntranceGradient = 5f;

		// Token: 0x04004C19 RID: 19481
		private const float EntranceSpan = 0.05f;

		// Token: 0x04004C1A RID: 19482
		private const float AdditionalEntranceSpan = 0.02f;

		// Token: 0x04004C1B RID: 19483
		private const float BasinNoiseFreq = 0.015f;

		// Token: 0x04004C1C RID: 19484
		private const float BasinNoiseStrength = 35f;

		// Token: 0x04004C1D RID: 19485
		private const float BasinThreshold = 0.5f;

		// Token: 0x04004C1E RID: 19486
		private const float MinAngleBetweenEntrances = 70f;

		// Token: 0x04004C1F RID: 19487
		private const float CoastFalloff = 0.1f;

		// Token: 0x04004C20 RID: 19488
		private static readonly IntRange AdditionalEntranceCountRange = new IntRange(1, 2);
	}
}
