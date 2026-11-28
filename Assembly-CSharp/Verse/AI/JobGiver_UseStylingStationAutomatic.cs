using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000831 RID: 2097
	public class JobGiver_UseStylingStationAutomatic : ThinkNode_JobGiver
	{
		// Token: 0x060041A3 RID: 16803 RVA: 0x0019FB9C File Offset: 0x0019DD9C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return null;
			}
			if (Find.TickManager.TicksGame < pawn.style.nextStyleChangeAttemptTick)
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.StylingStation), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, false);
			if (thing == null)
			{
				pawn.style.ResetNextStyleChangeAttemptTick();
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.UseStylingStationAutomatic, thing);
		}
	}
}
