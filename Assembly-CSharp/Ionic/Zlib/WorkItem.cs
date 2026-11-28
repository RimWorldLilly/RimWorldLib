using System;

namespace Ionic.Zlib
{
	// Token: 0x02002418 RID: 9240
	internal class WorkItem
	{
		// Token: 0x0600E7BD RID: 59325 RVA: 0x005640BC File Offset: 0x005622BC
		public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
		{
			this.buffer = new byte[size];
			int num = size + (size / 32768 + 1) * 5 * 2;
			this.compressed = new byte[num];
			this.compressor = new ZlibCodec();
			this.compressor.InitializeDeflate(compressLevel, false);
			this.compressor.OutputBuffer = this.compressed;
			this.compressor.InputBuffer = this.buffer;
			this.index = ix;
		}

		// Token: 0x04009343 RID: 37699
		public byte[] buffer;

		// Token: 0x04009344 RID: 37700
		public byte[] compressed;

		// Token: 0x04009345 RID: 37701
		public int crc;

		// Token: 0x04009346 RID: 37702
		public int index;

		// Token: 0x04009347 RID: 37703
		public int ordinal;

		// Token: 0x04009348 RID: 37704
		public int inputBytesAvailable;

		// Token: 0x04009349 RID: 37705
		public int compressedBytesAvailable;

		// Token: 0x0400934A RID: 37706
		public ZlibCodec compressor;
	}
}
