using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200131E RID: 4894
	public class TileMutatorWorker_Chasm : TileMutatorWorker
	{
		// Token: 0x06007966 RID: 31078 RVA: 0x002B27B9 File Offset: 0x002B09B9
		public TileMutatorWorker_Chasm(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007967 RID: 31079 RVA: 0x002B27C4 File Offset: 0x002B09C4
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.chasmNoise = new DistFromPoint((float)map.Size.x * 0.4f);
			this.chasmNoise = new Scale((double)TileMutatorWorker_Chasm.OpeningSquashRange.RandomInRange, 1.0, 1.0, this.chasmNoise);
			this.chasmNoise = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, this.chasmNoise);
			this.chasmNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.chasmNoise);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num2 = (num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			if (num2 != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num2.Value, 0.1f, map);
				this.chasmNoise = new SmoothMin(this.chasmNoise, rhs, 0.2);
			}
			NoiseDebugUI.StoreNoiseRender(this.chasmNoise, "opening shape");
			this.chasmNoise = MapNoiseUtility.AddDisplacementNoise(this.chasmNoise, 0.015f, 40f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.chasmNoise, "opening");
			List<float> list = new List<float>();
			ModuleBase moduleBase = new Const(1.0);
			int randomInRange = TileMutatorWorker_Chasm.EntranceCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				float entranceAngle = Rand.Range(0f, 360f);
				int num3 = 100;
				while (list.Any((float ua) => Mathf.Abs(Mathf.DeltaAngle(ua, entranceAngle)) < 40f) && num3-- > 0)
				{
					entranceAngle = Rand.Range(0f, 360f);
				}
				list.Add(entranceAngle);
				ModuleBase moduleBase2 = new DistFromAxis((float)map.Size.x * 0.06f);
				moduleBase2 = new ScaleBias(-1.0, 1.0, moduleBase2);
				moduleBase2 = new Multiply(moduleBase2, new CutOff(false, true));
				moduleBase2 = new ScaleBias(-1.0, 1.0, moduleBase2);
				moduleBase2 = new Rotate(0.0, (double)entranceAngle, 0.0, moduleBase2);
				moduleBase2 = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase2);
				moduleBase = new SmoothMin(moduleBase, moduleBase2, 0.2);
			}
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.007f, 50f, 6, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "entrances");
			this.chasmNoise = new SmoothMin(this.chasmNoise, moduleBase, 0.2);
			NoiseDebugUI.StoreNoiseRender(this.chasmNoise, "chasm");
		}

		// Token: 0x06007968 RID: 31080 RVA: 0x002B2AE8 File Offset: 0x002B0CE8
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.chasmNoise.GetValue(intVec);
				if (value > 0.5f)
				{
					elevation[intVec] = value;
				}
			}
		}

		// Token: 0x04004C43 RID: 19523
		private const float OpeningNoiseFreq = 0.015f;

		// Token: 0x04004C44 RID: 19524
		private const float OpeningNoiseStrength = 40f;

		// Token: 0x04004C45 RID: 19525
		private const float OpeningRadius = 0.4f;

		// Token: 0x04004C46 RID: 19526
		private const float ChasmThreshold = 0.5f;

		// Token: 0x04004C47 RID: 19527
		private static readonly FloatRange OpeningSquashRange = new FloatRange(1f, 1.3f);

		// Token: 0x04004C48 RID: 19528
		private static readonly IntRange EntranceCountRange = new IntRange(3, 6);

		// Token: 0x04004C49 RID: 19529
		private const float EntranceSpan = 0.06f;

		// Token: 0x04004C4A RID: 19530
		private const float EntranceNoiseFreq = 0.007f;

		// Token: 0x04004C4B RID: 19531
		private const float EntranceNoiseStrength = 50f;

		// Token: 0x04004C4C RID: 19532
		private const int EntranceNoiseOctaves = 6;

		// Token: 0x04004C4D RID: 19533
		private const float MinAngleBetweenEntrances = 40f;

		// Token: 0x04004C4E RID: 19534
		private const float CoastFalloff = 0.1f;

		// Token: 0x04004C4F RID: 19535
		private ModuleBase chasmNoise;
	}
}
