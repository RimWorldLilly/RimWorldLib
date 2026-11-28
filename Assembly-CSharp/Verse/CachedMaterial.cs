using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004EE RID: 1262
	public class CachedMaterial
	{
		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x000E3BB4 File Offset: 0x000E1DB4
		public Material Material
		{
			get
			{
				if (this.cachedMaterial == null || this.validationIndex != CachedMaterial.curValidationIndex)
				{
					if (this.texPath.NullOrEmpty())
					{
						this.cachedMaterial = BaseContent.BadMat;
					}
					else
					{
						this.cachedMaterial = MaterialPool.MatFrom(this.texPath, this.shader, this.color);
					}
					this.validationIndex = CachedMaterial.curValidationIndex;
				}
				return this.cachedMaterial;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002575 RID: 9589 RVA: 0x000E3C24 File Offset: 0x000E1E24
		public string TexturePath
		{
			get
			{
				return this.texPath;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002576 RID: 9590 RVA: 0x000E3C2C File Offset: 0x000E1E2C
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000E3C34 File Offset: 0x000E1E34
		public CachedMaterial(string texPath, Shader shader, Color color)
		{
			this.texPath = texPath;
			this.shader = shader;
			this.color = color;
			this.cachedMaterial = null;
			this.validationIndex = -1;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000E3C5F File Offset: 0x000E1E5F
		public CachedMaterial(string texPath, Shader shader)
		{
			this.texPath = texPath;
			this.shader = shader;
			this.color = Color.white;
			this.cachedMaterial = null;
			this.validationIndex = -1;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000E3C8E File Offset: 0x000E1E8E
		public static void ResetStaticData()
		{
			CachedMaterial.curValidationIndex++;
		}

		// Token: 0x04001B08 RID: 6920
		private string texPath;

		// Token: 0x04001B09 RID: 6921
		private Material cachedMaterial;

		// Token: 0x04001B0A RID: 6922
		private Shader shader;

		// Token: 0x04001B0B RID: 6923
		private Color color;

		// Token: 0x04001B0C RID: 6924
		private int validationIndex;

		// Token: 0x04001B0D RID: 6925
		private static int curValidationIndex;
	}
}
