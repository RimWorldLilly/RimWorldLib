using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBC RID: 7100
	public class Alert_FuelNodeIgnition : Alert_ActionDelay
	{
		// Token: 0x0600B617 RID: 46615 RVA: 0x00401C8E File Offset: 0x003FFE8E
		public Alert_FuelNodeIgnition()
		{
		}

		// Token: 0x0600B618 RID: 46616 RVA: 0x00401C96 File Offset: 0x003FFE96
		public Alert_FuelNodeIgnition(SignalAction_StartWick startWickAction)
		{
			this.startWickAction = startWickAction;
		}

		// Token: 0x0600B619 RID: 46617 RVA: 0x00401CA5 File Offset: 0x003FFEA5
		public override AlertReport GetReport()
		{
			if (this.startWickAction == null)
			{
				return AlertReport.Inactive;
			}
			return AlertReport.CulpritIs(this.startWickAction.thingWithWick);
		}

		// Token: 0x0600B61A RID: 46618 RVA: 0x00401CCA File Offset: 0x003FFECA
		public override string GetLabel()
		{
			return "AlertFuelNodeIgniting".Translate(this.startWickAction.delayTicks.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x0600B61B RID: 46619 RVA: 0x00401CF2 File Offset: 0x003FFEF2
		public override TaggedString GetExplanation()
		{
			return "AlertFuelNodeIgnitingDesc".Translate();
		}

		// Token: 0x040068A6 RID: 26790
		private SignalAction_StartWick startWickAction;
	}
}
