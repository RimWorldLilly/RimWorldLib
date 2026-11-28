using System;

namespace RimWorld
{
	// Token: 0x02001CF7 RID: 7415
	public struct TransferableCountToTransferStoppingPoint
	{
		// Token: 0x0600BF4B RID: 48971 RVA: 0x004549B9 File Offset: 0x00452BB9
		public TransferableCountToTransferStoppingPoint(int threshold, string leftLabel, string rightLabel)
		{
			this.threshold = threshold;
			this.leftLabel = leftLabel;
			this.rightLabel = rightLabel;
		}

		// Token: 0x04006DE1 RID: 28129
		public int threshold;

		// Token: 0x04006DE2 RID: 28130
		public string leftLabel;

		// Token: 0x04006DE3 RID: 28131
		public string rightLabel;
	}
}
