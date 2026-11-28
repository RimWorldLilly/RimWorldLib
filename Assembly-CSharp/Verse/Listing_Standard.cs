using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000627 RID: 1575
	[StaticConstructorOnStartup]
	public class Listing_Standard : Listing
	{
		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x0600313D RID: 12605 RVA: 0x0013ACAC File Offset: 0x00138EAC
		public Rect? BoundingRectCached
		{
			get
			{
				if (this.boundingRectCachedForFrame != Time.frameCount)
				{
					if (this.boundingRect != null && this.boundingScrollPositionGetter != null)
					{
						Rect value = this.boundingRect.Value;
						Vector2 vector = this.boundingScrollPositionGetter();
						value.x += vector.x;
						value.y += vector.y;
						this.boundingRectCached = new Rect?(value);
					}
					this.boundingRectCachedForFrame = Time.frameCount;
				}
				return this.boundingRectCached;
			}
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x0013AD38 File Offset: 0x00138F38
		public Listing_Standard(GameFont font)
		{
			this.font = font;
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x0013AD4E File Offset: 0x00138F4E
		public Listing_Standard()
		{
			this.font = GameFont.Small;
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x0013AD64 File Offset: 0x00138F64
		public Listing_Standard(Rect boundingRect, Func<Vector2> boundingScrollPositionGetter)
		{
			this.font = GameFont.Small;
			this.boundingRect = new Rect?(boundingRect);
			this.boundingScrollPositionGetter = boundingScrollPositionGetter;
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x0013AD8D File Offset: 0x00138F8D
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Font = this.font;
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x0013ADA4 File Offset: 0x00138FA4
		public override void End()
		{
			base.End();
			if (this.labelScrollbarPositions != null)
			{
				for (int i = this.labelScrollbarPositions.Count - 1; i >= 0; i--)
				{
					if (!this.labelScrollbarPositionsSetThisFrame.Contains(this.labelScrollbarPositions[i].First))
					{
						this.labelScrollbarPositions.RemoveAt(i);
					}
				}
				this.labelScrollbarPositionsSetThisFrame.Clear();
			}
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x0013AE0F File Offset: 0x0013900F
		public Rect Label(TaggedString label, float maxHeight = -1f, string tooltip = null)
		{
			return this.Label(label.Resolve(), maxHeight, new TipSignal?(tooltip));
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x0013AE2C File Offset: 0x0013902C
		public Rect Label(string label, float maxHeight = -1f, TipSignal? tipSignal = null)
		{
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool flag = false;
			if (maxHeight >= 0f && num > maxHeight)
			{
				num = maxHeight;
				flag = true;
			}
			Rect rect = base.GetRect(num, 1f);
			if (this.BoundingRectCached != null && !rect.Overlaps(this.BoundingRectCached.Value))
			{
				return rect;
			}
			if (flag)
			{
				Vector2 labelScrollbarPosition = this.GetLabelScrollbarPosition(this.curX, this.curY);
				Widgets.LabelScrollable(rect, label, ref labelScrollbarPosition, false, true, false);
				this.SetLabelScrollbarPosition(this.curX, this.curY, labelScrollbarPosition);
			}
			else
			{
				Widgets.Label(rect, label);
			}
			if (tipSignal != null)
			{
				TooltipHandler.TipRegion(rect, tipSignal.Value);
			}
			base.Gap(this.verticalSpacing);
			return rect;
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x0013AEF4 File Offset: 0x001390F4
		public void LabelDouble(string leftLabel, string rightLabel, string tip = null)
		{
			float num = base.ColumnWidth / 2f;
			float width = base.ColumnWidth - num;
			float a = Text.CalcHeight(leftLabel, num);
			float b = Text.CalcHeight(rightLabel, width);
			float height = Mathf.Max(a, b);
			Rect rect = base.GetRect(height, 1f);
			if (this.BoundingRectCached != null && !rect.Overlaps(this.BoundingRectCached.Value))
			{
				return;
			}
			if (!tip.NullOrEmpty())
			{
				Widgets.DrawHighlightIfMouseover(rect);
				TooltipHandler.TipRegion(rect, tip);
			}
			Widgets.Label(rect.LeftHalf(), leftLabel);
			Widgets.Label(rect.RightHalf(), rightLabel);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x0013AFA8 File Offset: 0x001391A8
		public Rect SubLabel(string label, float widthPct)
		{
			float height = Text.CalcHeight(label, base.ColumnWidth * widthPct);
			Rect rect = base.GetRect(height, widthPct);
			float num = 20f;
			rect.x += num;
			rect.width -= num;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.gray;
			Widgets.Label(rect, label);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			base.Gap(this.verticalSpacing);
			return rect;
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x0013B025 File Offset: 0x00139225
		public bool RadioButton(string label, bool active, float tabIn = 0f, string tooltip = null, float? tooltipDelay = null)
		{
			return this.RadioButton(label, active, tabIn, tooltip, tooltipDelay, false);
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x0013B035 File Offset: 0x00139235
		public bool RadioButton(string label, bool active, float tabIn, string tooltip, float? tooltipDelay, bool disabled)
		{
			return this.RadioButton(label, active, tabIn, 0f, tooltip, tooltipDelay, disabled);
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x0013B04C File Offset: 0x0013924C
		public bool RadioButton(string label, bool active, float tabIn, float tabInRight, string tooltip, float? tooltipDelay, bool disabled)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight, 1f);
			rect.xMin += tabIn;
			rect.xMax -= tabInRight;
			if (this.BoundingRectCached != null && !rect.Overlaps(this.BoundingRectCached.Value))
			{
				return false;
			}
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				TipSignal tip = (tooltipDelay != null) ? new TipSignal(tooltip, tooltipDelay.Value) : new TipSignal(tooltip);
				TooltipHandler.TipRegion(rect, tip);
			}
			bool result = Widgets.RadioButtonLabeled(rect, label, active, disabled);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x0013B10C File Offset: 0x0013930C
		public void CheckboxLabeled(string label, ref bool checkOn, float tabIn)
		{
			float height = Text.CalcHeight(label, base.ColumnWidth);
			Rect rect = base.GetRect(height, 1f);
			rect.xMin += tabIn;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false, false);
				base.Gap(this.verticalSpacing);
			}
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x0013B184 File Offset: 0x00139384
		public void CheckboxLabeled(string label, ref bool checkOn, string tooltip = null, float height = 0f, float labelPct = 1f)
		{
			float height2 = (height != 0f) ? height : Text.CalcHeight(label, base.ColumnWidth * labelPct);
			Rect rect = base.GetRect(height2, labelPct);
			rect.width = Math.Min(rect.width + 24f, base.ColumnWidth);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				if (!tooltip.NullOrEmpty())
				{
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
					}
					TooltipHandler.TipRegion(rect, tooltip);
				}
				Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false, false);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x0013B23C File Offset: 0x0013943C
		public bool CheckboxLabeledSelectable(string label, ref bool selected, ref bool checkOn)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight, 1f);
			bool result = false;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				result = Widgets.CheckboxLabeledSelectable(rect, label, ref selected, ref checkOn, null, 1f);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x0013B2A4 File Offset: 0x001394A4
		public bool ButtonText(string label, string highlightTag = null, float widthPct = 1f)
		{
			Rect rect = base.GetRect(30f, widthPct);
			bool result = false;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				result = Widgets.ButtonText(rect, label, true, true, true, null);
				if (highlightTag != null)
				{
					UIHighlighter.HighlightOpportunity(rect, highlightTag);
				}
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x0013B312 File Offset: 0x00139512
		public bool ButtonTextLabeled(string label, string buttonLabel, TextAnchor anchor = TextAnchor.UpperLeft, string highlightTag = null, string tooltip = null)
		{
			return this.ButtonTextLabeledPct(label, buttonLabel, 0.5f, anchor, highlightTag, tooltip, null);
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x0013B328 File Offset: 0x00139528
		public bool ButtonTextLabeledPct(string label, string buttonLabel, float labelPct, TextAnchor anchor = TextAnchor.UpperLeft, string highlightTag = null, string tooltip = null, Texture2D labelIcon = null)
		{
			float height = Math.Max(Text.CalcHeight(label, base.ColumnWidth * labelPct), 30f);
			Rect rect = base.GetRect(height, 1f);
			Rect rect2 = rect.RightPart(1f - labelPct);
			rect2.height = 30f;
			if (highlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, highlightTag);
			}
			bool result = false;
			Rect rect3 = rect.LeftPart(labelPct);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Text.Anchor = anchor;
				Widgets.Label(rect3, label);
				result = Widgets.ButtonText(rect2, buttonLabel.Truncate(rect2.width - 20f, null), true, true, true, null);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (labelIcon != null)
			{
				GUI.DrawTexture(new Rect(Text.CalcSize(label).x + 10f, rect3.y + (rect3.height - Text.LineHeight) / 2f, Text.LineHeight, Text.LineHeight), labelIcon);
			}
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
				}
				TooltipHandler.TipRegion(rect3, tooltip);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x0013B474 File Offset: 0x00139674
		public bool ButtonImage(Texture2D tex, float width, float height)
		{
			base.NewColumnIfNeeded(height);
			Rect butRect = new Rect(this.curX, this.curY, width, height);
			bool result = false;
			if (this.BoundingRectCached == null || butRect.Overlaps(this.BoundingRectCached.Value))
			{
				result = Widgets.ButtonImage(butRect, tex, true, null);
			}
			base.Gap(height + this.verticalSpacing);
			return result;
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x0013B4DF File Offset: 0x001396DF
		public void None()
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			this.Label("NoneBrackets".Translate(), -1f, null);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x0013B518 File Offset: 0x00139718
		public string TextEntry(string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount, 1f);
			string result;
			if (lineCount == 1)
			{
				result = Widgets.TextField(rect, text);
			}
			else
			{
				result = Widgets.TextArea(rect, text, false);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x0013B55D File Offset: 0x0013975D
		public string TextEntryLabeled(string label, string text, int lineCount = 1)
		{
			string result = Widgets.TextEntryLabeled(base.GetRect(Text.LineHeight * (float)lineCount, 1f), label, text);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x0013B588 File Offset: 0x00139788
		public void TextFieldNumeric<T>(ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.TextFieldNumeric<T>(rect, ref val, ref buffer, min, max);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x0013B5E4 File Offset: 0x001397E4
		public void TextFieldNumericLabeled<T>(string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.TextFieldNumericLabeled<T>(rect, label, ref val, ref buffer, min, max);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x0013B644 File Offset: 0x00139844
		public Rect IntRange(ref IntRange range, int min, int max)
		{
			Rect rect = base.GetRect(32f, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.IntRange(rect, (int)base.CurHeight, ref range, min, max, null, 0);
			}
			base.Gap(this.verticalSpacing);
			return rect;
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x0013B6A8 File Offset: 0x001398A8
		public float Slider(float val, float min, float max)
		{
			float result = Widgets.HorizontalSlider(base.GetRect(22f, 1f), val, min, max, false, null, null, null, -1f);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x0013B6E4 File Offset: 0x001398E4
		public float SliderLabeled(string label, float val, float min, float max, float labelPct = 0.5f, string tooltip = null)
		{
			Rect rect = base.GetRect(30f, 1f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect.LeftPart(labelPct), label);
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect.LeftPart(labelPct), tooltip);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			float result = Widgets.HorizontalSlider(rect.RightPart(1f - labelPct), val, min, max, true, null, null, null, -1f);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x0013B760 File Offset: 0x00139960
		public void IntAdjuster(ref int val, int countChange, int min = 0)
		{
			Rect rect = base.GetRect(24f, 1f);
			rect.width = 42f;
			if (Widgets.ButtonText(rect, "-" + countChange.ToString(), true, true, true, null))
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				val -= countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			rect.x += rect.width + 2f;
			if (Widgets.ButtonText(rect, "+" + countChange.ToString(), true, true, true, null))
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				val += countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x0013B83C File Offset: 0x00139A3C
		public void IntSetter(ref int val, int target, string label, float width = 42f)
		{
			if (Widgets.ButtonText(base.GetRect(24f, 1f), label, true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				val = target;
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x0013B888 File Offset: 0x00139A88
		public void IntEntry(ref int val, ref string editBuffer, int multiplier = 1, int min = 0)
		{
			Rect rect = base.GetRect(24f, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.IntEntry(rect, ref val, ref editBuffer, multiplier);
				if (val < min)
				{
					val = min;
					editBuffer = val.ToString();
				}
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x0013B8F4 File Offset: 0x00139AF4
		public Listing_Standard BeginSection(float height, float sectionBorder = 4f, float bottomBorder = 4f)
		{
			Rect rect = base.GetRect(height + sectionBorder + bottomBorder, 1f);
			Widgets.DrawMenuSection(rect);
			Listing_Standard listing_Standard = new Listing_Standard();
			Rect rect2 = new Rect(rect.x + sectionBorder, rect.y + sectionBorder, rect.width - sectionBorder * 2f, rect.height - (sectionBorder + bottomBorder));
			listing_Standard.Begin(rect2);
			return listing_Standard;
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x0013B957 File Offset: 0x00139B57
		public void EndSection(Listing_Standard listing)
		{
			listing.End();
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x0013B960 File Offset: 0x00139B60
		private Vector2 GetLabelScrollbarPosition(float x, float y)
		{
			if (this.labelScrollbarPositions == null)
			{
				return Vector2.zero;
			}
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					return this.labelScrollbarPositions[i].Second;
				}
			}
			return Vector2.zero;
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x0013B9D4 File Offset: 0x00139BD4
		private void SetLabelScrollbarPosition(float x, float y, Vector2 scrollbarPosition)
		{
			if (this.labelScrollbarPositions == null)
			{
				this.labelScrollbarPositions = new List<Pair<Vector2, Vector2>>();
				this.labelScrollbarPositionsSetThisFrame = new List<Vector2>();
			}
			this.labelScrollbarPositionsSetThisFrame.Add(new Vector2(x, y));
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					this.labelScrollbarPositions[i] = new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition);
					return;
				}
			}
			this.labelScrollbarPositions.Add(new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition));
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x0013BA80 File Offset: 0x00139C80
		public bool SelectableDef(string name, bool selected, Action deleteCallback)
		{
			Text.Font = GameFont.Tiny;
			float width = this.listingRect.width - 21f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect = new Rect(this.curX, this.curY, width, 21f);
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 1, null);
			}
			Text.WordWrap = false;
			Widgets.Label(rect, name);
			Text.WordWrap = true;
			if (deleteCallback != null && Widgets.ButtonImage(new Rect(rect.xMax, rect.y, 21f, 21f), TexButton.Delete, Color.white, GenUI.SubtleMouseoverColor, true, null))
			{
				deleteCallback();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			this.curY += 21f;
			return Widgets.ButtonInvisible(rect, true);
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x0013BB50 File Offset: 0x00139D50
		public void LabelCheckboxDebug(string label, ref bool checkOn, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth, 22f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.CheckboxLabeled(rect, label.Truncate(rect.width - 15f, null), ref checkOn, false, null, null, false, false);
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
			}
			base.Gap(22f + this.verticalSpacing);
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x0013BC04 File Offset: 0x00139E04
		public bool ButtonDebug(string label, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth, 22f);
			bool result = false;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				bool wordWrap = Text.WordWrap;
				Text.WordWrap = false;
				result = Widgets.ButtonText(rect, "  " + label, true, true, true, new TextAnchor?(TextAnchor.MiddleLeft));
				Text.WordWrap = wordWrap;
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x0013BCC8 File Offset: 0x00139EC8
		public DebugActionButtonResult ButtonDebugPinnable(string label, bool highlight, bool pinned)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth - 22f, 22f);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				bool wordWrap = Text.WordWrap;
				Text.WordWrap = false;
				if (Widgets.ButtonText(rect, "  " + label, true, true, true, new TextAnchor?(TextAnchor.MiddleLeft)))
				{
					result = DebugActionButtonResult.ButtonPressed;
				}
				Text.WordWrap = wordWrap;
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
				Rect rect2 = new Rect(rect.xMax + 2f, rect.y, 22f, 22f).ContractedBy(4f);
				GUI.color = (pinned ? Color.white : new Color(1f, 1f, 1f, 0.2f));
				GUI.DrawTexture(rect2, pinned ? Listing_Standard.PinTex : Listing_Standard.PinOutlineTex);
				GUI.color = Color.white;
				if (Widgets.ButtonInvisible(rect2, true))
				{
					result = DebugActionButtonResult.PinPressed;
				}
				Widgets.DrawHighlightIfMouseover(rect2);
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x0013BE1C File Offset: 0x0013A01C
		public DebugActionButtonResult CheckboxPinnable(string label, ref bool checkOn, bool highlight, bool pinned)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth - 22f, 22f);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.CheckboxLabeled(rect, label.Truncate(rect.width - 24f - 15f, null), ref checkOn, false, null, null, false, false);
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, label);
				}
				Rect rect2 = new Rect(rect.xMax + 2f, rect.y, 22f, 22f).ContractedBy(4f);
				GUI.color = (pinned ? Color.white : new Color(1f, 1f, 1f, 0.2f));
				GUI.DrawTexture(rect2, pinned ? Listing_Standard.PinTex : Listing_Standard.PinOutlineTex);
				GUI.color = Color.white;
				if (Widgets.ButtonInvisible(rect2, true))
				{
					result = DebugActionButtonResult.PinPressed;
				}
				Widgets.DrawHighlightIfMouseover(rect2);
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x04001F9B RID: 8091
		private GameFont font;

		// Token: 0x04001F9C RID: 8092
		private Rect? boundingRect;

		// Token: 0x04001F9D RID: 8093
		private Func<Vector2> boundingScrollPositionGetter;

		// Token: 0x04001F9E RID: 8094
		private List<Pair<Vector2, Vector2>> labelScrollbarPositions;

		// Token: 0x04001F9F RID: 8095
		private List<Vector2> labelScrollbarPositionsSetThisFrame;

		// Token: 0x04001FA0 RID: 8096
		private int boundingRectCachedForFrame = -1;

		// Token: 0x04001FA1 RID: 8097
		private Rect? boundingRectCached;

		// Token: 0x04001FA2 RID: 8098
		private static readonly Texture2D PinTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin", true);

		// Token: 0x04001FA3 RID: 8099
		private static readonly Texture2D PinOutlineTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin-Outline", true);

		// Token: 0x04001FA4 RID: 8100
		public const float PinnableActionHeight = 22f;

		// Token: 0x04001FA5 RID: 8101
		private const float DefSelectionLineHeight = 21f;
	}
}
