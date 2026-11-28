using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000051 RID: 81
	public abstract class Window_DevListing : Window_Dev
	{
		// Token: 0x0600021C RID: 540 RVA: 0x0000FE94 File Offset: 0x0000E094
		protected Window_DevListing()
		{
			this.draggable = true;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
			this.drawInScreenshotMode = false;
			this.onlyDrawInDevMode = true;
			this.doCloseX = true;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000FEE6 File Offset: 0x0000E0E6
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000FEE9 File Offset: 0x0000E0E9
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(275f, 400f);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000FF01 File Offset: 0x0000E101
		public virtual bool AutoUpdate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000FF04 File Offset: 0x0000E104
		// (set) Token: 0x06000222 RID: 546 RVA: 0x0000FF0C File Offset: 0x0000E10C
		protected bool Dirty
		{
			get
			{
				return this.dirty;
			}
			set
			{
				this.dirty = (value || this.dirty);
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000FF20 File Offset: 0x0000E120
		public sealed override void DoWindowContents(Rect inRect)
		{
			this.y = 4f;
			this.dirty = false;
			this.container = inRect;
			using (new TextBlock(GameFont.Tiny))
			{
				this.DoWindowListing();
			}
			this.windowRect.height = this.y + 4f;
			if (this.dirty && this.AutoUpdate)
			{
				this.OnChanged();
			}
		}

		// Token: 0x06000224 RID: 548
		protected abstract void DoWindowListing();

		// Token: 0x06000225 RID: 549 RVA: 0x0000FFA4 File Offset: 0x0000E1A4
		protected virtual void OnChanged()
		{
			this.dirty = false;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000FFB0 File Offset: 0x0000E1B0
		protected void PrintLabel(string text, GameFont font = GameFont.Tiny)
		{
			using (new TextBlock(font))
			{
				DevGUI.Label(new Rect(this.container.x, this.y, this.container.width, 20f), text);
				this.y += 20f;
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00010024 File Offset: 0x0000E224
		protected bool SliderFieldInt(string label, ref float sliderValue, ref int value, int min = 0, int max = 1, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			rect3.yMin += rect3.height / 4f;
			DevGUI.Label(rect2, label);
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			sliderValue = DevGUI.HorizontalSlider(rect3, sliderValue, (float)min, (float)max);
			int num = Mathf.RoundToInt(sliderValue);
			if (num != value)
			{
				value = num;
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000100A8 File Offset: 0x0000E2A8
		protected bool SliderFieldFloat(string label, ref float value, float min = 0f, float max = 1f, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			rect3.yMin += rect3.height / 4f;
			DevGUI.Label(rect2, label);
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			float b = value;
			value = DevGUI.HorizontalSlider(rect3, value, min, max);
			if (!Mathf.Approximately(value, b))
			{
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00010124 File Offset: 0x0000E324
		protected bool SliderFieldFloat(string label, ref float value, ref bool enabled, float min = 0f, float max = 1f, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			Rect rect4 = rect3.RightPartPixels(rect.height);
			rect3.yMin += rect3.height / 4f;
			rect3.xMax = rect4.x;
			DevGUI.Label(rect2, label);
			bool flag = enabled;
			DevGUI.Checkbox(rect4, ref enabled);
			if (enabled != flag)
			{
				this.dirty = true;
			}
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			float b = value;
			value = DevGUI.HorizontalSlider(rect3, value, min, max);
			if (!Mathf.Approximately(value, b))
			{
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000101DC File Offset: 0x0000E3DC
		protected bool SliderFieldDouble(string label, ref double value, ref bool fine, float min = 0f, float max = 1f, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			Rect rect4 = rect3.RightPartPixels(rect.height);
			rect3.yMin += rect3.height / 4f;
			rect3.xMax = rect4.x;
			bool flag = fine;
			DevGUI.Checkbox(rect4, ref fine);
			TooltipHandler.TipRegion(rect4, "Toggle fine value adjustment");
			if (fine)
			{
				max /= 10f;
			}
			if ((fine != flag & fine) && value > (double)max)
			{
				value = (double)max;
				this.dirty = true;
			}
			DevGUI.Label(rect2, label);
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			float num = (float)value;
			float num2 = DevGUI.HorizontalSlider(rect3, num, min, max);
			if (!Mathf.Approximately(num, num2))
			{
				value = (double)num2;
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000102CC File Offset: 0x0000E4CC
		protected bool TextFieldInt(string label, ref string source, ref int value, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			DevGUI.Label(rect2, label);
			source = DevGUI.TextField(rect3, source);
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			int num;
			if (int.TryParse(source, out num) && num != value)
			{
				value = num;
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00010330 File Offset: 0x0000E530
		protected bool TextFieldDouble(string label, ref string source, ref double value, string tooltip = null)
		{
			Rect rect = this.TakeRow();
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			DevGUI.Label(rect2, label);
			source = DevGUI.TextField(rect3, source);
			if (!string.IsNullOrEmpty(tooltip))
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			double num;
			if (double.TryParse(source, out num) && Math.Abs(num - value) > 9.999999747378752E-05)
			{
				value = num;
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000103A4 File Offset: 0x0000E5A4
		protected bool Button(string label)
		{
			if (DevGUI.ButtonText(this.TakeRow(), label, null))
			{
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000103D4 File Offset: 0x0000E5D4
		protected bool Toggle(string label, ref bool value)
		{
			this.y += 4f;
			if (DevGUI.ButtonText(this.TakeRow(), label, null))
			{
				value = !value;
				this.dirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0001041B File Offset: 0x0000E61B
		protected void Gap(float height)
		{
			this.y += height;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0001042B File Offset: 0x0000E62B
		protected Rect TakeRow()
		{
			Rect result = new Rect(this.container.x, this.y, this.container.width, 20f);
			this.y += 24f;
			return result;
		}

		// Token: 0x04000128 RID: 296
		private Rect container;

		// Token: 0x04000129 RID: 297
		private float y;

		// Token: 0x0400012A RID: 298
		private bool dirty;
	}
}
