using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02002353 RID: 9043
	public class WorldGenStep_Terrain : WorldGenStep_Tiles
	{
		// Token: 0x17002752 RID: 10066
		// (get) Token: 0x0600DFE4 RID: 57316 RVA: 0x005347FB File Offset: 0x005329FB
		public override int SeedPart
		{
			get
			{
				return 83469557;
			}
		}

		// Token: 0x17002753 RID: 10067
		// (get) Token: 0x0600DFE5 RID: 57317 RVA: 0x00534802 File Offset: 0x00532A02
		private static float FreqMultiplier
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0600DFE6 RID: 57318 RVA: 0x00534809 File Offset: 0x00532A09
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			this.SetupElevationNoise(layer);
			this.SetupTemperatureOffsetNoise();
			this.SetupRainfallNoise();
			this.SetupHillinessNoise();
			this.SetupSwampinessNoise();
			base.GenerateFresh(seed, layer);
		}

		// Token: 0x0600DFE7 RID: 57319 RVA: 0x00534834 File Offset: 0x00532A34
		private void SetupElevationNoise(PlanetLayer layer)
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase lhs = new Perlin((double)(0.035f * freqMultiplier), 2.0, 0.4000000059604645, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase = new RidgedMultifractal((double)(0.012f * freqMultiplier), 2.0, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase2 = new Perlin((double)(0.12f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase3 = new Perlin((double)(0.01f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.High);
			float num;
			if (Find.World.PlanetCoverage < 0.55f)
			{
				ModuleBase moduleBase4 = new DistanceFromPlanetViewCenter(layer.ViewCenter, Find.WorldGrid.SurfaceViewAngle, true);
				moduleBase4 = new ScaleBias(2.0, -1.0, moduleBase4);
				moduleBase3 = new Blend(moduleBase3, moduleBase4, new Const(0.4000000059604645));
				num = Rand.Range(-0.4f, -0.35f);
			}
			else
			{
				num = Rand.Range(0.15f, 0.25f);
			}
			NoiseDebugUI.StorePlanetNoise(moduleBase3, "elevContinents");
			moduleBase2 = new ScaleBias(0.5, 0.5, moduleBase2);
			moduleBase = new Multiply(moduleBase, moduleBase2);
			float num2 = Rand.Range(0.4f, 0.6f);
			this.noiseElevation = new Blend(lhs, moduleBase, new Const((double)num2));
			this.noiseElevation = new Blend(this.noiseElevation, moduleBase3, new Const((double)num));
			if (Find.World.PlanetCoverage < 0.9999f)
			{
				this.noiseElevation = new ConvertToIsland(Find.WorldGrid.SurfaceViewCenter, Find.WorldGrid.SurfaceViewAngle, this.noiseElevation);
			}
			this.noiseElevation = new ScaleBias(0.5, 0.5, this.noiseElevation);
			this.noiseElevation = new Power(this.noiseElevation, new Const(3.0));
			NoiseDebugUI.StorePlanetNoise(this.noiseElevation, "noiseElevation");
			this.noiseElevation = new ScaleBias((double)WorldGenStep_Terrain.ElevationRange.Span, (double)WorldGenStep_Terrain.ElevationRange.min, this.noiseElevation);
		}

		// Token: 0x0600DFE8 RID: 57320 RVA: 0x00534A98 File Offset: 0x00532C98
		private void SetupTemperatureOffsetNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseTemperatureOffset = new Perlin((double)(0.018f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			this.noiseTemperatureOffset = new Multiply(this.noiseTemperatureOffset, new Const(4.0));
		}

		// Token: 0x0600DFE9 RID: 57321 RVA: 0x00534AFC File Offset: 0x00532CFC
		private void SetupRainfallNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase moduleBase = new Perlin((double)(0.015f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			NoiseDebugUI.StorePlanetNoise(moduleBase, "basePerlin");
			ModuleBase moduleBase2 = new AbsLatitudeCurve(new SimpleCurve
			{
				{
					0f,
					1.12f,
					true
				},
				{
					25f,
					0.94f,
					true
				},
				{
					45f,
					0.7f,
					true
				},
				{
					70f,
					0.3f,
					true
				},
				{
					80f,
					0.05f,
					true
				},
				{
					90f,
					0.05f,
					true
				}
			}, 100f);
			NoiseDebugUI.StorePlanetNoise(moduleBase2, "latCurve");
			this.noiseRainfall = new Multiply(moduleBase, moduleBase2);
			float num = 0.00022222222f;
			float num2 = -500f * num;
			ModuleBase moduleBase3 = new ScaleBias((double)num, (double)num2, this.noiseElevation);
			moduleBase3 = new ScaleBias(-1.0, 1.0, moduleBase3);
			moduleBase3 = new Clamp(0.0, 1.0, moduleBase3);
			NoiseDebugUI.StorePlanetNoise(moduleBase3, "elevationRainfallEffect");
			this.noiseRainfall = new Multiply(this.noiseRainfall, moduleBase3);
			Func<double, double> processor = delegate(double val)
			{
				if (val < 0.0)
				{
					val = 0.0;
				}
				if (val < 0.12)
				{
					val = (val + 0.12) / 2.0;
					if (val < 0.03)
					{
						val = (val + 0.03) / 2.0;
					}
				}
				return val;
			};
			this.noiseRainfall = new Arbitrary(this.noiseRainfall, processor);
			this.noiseRainfall = new Power(this.noiseRainfall, new Const(1.5));
			this.noiseRainfall = new Clamp(0.0, 999.0, this.noiseRainfall);
			NoiseDebugUI.StorePlanetNoise(this.noiseRainfall, "noiseRainfall before mm");
			this.noiseRainfall = new ScaleBias(4000.0, 0.0, this.noiseRainfall);
			SimpleCurve rainfallCurve = Find.World.info.overallRainfall.GetRainfallCurve();
			if (rainfallCurve != null)
			{
				this.noiseRainfall = new CurveSimple(this.noiseRainfall, rainfallCurve);
			}
		}

		// Token: 0x0600DFEA RID: 57322 RVA: 0x00534D3C File Offset: 0x00532F3C
		private void SetupHillinessNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			this.noiseMountainLines = new Perlin((double)(0.025f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase = new Perlin((double)(0.06f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			this.noiseMountainLines = new Abs(this.noiseMountainLines);
			this.noiseMountainLines = new OneMinus(this.noiseMountainLines);
			moduleBase = new Filter(moduleBase, -0.3f, 1f);
			this.noiseMountainLines = new Multiply(this.noiseMountainLines, moduleBase);
			this.noiseMountainLines = new OneMinus(this.noiseMountainLines);
			NoiseDebugUI.StorePlanetNoise(this.noiseMountainLines, "noiseMountainLines");
			this.noiseHillsPatchesMacro = new Perlin((double)(0.032f * freqMultiplier), 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			this.noiseHillsPatchesMicro = new Perlin((double)(0.19f * freqMultiplier), 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
		}

		// Token: 0x0600DFEB RID: 57323 RVA: 0x00534E78 File Offset: 0x00533078
		private void SetupSwampinessNoise()
		{
			float freqMultiplier = WorldGenStep_Terrain.FreqMultiplier;
			ModuleBase moduleBase = new Perlin((double)(0.09f * freqMultiplier), 2.0, 0.4000000059604645, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			ModuleBase moduleBase2 = new RidgedMultifractal((double)(0.025f * freqMultiplier), 2.0, 6, Rand.Range(0, int.MaxValue), QualityMode.High);
			moduleBase = new ScaleBias(0.5, 0.5, moduleBase);
			moduleBase2 = new ScaleBias(0.5, 0.5, moduleBase2);
			this.noiseSwampiness = new Multiply(moduleBase, moduleBase2);
			InverseLerp rhs = new InverseLerp(this.noiseElevation, WorldGenStep_Terrain.SwampinessMaxElevation.max, WorldGenStep_Terrain.SwampinessMaxElevation.min);
			this.noiseSwampiness = new Multiply(this.noiseSwampiness, rhs);
			InverseLerp rhs2 = new InverseLerp(this.noiseRainfall, WorldGenStep_Terrain.SwampinessMinRainfall.min, WorldGenStep_Terrain.SwampinessMinRainfall.max);
			this.noiseSwampiness = new Multiply(this.noiseSwampiness, rhs2);
			NoiseDebugUI.StorePlanetNoise(this.noiseSwampiness, "noiseSwampiness");
		}

		// Token: 0x0600DFEC RID: 57324 RVA: 0x00534F94 File Offset: 0x00533194
		protected override Tile GenerateTileFor(PlanetTile tile, PlanetLayer layer)
		{
			SurfaceTile surfaceTile = new SurfaceTile(tile);
			Vector3 tileCenter = layer.GetTileCenter(tile);
			surfaceTile.elevation = this.noiseElevation.GetValue(tileCenter);
			float value = this.noiseMountainLines.GetValue(tileCenter);
			if (value > 0.235f || surfaceTile.elevation <= 0f)
			{
				if (surfaceTile.elevation > 0f && this.noiseHillsPatchesMicro.GetValue(tileCenter) > 0.46f && this.noiseHillsPatchesMacro.GetValue(tileCenter) > -0.3f)
				{
					if (Rand.Bool)
					{
						surfaceTile.hilliness = Hilliness.SmallHills;
					}
					else
					{
						surfaceTile.hilliness = Hilliness.LargeHills;
					}
				}
				else
				{
					surfaceTile.hilliness = Hilliness.Flat;
				}
			}
			else if (value > 0.12f)
			{
				switch (Rand.Range(0, 4))
				{
				case 0:
					surfaceTile.hilliness = Hilliness.Flat;
					break;
				case 1:
					surfaceTile.hilliness = Hilliness.SmallHills;
					break;
				case 2:
					surfaceTile.hilliness = Hilliness.LargeHills;
					break;
				case 3:
					surfaceTile.hilliness = Hilliness.Mountainous;
					break;
				}
			}
			else if (value > 0.0363f)
			{
				surfaceTile.hilliness = Hilliness.Mountainous;
			}
			else
			{
				surfaceTile.hilliness = Hilliness.Impassable;
			}
			float num = WorldGenStep_Terrain.BaseTemperatureAtLatitude(layer.LongLatOf(tile).y);
			num -= WorldGenStep_Terrain.TemperatureReductionAtElevation(surfaceTile.elevation);
			num += this.noiseTemperatureOffset.GetValue(tileCenter);
			SimpleCurve temperatureCurve = Find.World.info.overallTemperature.GetTemperatureCurve();
			if (temperatureCurve != null)
			{
				num = temperatureCurve.Evaluate(num);
			}
			surfaceTile.temperature = num;
			surfaceTile.rainfall = this.noiseRainfall.GetValue(tileCenter);
			if (float.IsNaN(surfaceTile.rainfall))
			{
				float value2 = this.noiseRainfall.GetValue(tileCenter);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
				defaultInterpolatedStringHandler.AppendFormatted<float>(value2);
				defaultInterpolatedStringHandler.AppendLiteral(" rain bad at ");
				defaultInterpolatedStringHandler.AppendFormatted<PlanetTile>(tile);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 694822);
			}
			if (surfaceTile.hilliness == Hilliness.Flat || surfaceTile.hilliness == Hilliness.SmallHills)
			{
				surfaceTile.swampiness = this.noiseSwampiness.GetValue(tileCenter);
			}
			surfaceTile.PrimaryBiome = this.BiomeFrom(surfaceTile, tile, layer);
			return surfaceTile;
		}

		// Token: 0x0600DFED RID: 57325 RVA: 0x00535194 File Offset: 0x00533394
		private BiomeDef BiomeFrom(Tile ws, PlanetTile tile, PlanetLayer layer)
		{
			List<BiomeDef> allDefsListForReading = DefDatabase<BiomeDef>.AllDefsListForReading;
			BiomeDef biomeDef = null;
			float num = 0f;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				BiomeDef biomeDef2 = allDefsListForReading[i];
				if (biomeDef2.implemented && biomeDef2.generatesNaturally && biomeDef2.Worker.CanPlaceOnLayer(biomeDef2, layer))
				{
					float score = biomeDef2.Worker.GetScore(biomeDef2, ws, tile);
					if (score > num || biomeDef == null)
					{
						biomeDef = biomeDef2;
						num = score;
					}
				}
			}
			return biomeDef;
		}

		// Token: 0x0600DFEE RID: 57326 RVA: 0x00535210 File Offset: 0x00533410
		private static float BaseTemperatureAtLatitude(float lat)
		{
			float x = Mathf.Abs(lat) / 90f;
			return WorldGenStep_Terrain.AvgTempByLatitudeCurve.Evaluate(x);
		}

		// Token: 0x0600DFEF RID: 57327 RVA: 0x00535238 File Offset: 0x00533438
		private static float TemperatureReductionAtElevation(float elev)
		{
			if (elev < 250f)
			{
				return 0f;
			}
			float t = (elev - 250f) / 4750f;
			return Mathf.Lerp(0f, 40f, t);
		}

		// Token: 0x04008FAB RID: 36779
		private ModuleBase noiseElevation;

		// Token: 0x04008FAC RID: 36780
		private ModuleBase noiseTemperatureOffset;

		// Token: 0x04008FAD RID: 36781
		private ModuleBase noiseRainfall;

		// Token: 0x04008FAE RID: 36782
		private ModuleBase noiseSwampiness;

		// Token: 0x04008FAF RID: 36783
		private ModuleBase noiseMountainLines;

		// Token: 0x04008FB0 RID: 36784
		private ModuleBase noiseHillsPatchesMicro;

		// Token: 0x04008FB1 RID: 36785
		private ModuleBase noiseHillsPatchesMacro;

		// Token: 0x04008FB2 RID: 36786
		private const float ElevationFrequencyMicro = 0.035f;

		// Token: 0x04008FB3 RID: 36787
		private const float ElevationFrequencyMacro = 0.012f;

		// Token: 0x04008FB4 RID: 36788
		private const float ElevationMacroFactorFrequency = 0.12f;

		// Token: 0x04008FB5 RID: 36789
		private const float ElevationContinentsFrequency = 0.01f;

		// Token: 0x04008FB6 RID: 36790
		private const float MountainLinesFrequency = 0.025f;

		// Token: 0x04008FB7 RID: 36791
		private const float MountainLinesHolesFrequency = 0.06f;

		// Token: 0x04008FB8 RID: 36792
		private const float HillsPatchesFrequencyMicro = 0.19f;

		// Token: 0x04008FB9 RID: 36793
		private const float HillsPatchesFrequencyMacro = 0.032f;

		// Token: 0x04008FBA RID: 36794
		private const float SwampinessFrequencyMacro = 0.025f;

		// Token: 0x04008FBB RID: 36795
		private const float SwampinessFrequencyMicro = 0.09f;

		// Token: 0x04008FBC RID: 36796
		private static readonly FloatRange SwampinessMaxElevation = new FloatRange(650f, 750f);

		// Token: 0x04008FBD RID: 36797
		private static readonly FloatRange SwampinessMinRainfall = new FloatRange(725f, 900f);

		// Token: 0x04008FBE RID: 36798
		private static readonly FloatRange ElevationRange = new FloatRange(-500f, 5000f);

		// Token: 0x04008FBF RID: 36799
		private const float TemperatureOffsetFrequency = 0.018f;

		// Token: 0x04008FC0 RID: 36800
		private const float TemperatureOffsetFactor = 4f;

		// Token: 0x04008FC1 RID: 36801
		private static readonly SimpleCurve AvgTempByLatitudeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 30f),
				true
			},
			{
				new CurvePoint(0.1f, 29f),
				true
			},
			{
				new CurvePoint(0.5f, 7f),
				true
			},
			{
				new CurvePoint(1f, -37f),
				true
			}
		};

		// Token: 0x04008FC2 RID: 36802
		private const float ElevationTempReductionStartAlt = 250f;

		// Token: 0x04008FC3 RID: 36803
		private const float ElevationTempReductionEndAlt = 5000f;

		// Token: 0x04008FC4 RID: 36804
		private const float MaxElevationTempReduction = 40f;

		// Token: 0x04008FC5 RID: 36805
		private const float RainfallPower = 1.5f;

		// Token: 0x04008FC6 RID: 36806
		private const float RainfallFactor = 4000f;

		// Token: 0x04008FC7 RID: 36807
		private const float RainfallStartFallAltitude = 500f;

		// Token: 0x04008FC8 RID: 36808
		private const float RainfallFinishFallAltitude = 5000f;
	}
}
