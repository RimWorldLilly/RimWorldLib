using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D1A RID: 7450
	public class FloatMenuOptionProvider_CarryPawn : FloatMenuOptionProvider
	{
		// Token: 0x17002321 RID: 8993
		// (get) Token: 0x0600C0BB RID: 49339 RVA: 0x004649F0 File Offset: 0x00462BF0
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002322 RID: 8994
		// (get) Token: 0x0600C0BC RID: 49340 RVA: 0x004649F3 File Offset: 0x00462BF3
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002323 RID: 8995
		// (get) Token: 0x0600C0BD RID: 49341 RVA: 0x004649F6 File Offset: 0x00462BF6
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002324 RID: 8996
		// (get) Token: 0x0600C0BE RID: 49342 RVA: 0x004649F9 File Offset: 0x00462BF9
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C0BF RID: 49343 RVA: 0x004649FC File Offset: 0x00462BFC
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return !context.FirstSelectedPawn.IsMutant || !context.FirstSelectedPawn.mutant.Def.canCarryPawns;
		}

		// Token: 0x0600C0C0 RID: 49344 RVA: 0x00464A28 File Offset: 0x00462C28
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.Downed && !clickedPawn.IsSelfShutdown())
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotCarry".Translate(clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return new FloatMenuOption("CannotCarry".Translate(context.FirstSelectedPawn) + ": " + "Incapable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Carry".Translate(clickedPawn), delegate()
			{
				clickedPawn.SetForbidden(false, false);
				Job job = JobMaker.MakeJob(JobDefOf.CarryDownedPawnDrafted, clickedPawn);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
