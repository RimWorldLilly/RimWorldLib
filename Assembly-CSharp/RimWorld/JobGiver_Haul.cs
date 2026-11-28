using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B4C RID: 2892
	public class JobGiver_Haul : ThinkNode_JobGiver
	{
		// Token: 0x060054AA RID: 21674 RVA: 0x001F76B0 File Offset: 0x001F58B0
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_Haul.<>c__DisplayClass0_0 CS$<>8__locals1 = new JobGiver_Haul.<>c__DisplayClass0_0();
			CS$<>8__locals1.pawn = pawn;
			Thing thing = GenClosest.ClosestThing_Global_Reachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, CS$<>8__locals1.pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling(), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<TryGiveJob>g__Validator|0), null, false);
			if (thing != null)
			{
				return HaulAIUtility.HaulToStorageJob(CS$<>8__locals1.pawn, thing, false);
			}
			return null;
		}
	}
}
