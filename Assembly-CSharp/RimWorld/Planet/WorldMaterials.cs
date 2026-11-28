using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200232C RID: 9004
	[StaticConstructorOnStartup]
	public static class WorldMaterials
	{
		// Token: 0x17002700 RID: 9984
		// (get) Token: 0x0600DE60 RID: 56928 RVA: 0x0052B04C File Offset: 0x0052924C
		public static Material CurTargetingMat
		{
			get
			{
				WorldMaterials.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return WorldMaterials.TargetSquareMatSingle;
			}
		}

		// Token: 0x0600DE61 RID: 56929 RVA: 0x0052B064 File Offset: 0x00529264
		static WorldMaterials()
		{
			WorldMaterials.GenerateMats(ref WorldMaterials.matsFertility, WorldMaterials.FertilitySpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsTemperature, WorldMaterials.TemperatureSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsElevation, WorldMaterials.ElevationSpectrum, WorldMaterials.NumMatsPerMode);
			WorldMaterials.GenerateMats(ref WorldMaterials.matsRainfall, WorldMaterials.RainfallSpectrum, WorldMaterials.NumMatsPerMode);
		}

		// Token: 0x0600DE62 RID: 56930 RVA: 0x0052B5B0 File Offset: 0x005297B0
		private static void GenerateMats(ref Material[] mats, Color[] colorSpectrum, int numMats)
		{
			mats = new Material[numMats];
			for (int i = 0; i < numMats; i++)
			{
				mats[i] = MatsFromSpectrum.Get(colorSpectrum, (float)i / (float)numMats);
			}
		}

		// Token: 0x0600DE63 RID: 56931 RVA: 0x0052B5E0 File Offset: 0x005297E0
		public static Material MatForFertilityOverlay(float fert)
		{
			int value = Mathf.FloorToInt(fert * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsFertility[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x0600DE64 RID: 56932 RVA: 0x0052B610 File Offset: 0x00529810
		public static Material MatForTemperature(float temp)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(-50f, 50f, temp) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsTemperature[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x0600DE65 RID: 56933 RVA: 0x0052B650 File Offset: 0x00529850
		public static Material MatForElevation(float elev)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(0f, 5000f, elev) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsElevation[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x0600DE66 RID: 56934 RVA: 0x0052B690 File Offset: 0x00529890
		public static Material MatForRainfallOverlay(float rain)
		{
			int value = Mathf.FloorToInt(Mathf.InverseLerp(0f, 5000f, rain) * (float)WorldMaterials.NumMatsPerMode);
			return WorldMaterials.matsRainfall[Mathf.Clamp(value, 0, WorldMaterials.NumMatsPerMode - 1)];
		}

		// Token: 0x04008EB9 RID: 36537
		public static readonly Material WorldTerrain = MatLoader.LoadMat("World/WorldTerrain", 3500);

		// Token: 0x04008EBA RID: 36538
		public static readonly Material WorldIce = MatLoader.LoadMat("World/WorldIce", 3500);

		// Token: 0x04008EBB RID: 36539
		public static readonly Material WorldOcean = MatLoader.LoadMat("World/WorldOcean", 3500);

		// Token: 0x04008EBC RID: 36540
		public static readonly Material UngeneratedPlanetParts = MatLoader.LoadMat("World/UngeneratedPlanetParts", 3500);

		// Token: 0x04008EBD RID: 36541
		public static readonly Material Clouds = MatLoader.LoadMat("World/Clouds", 3700);

		// Token: 0x04008EBE RID: 36542
		public static readonly Material Rivers = MatLoader.LoadMat("World/Rivers", 3530);

		// Token: 0x04008EBF RID: 36543
		public static readonly Material RiversBorder = MatLoader.LoadMat("World/RiversBorder", 3520);

		// Token: 0x04008EC0 RID: 36544
		public static readonly Material Roads = MatLoader.LoadMat("World/Roads", 3540);

		// Token: 0x04008EC1 RID: 36545
		public const int DebugTileRenderQueue = 3510;

		// Token: 0x04008EC2 RID: 36546
		public const int WorldObjectRenderQueue = 3550;

		// Token: 0x04008EC3 RID: 36547
		public const int WorldLineRenderQueue = 3590;

		// Token: 0x04008EC4 RID: 36548
		public const int DynamicObjectRenderQueue = 3600;

		// Token: 0x04008EC5 RID: 36549
		public const int FeatureNameRenderQueue = 3610;

		// Token: 0x04008EC6 RID: 36550
		public static readonly Material MouseTile = MaterialPool.MatFrom("World/MouseTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x04008EC7 RID: 36551
		public static readonly Material SelectedTile = MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x04008EC8 RID: 36552
		public static readonly Material ClosestTile = MaterialPool.MatFrom("World/CurrentMapTile", ShaderDatabase.WorldOverlayAdditive, 3560);

		// Token: 0x04008EC9 RID: 36553
		public static readonly Material CurrentMapTile = MaterialPool.MatFrom("World/CurrentMapTile", ShaderDatabase.WorldOverlayTransparent, 3560);

		// Token: 0x04008ECA RID: 36554
		public static readonly Material Stars = MatLoader.LoadMat("World/Stars", -1);

		// Token: 0x04008ECB RID: 36555
		public static readonly Material Sun = MatLoader.LoadMat("World/Sun", -1);

		// Token: 0x04008ECC RID: 36556
		public static readonly Material SmallHills = MaterialPool.MatFrom("World/Hills/SmallHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04008ECD RID: 36557
		public static readonly Material LargeHills = MaterialPool.MatFrom("World/Hills/LargeHills", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04008ECE RID: 36558
		public static readonly Material Mountains = MaterialPool.MatFrom("World/Hills/Mountains", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04008ECF RID: 36559
		public static readonly Material ImpassableMountains = MaterialPool.MatFrom("World/Hills/Impassable", ShaderDatabase.WorldOverlayTransparentLit, 3510);

		// Token: 0x04008ED0 RID: 36560
		public static readonly Material VertexColor = MatLoader.LoadMat("World/WorldVertexColor", -1);

		// Token: 0x04008ED1 RID: 36561
		public static readonly Material VertexColorTransparent = MatLoader.LoadMat("World/WorldVertexColorTransparent", 3545);

		// Token: 0x04008ED2 RID: 36562
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent, 3560);

		// Token: 0x04008ED3 RID: 36563
		private static int NumMatsPerMode = 50;

		// Token: 0x04008ED4 RID: 36564
		public static Material OverlayModeMatOcean = SolidColorMaterials.NewSolidColorMaterial(new Color(0.09f, 0.18f, 0.2f), ShaderDatabase.Transparent);

		// Token: 0x04008ED5 RID: 36565
		private static Material[] matsFertility;

		// Token: 0x04008ED6 RID: 36566
		private static readonly Color[] FertilitySpectrum = new Color[]
		{
			new Color(0f, 1f, 0f, 0f),
			new Color(0f, 1f, 0f, 0.5f)
		};

		// Token: 0x04008ED7 RID: 36567
		private const float TempRange = 50f;

		// Token: 0x04008ED8 RID: 36568
		private static Material[] matsTemperature;

		// Token: 0x04008ED9 RID: 36569
		private static readonly Color[] TemperatureSpectrum = new Color[]
		{
			new Color(1f, 1f, 1f),
			new Color(0f, 0f, 1f),
			new Color(0.25f, 0.25f, 1f),
			new Color(0.6f, 0.6f, 1f),
			new Color(0.5f, 0.5f, 0.5f),
			new Color(0.5f, 0.3f, 0f),
			new Color(1f, 0.6f, 0.18f),
			new Color(1f, 0f, 0f)
		};

		// Token: 0x04008EDA RID: 36570
		private const float ElevationMax = 5000f;

		// Token: 0x04008EDB RID: 36571
		private static Material[] matsElevation;

		// Token: 0x04008EDC RID: 36572
		private static readonly Color[] ElevationSpectrum = new Color[]
		{
			new Color(0.224f, 0.18f, 0.15f),
			new Color(0.447f, 0.369f, 0.298f),
			new Color(0.6f, 0.6f, 0.6f),
			new Color(1f, 1f, 1f)
		};

		// Token: 0x04008EDD RID: 36573
		private const float RainfallMax = 5000f;

		// Token: 0x04008EDE RID: 36574
		private static Material[] matsRainfall;

		// Token: 0x04008EDF RID: 36575
		private static readonly Color[] RainfallSpectrum = new Color[]
		{
			new Color(0.9f, 0.9f, 0.9f),
			GenColor.FromBytes(190, 190, 190, 255),
			new Color(0.58f, 0.58f, 0.58f),
			GenColor.FromBytes(196, 112, 110, 255),
			GenColor.FromBytes(200, 179, 150, 255),
			GenColor.FromBytes(255, 199, 117, 255),
			GenColor.FromBytes(255, 255, 84, 255),
			GenColor.FromBytes(145, 255, 253, 255),
			GenColor.FromBytes(0, 255, 0, 255),
			GenColor.FromBytes(63, 198, 55, 255),
			GenColor.FromBytes(13, 150, 5, 255),
			GenColor.FromBytes(5, 112, 94, 255)
		};
	}
}
