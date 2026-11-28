using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000500 RID: 1280
	public static class MaterialPool
	{
		// Token: 0x060025EE RID: 9710 RVA: 0x000E724A File Offset: 0x000E544A
		public static Material MatFrom(string texPath, bool reportFailure)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, reportFailure)));
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000E726F File Offset: 0x000E546F
		public static Material MatFrom(string texPath)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true)));
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000E7294 File Offset: 0x000E5494
		public static Material MatFrom(Texture2D srcTex)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex));
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000E72A1 File Offset: 0x000E54A1
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color));
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000E72B0 File Offset: 0x000E54B0
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000E72D5 File Offset: 0x000E54D5
		public static Material MatFrom(string texPath, Shader shader)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader));
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000E72EC File Offset: 0x000E54EC
		public static Material MatFrom(string texPath, Shader shader, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000E7316 File Offset: 0x000E5516
		public static Material MatFrom(string texPath, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color));
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000E732C File Offset: 0x000E552C
		public static Material MatFrom(string texPath, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000E7357 File Offset: 0x000E5557
		public static Material MatFrom(Shader shader)
		{
			return MaterialPool.MatFrom(new MaterialRequest(shader));
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000E7364 File Offset: 0x000E5564
		public static Material MatFrom(MaterialRequest req)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a material from a different thread.");
				return null;
			}
			if (req.mainTex == null && req.needsMainTex)
			{
				Log.Error("MatFrom with null sourceTex.");
				return BaseContent.BadMat;
			}
			if (req.shader == null)
			{
				Log.Warning("Matfrom with null shader.");
				return BaseContent.BadMat;
			}
			if (req.maskTex != null && !req.shader.SupportsMaskTex())
			{
				Log.Error("MaterialRequest has maskTex but shader does not support it. req=" + req.ToString());
				req.maskTex = null;
			}
			req.color = req.color;
			req.colorTwo = req.colorTwo;
			Material material;
			if (!MaterialPool.matDictionary.TryGetValue(req, out material))
			{
				material = MaterialAllocator.Create(req.shader);
				material.name = req.shader.name;
				if (req.mainTex != null)
				{
					Material material2 = material;
					material2.name = material2.name + "_" + req.mainTex.name;
					material.mainTexture = req.mainTex;
				}
				material.color = req.color;
				if (req.maskTex != null)
				{
					material.SetTexture(ShaderPropertyIDs.MaskTex, req.maskTex);
					material.SetColor(ShaderPropertyIDs.ColorTwo, req.colorTwo);
				}
				else if (req.colorTwo != default(Color))
				{
					material.SetColor(ShaderPropertyIDs.ColorTwo, req.colorTwo);
				}
				if (req.secondaryTex != null)
				{
					material.SetTexture(ShaderPropertyIDs.SecondaryTex, req.secondaryTex);
				}
				if (req.renderQueue != 0)
				{
					material.renderQueue = req.renderQueue;
				}
				if (!req.shaderParameters.NullOrEmpty<ShaderParameter>())
				{
					for (int i = 0; i < req.shaderParameters.Count; i++)
					{
						req.shaderParameters[i].Apply(material);
					}
				}
				MaterialPool.matDictionary.Add(req, material);
				MaterialPool.matDictionaryReverse.Add(material, req);
				if (req.shader == ShaderDatabase.CutoutPlant || req.shader == ShaderDatabase.TransparentPlant)
				{
					WindManager.Notify_PlantMaterialCreated(material);
				}
			}
			return material;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000E75AF File Offset: 0x000E57AF
		public static bool TryGetRequestForMat(Material material, out MaterialRequest request)
		{
			return MaterialPool.matDictionaryReverse.TryGetValue(material, out request);
		}

		// Token: 0x04001B54 RID: 6996
		private static Dictionary<MaterialRequest, Material> matDictionary = new Dictionary<MaterialRequest, Material>();

		// Token: 0x04001B55 RID: 6997
		private static Dictionary<Material, MaterialRequest> matDictionaryReverse = new Dictionary<Material, MaterialRequest>();
	}
}
