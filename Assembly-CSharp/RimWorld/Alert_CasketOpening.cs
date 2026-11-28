using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBB RID: 7099
	public class Alert_CasketOpening : Alert_ActionDelay
	{
		// Token: 0x0600B612 RID: 46610 RVA: 0x00401C01 File Offset: 0x003FFE01
		public Alert_CasketOpening()
		{
		}

		// Token: 0x0600B613 RID: 46611 RVA: 0x00401C14 File Offset: 0x003FFE14
		public Alert_CasketOpening(SignalAction_OpenCasket openCasketAction)
		{
			this.openCasketAction = openCasketAction;
			this.culprints.AddRange(openCasketAction.caskets);
		}

		// Token: 0x0600B614 RID: 46612 RVA: 0x00401C3F File Offset: 0x003FFE3F
		public override AlertReport GetReport()
		{
			if (this.openCasketAction == null)
			{
				return AlertReport.Inactive;
			}
			return AlertReport.CulpritsAre(this.culprints);
		}

		// Token: 0x0600B615 RID: 46613 RVA: 0x00401C5A File Offset: 0x003FFE5A
		public override string GetLabel()
		{
			return "AlertCasketOpening".Translate(this.openCasketAction.delayTicks.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x0600B616 RID: 46614 RVA: 0x00401C82 File Offset: 0x003FFE82
		public override TaggedString GetExplanation()
		{
			return "AlertCasketOpeningDesc".Translate();
		}

		// Token: 0x040068A4 RID: 26788
		private SignalAction_OpenCasket openCasketAction;

		// Token: 0x040068A5 RID: 26789
		private List<Thing> culprints = new List<Thing>();
	}
}
