using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004EF RID: 1263
	public class CachedTexture
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x000E3C9C File Offset: 0x000E1E9C
		public Texture2D Texture
		{
			get
			{
				if (this.cachedTexture == null || this.validationIndex != CachedTexture.curValidationIndex)
				{
					if (this.texPath.NullOrEmpty())
					{
						this.cachedTexture = BaseContent.BadTex;
					}
					else
					{
						this.cachedTexture = (ContentFinder<Texture2D>.Get(this.texPath, true) ?? BaseContent.BadTex);
					}
					this.validationIndex = CachedTexture.curValidationIndex;
				}
				return this.cachedTexture;
			}
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x000E3D0A File Offset: 0x000E1F0A
		public CachedTexture(string texPath)
		{
			this.texPath = texPath;
			this.cachedTexture = null;
			this.validationIndex = -1;
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000E3D27 File Offset: 0x000E1F27
		public static void ResetStaticData()
		{
			CachedTexture.curValidationIndex++;
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000E3D38 File Offset: 0x000E1F38
		public static Texture2D Get(string path)
		{
			CachedTexture cachedTexture;
			if (!CachedTexture.cachedTextures.TryGetValue(path, out cachedTexture))
			{
				cachedTexture = (CachedTexture.cachedTextures[path] = new CachedTexture(path));
			}
			return cachedTexture.Texture;
		}

		// Token: 0x04001B0E RID: 6926
		private string texPath;

		// Token: 0x04001B0F RID: 6927
		private Texture2D cachedTexture;

		// Token: 0x04001B10 RID: 6928
		private int validationIndex;

		// Token: 0x04001B11 RID: 6929
		private static int curValidationIndex = 0;

		// Token: 0x04001B12 RID: 6930
		private static readonly Dictionary<string, CachedTexture> cachedTextures = new Dictionary<string, CachedTexture>(32);
	}
}
