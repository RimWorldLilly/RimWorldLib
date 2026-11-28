using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AB5 RID: 2741
	public class JobGiver_EatRandom : ThinkNode_JobGiver
	{
		// Token: 0x06005288 RID: 21128 RVA: 0x001EBF10 File Offset: 0x001EA110
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			Predicate<Thing> validator = (Thing t) => t.def.category == ThingCategory.Item && t.IngestibleNow && pawn.RaceProps.CanEverEat(t) && pawn.CanReserve(t, 1, -1, null, false);
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 10f, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Ingest, thing);
		}
	}
}
