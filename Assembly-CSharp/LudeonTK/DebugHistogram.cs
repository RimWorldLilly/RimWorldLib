using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000037 RID: 55
	public class DebugHistogram
	{
		// Token: 0x0600017A RID: 378 RVA: 0x0000B0A6 File Offset: 0x000092A6
		public DebugHistogram(float[] buckets)
		{
			this.buckets = buckets.Concat(float.PositiveInfinity).ToArray<float>();
			this.counts = new int[this.buckets.Length];
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000B0D8 File Offset: 0x000092D8
		public void Add(float val)
		{
			for (int i = 0; i < this.buckets.Length; i++)
			{
				if (this.buckets[i] > val)
				{
					this.counts[i]++;
					return;
				}
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000B118 File Offset: 0x00009318
		public void Display()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Display(stringBuilder);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000B140 File Offset: 0x00009340
		public void Display(StringBuilder sb)
		{
			int num = Mathf.Max(this.counts.Max(), 1);
			int num2 = this.counts.Aggregate((int a, int b) => a + b);
			for (int i = 0; i < this.buckets.Length; i++)
			{
				sb.AppendLine(string.Format("{0}    {1}: {2} ({3:F2}%)", new object[]
				{
					new string('#', this.counts[i] * 40 / num),
					this.buckets[i],
					this.counts[i],
					(double)this.counts[i] * 100.0 / (double)num2
				}));
			}
		}

		// Token: 0x040000A5 RID: 165
		private float[] buckets;

		// Token: 0x040000A6 RID: 166
		private int[] counts;
	}
}
