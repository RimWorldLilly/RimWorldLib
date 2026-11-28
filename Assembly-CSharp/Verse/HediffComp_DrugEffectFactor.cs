using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200042B RID: 1067
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001F19 RID: 7961 RVA: 0x000BDC20 File Offset: 0x000BBE20
		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001F1A RID: 7962 RVA: 0x000BDC2D File Offset: 0x000BBE2D
		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001F1B RID: 7963 RVA: 0x000BDC44 File Offset: 0x000BBE44
		public override string CompTipStringExtra
		{
			get
			{
				return "DrugEffectMultiplier".Translate(this.Props.chemical.label, this.CurrentFactor.ToStringPercent()).CapitalizeFirst();
			}
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000BDC8D File Offset: 0x000BBE8D
		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}

		// Token: 0x04001850 RID: 6224
		private static readonly SimpleCurve EffectFactorSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};
	}
}
