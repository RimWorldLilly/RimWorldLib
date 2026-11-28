using System;

namespace Verse
{
	// Token: 0x020006D8 RID: 1752
	public class ExponentialMovingAverage
	{
		// Token: 0x0600372F RID: 14127 RVA: 0x00161EA2 File Offset: 0x001600A2
		public ExponentialMovingAverage(float alpha)
		{
			this.alpha = alpha;
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x00161EB1 File Offset: 0x001600B1
		public void AddValue(float value)
		{
			if (!this.initialized)
			{
				this.average = value;
				this.initialized = true;
				return;
			}
			this.average = this.alpha * value + (1f - this.alpha) * this.average;
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x00161EEC File Offset: 0x001600EC
		public float GetAverage()
		{
			return this.average;
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x00161EF4 File Offset: 0x001600F4
		public void Reset()
		{
			this.initialized = false;
			this.average = 0f;
		}

		// Token: 0x04002443 RID: 9283
		private readonly float alpha;

		// Token: 0x04002444 RID: 9284
		private float average;

		// Token: 0x04002445 RID: 9285
		private bool initialized;
	}
}
