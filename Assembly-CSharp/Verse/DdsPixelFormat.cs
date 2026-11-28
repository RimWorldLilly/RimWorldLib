using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000337 RID: 823
	public struct DdsPixelFormat
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001935 RID: 6453 RVA: 0x000972EF File Offset: 0x000954EF
		public bool IsRgb888
		{
			get
			{
				return this.RBitMask == 255U && this.GBitMask == 65280U && this.BBitMask == 16711680U;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001936 RID: 6454 RVA: 0x0009731A File Offset: 0x0009551A
		public bool IsBgr888
		{
			get
			{
				return this.RBitMask == 16711680U && this.GBitMask == 65280U && this.BBitMask == 255U;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001937 RID: 6455 RVA: 0x00097345 File Offset: 0x00095545
		public bool IsRgb565
		{
			get
			{
				return this.RBitMask == 63488U && this.GBitMask == 2016U && this.BBitMask == 31U;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001938 RID: 6456 RVA: 0x0009736D File Offset: 0x0009556D
		public bool IsArgb4444
		{
			get
			{
				return this.ABitMask == 61440U && this.RBitMask == 3840U && this.GBitMask == 240U && this.BBitMask == 15U;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001939 RID: 6457 RVA: 0x000973A2 File Offset: 0x000955A2
		public bool IsRgba4444
		{
			get
			{
				return this.RBitMask == 61440U && this.GBitMask == 3840U && this.BBitMask == 240U && this.ABitMask == 15U;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x0600193A RID: 6458 RVA: 0x000973D7 File Offset: 0x000955D7
		public bool IsCompressed
		{
			get
			{
				return (this.Flags & DdsPixelFormatFlags.FourCC) != DdsPixelFormatFlags.None && this.FourCC > 0U;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x000973EE File Offset: 0x000955EE
		public bool IsDxt1
		{
			get
			{
				return this.IsCompressed && this.FourCC == 827611204U;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600193C RID: 6460 RVA: 0x00097407 File Offset: 0x00095607
		public bool IsDxt5
		{
			get
			{
				return this.IsCompressed && this.FourCC == 894720068U;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x00097420 File Offset: 0x00095620
		public bool IsBc7
		{
			get
			{
				return this.IsCompressed && this.FourCC == 808540228U;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600193E RID: 6462 RVA: 0x00097439 File Offset: 0x00095639
		public bool IsUnsupportedCompressedFormat
		{
			get
			{
				return this.IsCompressed && !this.IsDxt1 && !this.IsDxt5 && !this.IsBc7;
			}
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x00097460 File Offset: 0x00095660
		public TextureFormat ToTextureFormat()
		{
			if (this.IsCompressed)
			{
				if (this.IsDxt1)
				{
					return TextureFormat.DXT1;
				}
				if (this.IsDxt5)
				{
					return TextureFormat.DXT5;
				}
				if (this.IsBc7)
				{
					return TextureFormat.BC7;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported compressed DDS texture format: ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(this.FourCC, "X8");
				throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else if ((this.Flags & DdsPixelFormatFlags.RGB) != DdsPixelFormatFlags.None)
			{
				bool flag = (this.Flags & DdsPixelFormatFlags.AlphaPixels) > DdsPixelFormatFlags.None;
				if (this.IsRgb888 || this.IsBgr888)
				{
					if (!flag)
					{
						return TextureFormat.RGB24;
					}
					return TextureFormat.RGBA32;
				}
				else
				{
					if (this.IsRgb565)
					{
						return TextureFormat.RGB565;
					}
					if (!this.IsRgba4444)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(47, 2);
						defaultInterpolatedStringHandler2.AppendLiteral("Unsupported DDS RGB pixel format: ");
						defaultInterpolatedStringHandler2.AppendFormatted<DdsPixelFormatFlags>(this.Flags);
						defaultInterpolatedStringHandler2.AppendLiteral(" with FourCC ");
						defaultInterpolatedStringHandler2.AppendFormatted<uint>(this.FourCC, "X8");
						throw new NotSupportedException(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
					if (!flag)
					{
						throw new NotSupportedException("RGBA4444 format without alpha is not supported.");
					}
					return TextureFormat.ARGB4444;
				}
			}
			else
			{
				if ((this.Flags & DdsPixelFormatFlags.Luminance) != DdsPixelFormatFlags.None || (this.Flags & DdsPixelFormatFlags.Alpha) != DdsPixelFormatFlags.None)
				{
					return TextureFormat.Alpha8;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(43, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("Unsupported DDS pixel format: ");
				defaultInterpolatedStringHandler3.AppendFormatted<DdsPixelFormatFlags>(this.Flags);
				defaultInterpolatedStringHandler3.AppendLiteral(" with FourCC ");
				defaultInterpolatedStringHandler3.AppendFormatted<uint>(this.FourCC, "X8");
				throw new NotSupportedException(defaultInterpolatedStringHandler3.ToStringAndClear());
			}
		}

		// Token: 0x040014D5 RID: 5333
		public const uint RequiredSize = 32U;

		// Token: 0x040014D6 RID: 5334
		public uint Size;

		// Token: 0x040014D7 RID: 5335
		public DdsPixelFormatFlags Flags;

		// Token: 0x040014D8 RID: 5336
		public uint FourCC;

		// Token: 0x040014D9 RID: 5337
		public uint RGBBitCount;

		// Token: 0x040014DA RID: 5338
		public uint RBitMask;

		// Token: 0x040014DB RID: 5339
		public uint GBitMask;

		// Token: 0x040014DC RID: 5340
		public uint BBitMask;

		// Token: 0x040014DD RID: 5341
		public uint ABitMask;
	}
}
