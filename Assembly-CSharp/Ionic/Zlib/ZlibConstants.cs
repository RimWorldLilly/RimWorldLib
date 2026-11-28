using System;

namespace Ionic.Zlib
{
	// Token: 0x02002427 RID: 9255
	public static class ZlibConstants
	{
		// Token: 0x040093CB RID: 37835
		public const int WindowBitsMax = 15;

		// Token: 0x040093CC RID: 37836
		public const int WindowBitsDefault = 15;

		// Token: 0x040093CD RID: 37837
		public const int Z_OK = 0;

		// Token: 0x040093CE RID: 37838
		public const int Z_STREAM_END = 1;

		// Token: 0x040093CF RID: 37839
		public const int Z_NEED_DICT = 2;

		// Token: 0x040093D0 RID: 37840
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x040093D1 RID: 37841
		public const int Z_DATA_ERROR = -3;

		// Token: 0x040093D2 RID: 37842
		public const int Z_BUF_ERROR = -5;

		// Token: 0x040093D3 RID: 37843
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x040093D4 RID: 37844
		public const int WorkingBufferSizeMin = 1024;
	}
}
