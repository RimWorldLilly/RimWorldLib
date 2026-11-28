using System;
using System.Diagnostics;
using UnityEngine;

namespace Verse.ProfilerUI
{
	// Token: 0x020007A5 RID: 1957
	public sealed class ProfilerViewAnimation
	{
		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06003EE6 RID: 16102 RVA: 0x00190CF6 File Offset: 0x0018EEF6
		public bool Active
		{
			get
			{
				return this.progress < 1f;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06003EE7 RID: 16103 RVA: 0x00190D05 File Offset: 0x0018EF05
		public bool NeedsRebuild
		{
			get
			{
				return this.Active || this.contentAnimating;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06003EE8 RID: 16104 RVA: 0x00190D17 File Offset: 0x0018EF17
		public double TargetStart
		{
			get
			{
				return this.toStart;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06003EE9 RID: 16105 RVA: 0x00190D1F File Offset: 0x0018EF1F
		public double TargetSpan
		{
			get
			{
				return this.toEnd - this.toStart;
			}
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x00190D2E File Offset: 0x0018EF2E
		public void Start(double currentStart, double currentSpan, double targetStart, double targetSpan)
		{
			this.fromStart = currentStart;
			this.fromEnd = currentStart + currentSpan;
			this.toStart = targetStart;
			this.toEnd = targetStart + targetSpan;
			this.progress = 0f;
			this.timestamp = Stopwatch.GetTimestamp();
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x00190D67 File Offset: 0x0018EF67
		public void Cancel()
		{
			this.progress = 1f;
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x00190D74 File Offset: 0x0018EF74
		public void SetContentAnimating(bool animating)
		{
			this.contentAnimating = animating;
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x00190D7D File Offset: 0x0018EF7D
		public void ShiftBy(double delta)
		{
			this.fromStart += delta;
			this.fromEnd += delta;
			this.toStart += delta;
			this.toEnd += delta;
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x00190DB8 File Offset: 0x0018EFB8
		public bool Advance(ref double start, ref double span)
		{
			if (this.progress >= 1f)
			{
				return false;
			}
			long num = Stopwatch.GetTimestamp();
			float num2 = (float)((double)(num - this.timestamp) / (double)Stopwatch.Frequency);
			this.timestamp = num;
			this.progress = Mathf.Min(1f, this.progress + num2 / 0.25f);
			float t = Mathf.Sqrt(Mathf.Sin(this.progress * 3.1415927f * 0.5f));
			start = ProfilerViewAnimation.Lerp(this.fromStart, this.toStart, t);
			span = ProfilerViewAnimation.Lerp(this.fromEnd, this.toEnd, t) - start;
			return this.progress < 1f;
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x00190E66 File Offset: 0x0018F066
		private static double Lerp(double a, double b, float t)
		{
			return a + (b - a) * (double)t;
		}

		// Token: 0x04002983 RID: 10627
		private const float DurationSeconds = 0.25f;

		// Token: 0x04002984 RID: 10628
		private double fromStart;

		// Token: 0x04002985 RID: 10629
		private double fromEnd;

		// Token: 0x04002986 RID: 10630
		private double toStart;

		// Token: 0x04002987 RID: 10631
		private double toEnd;

		// Token: 0x04002988 RID: 10632
		private float progress = 1f;

		// Token: 0x04002989 RID: 10633
		private long timestamp;

		// Token: 0x0400298A RID: 10634
		private bool contentAnimating;
	}
}
