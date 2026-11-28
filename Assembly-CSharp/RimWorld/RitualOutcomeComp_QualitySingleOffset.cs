using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BC RID: 5820
	public abstract class RitualOutcomeComp_QualitySingleOffset : RitualOutcomeComp_Quality
	{
		// Token: 0x1700190F RID: 6415
		// (get) Token: 0x060091B6 RID: 37302 RVA: 0x00354535 File Offset: 0x00352735
		protected virtual string LabelForDesc
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x060091B7 RID: 37303 RVA: 0x00354544 File Offset: 0x00352744
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			string str = (this.qualityOffset < 0f) ? "" : "+";
			return this.LabelForDesc + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + this.qualityOffset.ToStringPercent()) + ".";
		}

		// Token: 0x060091B8 RID: 37304 RVA: 0x003545AF File Offset: 0x003527AF
		public override string GetBonusDescShort()
		{
			return "OutcomeBonusDesc_QualitySingleOffset".Translate("+" + this.qualityOffset.ToStringPercent()) + ".";
		}

		// Token: 0x060091B9 RID: 37305 RVA: 0x003545E4 File Offset: 0x003527E4
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return 1f;
		}

		// Token: 0x060091BA RID: 37306 RVA: 0x003545EB File Offset: 0x003527EB
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.qualityOffset;
		}

		// Token: 0x060091BB RID: 37307 RVA: 0x003545F4 File Offset: 0x003527F4
		protected override string ExpectedOffsetDesc(bool positive, float quality = -1f)
		{
			quality = ((quality == -1f) ? this.qualityOffset : quality);
			return positive ? quality.ToStringWithSign("0.#%") : "QualityOutOf".Translate("+0", quality.ToStringWithSign("0.#%"));
		}
	}
}
