using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001192 RID: 4498
	public class IncidentWorker_ResourcePodCrash : IncidentWorker
	{
		// Token: 0x060070A3 RID: 28835 RVA: 0x00276B98 File Offset: 0x00274D98
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Thing> things = ThingSetMakerDefOf.ResourcePod.root.Generate();
			IntVec3 intVec = DropCellFinder.RandomDropSpot(map, true);
			DropPodUtility.DropThingsNear(intVec, map, things, 110, false, true, true, true, true, null);
			base.SendStandardLetter("LetterLabelCargoPodCrash".Translate(), "CargoPodCrash".Translate(), LetterDefOf.PositiveEvent, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}
	}
}
