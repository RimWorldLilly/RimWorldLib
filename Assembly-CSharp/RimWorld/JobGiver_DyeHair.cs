using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A28 RID: 2600
	public class JobGiver_DyeHair : ThinkNode_JobGiver
	{
		// Token: 0x06004E89 RID: 20105 RVA: 0x001DDFEC File Offset: 0x001DC1EC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return null;
			}
			if (pawn.style.nextHairColor == null || pawn.style.nextHairColor == pawn.story.HairColor)
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.StylingStation), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, false);
			if (thing == null)
			{
				return null;
			}
			Thing thing2 = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, 1, null, false), null, false);
			if (thing2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DyeHair, thing, thing2);
			job.count = 1;
			return job;
		}
	}
}
