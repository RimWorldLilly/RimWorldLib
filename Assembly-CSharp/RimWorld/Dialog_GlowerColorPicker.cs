using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB0 RID: 7344
	public class Dialog_GlowerColorPicker : Dialog_ColorPickerBase
	{
		// Token: 0x17002243 RID: 8771
		// (get) Token: 0x0600BC6B RID: 48235 RVA: 0x00435785 File Offset: 0x00433985
		protected override Color DefaultColor
		{
			get
			{
				return this.glower.Props.glowColor.ToColor;
			}
		}

		// Token: 0x17002244 RID: 8772
		// (get) Token: 0x0600BC6C RID: 48236 RVA: 0x0043579C File Offset: 0x0043399C
		protected override bool ShowDarklight { get; } = 1;

		// Token: 0x17002245 RID: 8773
		// (get) Token: 0x0600BC6D RID: 48237 RVA: 0x004357A4 File Offset: 0x004339A4
		protected override List<Color> PickableColors
		{
			get
			{
				return Dialog_GlowerColorPicker.colors;
			}
		}

		// Token: 0x17002246 RID: 8774
		// (get) Token: 0x0600BC6E RID: 48238 RVA: 0x004357AB File Offset: 0x004339AB
		protected override float ForcedColorValue
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17002247 RID: 8775
		// (get) Token: 0x0600BC6F RID: 48239 RVA: 0x004357B2 File Offset: 0x004339B2
		protected override bool ShowColorTemperatureBar
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BC70 RID: 48240 RVA: 0x004357B8 File Offset: 0x004339B8
		public Dialog_GlowerColorPicker(CompGlower glower, IList<CompGlower> extraGlowers, Widgets.ColorComponents visibleTextfields, Widgets.ColorComponents editableTextfields) : base(visibleTextfields, editableTextfields)
		{
			this.glower = glower;
			this.extraGlowers = new CompGlower[extraGlowers.Count];
			extraGlowers.CopyTo(this.extraGlowers, 0);
			float h;
			float s;
			float num;
			Color.RGBToHSV(glower.GlowColor.ToColor, out h, out s, out num);
			this.color = Color.HSVToRGB(h, s, 1f);
			this.oldColor = this.color;
		}

		// Token: 0x0600BC71 RID: 48241 RVA: 0x00435834 File Offset: 0x00433A34
		protected override void SaveColor(Color color)
		{
			float hue;
			float sat;
			float num;
			Color.RGBToHSV(color, out hue, out sat, out num);
			ColorInt glowColor = this.glower.GlowColor;
			glowColor.SetHueSaturation(hue, sat);
			this.glower.GlowColor = glowColor;
			foreach (CompGlower compGlower in this.extraGlowers)
			{
				glowColor = compGlower.GlowColor;
				glowColor.SetHueSaturation(hue, sat);
				compGlower.GlowColor = glowColor;
			}
		}

		// Token: 0x04006BB1 RID: 27569
		protected const float GlowValue = 1f;

		// Token: 0x04006BB2 RID: 27570
		private static readonly List<Color> colors = new List<Color>
		{
			Color.HSVToRGB(0f, 0f, 1f),
			Color.HSVToRGB(0f, 0.5f, 1f),
			Color.HSVToRGB(0f, 0.33f, 1f),
			Color.HSVToRGB(0.055555556f, 1f, 1f),
			Color.HSVToRGB(0.055555556f, 0.5f, 1f),
			Color.HSVToRGB(0.055555556f, 0.33f, 1f),
			Color.HSVToRGB(0.11111111f, 1f, 1f),
			Color.HSVToRGB(0.11111111f, 0.5f, 1f),
			Color.HSVToRGB(0.11111111f, 0.33f, 1f),
			Color.HSVToRGB(0.16666667f, 1f, 1f),
			Color.HSVToRGB(0.16666667f, 0.5f, 1f),
			Color.HSVToRGB(0.16666667f, 0.33f, 1f),
			Color.HSVToRGB(0.22222222f, 1f, 1f),
			Color.HSVToRGB(0.22222222f, 0.5f, 1f),
			Color.HSVToRGB(0.22222222f, 0.33f, 1f),
			Color.HSVToRGB(0.2777778f, 1f, 1f),
			Color.HSVToRGB(0.2777778f, 0.5f, 1f),
			Color.HSVToRGB(0.2777778f, 0.33f, 1f),
			Color.HSVToRGB(0.33333334f, 1f, 1f),
			Color.HSVToRGB(0.33333334f, 0.5f, 1f),
			Color.HSVToRGB(0.33333334f, 0.33f, 1f),
			Color.HSVToRGB(0.3888889f, 1f, 1f),
			Color.HSVToRGB(0.3888889f, 0.5f, 1f),
			Color.HSVToRGB(0.3888889f, 0.33f, 1f),
			Color.HSVToRGB(0.44444445f, 1f, 1f),
			Color.HSVToRGB(0.44444445f, 0.5f, 1f),
			Color.HSVToRGB(0.44444445f, 0.33f, 1f),
			Color.HSVToRGB(0.5f, 1f, 1f),
			Color.HSVToRGB(0.5f, 0.5f, 1f),
			Color.HSVToRGB(0.5f, 0.33f, 1f),
			Color.HSVToRGB(0.5555556f, 1f, 1f),
			Color.HSVToRGB(0.5555556f, 0.5f, 1f),
			Color.HSVToRGB(0.5555556f, 0.33f, 1f),
			Color.HSVToRGB(0.6111111f, 1f, 1f),
			Color.HSVToRGB(0.6111111f, 0.5f, 1f),
			Color.HSVToRGB(0.6111111f, 0.33f, 1f),
			Color.HSVToRGB(0.6666667f, 1f, 1f),
			Color.HSVToRGB(0.6666667f, 0.5f, 1f),
			Color.HSVToRGB(0.6666667f, 0.33f, 1f),
			Color.HSVToRGB(0.7222222f, 1f, 1f),
			Color.HSVToRGB(0.7222222f, 0.5f, 1f),
			Color.HSVToRGB(0.7222222f, 0.33f, 1f),
			Color.HSVToRGB(0.7777778f, 1f, 1f),
			Color.HSVToRGB(0.7777778f, 0.5f, 1f),
			Color.HSVToRGB(0.7777778f, 0.33f, 1f),
			Color.HSVToRGB(0.8333333f, 1f, 1f),
			Color.HSVToRGB(0.8333333f, 0.5f, 1f),
			Color.HSVToRGB(0.8333333f, 0.33f, 1f),
			Color.HSVToRGB(0.8888889f, 1f, 1f),
			Color.HSVToRGB(0.8888889f, 0.5f, 1f),
			Color.HSVToRGB(0.8888889f, 0.33f, 1f),
			Color.HSVToRGB(0.9444444f, 1f, 1f),
			Color.HSVToRGB(0.9444444f, 0.5f, 1f),
			Color.HSVToRGB(0.9444444f, 0.33f, 1f)
		};

		// Token: 0x04006BB3 RID: 27571
		private CompGlower glower;

		// Token: 0x04006BB4 RID: 27572
		private CompGlower[] extraGlowers;
	}
}
