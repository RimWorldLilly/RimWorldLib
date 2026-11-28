using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C4 RID: 452
	public class SubcameraDef : Def
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x00041378 File Offset: 0x0003F578
		public int LayerId
		{
			get
			{
				if (this.layerCached == -1)
				{
					this.layerCached = LayerMask.NameToLayer(this.layer);
				}
				return this.layerCached;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x0004139C File Offset: 0x0003F59C
		public RenderTextureFormat BestFormat
		{
			get
			{
				if (SystemInfo.SupportsRenderTextureFormat(this.format))
				{
					return this.format;
				}
				if (this.format == RenderTextureFormat.R8 && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RG16))
				{
					return RenderTextureFormat.RG16;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RG16) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
				{
					return RenderTextureFormat.ARGB32;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat))
				{
					return RenderTextureFormat.RGFloat;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat || this.format == RenderTextureFormat.RGFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
				{
					return RenderTextureFormat.ARGBFloat;
				}
				return this.format;
			}
		}

		// Token: 0x04000BC0 RID: 3008
		[NoTranslate]
		public string layer;

		// Token: 0x04000BC1 RID: 3009
		public int depth;

		// Token: 0x04000BC2 RID: 3010
		public RenderTextureFormat format;

		// Token: 0x04000BC3 RID: 3011
		public bool startEnabled = true;

		// Token: 0x04000BC4 RID: 3012
		public bool doNotUpdate;

		// Token: 0x04000BC5 RID: 3013
		public Color backgroundColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04000BC6 RID: 3014
		[Unsaved(false)]
		private int layerCached = -1;
	}
}
