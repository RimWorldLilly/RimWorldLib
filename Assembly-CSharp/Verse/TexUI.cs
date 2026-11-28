using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000658 RID: 1624
	[StaticConstructorOnStartup]
	public static class TexUI
	{
		// Token: 0x060032D6 RID: 13014 RVA: 0x00144E04 File Offset: 0x00143004
		static TexUI()
		{
			TexUI.GrayscaleGUI.SetTexture(ShaderPropertyIDs.MaskTex, Texture2D.redTexture);
		}

		// Token: 0x040020DD RID: 8413
		public static readonly Texture2D TitleBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x040020DE RID: 8414
		public static readonly Texture2D HighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x040020DF RID: 8415
		public static readonly Texture2D HighlightSelectedTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0.94f, 0.5f, 0.18f));

		// Token: 0x040020E0 RID: 8416
		public static readonly Texture2D ArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowRight", true);

		// Token: 0x040020E1 RID: 8417
		public static readonly Texture2D ArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowLeft", true);

		// Token: 0x040020E2 RID: 8418
		public static readonly Texture2D ConcaveArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/ConcaveArrowRight", true);

		// Token: 0x040020E3 RID: 8419
		public static readonly Texture2D ConcaveArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/ConcaveArrowLeft", true);

		// Token: 0x040020E4 RID: 8420
		public static readonly Texture2D WinExpandWidget = ContentFinder<Texture2D>.Get("UI/Widgets/WinExpandWidget", true);

		// Token: 0x040020E5 RID: 8421
		public static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Misc/AlertFlashArrow", true);

		// Token: 0x040020E6 RID: 8422
		public static readonly Texture2D RotLeftTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotLeft", true);

		// Token: 0x040020E7 RID: 8423
		public static readonly Texture2D RotRightTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotRight", true);

		// Token: 0x040020E8 RID: 8424
		public static readonly Texture2D GuiltyTex = ContentFinder<Texture2D>.Get("UI/Icons/Guilty", true);

		// Token: 0x040020E9 RID: 8425
		public static readonly Texture2D CopyTex = ContentFinder<Texture2D>.Get("UI/Buttons/Copy", true);

		// Token: 0x040020EA RID: 8426
		public static readonly Texture2D DismissTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dismiss", true);

		// Token: 0x040020EB RID: 8427
		public static readonly Texture2D RenameTex = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);

		// Token: 0x040020EC RID: 8428
		public static readonly Texture2D MakeDefault = ContentFinder<Texture2D>.Get("UI/Buttons/MakeDefault", true);

		// Token: 0x040020ED RID: 8429
		public static readonly Texture2D RectHighlight = ContentFinder<Texture2D>.Get("UI/Overlays/HighlightAtlas", true);

		// Token: 0x040020EE RID: 8430
		public static readonly Texture2D GrayBg = SolidColorMaterials.NewSolidColorTexture(new ColorInt(51, 63, 51, 200).ToColor);

		// Token: 0x040020EF RID: 8431
		public static readonly Texture2D DotHighlight = ContentFinder<Texture2D>.Get("UI/Overlays/DotHighlight", true);

		// Token: 0x040020F0 RID: 8432
		public static readonly Texture2D SelectionBracketWhole = ContentFinder<Texture2D>.Get("UI/Overlays/SelectionBracketWhole", true);

		// Token: 0x040020F1 RID: 8433
		public static readonly Texture2D Placeholder = ContentFinder<Texture2D>.Get("PlaceholderImage", true);

		// Token: 0x040020F2 RID: 8434
		public static readonly Color OldActiveResearchColor = new ColorInt(0, 64, 64, 255).ToColor;

		// Token: 0x040020F3 RID: 8435
		public static readonly Color OldFinishedResearchColor = new ColorInt(0, 64, 16, 255).ToColor;

		// Token: 0x040020F4 RID: 8436
		public static readonly Color AvailResearchColor = new ColorInt(32, 32, 32, 255).ToColor;

		// Token: 0x040020F5 RID: 8437
		public static readonly Color ActiveResearchColor = new ColorInt(81, 66, 7, 255).ToColor;

		// Token: 0x040020F6 RID: 8438
		public static readonly Color OtherActiveResearchColor = new ColorInt(78, 109, 129, 130).ToColor;

		// Token: 0x040020F7 RID: 8439
		public static readonly Color FinishedResearchColor = new ColorInt(0, 64, 64, 255).ToColor;

		// Token: 0x040020F8 RID: 8440
		public static readonly Color LockedResearchColor = new ColorInt(42, 42, 42, 255).ToColor;

		// Token: 0x040020F9 RID: 8441
		public static readonly Color HiddenResearchColor = new ColorInt(42, 42, 42, 255).ToColor;

		// Token: 0x040020FA RID: 8442
		public static readonly Color HighlightBgResearchColor = new ColorInt(30, 30, 30, 255).ToColor;

		// Token: 0x040020FB RID: 8443
		public static readonly Color HighlightBorderResearchColor = new ColorInt(160, 160, 160, 255).ToColor;

		// Token: 0x040020FC RID: 8444
		public static readonly Color BorderResearchSelectedColor = new ColorInt(240, 240, 240, 255).ToColor;

		// Token: 0x040020FD RID: 8445
		public static readonly Color BorderResearchingColor = new ColorInt(253, 225, 114, 255).ToColor;

		// Token: 0x040020FE RID: 8446
		public static readonly Color DefaultBorderResearchColor = new ColorInt(80, 80, 80, 255).ToColor;

		// Token: 0x040020FF RID: 8447
		public static readonly Color ResearchMainTabColor = new Color(0.2f, 0.8f, 0.85f);

		// Token: 0x04002100 RID: 8448
		public static readonly Color FinishedResearchColorTransparent = new ColorInt(78, 109, 129, 140).ToColor;

		// Token: 0x04002101 RID: 8449
		public static readonly Color DefaultLineResearchColor = new ColorInt(60, 60, 60, 255).ToColor;

		// Token: 0x04002102 RID: 8450
		public static readonly Color HighlightLineResearchColor = new ColorInt(51, 205, 217, 255).ToColor;

		// Token: 0x04002103 RID: 8451
		public static readonly Color DependencyOutlineResearchColor = new ColorInt(217, 20, 51, 255).ToColor;

		// Token: 0x04002104 RID: 8452
		public static readonly Texture2D FastFillTex = Texture2D.whiteTexture;

		// Token: 0x04002105 RID: 8453
		public static readonly GUIStyle FastFillStyle = new GUIStyle
		{
			normal = new GUIStyleState
			{
				background = TexUI.FastFillTex
			}
		};

		// Token: 0x04002106 RID: 8454
		public static readonly Texture2D TextBGBlack = ContentFinder<Texture2D>.Get("UI/Widgets/TextBGBlack", true);

		// Token: 0x04002107 RID: 8455
		public static readonly Texture2D GrayTextBG = ContentFinder<Texture2D>.Get("UI/Overlays/GrayTextBG", true);

		// Token: 0x04002108 RID: 8456
		public static readonly Texture2D FloatMenuOptionBG = ContentFinder<Texture2D>.Get("UI/Widgets/FloatMenuOptionBG", true);

		// Token: 0x04002109 RID: 8457
		public static readonly Material GrayscaleGUI = MatLoader.LoadMat("Misc/GrayscaleGUI", -1);

		// Token: 0x0400210A RID: 8458
		public static Texture2D SteamDeck_ButtonA = ContentFinder<Texture2D>.Get("UI/Icons/SteamDeck/button_a", true);
	}
}
