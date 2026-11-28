using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001D0 RID: 464
	public class ThingStyleDef : Def
	{
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x000438A3 File Offset: 0x00041AA3
		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x000438AB File Offset: 0x00041AAB
		public Texture2D UIIcon
		{
			get
			{
				return this.uiIcon;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x000438B3 File Offset: 0x00041AB3
		public bool UseWornGraphicMask
		{
			get
			{
				return this.useWornGraphicMask;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x000438BC File Offset: 0x00041ABC
		public StyleCategoryDef Category
		{
			get
			{
				if (this.hasCategory == null)
				{
					foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						if (styleCategoryDef.thingDefStyles.Any((ThingDefStyle x) => x.StyleDef == this))
						{
							this.cachedCategory = styleCategoryDef;
							this.hasCategory = new bool?(true);
							break;
						}
					}
					if (this.cachedCategory == null)
					{
						this.hasCategory = new bool?(false);
					}
				}
				return this.cachedCategory;
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00043958 File Offset: 0x00041B58
		public override void PostLoad()
		{
			if (this.graphicData != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					if (this.graphicData.shaderType == null)
					{
						this.graphicData.shaderType = ShaderTypeDefOf.Cutout;
					}
					this.graphic = this.graphicData.Graphic;
					this.blueprintGraphicData = new GraphicData();
					this.blueprintGraphicData.CopyFrom(this.graphicData);
					this.blueprintGraphicData.shaderType = ShaderTypeDefOf.EdgeDetect;
					this.blueprintGraphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
					this.blueprintGraphicData.colorTwo = Color.white;
					this.blueprintGraphicData.shadowData = null;
					this.blueprintGraphicData.renderQueue = 2950;
					if (this.graphic == BaseContent.BadGraphic)
					{
						this.graphic = null;
					}
					this.ResolveUIIcon();
				});
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00043974 File Offset: 0x00041B74
		public Texture2D IconForIndex(int index, Rot4? rot = null)
		{
			return (Texture2D)this.graphic.ExtractInnerGraphicFor(null, new int?(index)).MatAt(rot ?? Rot4.North, null).mainTexture;
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000439BC File Offset: 0x00041BBC
		private void ResolveUIIcon()
		{
			if (!this.uiIconPath.NullOrEmpty())
			{
				this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				return;
			}
			if (this.graphic != null)
			{
				Material material = this.graphic.ExtractInnerGraphicFor(null, null).MatAt(Rot4.North, null);
				this.uiIcon = (Texture2D)material.mainTexture;
			}
		}

		// Token: 0x04000D17 RID: 3351
		[MustTranslate]
		public string overrideLabel;

		// Token: 0x04000D18 RID: 3352
		public GraphicData graphicData;

		// Token: 0x04000D19 RID: 3353
		public GraphicData blueprintGraphicData;

		// Token: 0x04000D1A RID: 3354
		[NoTranslate]
		public string uiIconPath;

		// Token: 0x04000D1B RID: 3355
		public float uiIconScale = 1f;

		// Token: 0x04000D1C RID: 3356
		[NoTranslate]
		public string wornGraphicPath;

		// Token: 0x04000D1D RID: 3357
		public Color color;

		// Token: 0x04000D1E RID: 3358
		private Graphic graphic;

		// Token: 0x04000D1F RID: 3359
		private Texture2D uiIcon;

		// Token: 0x04000D20 RID: 3360
		private bool useWornGraphicMask;

		// Token: 0x04000D21 RID: 3361
		private StyleCategoryDef cachedCategory;

		// Token: 0x04000D22 RID: 3362
		private bool? hasCategory;
	}
}
