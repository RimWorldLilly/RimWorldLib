using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000062 RID: 98
	[BurstCompile]
	public struct ColorInt : IEquatable<ColorInt>
	{
		// Token: 0x06000324 RID: 804 RVA: 0x000167CC File Offset: 0x000149CC
		public ColorInt(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000167EE File Offset: 0x000149EE
		public ColorInt(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0001680D File Offset: 0x00014A0D
		public ColorInt(Color32 col)
		{
			this.r = (int)col.r;
			this.g = (int)col.g;
			this.b = (int)col.b;
			this.a = (int)col.a;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00016840 File Offset: 0x00014A40
		public ColorInt(Color color)
		{
			this.r = (int)ColorInt.FloatToByte(color.r);
			this.g = (int)ColorInt.FloatToByte(color.g);
			this.b = (int)ColorInt.FloatToByte(color.b);
			this.a = (int)ColorInt.FloatToByte(color.a);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00016891 File Offset: 0x00014A91
		private static byte FloatToByte(float value)
		{
			if (value >= 1f)
			{
				return byte.MaxValue;
			}
			if (value <= 0f)
			{
				return 0;
			}
			return (byte)Mathf.Floor(value * 256f);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x000168B8 File Offset: 0x00014AB8
		public static ColorInt operator +(ColorInt colA, ColorInt colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x000168F3 File Offset: 0x00014AF3
		public static ColorInt operator +(ColorInt colA, Color32 colB)
		{
			return new ColorInt(colA.r + (int)colB.r, colA.g + (int)colB.g, colA.b + (int)colB.b, colA.a + (int)colB.a);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0001692E File Offset: 0x00014B2E
		public static ColorInt operator -(ColorInt a, ColorInt b)
		{
			return new ColorInt(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00016969 File Offset: 0x00014B69
		public static ColorInt operator *(ColorInt a, int b)
		{
			return new ColorInt(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00016990 File Offset: 0x00014B90
		public static ColorInt operator *(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r * b), (int)((float)a.g * b), (int)((float)a.b * b), (int)((float)a.a * b));
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000169BF File Offset: 0x00014BBF
		public static ColorInt operator /(ColorInt a, int b)
		{
			return new ColorInt(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000169E6 File Offset: 0x00014BE6
		public static ColorInt operator /(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r / b), (int)((float)a.g / b), (int)((float)a.b / b), (int)((float)a.a / b));
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00016A15 File Offset: 0x00014C15
		public static bool operator ==(ColorInt a, ColorInt b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00016A51 File Offset: 0x00014C51
		public static bool operator !=(ColorInt a, ColorInt b)
		{
			return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00016A90 File Offset: 0x00014C90
		public override bool Equals(object o)
		{
			if (o is ColorInt)
			{
				ColorInt other = (ColorInt)o;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00016AB7 File Offset: 0x00014CB7
		public bool Equals(ColorInt other)
		{
			return this == other;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00016AC8 File Offset: 0x00014CC8
		public override int GetHashCode()
		{
			return this.r + this.g * 256 + this.b * 256 * 256 + this.a * 256 * 256 * 256;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00016B14 File Offset: 0x00014D14
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 4);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.r);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.g);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.b);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.a);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00016B8C File Offset: 0x00014D8C
		public void ClampToNonNegative()
		{
			if (this.r < 0)
			{
				this.r = 0;
			}
			if (this.g < 0)
			{
				this.g = 0;
			}
			if (this.b < 0)
			{
				this.b = 0;
			}
			if (this.a < 0)
			{
				this.a = 0;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00016BDC File Offset: 0x00014DDC
		public Color ToColor
		{
			get
			{
				return new Color
				{
					r = (float)this.r / 255f,
					g = (float)this.g / 255f,
					b = (float)this.b / 255f,
					a = (float)this.a / 255f
				};
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00016C44 File Offset: 0x00014E44
		public Color32 ProjectToColor32()
		{
			Color32 result = default(Color32);
			if (this.a > 255)
			{
				result.a = byte.MaxValue;
			}
			else
			{
				result.a = (byte)this.a;
			}
			int num = this.r;
			if (this.g > num)
			{
				num = this.g;
			}
			if (this.b > num)
			{
				num = this.b;
			}
			if (num > 255)
			{
				result.r = (byte)(this.r * 255 / num);
				result.g = (byte)(this.g * 255 / num);
				result.b = (byte)(this.b * 255 / num);
			}
			else
			{
				result.r = (byte)this.r;
				result.g = (byte)this.g;
				result.b = (byte)this.b;
			}
			return result;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00016D20 File Offset: 0x00014F20
		[BurstCompile]
		public void ProjectToColor32Fast(out Color32 outColor)
		{
			outColor = default(Color32);
			if (this.a > 255)
			{
				outColor.a = byte.MaxValue;
			}
			else
			{
				outColor.a = (byte)this.a;
			}
			int num = this.r;
			if (this.g > num)
			{
				num = this.g;
			}
			if (this.b > num)
			{
				num = this.b;
			}
			if (num > 255)
			{
				outColor.r = (byte)(this.r * 255 / num);
				outColor.g = (byte)(this.g * 255 / num);
				outColor.b = (byte)(this.b * 255 / num);
				return;
			}
			outColor.r = (byte)this.r;
			outColor.g = (byte)this.g;
			outColor.b = (byte)this.b;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00016DF4 File Offset: 0x00014FF4
		public void SetHueSaturation(float hue, float sat)
		{
			float v = (float)Mathf.Max(new int[]
			{
				this.r,
				this.g,
				this.b
			}) / 255f;
			ColorInt colorInt = ColorInt.FromHdrColor(Color.HSVToRGB(hue, sat, v, true), null);
			this.r = colorInt.r;
			this.g = colorInt.g;
			this.b = colorInt.b;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00016E6C File Offset: 0x0001506C
		public static ColorInt FromHdrColor(Color color, float? alphaOverride = null)
		{
			return new ColorInt
			{
				r = Mathf.RoundToInt(color.r * 255f),
				g = Mathf.RoundToInt(color.g * 255f),
				b = Mathf.RoundToInt(color.b * 255f),
				a = Mathf.RoundToInt((alphaOverride ?? color.a) * 255f)
			};
		}

		// Token: 0x04000171 RID: 369
		public int r;

		// Token: 0x04000172 RID: 370
		public int g;

		// Token: 0x04000173 RID: 371
		public int b;

		// Token: 0x04000174 RID: 372
		public int a;
	}
}
