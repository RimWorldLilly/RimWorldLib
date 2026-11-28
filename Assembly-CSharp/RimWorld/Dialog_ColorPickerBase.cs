using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C9F RID: 7327
	public abstract class Dialog_ColorPickerBase : Window
	{
		// Token: 0x17002212 RID: 8722
		// (get) Token: 0x0600BB72 RID: 47986 RVA: 0x0042800D File Offset: 0x0042620D
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 450f);
			}
		}

		// Token: 0x17002213 RID: 8723
		// (get) Token: 0x0600BB73 RID: 47987
		protected abstract bool ShowDarklight { get; }

		// Token: 0x17002214 RID: 8724
		// (get) Token: 0x0600BB74 RID: 47988
		protected abstract Color DefaultColor { get; }

		// Token: 0x17002215 RID: 8725
		// (get) Token: 0x0600BB75 RID: 47989
		protected abstract List<Color> PickableColors { get; }

		// Token: 0x17002216 RID: 8726
		// (get) Token: 0x0600BB76 RID: 47990
		protected abstract float ForcedColorValue { get; }

		// Token: 0x17002217 RID: 8727
		// (get) Token: 0x0600BB77 RID: 47991
		protected abstract bool ShowColorTemperatureBar { get; }

		// Token: 0x0600BB78 RID: 47992
		protected abstract void SaveColor(Color color);

		// Token: 0x0600BB79 RID: 47993 RVA: 0x0042801E File Offset: 0x0042621E
		public Dialog_ColorPickerBase(Widgets.ColorComponents visibleTextfields, Widgets.ColorComponents editableTextfields) : base(null)
		{
			this.visibleTextfields = visibleTextfields;
			this.editableTextfields = editableTextfields;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this.closeOnAccept = false;
		}

		// Token: 0x0600BB7A RID: 47994 RVA: 0x00428060 File Offset: 0x00426260
		private static void HeaderRow(ref RectDivider layout)
		{
			using (new TextBlock(GameFont.Medium))
			{
				TaggedString taggedString = "ChooseAColor".Translate().CapitalizeFirst();
				RectDivider rectDivider = layout.NewRow(Text.CalcHeight(taggedString, layout.Rect.width), VerticalJustification.Top, null);
				GUI.SetNextControlName(Dialog_ColorPickerBase.focusableControlNames[0]);
				Rect rect = rectDivider.Rect;
				rect.y -= 5f;
				Widgets.Label(rect, taggedString);
			}
		}

		// Token: 0x0600BB7B RID: 47995 RVA: 0x00428108 File Offset: 0x00426308
		private void BottomButtons(ref RectDivider layout)
		{
			RectDivider rectDivider = layout.NewRow(Dialog_ColorPickerBase.ButSize.y, VerticalJustification.Bottom, null);
			if (Widgets.ButtonText(rectDivider.NewCol(Dialog_ColorPickerBase.ButSize.x, HorizontalJustification.Left, null), "Cancel".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(rectDivider.NewCol(Dialog_ColorPickerBase.ButSize.x, HorizontalJustification.Right, null), "Accept".Translate(), true, true, true, null))
			{
				this.SaveColor(this.color);
				this.Close(true);
			}
		}

		// Token: 0x0600BB7C RID: 47996 RVA: 0x004281D0 File Offset: 0x004263D0
		private void ColorPalette(ref RectDivider layout, ref Color color, Color defaultColor, bool showDarklight, out float paletteHeight)
		{
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				RectDivider rectDivider = layout;
				RectDivider rect = rectDivider.NewCol(250f, HorizontalJustification.Right, null);
				int num = 26;
				RectDivider rectDivider2 = rect.NewRow((float)num, VerticalJustification.Top, null);
				int num2 = 4;
				Rect parent;
				Rect parent2;
				rectDivider2.Rect.SplitVertically((float)(num2 * (num + 2)), out parent, out parent2);
				RectDivider rect2 = new RectDivider(parent, 195906069, new Vector2?(new Vector2(10f, 2f)));
				Widgets.ColorBox(rect2.NewCol((float)num, HorizontalJustification.Left, null), ref color, defaultColor, 22, 2, null);
				Widgets.Label(rect2, "Default".Translate().CapitalizeFirst());
				RectDivider rect3 = new RectDivider(parent2, 195906069, new Vector2?(new Vector2(10f, 2f)));
				Color defaultDarklight = DarklightUtility.DefaultDarklight;
				Rect rect4 = rect3.NewCol((float)num, HorizontalJustification.Left, null);
				if (showDarklight)
				{
					Widgets.ColorBox(rect4, ref color, defaultDarklight, 22, 2, null);
					Widgets.Label(rect3, "Darklight".Translate().CapitalizeFirst());
				}
				Widgets.ColorSelector(rect, ref color, this.PickableColors, out paletteHeight, null, 22, 2, null);
				paletteHeight += (float)(num + 2);
			}
		}

		// Token: 0x0600BB7D RID: 47997 RVA: 0x00428364 File Offset: 0x00426564
		private void ColorTextfields(ref RectDivider layout, out Vector2 size)
		{
			RectAggregator rectAggregator = new RectAggregator(new Rect(layout.Rect.position, new Vector2(125f, 0f)), 195906069, null);
			bool flag = Widgets.ColorTextfields(ref rectAggregator, ref this.color, ref this.textfieldBuffers, ref this.textfieldColorBuffer, this.previousFocusedControlName, "colorTextfields", this.editableTextfields, this.visibleTextfields);
			size = rectAggregator.Rect.size;
			if (flag)
			{
				float h;
				float s;
				float num;
				Color.RGBToHSV(this.color, out h, out s, out num);
				if (this.ForcedColorValue >= 0f)
				{
					this.color = Color.HSVToRGB(h, s, this.ForcedColorValue);
				}
			}
		}

		// Token: 0x0600BB7E RID: 47998 RVA: 0x00428424 File Offset: 0x00426624
		private static void ColorReadback(Rect rect, Color color, Color oldColor, bool showDarklight)
		{
			Rect parent;
			Rect parent2;
			rect.SplitVertically((rect.width - 26f) / 2f, out parent, out parent2);
			RectDivider rectDivider = new RectDivider(parent, 195906069, null);
			TaggedString label = "CurrentColor".Translate().CapitalizeFirst();
			TaggedString label2 = "OldColor".Translate().CapitalizeFirst();
			float width = Mathf.Max(new float[]
			{
				100f,
				label.GetWidthCached(),
				label2.GetWidthCached()
			});
			RectDivider rect2 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top, null);
			Widgets.Label(rect2.NewCol(width, HorizontalJustification.Left, null), label);
			Widgets.DrawBoxSolid(rect2, color);
			RectDivider rect3 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top, null);
			Widgets.Label(rect3.NewCol(width, HorizontalJustification.Left, null), label2);
			Widgets.DrawBoxSolid(rect3, oldColor);
			RectDivider rect4 = new RectDivider(parent2, 195906069, null);
			rect4.NewCol(26f, HorizontalJustification.Left, null);
			if (showDarklight)
			{
				if (DarklightUtility.IsDarklight(color))
				{
					Widgets.Label(rect4, "Darklight".Translate().CapitalizeFirst());
					return;
				}
				Widgets.Label(rect4, "NotDarklight".Translate().CapitalizeFirst());
			}
		}

		// Token: 0x0600BB7F RID: 47999 RVA: 0x004285BC File Offset: 0x004267BC
		private static void TabControl()
		{
			if (Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.Tab)
			{
				bool flag = !Event.current.shift;
				Event.current.Use();
				string text = GUI.GetNameOfFocusedControl();
				if (text.NullOrEmpty())
				{
					text = Dialog_ColorPickerBase.focusableControlNames[0];
				}
				int num = Dialog_ColorPickerBase.focusableControlNames.IndexOf(text);
				if (num < 0)
				{
					num = Dialog_ColorPickerBase.focusableControlNames.Count;
				}
				if (flag)
				{
					num++;
				}
				else
				{
					num--;
				}
				if (num >= Dialog_ColorPickerBase.focusableControlNames.Count)
				{
					num = 0;
				}
				else if (num < 0)
				{
					num = Dialog_ColorPickerBase.focusableControlNames.Count - 1;
				}
				GUI.FocusControl(Dialog_ColorPickerBase.focusableControlNames[num]);
			}
		}

		// Token: 0x0600BB80 RID: 48000 RVA: 0x00428674 File Offset: 0x00426874
		public override void DoWindowContents(Rect inRect)
		{
			using (TextBlock.Default())
			{
				RectDivider rect = new RectDivider(inRect, 195906069, null);
				Dialog_ColorPickerBase.HeaderRow(ref rect);
				rect.NewRow(0f, VerticalJustification.Top, null);
				this.BottomButtons(ref rect);
				rect.NewRow(0f, VerticalJustification.Bottom, null);
				float h;
				float s;
				float num;
				Color.RGBToHSV(this.DefaultColor, out h, out s, out num);
				Color defaultColor = (this.ForcedColorValue >= 0f) ? Color.HSVToRGB(h, s, this.ForcedColorValue) : this.DefaultColor;
				defaultColor.a = 1f;
				float num2;
				this.ColorPalette(ref rect, ref this.color, defaultColor, this.ShowDarklight, out num2);
				Vector2 vector;
				this.ColorTextfields(ref rect, out vector);
				float height = Mathf.Max(new float[]
				{
					num2,
					128f,
					vector.y
				});
				RectDivider rectDivider = rect.NewRow(height, VerticalJustification.Top, null);
				rectDivider.NewCol(vector.x, HorizontalJustification.Left, null);
				rectDivider.NewCol(250f, HorizontalJustification.Right, null);
				Widgets.HSVColorWheel(rectDivider.Rect.ContractedBy((rectDivider.Rect.width - 128f) / 2f, (rectDivider.Rect.height - 128f) / 2f), ref this.color, ref this.hsvColorWheelDragging, (this.ForcedColorValue >= 0f) ? new float?(this.ForcedColorValue) : null, null);
				rect.NewRow(10f, VerticalJustification.Top, null);
				Rect rect2 = rect.NewRow(34f, VerticalJustification.Top, null);
				if (this.ShowColorTemperatureBar)
				{
					Widgets.ColorTemperatureBar(rect2, ref this.color, ref this.colorTemperatureDragging, (this.ForcedColorValue >= 0f) ? new float?(this.ForcedColorValue) : null);
				}
				rect.NewRow(26f, VerticalJustification.Top, null);
				Dialog_ColorPickerBase.ColorReadback(rect, this.color, this.oldColor, this.ShowDarklight);
				Dialog_ColorPickerBase.TabControl();
				if (Event.current.type == EventType.layout)
				{
					this.previousFocusedControlName = GUI.GetNameOfFocusedControl();
				}
			}
		}

		// Token: 0x04006AC5 RID: 27333
		private const int ContextHash = 195906069;

		// Token: 0x04006AC6 RID: 27334
		private static readonly List<string> focusableControlNames = new List<string>
		{
			"title",
			"colorTextfields_0",
			"colorTextfields_1",
			"colorTextfields_2",
			"colorTextfields_3",
			"colorTextfields_4"
		};

		// Token: 0x04006AC7 RID: 27335
		private const int ColorWheelSize = 128;

		// Token: 0x04006AC8 RID: 27336
		private const int ColorTextfieldsWidth = 125;

		// Token: 0x04006AC9 RID: 27337
		private const int PaletteColumns = 9;

		// Token: 0x04006ACA RID: 27338
		private const int ColorIconSize = 22;

		// Token: 0x04006ACB RID: 27339
		private const int ColorIconPadding = 2;

		// Token: 0x04006ACC RID: 27340
		private const int CurrentColorLabelWidth = 100;

		// Token: 0x04006ACD RID: 27341
		private const int PaletteWidth = 250;

		// Token: 0x04006ACE RID: 27342
		private const int ColorTemperatureBarHeight = 34;

		// Token: 0x04006ACF RID: 27343
		protected Color color;

		// Token: 0x04006AD0 RID: 27344
		protected Color oldColor;

		// Token: 0x04006AD1 RID: 27345
		private bool hsvColorWheelDragging;

		// Token: 0x04006AD2 RID: 27346
		private bool colorTemperatureDragging;

		// Token: 0x04006AD3 RID: 27347
		private string[] textfieldBuffers = new string[6];

		// Token: 0x04006AD4 RID: 27348
		private Color textfieldColorBuffer;

		// Token: 0x04006AD5 RID: 27349
		private string previousFocusedControlName;

		// Token: 0x04006AD6 RID: 27350
		private Widgets.ColorComponents visibleTextfields;

		// Token: 0x04006AD7 RID: 27351
		private Widgets.ColorComponents editableTextfields;

		// Token: 0x04006AD8 RID: 27352
		protected static readonly Vector2 ButSize = new Vector2(150f, 38f);
	}
}
