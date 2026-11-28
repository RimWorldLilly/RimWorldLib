using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200057D RID: 1405
	public class Graphic_Single : Graphic
	{
		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06002997 RID: 10647 RVA: 0x000FEFCC File Offset: 0x000FD1CC
		public override Material MatSingle
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002998 RID: 10648 RVA: 0x000FEFD4 File Offset: 0x000FD1D4
		public override Material MatWest
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002999 RID: 10649 RVA: 0x000FEFDC File Offset: 0x000FD1DC
		public override Material MatSouth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x0600299A RID: 10650 RVA: 0x000FEFE4 File Offset: 0x000FD1E4
		public override Material MatEast
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x0600299B RID: 10651 RVA: 0x000FEFEC File Offset: 0x000FD1EC
		public override Material MatNorth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x0600299C RID: 10652 RVA: 0x000FEFF4 File Offset: 0x000FD1F4
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.data == null || this.data.drawRotated;
			}
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x000FF010 File Offset: 0x000FD210
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			Texture2D mask = null;
			if (this.mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				mask = (Texture2D)this.mat.GetTexture(ShaderPropertyIDs.MaskTex);
			}
			GlobalTextureAtlasManager.TryInsertStatic(groupKey, (Texture2D)this.mat.mainTexture, mask);
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000FF060 File Offset: 0x000FD260
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			MaterialRequest req2 = new MaterialRequest(req.texture ?? ContentFinder<Texture2D>.Get(req.path, true), req.shader, this.color)
			{
				colorTwo = this.colorTwo,
				renderQueue = req.renderQueue,
				shaderParameters = req.shaderParameters
			};
			if (req.shader.SupportsMaskTex())
			{
				req2.maskTex = ContentFinder<Texture2D>.Get(this.maskPath.NullOrEmpty() ? (this.path + Graphic_Single.MaskSuffix) : this.maskPath, false);
			}
			this.mat = MaterialPool.MatFrom(req2);
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x000FF152 File Offset: 0x000FD352
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x000FF16F File Offset: 0x000FD36F
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.mat;
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000FF178 File Offset: 0x000FD378
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Single(path=",
				this.path,
				", color=",
				this.color.ToString(),
				", colorTwo=",
				this.colorTwo.ToString(),
				")"
			});
		}

		// Token: 0x04001CE6 RID: 7398
		protected Material mat;

		// Token: 0x04001CE7 RID: 7399
		public static readonly string MaskSuffix = "_m";
	}
}
