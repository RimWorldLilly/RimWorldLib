using System;

namespace Ionic.Zlib
{
	// Token: 0x02002421 RID: 9249
	internal static class InternalConstants
	{
		// Token: 0x04009390 RID: 37776
		internal static readonly int MAX_BITS = 15;

		// Token: 0x04009391 RID: 37777
		internal static readonly int BL_CODES = 19;

		// Token: 0x04009392 RID: 37778
		internal static readonly int D_CODES = 30;

		// Token: 0x04009393 RID: 37779
		internal static readonly int LITERALS = 256;

		// Token: 0x04009394 RID: 37780
		internal static readonly int LENGTH_CODES = 29;

		// Token: 0x04009395 RID: 37781
		internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;

		// Token: 0x04009396 RID: 37782
		internal static readonly int MAX_BL_BITS = 7;

		// Token: 0x04009397 RID: 37783
		internal static readonly int REP_3_6 = 16;

		// Token: 0x04009398 RID: 37784
		internal static readonly int REPZ_3_10 = 17;

		// Token: 0x04009399 RID: 37785
		internal static readonly int REPZ_11_138 = 18;
	}
}
