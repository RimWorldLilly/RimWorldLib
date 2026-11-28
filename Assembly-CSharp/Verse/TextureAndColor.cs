using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004FB RID: 1275
	public struct TextureAndColor
	{
		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060025D6 RID: 9686 RVA: 0x000E6B3E File Offset: 0x000E4D3E
		public bool HasValue
		{
			get
			{
				return this.texture != null;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060025D7 RID: 9687 RVA: 0x000E6B4C File Offset: 0x000E4D4C
		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060025D8 RID: 9688 RVA: 0x000E6B54 File Offset: 0x000E4D54
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060025D9 RID: 9689 RVA: 0x000E6B5C File Offset: 0x000E4D5C
		public static TextureAndColor None
		{
			get
			{
				return new TextureAndColor(null, Color.white);
			}
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000E6B69 File Offset: 0x000E4D69
		public TextureAndColor(Texture2D texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x000E6B79 File Offset: 0x000E4D79
		public static implicit operator TextureAndColor(Texture2D texture)
		{
			return new TextureAndColor(texture, Color.white);
		}

		// Token: 0x04001B4B RID: 6987
		private Texture2D texture;

		// Token: 0x04001B4C RID: 6988
		private Color color;
	}
}
