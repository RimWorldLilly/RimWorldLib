using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F9D RID: 8093
	public class StatPart_Quality : StatPart
	{
		// Token: 0x0600CDA1 RID: 52641 RVA: 0x004B9110 File Offset: 0x004B7310
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (val <= 0f && !this.applyToNegativeValues)
			{
				return;
			}
			float num = val * this.QualityMultiplier(req.QualityCategory) - val;
			num = Mathf.Min(num, this.MaxGain(req.QualityCategory));
			val += num;
		}

		// Token: 0x0600CDA2 RID: 52642 RVA: 0x004B9160 File Offset: 0x004B7360
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && !this.applyToNegativeValues && req.Thing.GetStatValue(this.parentStat, true, -1) <= 0f)
			{
				return null;
			}
			QualityCategory qc;
			if (req.HasThing && req.Thing.TryGetQuality(out qc))
			{
				string text = "StatsReport_QualityMultiplier".Translate() + ": x" + this.QualityMultiplier(qc).ToStringPercent();
				float num = this.MaxGain(qc);
				if (num < 999999f)
				{
					text += "\n    (" + "StatsReport_MaxGain".Translate() + ": " + num.ToStringByStyle(this.parentStat.ToStringStyleUnfinalized, this.parentStat.toStringNumberSense) + ")";
				}
				return text;
			}
			return null;
		}

		// Token: 0x0600CDA3 RID: 52643 RVA: 0x004B9250 File Offset: 0x004B7450
		private float QualityMultiplier(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.factorAwful;
			case QualityCategory.Poor:
				return this.factorPoor;
			case QualityCategory.Normal:
				return this.factorNormal;
			case QualityCategory.Good:
				return this.factorGood;
			case QualityCategory.Excellent:
				return this.factorExcellent;
			case QualityCategory.Masterwork:
				return this.factorMasterwork;
			case QualityCategory.Legendary:
				return this.factorLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0600CDA4 RID: 52644 RVA: 0x004B92B8 File Offset: 0x004B74B8
		private float MaxGain(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.maxGainAwful;
			case QualityCategory.Poor:
				return this.maxGainPoor;
			case QualityCategory.Normal:
				return this.maxGainNormal;
			case QualityCategory.Good:
				return this.maxGainGood;
			case QualityCategory.Excellent:
				return this.maxGainExcellent;
			case QualityCategory.Masterwork:
				return this.maxGainMasterwork;
			case QualityCategory.Legendary:
				return this.maxGainLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04008316 RID: 33558
		private bool applyToNegativeValues;

		// Token: 0x04008317 RID: 33559
		private float factorAwful = 1f;

		// Token: 0x04008318 RID: 33560
		private float factorPoor = 1f;

		// Token: 0x04008319 RID: 33561
		private float factorNormal = 1f;

		// Token: 0x0400831A RID: 33562
		private float factorGood = 1f;

		// Token: 0x0400831B RID: 33563
		private float factorExcellent = 1f;

		// Token: 0x0400831C RID: 33564
		private float factorMasterwork = 1f;

		// Token: 0x0400831D RID: 33565
		private float factorLegendary = 1f;

		// Token: 0x0400831E RID: 33566
		private float maxGainAwful = 9999999f;

		// Token: 0x0400831F RID: 33567
		private float maxGainPoor = 9999999f;

		// Token: 0x04008320 RID: 33568
		private float maxGainNormal = 9999999f;

		// Token: 0x04008321 RID: 33569
		private float maxGainGood = 9999999f;

		// Token: 0x04008322 RID: 33570
		private float maxGainExcellent = 9999999f;

		// Token: 0x04008323 RID: 33571
		private float maxGainMasterwork = 9999999f;

		// Token: 0x04008324 RID: 33572
		private float maxGainLegendary = 9999999f;
	}
}
