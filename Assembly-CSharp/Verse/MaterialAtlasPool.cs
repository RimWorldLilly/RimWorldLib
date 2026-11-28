using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004FE RID: 1278
	public static class MaterialAtlasPool
	{
		// Token: 0x060025EA RID: 9706 RVA: 0x000E7150 File Offset: 0x000E5350
		public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
		{
			if (!MaterialAtlasPool.atlasDict.ContainsKey(mat))
			{
				MaterialAtlasPool.atlasDict.Add(mat, new MaterialAtlasPool.MaterialAtlas(mat));
			}
			return MaterialAtlasPool.atlasDict[mat].SubMat(LinkSet);
		}

		// Token: 0x04001B53 RID: 6995
		private static Dictionary<Material, MaterialAtlasPool.MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlasPool.MaterialAtlas>();

		// Token: 0x0200274F RID: 10063
		private class MaterialAtlas
		{
			// Token: 0x0600F7B8 RID: 63416 RVA: 0x0059ED9C File Offset: 0x0059CF9C
			public MaterialAtlas(Material newRootMat)
			{
				Vector2 mainTextureScale = new Vector2(0.1875f, 0.1875f);
				for (int i = 0; i < 16; i++)
				{
					float x = (float)(i % 4) * 0.25f + 0.03125f;
					float y = (float)(i / 4) * 0.25f + 0.03125f;
					Vector2 mainTextureOffset = new Vector2(x, y);
					Material material = MaterialAllocator.Create(newRootMat);
					material.name = newRootMat.name + "_ASM" + i.ToString();
					material.mainTextureScale = mainTextureScale;
					material.mainTextureOffset = mainTextureOffset;
					this.subMats[i] = material;
				}
			}

			// Token: 0x0600F7B9 RID: 63417 RVA: 0x0059EE48 File Offset: 0x0059D048
			public Material SubMat(LinkDirections linkSet)
			{
				int num = (int)linkSet;
				if (num >= this.subMats.Length)
				{
					Log.Warning("Cannot get submat of index " + num.ToString() + ": out of range.");
					return BaseContent.BadMat;
				}
				return this.subMats[num];
			}

			// Token: 0x0400A0E7 RID: 41191
			protected Material[] subMats = new Material[16];

			// Token: 0x0400A0E8 RID: 41192
			private const float TexPadding = 0.03125f;
		}
	}
}
