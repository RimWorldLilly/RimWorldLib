using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AB8 RID: 2744
	public class JobGiver_Mine : ThinkNode_JobGiver
	{
		// Token: 0x06005290 RID: 21136 RVA: 0x001EC2C8 File Offset: 0x001EA4C8
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_Mine.<>c__DisplayClass0_0 CS$<>8__locals1 = new JobGiver_Mine.<>c__DisplayClass0_0();
			CS$<>8__locals1.pawn = pawn;
			IEnumerable<Thing> enumerable = MineAIUtility.PotentialMineables(CS$<>8__locals1.pawn);
			Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Undefined), PathEndMode.Touch, TraverseParms.For(CS$<>8__locals1.pawn, CS$<>8__locals1.pawn.NormalMaxDanger(), TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<TryGiveJob>g__Validator|0), enumerable, 0, -1, enumerable != null, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				return null;
			}
			return MineAIUtility.JobOnThing(CS$<>8__locals1.pawn, thing, false);
		}
	}
}
