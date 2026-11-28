using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBA RID: 7098
	public class Alert_InfestationDelay : Alert_ActionDelay
	{
		// Token: 0x0600B60D RID: 46605 RVA: 0x00401B5C File Offset: 0x003FFD5C
		public Alert_InfestationDelay()
		{
		}

		// Token: 0x0600B60E RID: 46606 RVA: 0x00401B64 File Offset: 0x003FFD64
		public Alert_InfestationDelay(SignalAction_Infestation infestationAction)
		{
			this.infestationAction = infestationAction;
		}

		// Token: 0x0600B60F RID: 46607 RVA: 0x00401B74 File Offset: 0x003FFD74
		public override AlertReport GetReport()
		{
			if (this.infestationAction == null)
			{
				return AlertReport.Inactive;
			}
			if (this.infestationAction.overrideLoc != null)
			{
				return AlertReport.CulpritIs(new GlobalTargetInfo(this.infestationAction.overrideLoc.Value, this.infestationAction.Map, false));
			}
			return AlertReport.Active;
		}

		// Token: 0x0600B610 RID: 46608 RVA: 0x00401BCD File Offset: 0x003FFDCD
		public override string GetLabel()
		{
			return "AlertInfestationArriving".Translate(this.infestationAction.delayTicks.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x0600B611 RID: 46609 RVA: 0x00401BF5 File Offset: 0x003FFDF5
		public override TaggedString GetExplanation()
		{
			return "AlertInfestationArrivingDesc".Translate();
		}

		// Token: 0x040068A3 RID: 26787
		private SignalAction_Infestation infestationAction;
	}
}
