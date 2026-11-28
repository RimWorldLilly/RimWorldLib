using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AB7 RID: 2743
	public class JobGiver_Mate : ThinkNode_JobGiver
	{
		// Token: 0x0600528E RID: 21134 RVA: 0x001EC1AC File Offset: 0x001EA3AC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.gender != Gender.Male || pawn.Sterile() || pawn.RaceProps.disableMating)
			{
				return null;
			}
			Predicate<Thing> validator = delegate(Thing t)
			{
				Pawn pawn3 = t as Pawn;
				return !pawn3.Downed && pawn3.CanCasuallyInteractNow(false, false, false, false) && !pawn3.IsForbidden(pawn) && pawn3.Faction == pawn.Faction && PawnUtility.FertileMateTarget(pawn, pawn3);
			};
			foreach (ThingDef singleDef in pawn.RaceProps.canCrossBreedWith.OrElseEmptyEnumerable<ThingDef>().Prepend(pawn.def))
			{
				Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(singleDef), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 30f, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (pawn2 != null)
				{
					return JobMaker.MakeJob(JobDefOf.Mate, pawn2);
				}
			}
			return null;
		}
	}
}
