using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002082 RID: 8322
	public abstract class GeneGizmo_Resource : Gizmo_Slider
	{
		// Token: 0x170025AC RID: 9644
		// (get) Token: 0x0600D1DF RID: 53727 RVA: 0x004D1852 File Offset: 0x004CFA52
		protected override Color BarColor { get; }

		// Token: 0x170025AD RID: 9645
		// (get) Token: 0x0600D1E0 RID: 53728 RVA: 0x004D185A File Offset: 0x004CFA5A
		protected override Color BarHighlightColor { get; }

		// Token: 0x170025AE RID: 9646
		// (get) Token: 0x0600D1E1 RID: 53729 RVA: 0x004D1862 File Offset: 0x004CFA62
		protected override bool IsDraggable
		{
			get
			{
				return this.gene.pawn.IsColonistPlayerControlled || this.gene.pawn.IsPrisonerOfColony;
			}
		}

		// Token: 0x170025AF RID: 9647
		// (get) Token: 0x0600D1E2 RID: 53730 RVA: 0x004D1888 File Offset: 0x004CFA88
		protected override string BarLabel
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.gene.ValueForDisplay);
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.gene.MaxForDisplay);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x170025B0 RID: 9648
		// (get) Token: 0x0600D1E3 RID: 53731 RVA: 0x004D18D5 File Offset: 0x004CFAD5
		protected override int Increments
		{
			get
			{
				return this.gene.MaxForDisplay / 10;
			}
		}

		// Token: 0x170025B1 RID: 9649
		// (get) Token: 0x0600D1E4 RID: 53732 RVA: 0x004D18E5 File Offset: 0x004CFAE5
		protected override float ValuePercent
		{
			get
			{
				return this.gene.ValuePercent;
			}
		}

		// Token: 0x170025B2 RID: 9650
		// (get) Token: 0x0600D1E5 RID: 53733 RVA: 0x004D18F2 File Offset: 0x004CFAF2
		protected override FloatRange DragRange
		{
			get
			{
				return new FloatRange(0f, this.gene.Max);
			}
		}

		// Token: 0x170025B3 RID: 9651
		// (get) Token: 0x0600D1E6 RID: 53734 RVA: 0x004D1909 File Offset: 0x004CFB09
		// (set) Token: 0x0600D1E7 RID: 53735 RVA: 0x004D1922 File Offset: 0x004CFB22
		protected override float Target
		{
			get
			{
				return this.gene.targetValue / this.gene.Max;
			}
			set
			{
				this.gene.SetTargetValuePct(value);
			}
		}

		// Token: 0x170025B4 RID: 9652
		// (get) Token: 0x0600D1E8 RID: 53736 RVA: 0x004D1930 File Offset: 0x004CFB30
		protected override string Title
		{
			get
			{
				string text = this.gene.ResourceLabel.CapitalizeFirst();
				if (Find.Selector.SelectedPawns.Count != 1)
				{
					text = text + " (" + this.gene.pawn.LabelShort + ")";
				}
				return text;
			}
		}

		// Token: 0x0600D1E9 RID: 53737 RVA: 0x004D1982 File Offset: 0x004CFB82
		public GeneGizmo_Resource(Gene_Resource gene, List<IGeneResourceDrain> drainGenes, Color barColor, Color barHighlightColor)
		{
			this.gene = gene;
			this.drainGenes = drainGenes;
			this.BarColor = barColor;
			this.BarHighlightColor = barHighlightColor;
		}

		// Token: 0x0600D1EA RID: 53738 RVA: 0x004D19A7 File Offset: 0x004CFBA7
		protected override IEnumerable<float> GetBarThresholds()
		{
			GeneGizmo_Resource.<GetBarThresholds>d__24 <GetBarThresholds>d__ = new GeneGizmo_Resource.<GetBarThresholds>d__24(-2);
			<GetBarThresholds>d__.<>4__this = this;
			return <GetBarThresholds>d__;
		}

		// Token: 0x0400850A RID: 34058
		protected Gene_Resource gene;

		// Token: 0x0400850B RID: 34059
		protected List<IGeneResourceDrain> drainGenes;
	}
}
