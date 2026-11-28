using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000494 RID: 1172
	public class HediffComp_SeverityPerDay : HediffComp_SeverityModifierBase
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x000C2F38 File Offset: 0x000C1138
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000C2F45 File Offset: 0x000C1145
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDay = this.Props.CalculateSeverityPerDay();
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x000C2F60 File Offset: 0x000C1160
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDay, "severityPerDay", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.severityPerDay == 0f && this.Props.severityPerDay != 0f && this.Props.severityPerDayRange == FloatRange.Zero)
			{
				this.severityPerDay = this.Props.CalculateSeverityPerDay();
				Log.Warning("Hediff " + this.parent.Label + " had severityPerDay not matching parent.");
			}
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000C2FF8 File Offset: 0x000C11F8
		public override float SeverityChangePerDay()
		{
			if (base.Pawn.ageTracker.AgeBiologicalYearsFloat < this.Props.minAge)
			{
				return 0f;
			}
			float num = this.severityPerDay;
			HediffStage curStage = this.parent.CurStage;
			float num2 = num * ((curStage != null) ? curStage.severityGainFactor : 1f);
			if (ModsConfig.BiotechActive && MechanitorUtility.IsMechanitor(base.Pawn))
			{
				num2 *= this.Props.mechanitorFactor;
			}
			return num2;
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06002082 RID: 8322 RVA: 0x000C3070 File Offset: 0x000C1270
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (this.Props.showHoursToRecover && this.SeverityChangePerDay() < 0f)
				{
					return Mathf.RoundToInt(this.parent.Severity / Mathf.Abs(this.SeverityChangePerDay()) * 24f).ToString() + "LetterHour".Translate();
				}
				return null;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06002083 RID: 8323 RVA: 0x000C30D8 File Offset: 0x000C12D8
		public override string CompTipStringExtra
		{
			get
			{
				if (this.Props.showDaysToRecover && this.SeverityChangePerDay() < 0f)
				{
					return "DaysToRecover".Translate((this.parent.Severity / Mathf.Abs(this.SeverityChangePerDay())).ToString("0.0")).Resolve();
				}
				return null;
			}
		}

		// Token: 0x04001926 RID: 6438
		public float severityPerDay;
	}
}
