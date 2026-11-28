using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200065B RID: 1627
	public class WidgetRow
	{
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060032EB RID: 13035 RVA: 0x001456AA File Offset: 0x001438AA
		public float FinalX
		{
			get
			{
				return this.curX;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060032EC RID: 13036 RVA: 0x001456B2 File Offset: 0x001438B2
		public float FinalY
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x001456BA File Offset: 0x001438BA
		// (set) Token: 0x060032EE RID: 13038 RVA: 0x001456C2 File Offset: 0x001438C2
		public float CellGap
		{
			get
			{
				return this.gap;
			}
			set
			{
				this.gap = value;
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x001456CB File Offset: 0x001438CB
		public WidgetRow()
		{
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x001456E5 File Offset: 0x001438E5
		public WidgetRow(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.Init(x, y, growDirection, maxWidth, gap);
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x0014570C File Offset: 0x0014390C
		public void Init(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.growDirection = growDirection;
			this.startX = x;
			this.curX = x;
			this.curY = y;
			this.maxWidth = maxWidth;
			this.gap = gap;
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x0014573A File Offset: 0x0014393A
		private float LeftX(float elementWidth)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				return this.curX;
			}
			return this.curX - elementWidth;
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x00145760 File Offset: 0x00143960
		private void IncrementPosition(float amount)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				this.curX += amount;
			}
			else
			{
				this.curX -= amount;
			}
			if (Mathf.Abs(this.curX - this.startX) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x001457C0 File Offset: 0x001439C0
		private void IncrementY()
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.LeftThenUp)
			{
				this.curY -= 24f + this.gap;
			}
			else
			{
				this.curY += 24f + this.gap;
			}
			this.curX = this.startX;
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x0014581E File Offset: 0x00143A1E
		private void IncrementYIfWillExceedMaxWidth(float width)
		{
			if (Mathf.Abs(this.curX - this.startX) + Mathf.Abs(width) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x00145847 File Offset: 0x00143A47
		public void Gap(float width)
		{
			if (this.curX != this.startX)
			{
				this.IncrementPosition(width);
			}
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x00145860 File Offset: 0x00143A60
		public bool ButtonIcon(Texture2D tex, string tooltip = null, Color? mouseoverColor = null, Color? backgroundColor = null, Color? mouseoverBackgroundColor = null, bool doMouseoverSound = true, float overrideSize = -1f)
		{
			Rect rect = this.ButtonIconRect(overrideSize);
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (mouseoverBackgroundColor != null && Mouse.IsOver(rect))
			{
				Widgets.DrawRectFast(rect, mouseoverBackgroundColor.Value, null);
			}
			else if (backgroundColor != null && !Mouse.IsOver(rect))
			{
				Widgets.DrawRectFast(rect, backgroundColor.Value, null);
			}
			bool result = Widgets.ButtonImage(rect, tex, Color.white, mouseoverColor ?? GenUI.MouseoverColor, true, null);
			this.IncrementPosition((overrideSize > 0f) ? overrideSize : 24f);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x00145914 File Offset: 0x00143B14
		public Rect ButtonIconRect(float overrideSize = -1f)
		{
			float num = (overrideSize > 0f) ? overrideSize : 24f;
			float num2 = (24f - num) / 2f;
			this.IncrementYIfWillExceedMaxWidth(num);
			return new Rect(this.LeftX(num) + num2, this.curY + num2, num, num);
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x00145960 File Offset: 0x00143B60
		public bool ButtonIconWithBG(Texture2D texture, float width = -1f, string tooltip = null, bool doMouseoverSound = true)
		{
			if (width < 0f)
			{
				width = 24f;
			}
			width += 16f;
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 26f);
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			bool result = Widgets.ButtonImageWithBG(rect, texture, new Vector2?(Vector2.one * 24f));
			this.IncrementPosition(width + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x001459F0 File Offset: 0x00143BF0
		public void ToggleableIcon(ref bool toggleable, Texture2D tex, string tooltip, SoundDef mouseoverSound = null, string tutorTag = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			bool flag = Widgets.ButtonImage(rect, tex, true, null);
			this.IncrementPosition(24f + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			Rect position = new Rect(rect.x + rect.width / 2f, rect.y, rect.height / 2f, rect.height / 2f);
			Texture2D image = toggleable ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
			GUI.DrawTexture(position, image);
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect, mouseoverSound);
			}
			if (flag)
			{
				toggleable = !toggleable;
				if (toggleable)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, tutorTag);
			}
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x00145AE8 File Offset: 0x00143CE8
		public Rect Icon(Texture tex, string tooltip = null, float contractedBy = 0f)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			rect = rect.ContractedBy(contractedBy);
			GUI.DrawTexture(rect, tex);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(24f + this.gap);
			return rect;
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x00145B58 File Offset: 0x00143D58
		public Rect DefIcon(ThingDef def, string tooltip = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			Widgets.DefIcon(rect, def, null, 1f, null, false, null, null, null, 1f);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(24f + this.gap);
			return rect;
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x00145BE0 File Offset: 0x00143DE0
		public bool ButtonText(string label, string tooltip = null, bool drawBackground = true, bool doMouseoverSound = true, bool active = true, float? fixedWidth = null)
		{
			Rect rect = this.ButtonRect(label, fixedWidth);
			bool result = Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, active, null);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x00145C20 File Offset: 0x00143E20
		public Rect ButtonRect(string label, float? fixedWidth = null)
		{
			Vector2 vector = (fixedWidth != null) ? new Vector2(fixedWidth.Value, 24f) : Text.CalcSize(label);
			vector.x += 16f;
			vector.y += 2f;
			this.IncrementYIfWillExceedMaxWidth(vector.x);
			Rect result = new Rect(this.LeftX(vector.x), this.curY, vector.x, vector.y);
			this.IncrementPosition(result.width + this.gap);
			return result;
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x00145CB8 File Offset: 0x00143EB8
		public Rect Label(string text, float width = -1f, string tooltip = null, float height = -1f)
		{
			if (height < 0f)
			{
				height = 24f;
			}
			if (width < 0f)
			{
				width = Text.CalcSize(text).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width + 2f);
			this.IncrementPosition(2f);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.Label(rect, text);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x00145D50 File Offset: 0x00143F50
		public Rect LabelEllipses(string text, float width, string tooltip = null, float height = -1f)
		{
			if (height < 0f)
			{
				height = 24f;
			}
			this.IncrementYIfWillExceedMaxWidth(width + 2f);
			this.IncrementPosition(2f);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.LabelEllipses(rect, text);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x00145DD0 File Offset: 0x00143FD0
		public Rect TextFieldNumeric<T>(ref int val, ref string buffer, float width = -1f) where T : struct
		{
			if (width < 0f)
			{
				width = Text.CalcSize(val.ToString()).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width + 2f);
			this.IncrementPosition(2f);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 24f);
			Widgets.TextFieldNumeric<int>(rect, ref val, ref buffer, 0f, 1E+09f);
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x00145E54 File Offset: 0x00144054
		public Rect FillableBar(float width, float height, float fillPct, string label, Texture2D fillTex, Texture2D bgTex = null)
		{
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.FillableBar(rect, fillPct, fillTex, bgTex, false);
			if (!label.NullOrEmpty())
			{
				Rect rect2 = rect;
				rect2.xMin += 2f;
				rect2.xMax -= 2f;
				if (!Text.TinyFontSupported)
				{
					rect2.y -= 2f;
				}
				if (Text.Anchor >= TextAnchor.UpperLeft)
				{
					rect2.height += 14f;
				}
				using (new TextBlock(new GameFont?(GameFont.Tiny), null, new bool?(false)))
				{
					Widgets.Label(rect2, label);
				}
			}
			this.IncrementPosition(width);
			return rect;
		}

		// Token: 0x04002119 RID: 8473
		private float startX;

		// Token: 0x0400211A RID: 8474
		private float curX;

		// Token: 0x0400211B RID: 8475
		private float curY;

		// Token: 0x0400211C RID: 8476
		private float maxWidth = 99999f;

		// Token: 0x0400211D RID: 8477
		private float gap;

		// Token: 0x0400211E RID: 8478
		private UIDirection growDirection = UIDirection.RightThenUp;

		// Token: 0x0400211F RID: 8479
		public const float IconSize = 24f;

		// Token: 0x04002120 RID: 8480
		public const float DefaultGap = 4f;

		// Token: 0x04002121 RID: 8481
		private const float DefaultMaxWidth = 99999f;

		// Token: 0x04002122 RID: 8482
		public const float LabelGap = 2f;

		// Token: 0x04002123 RID: 8483
		public const float ButtonExtraSpace = 16f;
	}
}
