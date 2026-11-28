using System;

namespace Verse
{
	// Token: 0x020000D3 RID: 211
	public class MoteCounter
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x0002F6C9 File Offset: 0x0002D8C9
		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0002F6D1 File Offset: 0x0002D8D1
		public float Saturation
		{
			get
			{
				return (float)this.moteCount / 250f;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x0002F6E0 File Offset: 0x0002D8E0
		public bool Saturated
		{
			get
			{
				return this.Saturation > 1f;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x0002F6EF File Offset: 0x0002D8EF
		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.8f;
			}
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0002F6FE File Offset: 0x0002D8FE
		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0002F70E File Offset: 0x0002D90E
		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}

		// Token: 0x040003AF RID: 943
		private int moteCount;

		// Token: 0x040003B0 RID: 944
		private const int SaturatedCount = 250;
	}
}
