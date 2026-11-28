using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001185 RID: 4485
	public class IncidentWorker_LavaFlow : IncidentWorker
	{
		// Token: 0x06007070 RID: 28784 RVA: 0x00275D09 File Offset: 0x00273F09
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			((Map)parms.target).GameConditionManager.RegisterCondition(GameConditionMaker.MakeConditionPermanent(GameConditionDefOf.LavaFlow));
			return true;
		}
	}
}
