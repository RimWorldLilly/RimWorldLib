using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018D RID: 397
	public static class MaterialDefUtility
	{
		// Token: 0x06000B46 RID: 2886 RVA: 0x0003D25A File Offset: 0x0003B45A
		public static bool TryGetByResourcePath(string resourcePath, out MaterialDef def)
		{
			MaterialDefUtility.EnsureCache();
			return MaterialDefUtility.materialDefsByResourcePath.TryGetValue(MaterialDefUtility.NormalizeResourcePath(resourcePath), out def);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0003D274 File Offset: 0x0003B474
		public static Texture2D AlphaFromGrayscale(string path, Texture2D source)
		{
			Texture2D texture2D;
			if (MaterialDefUtility.alphaFromGrayscaleTextures.TryGetValue(path, out texture2D) && texture2D != null)
			{
				return texture2D;
			}
			if (!source.isReadable)
			{
				Log.ErrorOnce("Texture '" + path + "' is flagged alphaFromGrayscale but is not readable.", path.GetHashCode() ^ 5904323);
				return source;
			}
			Color32[] pixels = source.GetPixels32();
			for (int i = 0; i < pixels.Length; i++)
			{
				Color32 color = pixels[i];
				pixels[i].a = (byte)(color.r * 77 + color.g * 150 + color.b * 29 >> 8);
			}
			Texture2D texture2D2 = new Texture2D(source.width, source.height, TextureFormat.RGBA32, source.mipmapCount > 1);
			texture2D2.name = source.name;
			texture2D2.wrapMode = source.wrapMode;
			texture2D2.filterMode = source.filterMode;
			texture2D2.anisoLevel = source.anisoLevel;
			texture2D2.SetPixels32(pixels);
			texture2D2.Apply(true);
			texture2D2.Compress(true);
			MaterialDefUtility.alphaFromGrayscaleTextures[path] = texture2D2;
			return texture2D2;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0003D384 File Offset: 0x0003B584
		public static string NormalizeResourcePath(string resourcePath)
		{
			string text = (resourcePath ?? string.Empty).Replace('\\', '/').Trim();
			if (text.StartsWith("Materials/", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring("Materials/".Length);
			}
			if (text.EndsWith(".mat", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring(0, text.Length - ".mat".Length);
			}
			return text;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0003D3F4 File Offset: 0x0003B5F4
		private static void EnsureCache()
		{
			if (MaterialDefUtility.materialDefsByResourcePath != null && MaterialDefUtility.materialDefCount == DefDatabase<MaterialDef>.DefCount)
			{
				return;
			}
			MaterialDefUtility.materialDefsByResourcePath = new Dictionary<string, MaterialDef>();
			MaterialDefUtility.materialDefCount = DefDatabase<MaterialDef>.DefCount;
			foreach (MaterialDef materialDef in DefDatabase<MaterialDef>.AllDefs)
			{
				string text = MaterialDefUtility.NormalizeResourcePath(materialDef.resourcePath);
				if (text.NullOrEmpty())
				{
					text = MaterialDefUtility.NormalizeResourcePath(materialDef.defName);
				}
				MaterialDefUtility.materialDefsByResourcePath[text] = materialDef;
			}
		}

		// Token: 0x04000999 RID: 2457
		private static Dictionary<string, MaterialDef> materialDefsByResourcePath;

		// Token: 0x0400099A RID: 2458
		private static int materialDefCount = -1;

		// Token: 0x0400099B RID: 2459
		private static Dictionary<string, Texture2D> alphaFromGrayscaleTextures = new Dictionary<string, Texture2D>();
	}
}
