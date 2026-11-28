using System;

namespace Verse
{
	// Token: 0x0200072B RID: 1835
	public class SimpleMovingAverage
	{
		// Token: 0x060039F0 RID: 14832 RVA: 0x00171B08 File Offset: 0x0016FD08
		public SimpleMovingAverage(int numValues)
		{
			this.values = new float[numValues];
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x00171B1C File Offset: 0x0016FD1C
		public void AddValue(float value)
		{
			this.values[this.lastValueIndex] = value;
			this.lastValueIndex = (this.lastValueIndex + 1) % this.values.Length;
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x00171B44 File Offset: 0x0016FD44
		public float GetAverage()
		{
			float num = 0f;
			float[] array = this.values;
			int num2 = (array[array.Length - 1] == 0f) ? this.lastValueIndex : this.values.Length;
			for (int i = 0; i < num2; i++)
			{
				num += this.values[i];
			}
			return num / (float)num2;
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x00171B96 File Offset: 0x0016FD96
		public void Reset()
		{
			this.lastValueIndex = 0;
			Array.Clear(this.values, 0, this.values.Length);
		}

		// Token: 0x04002647 RID: 9799
		private readonly float[] values;

		// Token: 0x04002648 RID: 9800
		private int lastValueIndex;
	}
}
