using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200013B RID: 315
	public class DamageGraphicData
	{
		// Token: 0x060009F9 RID: 2553 RVA: 0x0003711B File Offset: 0x0003531B
		public void ResolveReferencesSpecial()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.scratches != null)
				{
					this.scratchMats = new List<Material>();
					for (int i = 0; i < this.scratches.Count; i++)
					{
						Material material = MaterialPool.MatFrom(this.scratches[i], ShaderDatabase.Transparent, 2905);
						this.scratchMats.Add(material);
						GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)material.mainTexture, null);
					}
				}
				if (this.cornerTL != null)
				{
					this.cornerTLMat = MaterialPool.MatFrom(this.cornerTL, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerTLMat.mainTexture, null);
				}
				if (this.cornerTR != null)
				{
					this.cornerTRMat = MaterialPool.MatFrom(this.cornerTR, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerTRMat.mainTexture, null);
				}
				if (this.cornerBL != null)
				{
					this.cornerBLMat = MaterialPool.MatFrom(this.cornerBL, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerBLMat.mainTexture, null);
				}
				if (this.cornerBR != null)
				{
					this.cornerBRMat = MaterialPool.MatFrom(this.cornerBR, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerBRMat.mainTexture, null);
				}
				if (this.edgeTop != null)
				{
					this.edgeTopMat = MaterialPool.MatFrom(this.edgeTop, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeTopMat.mainTexture, null);
				}
				if (this.edgeBot != null)
				{
					this.edgeBotMat = MaterialPool.MatFrom(this.edgeBot, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeBotMat.mainTexture, null);
				}
				if (this.edgeLeft != null)
				{
					this.edgeLeftMat = MaterialPool.MatFrom(this.edgeLeft, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeLeftMat.mainTexture, null);
				}
				if (this.edgeRight != null)
				{
					this.edgeRightMat = MaterialPool.MatFrom(this.edgeRight, ShaderDatabase.Transparent, 2905);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeRightMat.mainTexture, null);
				}
			});
		}

		// Token: 0x040005A2 RID: 1442
		public bool enabled = true;

		// Token: 0x040005A3 RID: 1443
		public Rect rectN;

		// Token: 0x040005A4 RID: 1444
		public Rect rectE;

		// Token: 0x040005A5 RID: 1445
		public Rect rectS;

		// Token: 0x040005A6 RID: 1446
		public Rect rectW;

		// Token: 0x040005A7 RID: 1447
		public Rect rect;

		// Token: 0x040005A8 RID: 1448
		[NoTranslate]
		public List<string> scratches;

		// Token: 0x040005A9 RID: 1449
		[NoTranslate]
		public string cornerTL;

		// Token: 0x040005AA RID: 1450
		[NoTranslate]
		public string cornerTR;

		// Token: 0x040005AB RID: 1451
		[NoTranslate]
		public string cornerBL;

		// Token: 0x040005AC RID: 1452
		[NoTranslate]
		public string cornerBR;

		// Token: 0x040005AD RID: 1453
		[NoTranslate]
		public string edgeLeft;

		// Token: 0x040005AE RID: 1454
		[NoTranslate]
		public string edgeRight;

		// Token: 0x040005AF RID: 1455
		[NoTranslate]
		public string edgeTop;

		// Token: 0x040005B0 RID: 1456
		[NoTranslate]
		public string edgeBot;

		// Token: 0x040005B1 RID: 1457
		[Unsaved(false)]
		public List<Material> scratchMats;

		// Token: 0x040005B2 RID: 1458
		[Unsaved(false)]
		public Material cornerTLMat;

		// Token: 0x040005B3 RID: 1459
		[Unsaved(false)]
		public Material cornerTRMat;

		// Token: 0x040005B4 RID: 1460
		[Unsaved(false)]
		public Material cornerBLMat;

		// Token: 0x040005B5 RID: 1461
		[Unsaved(false)]
		public Material cornerBRMat;

		// Token: 0x040005B6 RID: 1462
		[Unsaved(false)]
		public Material edgeLeftMat;

		// Token: 0x040005B7 RID: 1463
		[Unsaved(false)]
		public Material edgeRightMat;

		// Token: 0x040005B8 RID: 1464
		[Unsaved(false)]
		public Material edgeTopMat;

		// Token: 0x040005B9 RID: 1465
		[Unsaved(false)]
		public Material edgeBotMat;
	}
}
