using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200070C RID: 1804
	public static class SolidColorMaterials
	{
		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06003900 RID: 14592 RVA: 0x0016E27E File Offset: 0x0016C47E
		public static int SimpleColorMatCount
		{
			get
			{
				return SolidColorMaterials.simpleColorMats.Count + SolidColorMaterials.simpleColorAndVertexColorMats.Count;
			}
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x0016E298 File Offset: 0x0016C498
		public static Material SimpleSolidColorMaterial(Color col, bool careAboutVertexColors = false)
		{
			col = col;
			Material material;
			if (careAboutVertexColors)
			{
				if (!SolidColorMaterials.simpleColorAndVertexColorMats.TryGetValue(col, out material))
				{
					material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.VertexColor);
					SolidColorMaterials.simpleColorAndVertexColorMats.Add(col, material);
				}
			}
			else if (!SolidColorMaterials.simpleColorMats.TryGetValue(col, out material))
			{
				material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.SolidColor);
				SolidColorMaterials.simpleColorMats.Add(col, material);
			}
			return material;
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x0016E308 File Offset: 0x0016C508
		public static Material NewSolidColorMaterial(Color col, Shader shader)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a material from a different thread.");
				return null;
			}
			Material material = MaterialAllocator.Create(shader);
			material.color = col;
			material.name = "SolidColorMat-" + shader.name + "-" + col.ToString();
			return material;
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x0016E35D File Offset: 0x0016C55D
		public static Texture2D NewSolidColorTexture(float r, float g, float b, float a)
		{
			return SolidColorMaterials.NewSolidColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x0016E370 File Offset: 0x0016C570
		public static Texture2D NewSolidColorTexture(Color color)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a texture from a different thread.");
				return null;
			}
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.name = "SolidColorTex-" + color.ToString();
			texture2D.SetPixel(0, 0, color);
			texture2D.SetPixel(1, 0, color);
			texture2D.SetPixel(0, 1, color);
			texture2D.SetPixel(1, 1, color);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x0400259A RID: 9626
		private static Dictionary<Color, Material> simpleColorMats = new Dictionary<Color, Material>();

		// Token: 0x0400259B RID: 9627
		private static Dictionary<Color, Material> simpleColorAndVertexColorMats = new Dictionary<Color, Material>();
	}
}
