using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001953 RID: 6483
	public class CompProperties_ReportWorkSpeed : CompProperties
	{
		// Token: 0x0600A2C7 RID: 41671 RVA: 0x003A25D2 File Offset: 0x003A07D2
		public CompProperties_ReportWorkSpeed()
		{
			this.compClass = typeof(CompReportWorkSpeed);
		}

		// Token: 0x04006030 RID: 24624
		public StatDef workSpeedStat;
	}
}
