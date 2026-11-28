using System;
using UnityEngine;
using Waylith.Graphics.Textures.Mips;

namespace LudeonTK
{
	// Token: 0x0200002F RID: 47
	public static class GenTexture
	{
		// Token: 0x0600014D RID: 333 RVA: 0x00009F62 File Offset: 0x00008162
		public static void GenerateMipmaps(Texture2D baseTexture, int minMipDimension, bool preserveAlphaCoverage = false, float alphaCoverageCutoff = 0.5f)
		{
			if (((baseTexture != null) ? baseTexture.GpuTexture : null) == null)
			{
				return;
			}
			Mipmapper.GenerateMipmaps(baseTexture.GpuTexture, baseTexture.width, baseTexture.height, baseTexture.mipmapCount, minMipDimension, preserveAlphaCoverage, alphaCoverageCutoff, false);
		}
	}
}
