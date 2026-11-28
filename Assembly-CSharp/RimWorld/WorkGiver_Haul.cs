using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C15 RID: 3093
	public abstract class WorkGiver_Haul : WorkGiver_Scanner
	{
		// Token: 0x06005845 RID: 22597 RVA: 0x0020A924 File Offset: 0x00208B24
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06005846 RID: 22598 RVA: 0x0020A927 File Offset: 0x00208B27
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling();
		}

		// Token: 0x06005847 RID: 22599 RVA: 0x0020A939 File Offset: 0x00208B39
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling().Count == 0;
		}

		// Token: 0x06005848 RID: 22600 RVA: 0x0020A953 File Offset: 0x00208B53
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, forced))
			{
				return null;
			}
			return HaulAIUtility.HaulToStorageJob(pawn, t, forced);
		}
	}
}
