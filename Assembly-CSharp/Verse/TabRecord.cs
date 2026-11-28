using System;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000655 RID: 1621
	[StaticConstructorOnStartup]
	public class TabRecord
	{
		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x060032C8 RID: 13000 RVA: 0x0014408F File Offset: 0x0014228F
		public bool Selected
		{
			get
			{
				if (this.selectedGetter == null)
				{
					return this.selected;
				}
				return this.selectedGetter();
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060032C9 RID: 13001 RVA: 0x001440AB File Offset: 0x001422AB
		public virtual string TutorTag
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x001440AE File Offset: 0x001422AE
		public TabRecord(string label, Action clickedAction, bool selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selected = selected;
		}

		// Token: 0x060032CB RID: 13003 RVA: 0x001440D6 File Offset: 0x001422D6
		public TabRecord(string label, Action clickedAction, Func<bool> selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selectedGetter = selected;
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x001440FE File Offset: 0x001422FE
		public virtual string GetTip()
		{
			return null;
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x00144104 File Offset: 0x00142304
		public void Draw(Rect rect)
		{
			Rect drawRect = new Rect(rect);
			drawRect.width = 30f;
			Rect drawRect2 = new Rect(rect);
			drawRect2.width = 30f;
			drawRect2.x = rect.x + rect.width - 30f;
			Rect uvRect = new Rect(0.53125f, 0f, 0.46875f, 1f);
			Rect drawRect3 = new Rect(rect);
			drawRect3.x += drawRect.width;
			drawRect3.width -= 60f;
			drawRect3.xMin = UIScaling.AdjustCoordToUIScalingFloor(drawRect3.xMin);
			drawRect3.xMax = UIScaling.AdjustCoordToUIScalingCeil(drawRect3.xMax);
			Rect uvRect2 = new Rect(30f, 0f, 4f, (float)TabRecord.TabAtlas.height).ToUVRect(new Vector2((float)TabRecord.TabAtlas.width, (float)TabRecord.TabAtlas.height));
			Widgets.DrawTexturePart(drawRect, new Rect(0f, 0f, 0.46875f, 1f), TabRecord.TabAtlas);
			Widgets.DrawTexturePart(drawRect3, uvRect2, TabRecord.TabAtlas);
			Widgets.DrawTexturePart(drawRect2, uvRect, TabRecord.TabAtlas);
			GUI.color = (this.labelColor ?? Color.white);
			Rect rect2 = rect;
			rect2.width -= 10f;
			if (Mouse.IsOver(rect2))
			{
				GUI.color = Color.yellow;
				rect2.x += 2f;
				rect2.y -= 2f;
			}
			if (!this.TutorTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect2, this.TutorTag);
			}
			Text.WordWrap = false;
			Widgets.Label(rect, this.label);
			Text.WordWrap = true;
			GUI.color = Color.white;
			if (!this.Selected)
			{
				Rect drawRect4 = new Rect(rect);
				drawRect4.y += rect.height;
				drawRect4.y -= 1f;
				drawRect4.height = 1f;
				Rect uvRect3 = new Rect(0.5f, 0.01f, 0.01f, 0.01f);
				Widgets.DrawTexturePart(drawRect4, uvRect3, TabRecord.TabAtlas);
			}
		}

		// Token: 0x0400207C RID: 8316
		public string label = "Tab";

		// Token: 0x0400207D RID: 8317
		public Action clickedAction;

		// Token: 0x0400207E RID: 8318
		public bool selected;

		// Token: 0x0400207F RID: 8319
		public Func<bool> selectedGetter;

		// Token: 0x04002080 RID: 8320
		public Color? labelColor;

		// Token: 0x04002081 RID: 8321
		private const float TabEndWidth = 30f;

		// Token: 0x04002082 RID: 8322
		private const float TabMiddleGraphicWidth = 4f;

		// Token: 0x04002083 RID: 8323
		private static readonly Texture2D TabAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TabAtlas", true);
	}
}
