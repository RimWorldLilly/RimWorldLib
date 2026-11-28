using System;
using System.Text;
using UnityEngine;

namespace Verse.ProfilerUI
{
	// Token: 0x02000798 RID: 1944
	public sealed class ProfilerRowData
	{
		// Token: 0x06003DEB RID: 15851 RVA: 0x0018A3B0 File Offset: 0x001885B0
		public void Reset()
		{
			this.BarFraction = 0f;
			this.PercentSign = (this.TotalIsBytes = (this.HasSelf = (this.HasDelta = (this.DeltaIsBytes = (this.CallsGrouped = (this.Dimmed = (this.HasChildren = (this.Expanded = false))))))));
			this.TotalMicroseconds = (this.SelfMicroseconds = 0.0);
			this.TotalBytes = (this.DeltaBytes = (this.Calls = (this.AllocBytes = 0L)));
			this.DeltaMs = 0f;
			this.DeltaSeverity = (this.Depth = 0);
			this.Swatch = default(Color);
			this.Label = (this.Detail = null);
			this.PercentText = null;
			this.fittedLabel = null;
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x0018A49C File Offset: 0x0018869C
		public void EnsureDisplayText()
		{
			if (this.PercentText != null)
			{
				return;
			}
			StringBuilder scratch = ProfilerGui.Scratch;
			this.PercentText = ProfilerRowData.Format(scratch, new Action<StringBuilder>(this.AppendPercent));
			this.TotalText = ProfilerRowData.Format(scratch, new Action<StringBuilder>(this.AppendTotal));
			this.SelfText = ProfilerRowData.Format(scratch, new Action<StringBuilder>(this.AppendSelf));
			this.DeltaText = ProfilerRowData.Format(scratch, new Action<StringBuilder>(this.AppendDelta));
			this.CallsText = ProfilerRowData.Format(scratch, delegate(StringBuilder b)
			{
				this.AppendCalls(b, this.CallsGrouped);
			});
			scratch.Clear();
			ProfilerTreeModel.AppendAllocBytes(scratch, this.AllocBytes, " ");
			this.AllocText = scratch.ToString();
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x0018A554 File Offset: 0x00188754
		private static string Format(StringBuilder scratch, Action<StringBuilder> append)
		{
			scratch.Clear();
			append(scratch);
			return scratch.ToString();
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x0018A56C File Offset: 0x0018876C
		public string FittedLabel(float width)
		{
			if (this.fittedLabel == null || Mathf.Abs(this.fittedLabelWidth - width) > 0.5f)
			{
				this.fittedLabelWidth = width;
				this.fittedLabel = ProfilerGui.Ellipsize(this.Label ?? "", width);
			}
			return this.fittedLabel;
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x0018A5C0 File Offset: 0x001887C0
		public void AppendPercent(StringBuilder builder)
		{
			builder.Append((this.BarFraction * 100f).ToString("0.0"));
			if (this.PercentSign)
			{
				builder.Append('%');
			}
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x0018A5FE File Offset: 0x001887FE
		public void AppendTotal(StringBuilder builder)
		{
			if (this.TotalIsBytes)
			{
				ProfilerTreeModel.AppendAllocBytes(builder, this.TotalBytes, " ");
				return;
			}
			ProfilerTreeModel.AppendMs(builder, (float)(this.TotalMicroseconds / 1000.0), "0.000", " ");
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x0018A63B File Offset: 0x0018883B
		public void AppendSelf(StringBuilder builder)
		{
			if (this.HasSelf)
			{
				ProfilerTreeModel.AppendMs(builder, (float)(this.SelfMicroseconds / 1000.0), "0.000", " ");
			}
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x0018A668 File Offset: 0x00188868
		public void AppendDelta(StringBuilder builder)
		{
			if (!this.HasDelta)
			{
				return;
			}
			if (this.DeltaIsBytes)
			{
				if (this.DeltaBytes == 0L)
				{
					builder.Append('—');
					return;
				}
				builder.Append((this.DeltaBytes > 0L) ? '+' : '-');
				ProfilerTreeModel.AppendAllocBytes(builder, Math.Abs(this.DeltaBytes), " ");
				return;
			}
			else
			{
				if (float.IsNaN(this.DeltaMs))
				{
					builder.Append('—');
					return;
				}
				builder.Append((this.DeltaMs >= 0f) ? "+" : "").Append(this.DeltaMs.ToString("0.0"));
				return;
			}
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0018A718 File Offset: 0x00188918
		public void AppendCalls(StringBuilder builder, bool grouped)
		{
			if (this.Calls <= 0L)
			{
				return;
			}
			if (grouped)
			{
				builder.Append(this.Calls.ToString("n0"));
				return;
			}
			builder.Append(this.Calls);
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x0018A74D File Offset: 0x0018894D
		public void AppendCopyLine(StringBuilder builder)
		{
			this.AppendCopyLine(builder, this.Depth);
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x0018A75C File Offset: 0x0018895C
		public void AppendCopyLine(StringBuilder builder, int depth)
		{
			this.AppendPercent(builder);
			builder.Append('\t');
			this.AppendTotal(builder);
			if (this.TotalIsBytes && this.HasDelta)
			{
				builder.Append('\t');
				this.AppendDelta(builder);
			}
			if (!this.TotalIsBytes)
			{
				builder.Append('\t');
				if (this.HasSelf)
				{
					this.AppendSelf(builder);
				}
				else
				{
					builder.Append('—');
				}
			}
			builder.Append('\t');
			this.AppendCalls(builder, false);
			if (!this.TotalIsBytes)
			{
				builder.Append('\t');
				ProfilerTreeModel.AppendAllocBytes(builder, this.AllocBytes, " ");
			}
			builder.Append('\t');
			for (int i = 0; i < depth; i++)
			{
				builder.Append("  ");
			}
			builder.Append(this.Label);
			if (this.Detail != null)
			{
				builder.Append('\t').Append(this.Detail);
			}
		}

		// Token: 0x040028AE RID: 10414
		public float BarFraction;

		// Token: 0x040028AF RID: 10415
		public bool PercentSign;

		// Token: 0x040028B0 RID: 10416
		public bool TotalIsBytes;

		// Token: 0x040028B1 RID: 10417
		public double TotalMicroseconds;

		// Token: 0x040028B2 RID: 10418
		public long TotalBytes;

		// Token: 0x040028B3 RID: 10419
		public bool HasSelf;

		// Token: 0x040028B4 RID: 10420
		public double SelfMicroseconds;

		// Token: 0x040028B5 RID: 10421
		public bool HasDelta;

		// Token: 0x040028B6 RID: 10422
		public float DeltaMs;

		// Token: 0x040028B7 RID: 10423
		public bool DeltaIsBytes;

		// Token: 0x040028B8 RID: 10424
		public long DeltaBytes;

		// Token: 0x040028B9 RID: 10425
		public int DeltaSeverity;

		// Token: 0x040028BA RID: 10426
		public long Calls;

		// Token: 0x040028BB RID: 10427
		public bool CallsGrouped;

		// Token: 0x040028BC RID: 10428
		public long AllocBytes;

		// Token: 0x040028BD RID: 10429
		public Color Swatch;

		// Token: 0x040028BE RID: 10430
		public string Label;

		// Token: 0x040028BF RID: 10431
		public string Detail;

		// Token: 0x040028C0 RID: 10432
		public bool Dimmed;

		// Token: 0x040028C1 RID: 10433
		public int Depth;

		// Token: 0x040028C2 RID: 10434
		public bool HasChildren;

		// Token: 0x040028C3 RID: 10435
		public bool Expanded;

		// Token: 0x040028C4 RID: 10436
		public string PercentText;

		// Token: 0x040028C5 RID: 10437
		public string TotalText;

		// Token: 0x040028C6 RID: 10438
		public string SelfText;

		// Token: 0x040028C7 RID: 10439
		public string DeltaText;

		// Token: 0x040028C8 RID: 10440
		public string CallsText;

		// Token: 0x040028C9 RID: 10441
		public string AllocText;

		// Token: 0x040028CA RID: 10442
		private string fittedLabel;

		// Token: 0x040028CB RID: 10443
		private float fittedLabelWidth;
	}
}
