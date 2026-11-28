using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BB RID: 5819
	public abstract class RitualOutcomeComp_Quality : RitualOutcomeComp
	{
		// Token: 0x1700190E RID: 6414
		// (get) Token: 0x060091AD RID: 37293 RVA: 0x003542A0 File Offset: 0x003524A0
		protected float MaxValue
		{
			get
			{
				return this.curve.Points[this.curve.PointsCount - 1].x;
			}
		}

		// Token: 0x060091AE RID: 37294 RVA: 0x003542D2 File Offset: 0x003524D2
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x060091AF RID: 37295
		public abstract float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data);

		// Token: 0x060091B0 RID: 37296 RVA: 0x003542D8 File Offset: 0x003524D8
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			if ((this.DataRequired || ritual == null) && data == null)
			{
				return this.label + " (" + "MaxValue".Translate(this.MaxValue) + "): " + "OutcomeBonusDesc_Quality".Translate("+" + this.curve.Points[this.curve.PointsCount - 1].y.ToStringPercent()) + ".";
			}
			return string.Concat(new string[]
			{
				this.Count(ritual, data).ToString(),
				" / ",
				this.MaxValue.ToString(),
				" ",
				this.label,
				": "
			}) + "OutcomeBonusDesc_QualitySingleOffset".Translate("+" + this.QualityOffset(ritual, data).ToStringPercent()) + ".";
		}

		// Token: 0x060091B1 RID: 37297 RVA: 0x00354410 File Offset: 0x00352610
		public override string GetBonusDescShort()
		{
			return "OutcomeBonusDesc_Quality".Translate("+" + this.curve.Points[this.curve.PointsCount - 1].y.ToStringPercent()) + ".";
		}

		// Token: 0x060091B2 RID: 37298 RVA: 0x0035446F File Offset: 0x0035266F
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			if (this.curve == null)
			{
				return 0f;
			}
			return this.curve.Evaluate(this.Count(ritual, data));
		}

		// Token: 0x060091B3 RID: 37299 RVA: 0x00354494 File Offset: 0x00352694
		protected override string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			return "QualityOutOf".Translate(quality.ToStringWithSign("0.#%"), this.curve.Points[this.curve.PointsCount - 1].y.ToStringWithSign("0.#%"));
		}

		// Token: 0x060091B4 RID: 37300 RVA: 0x003544F4 File Offset: 0x003526F4
		public override string GetDescAbstract(bool positive, float quality = -1f)
		{
			quality = ((quality == -1f) ? this.qualityOffset : quality);
			return "OutcomeBonusDesc_QualitySingleOffset".Translate(quality.ToStringWithSign("0.#%"));
		}

		// Token: 0x04005879 RID: 22649
		public SimpleCurve curve;
	}
}
