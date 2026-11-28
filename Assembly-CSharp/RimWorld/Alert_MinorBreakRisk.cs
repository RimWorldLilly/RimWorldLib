using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBE RID: 7102
	public class Alert_MinorBreakRisk : Alert
	{
		// Token: 0x0600B621 RID: 46625 RVA: 0x00401E57 File Offset: 0x00400057
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600B622 RID: 46626 RVA: 0x00401E66 File Offset: 0x00400066
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x0600B623 RID: 46627 RVA: 0x00401E6D File Offset: 0x0040006D
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x0600B624 RID: 46628 RVA: 0x00401E79 File Offset: 0x00400079
		public override AlertReport GetReport()
		{
			if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>() || BreakRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
			{
				return false;
			}
			return AlertReport.CulpritsAre(BreakRiskAlertUtility.PawnsAtRiskMinor);
		}
	}
}
