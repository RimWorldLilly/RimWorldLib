using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x020007F1 RID: 2033
	public class SoundSizeAggregator
	{
		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x0600405A RID: 16474 RVA: 0x00199F8C File Offset: 0x0019818C
		public float AggregateSize
		{
			get
			{
				if (this.reporters.Count == 0)
				{
					return this.testSize;
				}
				float num = 0f;
				foreach (ISizeReporter sizeReporter in this.reporters)
				{
					num += sizeReporter.CurrentSize();
				}
				return num;
			}
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x00199FFC File Offset: 0x001981FC
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0019A033 File Offset: 0x00198233
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x0019A041 File Offset: 0x00198241
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}

		// Token: 0x04002ABB RID: 10939
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		// Token: 0x04002ABC RID: 10940
		private float testSize;
	}
}
