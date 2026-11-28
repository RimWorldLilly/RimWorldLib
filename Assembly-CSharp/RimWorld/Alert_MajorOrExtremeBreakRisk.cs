using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA1 RID: 7073
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		// Token: 0x170020D5 RID: 8405
		// (get) Token: 0x0600B59B RID: 46491 RVA: 0x003FF7B6 File Offset: 0x003FD9B6
		private List<Pawn> Culprits
		{
			get
			{
				this.culpritsResult.Clear();
				this.culpritsResult.AddRange(BreakRiskAlertUtility.PawnsAtRiskExtreme);
				this.culpritsResult.AddRange(BreakRiskAlertUtility.PawnsAtRiskMajor);
				return this.culpritsResult;
			}
		}

		// Token: 0x0600B59C RID: 46492 RVA: 0x003FF7E9 File Offset: 0x003FD9E9
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x0600B59D RID: 46493 RVA: 0x003FF7F0 File Offset: 0x003FD9F0
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x0600B59E RID: 46494 RVA: 0x003FF7FC File Offset: 0x003FD9FC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Culprits);
		}

		// Token: 0x04006881 RID: 26753
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
