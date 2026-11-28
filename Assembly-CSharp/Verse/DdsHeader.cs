using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Verse
{
	// Token: 0x02000335 RID: 821
	public struct DdsHeader
	{
		// Token: 0x040014BB RID: 5307
		public const uint RequiredMagic = 542327876U;

		// Token: 0x040014BC RID: 5308
		public const uint RequiredSize = 124U;

		// Token: 0x040014BD RID: 5309
		public uint Magic;

		// Token: 0x040014BE RID: 5310
		public uint Size;

		// Token: 0x040014BF RID: 5311
		public DdsHeaderFlags Flags;

		// Token: 0x040014C0 RID: 5312
		public uint Height;

		// Token: 0x040014C1 RID: 5313
		public uint Width;

		// Token: 0x040014C2 RID: 5314
		public uint PitchOrLinearSize;

		// Token: 0x040014C3 RID: 5315
		public uint Depth;

		// Token: 0x040014C4 RID: 5316
		public uint MipMapCount;

		// Token: 0x040014C5 RID: 5317
		[FixedBuffer(typeof(uint), 11)]
		public DdsHeader.<Reserved>e__FixedBuffer Reserved;

		// Token: 0x040014C6 RID: 5318
		public DdsPixelFormat PixelFormat;

		// Token: 0x040014C7 RID: 5319
		public uint Caps;

		// Token: 0x040014C8 RID: 5320
		public uint Caps2;

		// Token: 0x040014C9 RID: 5321
		public uint Caps3;

		// Token: 0x040014CA RID: 5322
		public uint Caps4;

		// Token: 0x040014CB RID: 5323
		public uint Reserved2;

		// Token: 0x02002667 RID: 9831
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 44)]
		public struct <Reserved>e__FixedBuffer
		{
			// Token: 0x04009D54 RID: 40276
			public uint FixedElementField;
		}
	}
}
