using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B05 RID: 6917
	public class SignalAction_Incident : SignalAction
	{
		// Token: 0x0600B0DE RID: 45278 RVA: 0x003E5060 File Offset: 0x003E3260
		protected override void DoAction(SignalArgs args)
		{
			if (this.incident.Worker.CanFireNow(this.incidentParms))
			{
				this.incident.Worker.TryExecute(this.incidentParms);
			}
		}

		// Token: 0x0600B0DF RID: 45279 RVA: 0x003E5091 File Offset: 0x003E3291
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			Scribe_Deep.Look<IncidentParms>(ref this.incidentParms, "incidentParms", Array.Empty<object>());
		}

		// Token: 0x04006613 RID: 26131
		public IncidentDef incident;

		// Token: 0x04006614 RID: 26132
		public IncidentParms incidentParms;
	}
}
