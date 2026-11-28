using System;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001328 RID: 4904
	public class TileMutatorWorker_Hollow : TileMutatorWorker
	{
		// Token: 0x0600799B RID: 31131 RVA: 0x002B4535 File Offset: 0x002B2735
		public TileMutatorWorker_Hollow(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600799C RID: 31132 RVA: 0x002B4540 File Offset: 0x002B2740
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float? num2 = (num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake);
			float num3 = num2 ?? ((float)Rand.Range(0, 360));
			this.hollowNoise = MapNoiseUtility.CreateFalloffRadius((float)map.Size.x * TileMutatorWorker_Hollow.HollowRadius, Vector2.zero, 1f, false);
			this.hollowNoise = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * TileMutatorWorker_Hollow.HollowOffset), this.hollowNoise);
			this.hollowNoise = new Rotate(0.0, (double)(num3 + 90f), 0.0, this.hollowNoise);
			this.hollowNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.hollowNoise);
			ModuleBase moduleBase = new DistFromCone(TileMutatorWorker_Hollow.ConeGradient, (float)map.Size.x * TileMutatorWorker_Hollow.ConeSpan);
			moduleBase = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * TileMutatorWorker_Hollow.ConeOffset), moduleBase);
			moduleBase = new Rotate(0.0, (double)(num3 + 90f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			this.hollowNoise = new SmoothMin(this.hollowNoise, moduleBase, (double)TileMutatorWorker_Hollow.Smoothing);
			moduleBase = new DistFromCone(TileMutatorWorker_Hollow.InnerConeGradient, (float)map.Size.x * TileMutatorWorker_Hollow.ConeSpan);
			moduleBase = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * TileMutatorWorker_Hollow.InnerConeOffset), moduleBase);
			moduleBase = new Rotate(0.0, (double)(num3 + 90f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			this.hollowNoise = new SmoothMin(this.hollowNoise, moduleBase, (double)TileMutatorWorker_Hollow.Smoothing);
			if (num2 != null)
			{
				ModuleBase rhs = MapNoiseUtility.FalloffAtAngle(num2.Value, TileMutatorWorker_Hollow.CoastFalloff, map);
				this.hollowNoise = new SmoothMin(this.hollowNoise, rhs, 0.1);
			}
			NoiseDebugUI.StoreNoiseRender(this.hollowNoise, "Hollow base shape");
			this.hollowNoise = MapNoiseUtility.AddDisplacementNoise(this.hollowNoise, TileMutatorWorker_Hollow.HollowNoiseFreq, TileMutatorWorker_Hollow.HollowNoiseStrength, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.hollowNoise, "Hollow noise", map.Size.ToIntVec2);
		}

		// Token: 0x0600799D RID: 31133 RVA: 0x002B4854 File Offset: 0x002B2A54
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.hollowNoise.GetValue(intVec);
				if (value > TileMutatorWorker_Hollow.HollowThreshold)
				{
					elevation[intVec] = value;
				}
			}
		}

		// Token: 0x04004C95 RID: 19605
		private ModuleBase hollowNoise;

		// Token: 0x04004C96 RID: 19606
		[TweakValue("Hollow", 0f, 1f)]
		private static float HollowRadius = 0.25f;

		// Token: 0x04004C97 RID: 19607
		[TweakValue("Hollow", 0f, 0.1f)]
		private static float HollowNoiseFreq = 0.015f;

		// Token: 0x04004C98 RID: 19608
		[TweakValue("Hollow", 0f, 50f)]
		private static float HollowNoiseStrength = 40f;

		// Token: 0x04004C99 RID: 19609
		[TweakValue("Hollow", -1f, 1f)]
		private static float HollowOffset = -0.175f;

		// Token: 0x04004C9A RID: 19610
		[TweakValue("Hollow", -1f, 1f)]
		private static float ConeOffset = 0.025f;

		// Token: 0x04004C9B RID: 19611
		[TweakValue("Hollow", -1f, 1f)]
		private static float InnerConeOffset = -0.375f;

		// Token: 0x04004C9C RID: 19612
		[TweakValue("Hollow", 0f, 1f)]
		private static float HollowThreshold = 0.5f;

		// Token: 0x04004C9D RID: 19613
		[TweakValue("Hollow", 0f, 5f)]
		private static float ConeGradient = 0.5f;

		// Token: 0x04004C9E RID: 19614
		[TweakValue("Hollow", 0f, 5f)]
		private static float InnerConeGradient = 3f;

		// Token: 0x04004C9F RID: 19615
		[TweakValue("Hollow", 0f, 1f)]
		private static float ConeSpan = 0.25f;

		// Token: 0x04004CA0 RID: 19616
		[TweakValue("Hollow", 0f, 1f)]
		private static float Smoothing = 0.5f;

		// Token: 0x04004CA1 RID: 19617
		[TweakValue("Hollow", 0f, 1f)]
		private static float CoastFalloff = 0.1f;
	}
}
