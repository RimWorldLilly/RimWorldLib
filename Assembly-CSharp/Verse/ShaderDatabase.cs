using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A9 RID: 169
	[StaticConstructorOnStartup]
	public static class ShaderDatabase
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00029507 File Offset: 0x00027707
		public static Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00029510 File Offset: 0x00027710
		public static Shader LoadShader(string shaderPath)
		{
			if (ShaderDatabase.lookup == null)
			{
				ShaderDatabase.lookup = new Dictionary<string, Shader>();
			}
			if (ShaderDatabase.uiLookup == null)
			{
				ShaderDatabase.uiLookup = new Dictionary<Shader, Shader>();
			}
			Shader result;
			ShaderDatabase.TryLoadShader(shaderPath, out result);
			return result;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0002954C File Offset: 0x0002774C
		public static Shader LoadShader(ShaderTypeDef shaderDef)
		{
			Shader result;
			Shader shader;
			ShaderDatabase.LoadShader(shaderDef, out result, out shader);
			return result;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00029564 File Offset: 0x00027764
		public static void LoadShader(ShaderTypeDef shaderDef, out Shader result, out Shader uiShader)
		{
			if (shaderDef == null)
			{
				Log.Warning("Tried to LoadShader with a null ShaderTypeDef. Returning default shader instead.");
				result = ShaderDatabase.DefaultShader;
				uiShader = null;
				return;
			}
			ShaderDatabase.LoadShader(shaderDef.shaderPath, shaderDef.uiShaderPath, out result, out uiShader);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00029591 File Offset: 0x00027791
		public static void LoadShader(string shaderPath, string uiShaderPath, out Shader result, out Shader uiShader)
		{
			uiShader = null;
			if (!string.IsNullOrEmpty(uiShaderPath))
			{
				ShaderDatabase.TryLoadShader(uiShaderPath, out uiShader);
			}
			if (ShaderDatabase.TryLoadShader(shaderPath, out result) && uiShader != null)
			{
				ShaderDatabase.uiLookup[result] = uiShader;
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000295C7 File Offset: 0x000277C7
		public static bool TryGetUIShader(Shader shader, out Shader uiShader)
		{
			return ShaderDatabase.uiLookup.TryGetValue(shader, out uiShader);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000295D8 File Offset: 0x000277D8
		private static bool TryLoadShader(string shaderPath, out Shader result)
		{
			if (!ShaderDatabase.lookup.TryGetValue(shaderPath, out result))
			{
				string text = ShaderDatabase.ShaderDefNameFor(shaderPath);
				if (DefDatabase<ShaderDef>.GetNamedSilentFail(text) == null)
				{
					Shader shader = ContentFinder<Shader>.TryFindAssetInModBundles(shaderPath);
					if (shader != null)
					{
						result = shader;
						goto IL_34;
					}
				}
				result = Shader.CreateLateBound(text);
				IL_34:
				ShaderDatabase.lookup[shaderPath] = result;
			}
			return result != null;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00029630 File Offset: 0x00027830
		private static string ShaderDefNameFor(string shaderPath)
		{
			if (string.IsNullOrEmpty(shaderPath))
			{
				return shaderPath;
			}
			int num = shaderPath.LastIndexOf('/');
			if (num < 0)
			{
				return shaderPath;
			}
			return shaderPath.Substring(num + 1);
		}

		// Token: 0x04000275 RID: 629
		public static readonly Shader Cutout = ShaderDatabase.LoadShader("Map/Cutout");

		// Token: 0x04000276 RID: 630
		public static readonly Shader CutoutHair = ShaderDatabase.LoadShader("Map/CutoutHair");

		// Token: 0x04000277 RID: 631
		public static readonly Shader CutoutPlant = ShaderDatabase.LoadShader("Map/CutoutPlant");

		// Token: 0x04000278 RID: 632
		public static readonly Shader CutoutComplex = ShaderDatabase.LoadShader("Map/CutoutComplex");

		// Token: 0x04000279 RID: 633
		public static readonly Shader CutoutComplexUI = ShaderDatabase.LoadShader("Map/CutoutComplexUI");

		// Token: 0x0400027A RID: 634
		public static readonly Shader ExpandingIconUI = ShaderDatabase.LoadShader("Map/ExpandingIconUI");

		// Token: 0x0400027B RID: 635
		public static readonly Shader CutoutComplexBlend = ShaderDatabase.LoadShader("Map/CutoutComplexBlend");

		// Token: 0x0400027C RID: 636
		public static readonly Shader CutoutSkinOverlay = ShaderDatabase.LoadShader("Map/CutoutSkinOverlay");

		// Token: 0x0400027D RID: 637
		public static readonly Shader CutoutSkin = ShaderDatabase.LoadShader("Map/CutoutSkin");

		// Token: 0x0400027E RID: 638
		public static readonly Shader Wound = ShaderDatabase.LoadShader("Map/Wound");

		// Token: 0x0400027F RID: 639
		public static readonly Shader WoundSkin = ShaderDatabase.LoadShader("Map/WoundSkin");

		// Token: 0x04000280 RID: 640
		public static readonly Shader CutoutSkinColorOverride = ShaderDatabase.LoadShader("Map/CutoutSkinOverride");

		// Token: 0x04000281 RID: 641
		public static readonly Shader CutoutFlying = ShaderDatabase.LoadShader("Map/CutoutFlying");

		// Token: 0x04000282 RID: 642
		public static readonly Shader CutoutFlying01 = ShaderDatabase.LoadShader("Map/CutoutFlying01");

		// Token: 0x04000283 RID: 643
		public static readonly Shader FirefoamOverlay = ShaderDatabase.LoadShader("Map/FirefoamOverlay");

		// Token: 0x04000284 RID: 644
		public static readonly Shader CutoutWithOverlay = ShaderDatabase.LoadShader("Map/CutoutWithOverlay");

		// Token: 0x04000285 RID: 645
		public static readonly Shader CutoutOverlay = ShaderDatabase.LoadShader("Map/CutoutOverlay");

		// Token: 0x04000286 RID: 646
		public static readonly Shader TerrainEdge = ShaderDatabase.LoadShader("Map/TerrainEdge");

		// Token: 0x04000287 RID: 647
		public static readonly Shader Transparent = ShaderDatabase.LoadShader("Map/Transparent");

		// Token: 0x04000288 RID: 648
		public static readonly Shader TransparentPostLight = ShaderDatabase.LoadShader("Map/TransparentPostLight");

		// Token: 0x04000289 RID: 649
		public static readonly Shader TransparentPlant = ShaderDatabase.LoadShader("Map/TransparentPlant");

		// Token: 0x0400028A RID: 650
		public static readonly Shader Mote = ShaderDatabase.LoadShader("Map/Mote");

		// Token: 0x0400028B RID: 651
		public static readonly Shader MotePulse = ShaderDatabase.LoadShader("Map/MotePulse");

		// Token: 0x0400028C RID: 652
		public static readonly Shader MoteGlow = ShaderDatabase.LoadShader("Map/MoteGlow");

		// Token: 0x0400028D RID: 653
		public static readonly Shader MoteGlowPulse = ShaderDatabase.LoadShader("Map/MoteGlowPulse");

		// Token: 0x0400028E RID: 654
		public static readonly Shader MoteWater = ShaderDatabase.LoadShader("Map/MoteWater");

		// Token: 0x0400028F RID: 655
		public static readonly Shader MoteGlowDistorted = ShaderDatabase.LoadShader("Map/MoteGlowDistorted");

		// Token: 0x04000290 RID: 656
		public static readonly Shader MoteGlowDistortBG = ShaderDatabase.LoadShader("Map/MoteGlowDistortBackground");

		// Token: 0x04000291 RID: 657
		public static readonly Shader MoteProximityScannerRadius = ShaderDatabase.LoadShader("Map/MoteProximityScannerRadius");

		// Token: 0x04000292 RID: 658
		public static readonly Shader GasRotating = ShaderDatabase.LoadShader("Map/GasRotating");

		// Token: 0x04000293 RID: 659
		public static readonly Shader TerrainHard = ShaderDatabase.LoadShader("Map/TerrainHard");

		// Token: 0x04000294 RID: 660
		public static readonly Shader TerrainFade = ShaderDatabase.LoadShader("Map/TerrainFade");

		// Token: 0x04000295 RID: 661
		public static readonly Shader TerrainFadeRough = ShaderDatabase.LoadShader("Map/TerrainFadeRough");

		// Token: 0x04000296 RID: 662
		public static readonly Shader TerrainWater = ShaderDatabase.LoadShader("Map/TerrainWater");

		// Token: 0x04000297 RID: 663
		public static readonly Shader TerrainHardPolluted = ShaderDatabase.LoadShader("Map/TerrainHardLinearBurn");

		// Token: 0x04000298 RID: 664
		public static readonly Shader TerrainFadePolluted = ShaderDatabase.LoadShader("Map/TerrainFadeLinearBurn");

		// Token: 0x04000299 RID: 665
		public static readonly Shader TerrainFadeRoughPolluted = ShaderDatabase.LoadShader("Map/TerrainFadeRoughLinearBurn");

		// Token: 0x0400029A RID: 666
		public static readonly Shader PollutionCloud = ShaderDatabase.LoadShader("Map/PollutionCloud");

		// Token: 0x0400029B RID: 667
		public static readonly Shader MapEdgeTerrain = ShaderDatabase.LoadShader("Map/MapEdgeTerrain");

		// Token: 0x0400029C RID: 668
		public static readonly Shader WorldTerrain = ShaderDatabase.LoadShader("World/WorldTerrain");

		// Token: 0x0400029D RID: 669
		public static readonly Shader WorldOcean = ShaderDatabase.LoadShader("World/WorldOcean");

		// Token: 0x0400029E RID: 670
		public static readonly Shader WorldOverlayCutout = ShaderDatabase.LoadShader("World/WorldOverlayCutout");

		// Token: 0x0400029F RID: 671
		public static readonly Shader WorldOverlayTransparent = ShaderDatabase.LoadShader("World/WorldOverlayTransparent");

		// Token: 0x040002A0 RID: 672
		public static readonly Shader WorldOverlayTransparentLit = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLit");

		// Token: 0x040002A1 RID: 673
		public static readonly Shader WorldOverlayTransparentLitPollution = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLitPollution");

		// Token: 0x040002A2 RID: 674
		public static readonly Shader WorldOverlayAdditive = ShaderDatabase.LoadShader("World/WorldOverlayAdditive");

		// Token: 0x040002A3 RID: 675
		public static readonly Shader WorldOverlayAdditiveTwoSided = ShaderDatabase.LoadShader("World/WorldOverlayAdditiveTwoSided");

		// Token: 0x040002A4 RID: 676
		public static readonly Shader PlanetGlow = ShaderDatabase.LoadShader("World/PlanetGlow");

		// Token: 0x040002A5 RID: 677
		public static readonly Shader Clouds = ShaderDatabase.LoadShader("World/Clouds");

		// Token: 0x040002A6 RID: 678
		public static readonly Shader MetaOverlay = ShaderDatabase.LoadShader("Map/MetaOverlay");

		// Token: 0x040002A7 RID: 679
		public static readonly Shader MetaOverlayDesaturated = ShaderDatabase.LoadShader("Map/MetaOverlayDesaturated");

		// Token: 0x040002A8 RID: 680
		public static readonly Shader SolidColor = ShaderDatabase.LoadShader("Map/SolidColor");

		// Token: 0x040002A9 RID: 681
		public static readonly Shader SolidColorBehind = ShaderDatabase.LoadShader("Map/SolidColorBehind");

		// Token: 0x040002AA RID: 682
		public static readonly Shader VertexColor = ShaderDatabase.LoadShader("Map/VertexColor");

		// Token: 0x040002AB RID: 683
		public static readonly Shader RitualStencil = ShaderDatabase.LoadShader("Map/RitualStencil");

		// Token: 0x040002AC RID: 684
		public static readonly Shader Invisible = ShaderDatabase.LoadShader("Misc/Invisible");

		// Token: 0x040002AD RID: 685
		public static readonly Shader Silhouette = ShaderDatabase.LoadShader("Misc/Silhouette");

		// Token: 0x040002AE RID: 686
		public static readonly Shader Metalblood = ShaderDatabase.LoadShader("Misc/Metalblood");

		// Token: 0x040002AF RID: 687
		public static readonly Shader CaveExitRope = ShaderDatabase.LoadShader("Map/CaveExitRope");

		// Token: 0x040002B0 RID: 688
		public static readonly Shader BioferriteHarvester = ShaderDatabase.LoadShader("Map/BioferriteHarvester");

		// Token: 0x040002B1 RID: 689
		public static readonly Shader GrayscaleGUI = ShaderDatabase.LoadShader("Misc/GrayscaleGUI");

		// Token: 0x040002B2 RID: 690
		public static readonly Shader GravshipMaskMasked = ShaderDatabase.LoadShader("Map/GravshipMaskMasked");

		// Token: 0x040002B3 RID: 691
		public static readonly Shader IndoorMaskMasked = ShaderDatabase.LoadShader("Map/IndoorMaskMasked");

		// Token: 0x040002B4 RID: 692
		private static Dictionary<string, Shader> lookup;

		// Token: 0x040002B5 RID: 693
		private static Dictionary<Shader, Shader> uiLookup;
	}
}
