using System;
using UnityEngine;

namespace Verse.ProfilerUI
{
	// Token: 0x02000793 RID: 1939
	internal sealed class ProfilerPanZoomView
	{
		// Token: 0x06003DB3 RID: 15795 RVA: 0x001894A4 File Offset: 0x001876A4
		public ProfilerPanZoomView(double minSpan)
		{
			this.MinSpan = minSpan;
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x001894DC File Offset: 0x001876DC
		// (set) Token: 0x06003DB5 RID: 15797 RVA: 0x001894E4 File Offset: 0x001876E4
		public double Start { get; set; }

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06003DB6 RID: 15798 RVA: 0x001894ED File Offset: 0x001876ED
		// (set) Token: 0x06003DB7 RID: 15799 RVA: 0x001894F5 File Offset: 0x001876F5
		public double Span { get; set; } = 1.0;

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x001894FE File Offset: 0x001876FE
		// (set) Token: 0x06003DB9 RID: 15801 RVA: 0x00189506 File Offset: 0x00187706
		public float TopLane { get; set; }

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x0018950F File Offset: 0x0018770F
		// (set) Token: 0x06003DBB RID: 15803 RVA: 0x00189517 File Offset: 0x00187717
		public double Extent { get; set; } = 1.0;

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06003DBC RID: 15804 RVA: 0x00189520 File Offset: 0x00187720
		// (set) Token: 0x06003DBD RID: 15805 RVA: 0x00189528 File Offset: 0x00187728
		public bool UserView { get; set; }

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06003DBE RID: 15806 RVA: 0x00189531 File Offset: 0x00187731
		// (set) Token: 0x06003DBF RID: 15807 RVA: 0x00189539 File Offset: 0x00187739
		public bool Panning { get; private set; }

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x00189542 File Offset: 0x00187742
		public double MinSpan { get; }

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06003DC1 RID: 15809 RVA: 0x0018954A File Offset: 0x0018774A
		private double MaxSpan
		{
			get
			{
				return Math.Max(this.MinSpan, this.Extent * 4.0);
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x00189567 File Offset: 0x00187767
		public ProfilerViewAnimation Animation { get; } = new ProfilerViewAnimation();

		// Token: 0x06003DC3 RID: 15811 RVA: 0x0018956F File Offset: 0x0018776F
		public float ToX(double value, float plotX0, float plotWidth)
		{
			return plotX0 + (float)((value - this.Start) / this.Span) * plotWidth;
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x00189585 File Offset: 0x00187785
		public double ToValue(float x, float plotX0, float plotWidth)
		{
			return this.Start + (double)(x - plotX0) / (double)plotWidth * this.Span;
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x0018959C File Offset: 0x0018779C
		private double ClampStart(double start, double span)
		{
			double num = span * 0.5;
			return Math.Clamp(start, -num, Math.Max(-num, this.Extent - span + num));
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x001895CE File Offset: 0x001877CE
		public void ClampRange()
		{
			this.Span = Math.Clamp(this.Span, this.MinSpan, this.MaxSpan);
			this.Start = this.ClampStart(this.Start, this.Span);
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x00189605 File Offset: 0x00187805
		public void Clamp(float visibleLanes, float totalLanes)
		{
			this.ClampRange();
			this.TopLane = Mathf.Clamp(this.TopLane, 0f, Mathf.Max(0f, totalLanes - visibleLanes));
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x00189630 File Offset: 0x00187830
		public void SetTo(double start, double span)
		{
			this.CancelAnimation();
			this.Span = Math.Clamp(span, this.MinSpan, this.MaxSpan);
			this.Start = this.ClampStart(start, this.Span);
			this.UserView = true;
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x0018966C File Offset: 0x0018786C
		public void AnimateTo(double start, double span, bool releaseUserViewOnFinish = false)
		{
			span = Math.Clamp(span, this.MinSpan, this.MaxSpan);
			this.Animation.Start(this.Start, this.Span, this.ClampStart(start, span), span);
			this.UserView = true;
			this.releaseUserViewOnFinish = releaseUserViewOnFinish;
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x001896BB File Offset: 0x001878BB
		public void CancelAnimation()
		{
			this.Animation.Cancel();
			this.releaseUserViewOnFinish = false;
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x001896D0 File Offset: 0x001878D0
		public bool Advance()
		{
			if (!this.Animation.Active)
			{
				return false;
			}
			double start = this.Start;
			double span = this.Span;
			bool flag = this.Animation.Advance(ref start, ref span);
			this.Start = start;
			this.Span = Math.Max(this.MinSpan, span);
			if (flag)
			{
				return false;
			}
			if (this.releaseUserViewOnFinish)
			{
				this.UserView = false;
			}
			this.releaseUserViewOnFinish = false;
			return true;
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x0018973D File Offset: 0x0018793D
		public void ShiftOrigin(double delta)
		{
			this.Start -= delta;
			this.Animation.ShiftBy(-delta);
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x0018975A File Offset: 0x0018795A
		public void BeginPan(Vector2 position)
		{
			this.Panning = true;
			this.panLastX = position.x;
			this.panLastY = position.y;
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x0018977B File Offset: 0x0018797B
		public void EndPan()
		{
			this.Panning = false;
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x00189784 File Offset: 0x00187984
		public bool TryPan(Vector2 position, float plotWidth, float laneHeight)
		{
			if (!this.Panning || (position.x == this.panLastX && position.y == this.panLastY))
			{
				return false;
			}
			this.CancelAnimation();
			this.Start -= (double)((position.x - this.panLastX) / plotWidth) * this.Span;
			this.TopLane -= (position.y - this.panLastY) / laneHeight;
			this.panLastX = position.x;
			this.panLastY = position.y;
			this.UserView = true;
			return true;
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x00189820 File Offset: 0x00187A20
		public bool HandleWheel(Vector2 delta, float mouseX, float plotX0, float plotWidth)
		{
			double num = this.Animation.Active ? this.Animation.TargetStart : this.Start;
			double num2 = this.Animation.Active ? this.Animation.TargetSpan : this.Span;
			if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
			{
				this.CancelAnimation();
				this.Span = num2;
				this.Start = num + (double)(delta.x / 3f * 0.15f) * num2;
				this.UserView = true;
				return false;
			}
			double num3 = num + (double)(Mathf.Max(plotX0, mouseX) - plotX0) / (double)plotWidth * num2;
			float num4 = Mathf.Pow(1.25f, delta.y / 3f);
			double num5 = Math.Clamp(num2 * (double)num4, this.MinSpan, this.MaxSpan);
			this.AnimateTo(num3 - (num3 - num) * (num5 / num2), num5, false);
			return true;
		}

		// Token: 0x0400288A RID: 10378
		private const float PanPerNotch = 0.15f;

		// Token: 0x0400288B RID: 10379
		private const float ZoomPerNotch = 1.25f;

		// Token: 0x0400288C RID: 10380
		private const double PanSlack = 0.5;

		// Token: 0x0400288D RID: 10381
		private const float ScrollUnitsPerNotch = 3f;

		// Token: 0x0400288E RID: 10382
		private float panLastX;

		// Token: 0x0400288F RID: 10383
		private float panLastY;

		// Token: 0x04002890 RID: 10384
		private bool releaseUserViewOnFinish;
	}
}
