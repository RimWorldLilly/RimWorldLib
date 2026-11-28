using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006A9 RID: 1705
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060035F9 RID: 13817 RVA: 0x0015D439 File Offset: 0x0015B639
		// (set) Token: 0x060035FA RID: 13818 RVA: 0x0015D441 File Offset: 0x0015B641
		public bool DrawBackground { get; set; }

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060035FB RID: 13819 RVA: 0x0015D44A File Offset: 0x0015B64A
		// (set) Token: 0x060035FC RID: 13820 RVA: 0x0015D452 File Offset: 0x0015B652
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060035FD RID: 13821 RVA: 0x0015D45B File Offset: 0x0015B65B
		// (set) Token: 0x060035FE RID: 13822 RVA: 0x0015D463 File Offset: 0x0015B663
		public bool DrawMeasures { get; set; }

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060035FF RID: 13823 RVA: 0x0015D46C File Offset: 0x0015B66C
		// (set) Token: 0x06003600 RID: 13824 RVA: 0x0015D474 File Offset: 0x0015B674
		public bool DrawPoints { get; set; }

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06003601 RID: 13825 RVA: 0x0015D47D File Offset: 0x0015B67D
		// (set) Token: 0x06003602 RID: 13826 RVA: 0x0015D485 File Offset: 0x0015B685
		public bool DrawLegend { get; set; }

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06003603 RID: 13827 RVA: 0x0015D48E File Offset: 0x0015B68E
		// (set) Token: 0x06003604 RID: 13828 RVA: 0x0015D496 File Offset: 0x0015B696
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06003605 RID: 13829 RVA: 0x0015D49F File Offset: 0x0015B69F
		// (set) Token: 0x06003606 RID: 13830 RVA: 0x0015D4A7 File Offset: 0x0015B6A7
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06003607 RID: 13831 RVA: 0x0015D4B0 File Offset: 0x0015B6B0
		// (set) Token: 0x06003608 RID: 13832 RVA: 0x0015D4B8 File Offset: 0x0015B6B8
		public bool UseFixedSection { get; set; }

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06003609 RID: 13833 RVA: 0x0015D4C1 File Offset: 0x0015B6C1
		// (set) Token: 0x0600360A RID: 13834 RVA: 0x0015D4C9 File Offset: 0x0015B6C9
		public bool UseFixedScale { get; set; }

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x0600360B RID: 13835 RVA: 0x0015D4D2 File Offset: 0x0015B6D2
		// (set) Token: 0x0600360C RID: 13836 RVA: 0x0015D4DA File Offset: 0x0015B6DA
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x0600360D RID: 13837 RVA: 0x0015D4E3 File Offset: 0x0015B6E3
		// (set) Token: 0x0600360E RID: 13838 RVA: 0x0015D4EB File Offset: 0x0015B6EB
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x0600360F RID: 13839 RVA: 0x0015D4F4 File Offset: 0x0015B6F4
		// (set) Token: 0x06003610 RID: 13840 RVA: 0x0015D4FC File Offset: 0x0015B6FC
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06003611 RID: 13841 RVA: 0x0015D505 File Offset: 0x0015B705
		// (set) Token: 0x06003612 RID: 13842 RVA: 0x0015D50D File Offset: 0x0015B70D
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06003613 RID: 13843 RVA: 0x0015D516 File Offset: 0x0015B716
		// (set) Token: 0x06003614 RID: 13844 RVA: 0x0015D51E File Offset: 0x0015B71E
		public bool XIntegersOnly { get; set; }

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06003615 RID: 13845 RVA: 0x0015D527 File Offset: 0x0015B727
		// (set) Token: 0x06003616 RID: 13846 RVA: 0x0015D52F File Offset: 0x0015B72F
		public bool YIntegersOnly { get; set; }

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06003617 RID: 13847 RVA: 0x0015D538 File Offset: 0x0015B738
		// (set) Token: 0x06003618 RID: 13848 RVA: 0x0015D540 File Offset: 0x0015B740
		public string LabelX { get; set; }

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06003619 RID: 13849 RVA: 0x0015D549 File Offset: 0x0015B749
		// (set) Token: 0x0600361A RID: 13850 RVA: 0x0015D551 File Offset: 0x0015B751
		public FloatRange FixedSection { get; set; }

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x0600361B RID: 13851 RVA: 0x0015D55A File Offset: 0x0015B75A
		// (set) Token: 0x0600361C RID: 13852 RVA: 0x0015D562 File Offset: 0x0015B762
		public Vector2 FixedScale { get; set; }

		// Token: 0x0600361D RID: 13853 RVA: 0x0015D56C File Offset: 0x0015B76C
		public SimpleCurveDrawerStyle()
		{
			this.DrawBackground = false;
			this.DrawBackgroundLines = true;
			this.DrawMeasures = false;
			this.DrawPoints = true;
			this.DrawLegend = false;
			this.DrawCurveMousePoint = false;
			this.OnlyPositiveValues = false;
			this.UseFixedSection = false;
			this.UseFixedScale = false;
			this.UseAntiAliasedLines = false;
			this.PointsRemoveOptimization = false;
			this.MeasureLabelsXCount = 5;
			this.MeasureLabelsYCount = 5;
			this.XIntegersOnly = false;
			this.YIntegersOnly = false;
			this.LabelX = "x";
		}
	}
}
