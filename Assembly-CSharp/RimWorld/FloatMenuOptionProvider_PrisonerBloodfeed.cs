using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D39 RID: 7481
	public class FloatMenuOptionProvider_PrisonerBloodfeed : FloatMenuOptionProvider
	{
		// Token: 0x17002394 RID: 9108
		// (get) Token: 0x0600C191 RID: 49553 RVA: 0x00467EDB File Offset: 0x004660DB
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002395 RID: 9109
		// (get) Token: 0x0600C192 RID: 49554 RVA: 0x00467EDE File Offset: 0x004660DE
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002396 RID: 9110
		// (get) Token: 0x0600C193 RID: 49555 RVA: 0x00467EE1 File Offset: 0x004660E1
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C194 RID: 49556 RVA: 0x00467EE4 File Offset: 0x004660E4
		protected override bool AppliesInt(FloatMenuContext context)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (!context.FirstSelectedPawn.IsBloodfeeder())
			{
				return false;
			}
			Pawn_GeneTracker genes = context.FirstSelectedPawn.genes;
			return ((genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null) != null;
		}

		// Token: 0x0600C195 RID: 49557 RVA: 0x00467F1C File Offset: 0x0046611C
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.IsBloodfeeder())
			{
				return null;
			}
			if (!clickedPawn.IsPrisonerOfColony || !clickedPawn.guest.PrisonerIsSecure || clickedPawn.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Bloodfeed))
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotBloodfeedOn".Translate(clickedPawn.Named("PAWN")) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			AcceptanceReport acceptanceReport = JobGiver_GetHemogen.CanFeedOnPrisoner(context.FirstSelectedPawn, clickedPawn);
			if (acceptanceReport.Accepted)
			{
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("BloodfeedOn".Translate(clickedPawn.Named("PAWN")), delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.PrisonerBloodfeed, clickedPawn);
					context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			if (acceptanceReport.Reason.NullOrEmpty())
			{
				return null;
			}
			return new FloatMenuOption("CannotBloodfeedOn".Translate(clickedPawn.Named("PAWN")) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
		}
	}
}
