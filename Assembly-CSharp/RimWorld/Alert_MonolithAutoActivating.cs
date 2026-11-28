using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C1C RID: 7196
	public class Alert_MonolithAutoActivating : Alert
	{
		// Token: 0x0600B7A0 RID: 47008 RVA: 0x0040BBEC File Offset: 0x00409DEC
		public Alert_MonolithAutoActivating()
		{
			this.defaultExplanation = "MonolithTwistingExplanation".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0600B7A1 RID: 47009 RVA: 0x0040BC1C File Offset: 0x00409E1C
		public override string GetLabel()
		{
			return "MonolithTwistingAlert".Translate() + ": " + Find.Anomaly.monolith.TicksUntilAutoActivate.ToStringTicksToPeriodVerbose(true, true);
		}

		// Token: 0x0600B7A2 RID: 47010 RVA: 0x0040BC54 File Offset: 0x00409E54
		public override AlertReport GetReport()
		{
			if (Find.Anomaly.monolith == null || !Find.Anomaly.monolith.IsAutoActivating)
			{
				return false;
			}
			this.targets.Clear();
			this.targets.Add(Find.Anomaly.monolith);
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006946 RID: 26950
		private List<Thing> targets = new List<Thing>(1);
	}
}
