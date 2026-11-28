using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200008E RID: 142
	[StaticConstructorOnStartup]
	public static class BaseContent
	{
		// Token: 0x06000666 RID: 1638 RVA: 0x00025857 File Offset: 0x00023A57
		public static bool NullOrBad(this Material mat)
		{
			return mat == null || mat == BaseContent.BadMat;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0002586F File Offset: 0x00023A6F
		public static bool NullOrBad(this Texture2D tex)
		{
			return tex == null || tex == BaseContent.BadTex;
		}

		// Token: 0x0400021B RID: 539
		public static readonly string BadTexPath = "UI/Misc/BadTexture";

		// Token: 0x0400021C RID: 540
		public static readonly string PlaceholderImagePath = "PlaceholderImage";

		// Token: 0x0400021D RID: 541
		public static readonly string PlaceholderGearImagePath = "PlaceholderImage_Gear";

		// Token: 0x0400021E RID: 542
		public static readonly Material BadMat = MaterialPool.MatFrom(BaseContent.BadTexPath, ShaderDatabase.Cutout);

		// Token: 0x0400021F RID: 543
		public static readonly Texture2D BadTex = ContentFinder<Texture2D>.Get(BaseContent.BadTexPath, true);

		// Token: 0x04000220 RID: 544
		public static readonly Graphic BadGraphic = GraphicDatabase.Get<Graphic_Single>(BaseContent.BadTexPath);

		// Token: 0x04000221 RID: 545
		public static readonly Texture2D PlaceholderImage = ContentFinder<Texture2D>.Get(BaseContent.PlaceholderImagePath, true);

		// Token: 0x04000222 RID: 546
		public static readonly Texture2D BlackTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x04000223 RID: 547
		public static readonly Texture2D GreyTex = SolidColorMaterials.NewSolidColorTexture(Color.grey);

		// Token: 0x04000224 RID: 548
		public static readonly Texture2D WhiteTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x04000225 RID: 549
		public static readonly Texture2D ClearTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x04000226 RID: 550
		public static readonly Texture2D YellowTex = SolidColorMaterials.NewSolidColorTexture(Color.yellow);

		// Token: 0x04000227 RID: 551
		public static readonly Material BlackMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.black, false);

		// Token: 0x04000228 RID: 552
		public static readonly Material WhiteMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.white, false);

		// Token: 0x04000229 RID: 553
		public static readonly Material ClearMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.clear, false);
	}
}
