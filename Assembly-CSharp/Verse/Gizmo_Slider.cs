using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000613 RID: 1555
	[StaticConstructorOnStartup]
	public abstract class Gizmo_Slider : Gizmo
	{
		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003095 RID: 12437 RVA: 0x00137A1E File Offset: 0x00135C1E
		protected virtual float Width
		{
			get
			{
				return 160f;
			}
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x00137A25 File Offset: 0x00135C25
		public sealed override float GetWidth(float maxWidth)
		{
			return this.Width;
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003097 RID: 12439
		// (set) Token: 0x06003098 RID: 12440
		protected abstract float Target { get; set; }

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003099 RID: 12441
		protected abstract float ValuePercent { get; }

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x0600309A RID: 12442 RVA: 0x00137A2D File Offset: 0x00135C2D
		protected virtual Color BarColor { get; }

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x0600309B RID: 12443 RVA: 0x00137A35 File Offset: 0x00135C35
		protected virtual Color BarHighlightColor { get; }

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x0600309C RID: 12444 RVA: 0x00137A3D File Offset: 0x00135C3D
		protected virtual Color BarDragColor { get; }

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x0600309D RID: 12445 RVA: 0x00137A45 File Offset: 0x00135C45
		protected virtual FloatRange DragRange { get; } = FloatRange.ZeroToOne;

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x00137A4D File Offset: 0x00135C4D
		protected virtual bool IsDraggable { get; }

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x0600309F RID: 12447 RVA: 0x00137A55 File Offset: 0x00135C55
		protected virtual string BarLabel
		{
			get
			{
				return this.ValuePercent.ToStringPercent("0");
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060030A0 RID: 12448
		protected abstract string Title { get; }

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060030A1 RID: 12449 RVA: 0x00137A67 File Offset: 0x00135C67
		protected virtual int Increments { get; } = 20;

		// Token: 0x060030A2 RID: 12450 RVA: 0x00137A6F File Offset: 0x00135C6F
		protected virtual IEnumerable<float> GetBarThresholds()
		{
			return new Gizmo_Slider.<GetBarThresholds>d__41(-2);
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060030A3 RID: 12451 RVA: 0x00137A78 File Offset: 0x00135C78
		protected virtual string HighlightTag
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060030A4 RID: 12452
		// (set) Token: 0x060030A5 RID: 12453
		protected abstract bool DraggingBar { get; set; }

		// Token: 0x060030A6 RID: 12454
		protected abstract string GetTooltip();

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x00137A7B File Offset: 0x00135C7B
		// (set) Token: 0x060030AD RID: 12461 RVA: 0x00137EB1 File Offset: 0x001360B1
		public sealed override float Order
		{
			get
			{
				return -100f;
			}
			set
			{
				base.Order = value;
			}
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x00137A84 File Offset: 0x00135C84
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.initialized = true;
			this.targetValuePct = Mathf.Clamp(this.Target, this.DragRange.min, this.DragRange.max);
			this.barTex = ((this.BarColor == default(Color)) ? Gizmo_Slider.BarTex : SolidColorMaterials.NewSolidColorTexture(this.BarColor));
			this.barHighlightTex = ((this.BarHighlightColor == default(Color)) ? Gizmo_Slider.BarHighlightTex : SolidColorMaterials.NewSolidColorTexture(this.BarHighlightColor));
			this.barDragTex = ((this.BarDragColor == default(Color)) ? Gizmo_Slider.DragBarTex : SolidColorMaterials.NewSolidColorTexture(this.BarDragColor));
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x00137B54 File Offset: 0x00135D54
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(8f);
			Widgets.DrawWindowBackground(rect);
			bool flag = false;
			Text.Font = GameFont.Small;
			Rect headerRect = rect2;
			headerRect.height = Text.LineHeight;
			this.DrawHeader(headerRect, ref flag);
			this.barRect = rect2;
			this.barRect.yMin = headerRect.yMax + 8f;
			if (!this.IsDraggable)
			{
				Widgets.FillableBar(this.barRect, this.ValuePercent, this.barTex, Gizmo_Slider.EmptyBarTex, true);
				using (IEnumerator<float> enumerator = this.GetBarThresholds().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						float num = enumerator.Current;
						GUI.DrawTexture(new Rect
						{
							x = this.barRect.x + 3f + (this.barRect.width - 8f) * num,
							y = this.barRect.y + this.barRect.height - 9f,
							width = 2f,
							height = 6f
						}, (this.ValuePercent < num) ? BaseContent.GreyTex : BaseContent.BlackTex);
					}
					goto IL_201;
				}
			}
			bool draggingBar = this.DraggingBar;
			Widgets.DraggableBar(this.barRect, this.barTex, this.barHighlightTex, Gizmo_Slider.EmptyBarTex, this.barDragTex, ref draggingBar, this.ValuePercent, ref this.targetValuePct, this.GetBarThresholds(), this.Increments, this.DragRange.min, this.DragRange.max);
			this.DraggingBar = draggingBar;
			this.targetValuePct = Mathf.Clamp(this.targetValuePct, this.DragRange.min, this.DragRange.max);
			this.Target = this.targetValuePct;
			IL_201:
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(this.barRect, this.BarLabel);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect) && !flag)
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, new Func<string>(this.GetTooltip), Gen.HashCombineInt(this.GetHashCode(), 8573612));
			}
			if (!this.HighlightTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, this.HighlightTag);
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x00137DE4 File Offset: 0x00135FE4
		protected virtual void DrawHeader(Rect headerRect, ref bool mouseOverElement)
		{
			string text = this.Title;
			text = text.Truncate(headerRect.width, null);
			Widgets.Label(headerRect, text);
		}

		// Token: 0x04001F47 RID: 8007
		private Texture2D barTex;

		// Token: 0x04001F48 RID: 8008
		private Texture2D barHighlightTex;

		// Token: 0x04001F49 RID: 8009
		private Texture2D barDragTex;

		// Token: 0x04001F4A RID: 8010
		private float targetValuePct;

		// Token: 0x04001F4B RID: 8011
		private bool initialized;

		// Token: 0x04001F4C RID: 8012
		protected Rect barRect;

		// Token: 0x04001F4D RID: 8013
		private const float Spacing = 8f;

		// Token: 0x04001F4E RID: 8014
		private static readonly Texture2D BarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

		// Token: 0x04001F4F RID: 8015
		private static readonly Texture2D BarHighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.43f, 0.54f, 0.55f));

		// Token: 0x04001F50 RID: 8016
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x04001F51 RID: 8017
		private static readonly Texture2D DragBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74f, 0.97f, 0.8f));
	}
}
