using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001187 RID: 4487
	public class IncidentWorker_MechCluster : IncidentWorker
	{
		// Token: 0x06007074 RID: 28788 RVA: 0x00275D68 File Offset: 0x00273F68
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ModsConfig.RoyaltyActive && base.CanFireNowSub(parms) && Faction.OfMechanoids != null;
		}

		// Token: 0x06007075 RID: 28789 RVA: 0x00275D88 File Offset: 0x00273F88
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(parms.points, map, true, false);
			IntVec3 center = MechClusterUtility.FindClusterPosition(map, sketch, 100, 0.5f);
			if (!center.IsValid)
			{
				return false;
			}
			IEnumerable<Thing> targets = from t in MechClusterUtility.SpawnCluster(center, map, sketch, true, true, parms.questTag)
			where t.def != ThingDefOf.Wall && t.def != ThingDefOf.Barricade
			select t;
			base.SendStandardLetter(parms, new LookTargets(targets), Array.Empty<NamedArgument>());
			return true;
		}
	}
}
