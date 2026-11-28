using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B3C RID: 6972
	public static class PlantFallColors
	{
		// Token: 0x0600B31D RID: 45853 RVA: 0x003F1A08 File Offset: 0x003EFC08
		public static float GetFallColorFactor(float latitude, int dayOfYear)
		{
			float a = GenCelestial.AverageGlow(latitude, dayOfYear);
			float b = GenCelestial.AverageGlow(latitude, dayOfYear + 1);
			float x = Mathf.LerpUnclamped(a, b, PlantFallColors.FallSlopeComponent);
			return GenMath.LerpDoubleClamped(PlantFallColors.FallColorBegin, PlantFallColors.FallColorEnd, 0f, 1f, x);
		}

		// Token: 0x0600B31E RID: 45854 RVA: 0x003F1A4C File Offset: 0x003EFC4C
		public static void SetFallShaderGlobals(Map map)
		{
			if (PlantFallColors.FallIntensityOverride)
			{
				Shader.SetGlobalFloat(ShaderPropertyIDs.FallIntensity, PlantFallColors.FallIntensity);
			}
			else
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);
				Shader.SetGlobalFloat(ShaderPropertyIDs.FallIntensity, PlantFallColors.GetFallColorFactor(vector.y, GenLocalDate.DayOfYear(map)));
			}
			Shader.SetGlobalInt(PlantFallColors.Controls, (PlantFallColors.FallGlobalControls > false) ? 1 : 0);
			if (PlantFallColors.FallGlobalControls)
			{
				Shader.SetGlobalVector(PlantFallColors.FallSrc, new Vector3(PlantFallColors.FallSrcR, PlantFallColors.FallSrcG, PlantFallColors.FallSrcB));
				Shader.SetGlobalVector(PlantFallColors.FallDst, new Vector3(PlantFallColors.FallDstR, PlantFallColors.FallDstG, PlantFallColors.FallDstB));
				Shader.SetGlobalVector(PlantFallColors.FallRange, new Vector3(PlantFallColors.FallRangeBegin, PlantFallColors.FallRangeEnd));
			}
		}

		// Token: 0x04006754 RID: 26452
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorBegin = 0.55f;

		// Token: 0x04006755 RID: 26453
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorEnd = 0.45f;

		// Token: 0x04006756 RID: 26454
		[TweakValue("Graphics", 0f, 30f)]
		private static float FallSlopeComponent = 15f;

		// Token: 0x04006757 RID: 26455
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallIntensityOverride = false;

		// Token: 0x04006758 RID: 26456
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallIntensity = 0f;

		// Token: 0x04006759 RID: 26457
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallGlobalControls = false;

		// Token: 0x0400675A RID: 26458
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcR = 0.3803f;

		// Token: 0x0400675B RID: 26459
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcG = 0.4352f;

		// Token: 0x0400675C RID: 26460
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcB = 0.1451f;

		// Token: 0x0400675D RID: 26461
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstR = 0.4392f;

		// Token: 0x0400675E RID: 26462
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstG = 0.3254f;

		// Token: 0x0400675F RID: 26463
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstB = 0.1765f;

		// Token: 0x04006760 RID: 26464
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeBegin = 0.02f;

		// Token: 0x04006761 RID: 26465
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeEnd = 0.1f;

		// Token: 0x04006762 RID: 26466
		private static readonly int FallSrc = Shader.PropertyToID("_FallSrc");

		// Token: 0x04006763 RID: 26467
		private static readonly int FallDst = Shader.PropertyToID("_FallDst");

		// Token: 0x04006764 RID: 26468
		private static readonly int FallRange = Shader.PropertyToID("_FallRange");

		// Token: 0x04006765 RID: 26469
		private static readonly int Controls = Shader.PropertyToID("_FallGlobalControls");
	}
}
