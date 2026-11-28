using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace LudeonTK
{
	// Token: 0x02000048 RID: 72
	[StaticConstructorOnStartup]
	public abstract class Dialog_DevNoiseBase : Window_DevListing
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000DB74 File Offset: 0x0000BD74
		public override bool AutoUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001DF RID: 479
		protected abstract string Title { get; }

		// Token: 0x060001E0 RID: 480
		protected abstract void OnNoiseChanged();

		// Token: 0x060001E1 RID: 481
		protected abstract int GetSeed();

		// Token: 0x060001E2 RID: 482 RVA: 0x0000DB78 File Offset: 0x0000BD78
		public override void PreOpen()
		{
			base.PreOpen();
			this.noise = new Perlin(0.017000000923871994, 2.0, 0.5, 6, true, false, this.GetSeed(), QualityMode.Medium);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000DBBC File Offset: 0x0000BDBC
		protected override void DoWindowListing()
		{
			base.PrintLabel(this.Title, GameFont.Small);
			base.Gap(4f);
			if (this.planetLayer != PlanetLayer.Selected)
			{
				this.RefreshNoiseVars();
			}
			base.Toggle("Sliders: " + (this.toggleSliders ? "Enabled" : "Disabled"), ref this.toggleSliders);
			base.Toggle("Updating: " + (this.toggleUpdate ? "Enabled" : "Disabled"), ref this.toggleUpdate);
			base.Gap(17f);
			if (this.toggleSliders)
			{
				this.DoSliderControls();
			}
			else
			{
				this.DoTextControls();
			}
			base.Gap(17f);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Seed Offset: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.seedOffset);
			if (base.SliderFieldInt(defaultInterpolatedStringHandler.ToStringAndClear(), ref this.seedOffsetSlider, ref this.seedOffset, -10, 10, null))
			{
				this.noise.Seed = Gen.HashCombineInt(this.GetSeed() + this.seedOffset, this.seedKey);
			}
			if (base.TextFieldInt("Seed part", ref this.seedString, ref this.seedKey, null))
			{
				this.noise.Seed = Gen.HashCombineInt(this.GetSeed() + this.seedOffset, this.seedKey);
			}
			base.Gap(17f);
			Rect rect = base.TakeRow();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Quality Mode: ");
			defaultInterpolatedStringHandler2.AppendFormatted<QualityMode>(this.noise.Quality);
			if (DevGUI.ButtonText(rect, defaultInterpolatedStringHandler2.ToStringAndClear(), null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(QualityMode)))
				{
					QualityMode local2 = (QualityMode)obj;
					QualityMode local = local2;
					list.Add(new FloatMenuOption(local.ToStringSafe<QualityMode>(), delegate()
					{
						this.noise.Quality = local;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					base.Dirty = true;
				}
				FloatMenu window = new FloatMenu(list, "Select quality mode", false);
				Find.WindowStack.Add(window);
			}
			bool normalized = this.noise.Normalized;
			if (base.Toggle("Normalized: " + (normalized ? "Enabled" : "Disabled"), ref normalized))
			{
				this.noise.Normalized = normalized;
			}
			bool invert = this.noise.Invert;
			if (base.Toggle("Invert: " + (invert ? "Enabled" : "Disabled"), ref invert))
			{
				this.noise.Invert = invert;
			}
			base.Gap(17f);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler3.AppendLiteral("Cutoff: ");
			defaultInterpolatedStringHandler3.AppendFormatted<float>(this.cutoff, "0.##");
			base.SliderFieldFloat(defaultInterpolatedStringHandler3.ToStringAndClear(), ref this.cutoff, ref this.cutoffEnabled, 0f, 1f, null);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler4.AppendLiteral("Alpha: ");
			defaultInterpolatedStringHandler4.AppendFormatted<float>(this.alpha * 100f, "0");
			defaultInterpolatedStringHandler4.AppendLiteral("%");
			base.SliderFieldFloat(defaultInterpolatedStringHandler4.ToStringAndClear(), ref this.alpha, 0f, 1f, null);
			base.Gap(17f);
			if (base.Dirty && this.toggleUpdate)
			{
				this.noise.Frequency = this.freq;
				this.noise.Lacunarity = this.lac;
				this.noise.Persistence = this.pers;
				this.noise.OctaveCount = this.oct;
				this.OnNoiseChanged();
				this.OnChanged();
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000DFC8 File Offset: 0x0000C1C8
		private void RefreshNoiseVars()
		{
			this.freq = this.noise.Frequency;
			this.lac = this.noise.Lacunarity;
			this.oct = this.noise.OctaveCount;
			this.pers = this.noise.Persistence;
			this.freqStr = this.freq.ToString(CultureInfo.InvariantCulture);
			this.lacStr = this.lac.ToString(CultureInfo.InvariantCulture);
			this.octStr = this.oct.ToString(CultureInfo.InvariantCulture);
			this.persStr = this.pers.ToString(CultureInfo.InvariantCulture);
			this.planetLayer = PlanetLayer.Selected;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000E07C File Offset: 0x0000C27C
		private void DoSliderControls()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Frequency: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.noise.Frequency, "0.####");
			base.SliderFieldDouble(defaultInterpolatedStringHandler.ToStringAndClear(), ref this.freq, ref this.freqFine, 0f, 0.2f, "Scale of the noise-map.\n\nLarger values zoom the noise out.\nSmaller values zoom it in.");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Lacunarity: ");
			defaultInterpolatedStringHandler2.AppendFormatted<double>(this.noise.Lacunarity, "0.##");
			base.SliderFieldDouble(defaultInterpolatedStringHandler2.ToStringAndClear(), ref this.lac, ref this.lacFine, 0f, 5f, "Muliplies frequency at each octave (adjusts frequency)\n\n1: octaves have same frequency.\n< 1: successive octaves are smoother.\n> 1: successive octaves are rougher.\na");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(13, 1);
			defaultInterpolatedStringHandler3.AppendLiteral("Persistence: ");
			defaultInterpolatedStringHandler3.AppendFormatted<double>(this.noise.Persistence, "0.##");
			base.SliderFieldDouble(defaultInterpolatedStringHandler3.ToStringAndClear(), ref this.pers, ref this.persFine, 0f, 5f, "How much each octave contributes to the final value (adjusts amplitude)\n\n1: octaves contribute equally.\n< 1: successive octaves contribute less.\n> 1: successive octaves contribute more.\na");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler4.AppendLiteral("Octave Count: ");
			defaultInterpolatedStringHandler4.AppendFormatted<int>(this.noise.OctaveCount);
			base.SliderFieldInt(defaultInterpolatedStringHandler4.ToStringAndClear(), ref this.octSliderPos, ref this.oct, 0, 8, "How many octaves contribute to the final value.\n\nHigher octaves contribute according to lacunarity and persistence values.");
			if (!base.Dirty)
			{
				return;
			}
			this.freqStr = this.freq.ToString(CultureInfo.InvariantCulture);
			this.lacStr = this.lac.ToString(CultureInfo.InvariantCulture);
			this.octStr = this.oct.ToString(CultureInfo.InvariantCulture);
			this.persStr = this.pers.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000E238 File Offset: 0x0000C438
		private void DoTextControls()
		{
			base.TextFieldDouble("Frequency", ref this.freqStr, ref this.freq, "Scale of the noise-map.\n\nLarger values zoom the noise out.\nSmaller values zoom it in.");
			base.TextFieldDouble("Lacunarity", ref this.lacStr, ref this.lac, "Muliplies frequency at each octave (adjusts frequency)\n\n1: octaves have same frequency.\n< 1: successive octaves are smoother.\n> 1: successive octaves are rougher.\na");
			base.TextFieldDouble("Persistence", ref this.persStr, ref this.pers, "How much each octave contributes to the final value (adjusts amplitude)\n\n1: octaves contribute equally.\n< 1: successive octaves contribute less.\n> 1: successive octaves contribute more.\na");
			base.TextFieldInt("Octave Count", ref this.octStr, ref this.oct, "How many octaves contribute to the final value.\n\nHigher octaves contribute according to lacunarity and persistence values.");
		}

		// Token: 0x040000E6 RID: 230
		private int seedOffset;

		// Token: 0x040000E7 RID: 231
		private int seedKey;

		// Token: 0x040000E8 RID: 232
		private string seedString;

		// Token: 0x040000E9 RID: 233
		private string freqStr;

		// Token: 0x040000EA RID: 234
		private string lacStr;

		// Token: 0x040000EB RID: 235
		private string octStr;

		// Token: 0x040000EC RID: 236
		private string persStr;

		// Token: 0x040000ED RID: 237
		private double freq;

		// Token: 0x040000EE RID: 238
		private double lac;

		// Token: 0x040000EF RID: 239
		private double pers;

		// Token: 0x040000F0 RID: 240
		private bool freqFine;

		// Token: 0x040000F1 RID: 241
		private bool lacFine;

		// Token: 0x040000F2 RID: 242
		private bool persFine;

		// Token: 0x040000F3 RID: 243
		private float octSliderPos;

		// Token: 0x040000F4 RID: 244
		private int oct;

		// Token: 0x040000F5 RID: 245
		private float seedOffsetSlider;

		// Token: 0x040000F6 RID: 246
		private bool toggleSliders;

		// Token: 0x040000F7 RID: 247
		private bool toggleUpdate = true;

		// Token: 0x040000F8 RID: 248
		private PlanetLayer planetLayer;

		// Token: 0x040000F9 RID: 249
		public Perlin noise;

		// Token: 0x040000FA RID: 250
		public float cutoff;

		// Token: 0x040000FB RID: 251
		public bool cutoffEnabled = true;

		// Token: 0x040000FC RID: 252
		public float alpha = 1f;

		// Token: 0x040000FD RID: 253
		private const string TooltipFreq = "Scale of the noise-map.\n\nLarger values zoom the noise out.\nSmaller values zoom it in.";

		// Token: 0x040000FE RID: 254
		private const string TooltipOct = "How many octaves contribute to the final value.\n\nHigher octaves contribute according to lacunarity and persistence values.";

		// Token: 0x040000FF RID: 255
		private const string TooltipLac = "Muliplies frequency at each octave (adjusts frequency)\n\n1: octaves have same frequency.\n< 1: successive octaves are smoother.\n> 1: successive octaves are rougher.\na";

		// Token: 0x04000100 RID: 256
		private const string TooltipPers = "How much each octave contributes to the final value (adjusts amplitude)\n\n1: octaves contribute equally.\n< 1: successive octaves contribute less.\n> 1: successive octaves contribute more.\na";
	}
}
