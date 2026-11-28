using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C8E RID: 7310
	public class Dialog_AllowedAreaColorPicker : Dialog_ColorPickerBase
	{
		// Token: 0x170021DB RID: 8667
		// (get) Token: 0x0600BA95 RID: 47765 RVA: 0x0041C6D3 File Offset: 0x0041A8D3
		protected override Color DefaultColor
		{
			get
			{
				return this.oldColor;
			}
		}

		// Token: 0x170021DC RID: 8668
		// (get) Token: 0x0600BA96 RID: 47766 RVA: 0x0041C6DB File Offset: 0x0041A8DB
		protected override bool ShowDarklight
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170021DD RID: 8669
		// (get) Token: 0x0600BA97 RID: 47767 RVA: 0x0041C6DE File Offset: 0x0041A8DE
		protected override List<Color> PickableColors
		{
			get
			{
				return Dialog_AllowedAreaColorPicker.colors;
			}
		}

		// Token: 0x170021DE RID: 8670
		// (get) Token: 0x0600BA98 RID: 47768 RVA: 0x0041C6E5 File Offset: 0x0041A8E5
		protected override float ForcedColorValue
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170021DF RID: 8671
		// (get) Token: 0x0600BA99 RID: 47769 RVA: 0x0041C6EC File Offset: 0x0041A8EC
		protected override bool ShowColorTemperatureBar
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600BA9A RID: 47770 RVA: 0x0041C6F0 File Offset: 0x0041A8F0
		public Dialog_AllowedAreaColorPicker(Area_Allowed area) : base(Widgets.ColorComponents.None, Widgets.ColorComponents.None)
		{
			this.area = area;
			float h;
			float s;
			float num;
			Color.RGBToHSV(area.Color, out h, out s, out num);
			this.color = Color.HSVToRGB(h, s, 0.5f);
			this.oldColor = this.color;
		}

		// Token: 0x0600BA9B RID: 47771 RVA: 0x0041C73C File Offset: 0x0041A93C
		protected override void SaveColor(Color color)
		{
			float hue;
			float sat;
			float num;
			Color.RGBToHSV(color, out hue, out sat, out num);
			ColorInt colorInt = new ColorInt(this.area.Color);
			colorInt.SetHueSaturation(hue, sat);
			this.area.SetColor(colorInt.ToColor);
		}

		// Token: 0x04006A02 RID: 27138
		protected const float AreaColorValue = 0.5f;

		// Token: 0x04006A03 RID: 27139
		private static readonly List<Color> colors = new List<Color>
		{
			Color.HSVToRGB(0f, 0f, 0.5f),
			Color.HSVToRGB(0f, 0.5f, 0.5f),
			Color.HSVToRGB(0f, 0.33f, 0.5f),
			Color.HSVToRGB(0.055555556f, 1f, 0.5f),
			Color.HSVToRGB(0.055555556f, 0.5f, 0.5f),
			Color.HSVToRGB(0.055555556f, 0.33f, 0.5f),
			Color.HSVToRGB(0.11111111f, 1f, 0.5f),
			Color.HSVToRGB(0.11111111f, 0.5f, 0.5f),
			Color.HSVToRGB(0.11111111f, 0.33f, 0.5f),
			Color.HSVToRGB(0.16666667f, 1f, 0.5f),
			Color.HSVToRGB(0.16666667f, 0.5f, 0.5f),
			Color.HSVToRGB(0.16666667f, 0.33f, 0.5f),
			Color.HSVToRGB(0.22222222f, 1f, 0.5f),
			Color.HSVToRGB(0.22222222f, 0.5f, 0.5f),
			Color.HSVToRGB(0.22222222f, 0.33f, 0.5f),
			Color.HSVToRGB(0.2777778f, 1f, 0.5f),
			Color.HSVToRGB(0.2777778f, 0.5f, 0.5f),
			Color.HSVToRGB(0.2777778f, 0.33f, 0.5f),
			Color.HSVToRGB(0.33333334f, 1f, 0.5f),
			Color.HSVToRGB(0.33333334f, 0.5f, 0.5f),
			Color.HSVToRGB(0.33333334f, 0.33f, 0.5f),
			Color.HSVToRGB(0.3888889f, 1f, 0.5f),
			Color.HSVToRGB(0.3888889f, 0.5f, 0.5f),
			Color.HSVToRGB(0.3888889f, 0.33f, 0.5f),
			Color.HSVToRGB(0.44444445f, 1f, 0.5f),
			Color.HSVToRGB(0.44444445f, 0.5f, 0.5f),
			Color.HSVToRGB(0.44444445f, 0.33f, 0.5f),
			Color.HSVToRGB(0.5f, 1f, 0.5f),
			Color.HSVToRGB(0.5f, 0.5f, 0.5f),
			Color.HSVToRGB(0.5f, 0.33f, 0.5f),
			Color.HSVToRGB(0.5555556f, 1f, 0.5f),
			Color.HSVToRGB(0.5555556f, 0.5f, 0.5f),
			Color.HSVToRGB(0.5555556f, 0.33f, 0.5f),
			Color.HSVToRGB(0.6111111f, 1f, 0.5f),
			Color.HSVToRGB(0.6111111f, 0.5f, 0.5f),
			Color.HSVToRGB(0.6111111f, 0.33f, 0.5f),
			Color.HSVToRGB(0.6666667f, 1f, 0.5f),
			Color.HSVToRGB(0.6666667f, 0.5f, 0.5f),
			Color.HSVToRGB(0.6666667f, 0.33f, 0.5f),
			Color.HSVToRGB(0.7222222f, 1f, 0.5f),
			Color.HSVToRGB(0.7222222f, 0.5f, 0.5f),
			Color.HSVToRGB(0.7222222f, 0.33f, 0.5f),
			Color.HSVToRGB(0.7777778f, 1f, 0.5f),
			Color.HSVToRGB(0.7777778f, 0.5f, 0.5f),
			Color.HSVToRGB(0.7777778f, 0.33f, 0.5f),
			Color.HSVToRGB(0.8333333f, 1f, 0.5f),
			Color.HSVToRGB(0.8333333f, 0.5f, 0.5f),
			Color.HSVToRGB(0.8333333f, 0.33f, 0.5f),
			Color.HSVToRGB(0.8888889f, 1f, 0.5f),
			Color.HSVToRGB(0.8888889f, 0.5f, 0.5f),
			Color.HSVToRGB(0.8888889f, 0.33f, 0.5f),
			Color.HSVToRGB(0.9444444f, 1f, 0.5f),
			Color.HSVToRGB(0.9444444f, 0.5f, 0.5f),
			Color.HSVToRGB(0.9444444f, 0.33f, 0.5f)
		};

		// Token: 0x04006A04 RID: 27140
		private Area_Allowed area;
	}
}
