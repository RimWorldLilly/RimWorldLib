using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D1B RID: 7451
	public class FloatMenuOptionProvider_CarryPawnToExit : FloatMenuOptionProvider
	{
		// Token: 0x17002325 RID: 8997
		// (get) Token: 0x0600C0C2 RID: 49346 RVA: 0x00464BA9 File Offset: 0x00462DA9
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002326 RID: 8998
		// (get) Token: 0x0600C0C3 RID: 49347 RVA: 0x00464BAC File Offset: 0x00462DAC
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002327 RID: 8999
		// (get) Token: 0x0600C0C4 RID: 49348 RVA: 0x00464BAF File Offset: 0x00462DAF
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C0C5 RID: 49349 RVA: 0x00464BB2 File Offset: 0x00462DB2
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return !context.map.IsPlayerHome && context.map.exitMapGrid.MapUsesExitGrid;
		}

		// Token: 0x0600C0C6 RID: 49350 RVA: 0x00464BD4 File Offset: 0x00462DD4
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.Downed)
			{
				return null;
			}
			if (clickedPawn.Faction != Faction.OfPlayer && !clickedPawn.IsPrisonerOfColony && !CaravanUtility.ShouldAutoCapture(clickedPawn, Faction.OfPlayer))
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotCarryToExit".Translate(clickedPawn.Label, clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (context.FirstSelectedPawn.Map.IsPocketMap)
			{
				Thing portal;
				if (!RCellFinder.TryFindExitPortal(context.FirstSelectedPawn, out portal))
				{
					return new FloatMenuOption("CannotCarryToExit".Translate(clickedPawn.Label, clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption((clickedPawn.Faction == Faction.OfPlayer || clickedPawn.IsPrisonerOfColony) ? "CarryToExit".Translate(clickedPawn.Label, clickedPawn) : "CarryToExitAndCapture".Translate(clickedPawn.Label, clickedPawn), delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.CarryDownedPawnToPortal, portal, clickedPawn);
					job.count = 1;
					context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			else
			{
				IntVec3 exitSpot;
				if (!RCellFinder.TryFindBestExitSpot(context.FirstSelectedPawn, out exitSpot, TraverseMode.ByPawn, true))
				{
					return new FloatMenuOption("CannotCarryToExit".Translate(clickedPawn.Label, clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption((clickedPawn.Faction == Faction.OfPlayer || clickedPawn.IsPrisonerOfColony) ? "CarryToExit".Translate(clickedPawn.Label, clickedPawn) : "CarryToExitAndCapture".Translate(clickedPawn.Label, clickedPawn), delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.CarryDownedPawnToExit, clickedPawn, exitSpot);
					job.count = 1;
					job.failIfCantJoinOrCreateCaravan = true;
					context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
		}
	}
}
