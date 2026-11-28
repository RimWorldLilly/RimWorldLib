using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD0 RID: 4048
	public class RaidAgeRestrictionWorker_Children : RaidAgeRestrictionWorker
	{
		// Token: 0x060066B1 RID: 26289 RVA: 0x0024716F File Offset: 0x0024536F
		public override bool CanUseWith(IncidentParms parms)
		{
			return Find.Storyteller.difficulty.ChildRaidersAllowed && !Find.Storyteller.difficulty.babiesAreHealthy && base.CanUseWith(parms);
		}
	}
}
