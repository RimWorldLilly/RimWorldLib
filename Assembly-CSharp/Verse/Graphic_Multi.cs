using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000575 RID: 1397
	public class Graphic_Multi : Graphic
	{
		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x0600295E RID: 10590 RVA: 0x000FDE7A File Offset: 0x000FC07A
		public string GraphicPath
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x0600295F RID: 10591 RVA: 0x000FDE82 File Offset: 0x000FC082
		public override Material MatSingle
		{
			get
			{
				return this.MatSouth;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002960 RID: 10592 RVA: 0x000FDE8A File Offset: 0x000FC08A
		public override Material MatWest
		{
			get
			{
				return this.mats[3];
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002961 RID: 10593 RVA: 0x000FDE94 File Offset: 0x000FC094
		public override Material MatSouth
		{
			get
			{
				return this.mats[2];
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002962 RID: 10594 RVA: 0x000FDE9E File Offset: 0x000FC09E
		public override Material MatEast
		{
			get
			{
				return this.mats[1];
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002963 RID: 10595 RVA: 0x000FDEA8 File Offset: 0x000FC0A8
		public override Material MatNorth
		{
			get
			{
				return this.mats[0];
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002964 RID: 10596 RVA: 0x000FDEB2 File Offset: 0x000FC0B2
		public override bool WestFlipped
		{
			get
			{
				return this.westFlipped;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002965 RID: 10597 RVA: 0x000FDEBA File Offset: 0x000FC0BA
		public override bool EastFlipped
		{
			get
			{
				return this.eastFlipped;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002966 RID: 10598 RVA: 0x000FDEC2 File Offset: 0x000FC0C2
		public override bool ShouldDrawRotated
		{
			get
			{
				return (this.data == null || this.data.drawRotated) && (this.MatEast == this.MatNorth || this.MatWest == this.MatNorth);
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002967 RID: 10599 RVA: 0x000FDF01 File Offset: 0x000FC101
		public override float DrawRotatedExtraAngleOffset
		{
			get
			{
				return this.drawRotatedExtraAngleOffset;
			}
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x000FDF0C File Offset: 0x000FC10C
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			foreach (Material material in this.mats)
			{
				Texture2D mask = null;
				if (material.HasProperty(ShaderPropertyIDs.MaskTex))
				{
					mask = (Texture2D)material.GetTexture(ShaderPropertyIDs.MaskTex);
				}
				GlobalTextureAtlasManager.TryInsertStatic(groupKey, (Texture2D)material.mainTexture, mask);
			}
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000FDF68 File Offset: 0x000FC168
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			Texture2D[] array = new Texture2D[this.mats.Length];
			array[0] = ContentFinder<Texture2D>.Get(req.path + "_north", false);
			array[1] = ContentFinder<Texture2D>.Get(req.path + "_east", false);
			array[2] = ContentFinder<Texture2D>.Get(req.path + "_south", false);
			array[3] = ContentFinder<Texture2D>.Get(req.path + "_west", false);
			if (array[0] == null)
			{
				if (array[2] != null)
				{
					array[0] = array[2];
					this.drawRotatedExtraAngleOffset = 180f;
				}
				else if (array[1] != null)
				{
					array[0] = array[1];
					this.drawRotatedExtraAngleOffset = -90f;
				}
				else if (array[3] != null)
				{
					array[0] = array[3];
					this.drawRotatedExtraAngleOffset = 90f;
				}
				else
				{
					array[0] = ContentFinder<Texture2D>.Get(req.path, false);
				}
			}
			if (array[0] == null)
			{
				Log.Error("Failed to find any textures at " + req.path + " while constructing " + this.ToStringSafe<Graphic_Multi>());
				this.mats[0] = (this.mats[1] = (this.mats[2] = (this.mats[3] = BaseContent.BadMat)));
				return;
			}
			if (array[2] == null)
			{
				array[2] = array[0];
			}
			if (array[1] == null)
			{
				if (array[3] != null)
				{
					array[1] = array[3];
					this.eastFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[1] = array[0];
				}
			}
			if (array[3] == null)
			{
				if (array[1] != null)
				{
					array[3] = array[1];
					this.westFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[3] = array[0];
				}
			}
			Texture2D[] array2 = new Texture2D[this.mats.Length];
			if (req.shader.SupportsMaskTex())
			{
				string str = this.maskPath.NullOrEmpty() ? this.path : this.maskPath;
				string str2 = this.maskPath.NullOrEmpty() ? "m" : string.Empty;
				array2[0] = ContentFinder<Texture2D>.Get(str + "_north" + str2, false);
				array2[1] = ContentFinder<Texture2D>.Get(str + "_east" + str2, false);
				array2[2] = ContentFinder<Texture2D>.Get(str + "_south" + str2, false);
				array2[3] = ContentFinder<Texture2D>.Get(str + "_west" + str2, false);
				if (array2[0] == null)
				{
					if (array2[2] != null)
					{
						array2[0] = array2[2];
					}
					else if (array2[1] != null)
					{
						array2[0] = array2[1];
					}
					else if (array2[3] != null)
					{
						array2[0] = array2[3];
					}
				}
				if (array2[2] == null)
				{
					array2[2] = array2[0];
				}
				if (array2[1] == null)
				{
					if (array2[3] != null)
					{
						array2[1] = array2[3];
					}
					else
					{
						array2[1] = array2[0];
					}
				}
				if (array2[3] == null)
				{
					if (array2[1] != null)
					{
						array2[3] = array2[1];
					}
					else
					{
						array2[3] = array2[0];
					}
				}
			}
			for (int i = 0; i < this.mats.Length; i++)
			{
				MaterialRequest req2 = default(MaterialRequest);
				req2.mainTex = array[i];
				req2.shader = req.shader;
				req2.color = this.color;
				req2.colorTwo = this.colorTwo;
				req2.maskTex = array2[i];
				req2.shaderParameters = req.shaderParameters;
				req2.renderQueue = req.renderQueue;
				this.mats[i] = MaterialPool.MatFrom(req2);
			}
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000FE33F File Offset: 0x000FC53F
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Multi>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, this.maskPath);
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x000FE364 File Offset: 0x000FC564
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Multi(initPath=",
				this.path,
				", color=",
				this.color.ToString(),
				", colorTwo=",
				this.colorTwo.ToString(),
				")"
			});
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000FE3CD File Offset: 0x000FC5CD
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<string>(0, this.path), this.color), this.colorTwo);
		}

		// Token: 0x04001CD2 RID: 7378
		private Material[] mats = new Material[4];

		// Token: 0x04001CD3 RID: 7379
		private bool westFlipped;

		// Token: 0x04001CD4 RID: 7380
		private bool eastFlipped;

		// Token: 0x04001CD5 RID: 7381
		private float drawRotatedExtraAngleOffset;

		// Token: 0x04001CD6 RID: 7382
		public const string NorthSuffix = "_north";

		// Token: 0x04001CD7 RID: 7383
		public const string SouthSuffix = "_south";

		// Token: 0x04001CD8 RID: 7384
		public const string EastSuffix = "_east";

		// Token: 0x04001CD9 RID: 7385
		public const string WestSuffix = "_west";
	}
}
