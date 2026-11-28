using System;

namespace Verse
{
	// Token: 0x02000404 RID: 1028
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x06001DD5 RID: 7637 RVA: 0x000B7F90 File Offset: 0x000B6190
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if ((this.severityToMtbDaysCurve == null && cause.Severity <= this.minSeverity) || (this.severityToMtbDaysCurve != null && cause.Severity <= this.severityToMtbDaysCurve.Points[0].x))
			{
				return;
			}
			float num = (this.severityToMtbDaysCurve != null) ? this.severityToMtbDaysCurve.Evaluate(cause.Severity) : this.baseMtbDays;
			float num2 = this.ChanceFactor(pawn);
			if (num2 == 0f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(num / num2, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		// Token: 0x040017DD RID: 6109
		public SimpleCurve severityToMtbDaysCurve;

		// Token: 0x040017DE RID: 6110
		public float baseMtbDays;

		// Token: 0x040017DF RID: 6111
		public float minSeverity;
	}
}
