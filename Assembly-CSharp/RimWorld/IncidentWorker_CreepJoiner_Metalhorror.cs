using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200116C RID: 4460
	public class IncidentWorker_CreepJoiner_Metalhorror : IncidentWorker_GiveQuest
	{
		// Token: 0x06007006 RID: 28678 RVA: 0x00273650 File Offset: 0x00271850
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && Find.Anomaly.CanNewMetalhorrorBiosignatureImplantOccur;
		}
	}
}
