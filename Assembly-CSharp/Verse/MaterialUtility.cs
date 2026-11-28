using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000707 RID: 1799
	public static class MaterialUtility
	{
		// Token: 0x060038F4 RID: 14580 RVA: 0x0016D9CD File Offset: 0x0016BBCD
		public static Texture2D GetMaskTexture(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				return null;
			}
			return (Texture2D)mat.GetTexture(ShaderPropertyIDs.MaskTex);
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x0016D9EE File Offset: 0x0016BBEE
		public static Color GetColorTwo(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.ColorTwo))
			{
				return Color.white;
			}
			return mat.GetColor(ShaderPropertyIDs.ColorTwo);
		}
	}
}
