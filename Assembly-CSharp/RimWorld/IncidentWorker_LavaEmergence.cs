using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001184 RID: 4484
	public class IncidentWorker_LavaEmergence : IncidentWorker
	{
		// Token: 0x0600706E RID: 28782 RVA: 0x00275CD4 File Offset: 0x00273ED4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			GenSpawn.Spawn(ThingDefOf.LavaEmergence, map.Center, map, WipeMode.Vanish);
			return true;
		}
	}
}
