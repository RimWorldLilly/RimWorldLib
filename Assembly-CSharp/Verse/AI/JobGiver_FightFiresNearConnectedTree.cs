using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008F5 RID: 2293
	public class JobGiver_FightFiresNearConnectedTree : ThinkNode_JobGiver
	{
		// Token: 0x06004638 RID: 17976 RVA: 0x001B8A98 File Offset: 0x001B6C98
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = pawn.connections.ConnectedThings.FirstOrDefault((Thing x) => x.Spawned && x.Map == pawn.Map && pawn.CanReach(x, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn));
			if (thing == null)
			{
				return null;
			}
			Thing thing2 = GenClosest.ClosestThingReachable(thing.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 10f, JobGiver_FightFiresNearPoint.FireValidator(pawn), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing2 != null)
			{
				return JobMaker.MakeJob(JobDefOf.BeatFire, thing2);
			}
			return null;
		}

		// Token: 0x04002D90 RID: 11664
		private const float FightFireDistance = 10f;
	}
}
